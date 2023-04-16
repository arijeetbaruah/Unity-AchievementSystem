using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    private List<T> activePool = new List<T>();
    private Queue<T> inActivePool = new Queue<T>();

    public abstract void CreateNewInstance(System.Action<T> callback);

    public void GetInstance(System.Action<T> callback)
    {
        if (inActivePool.Count == 0)
        {
            CreateNewInstance(instance =>
            {
                activePool.Add(instance);
                //instance.transform.SetParent(transform);
                instance.gameObject.SetActive(true);
                callback?.Invoke(instance);
            });
        }
        else
        {
            T instance = inActivePool.Dequeue();

            //instance.transform.SetParent(transform);
            activePool.Add(instance);
            instance.gameObject.SetActive(true);
            callback?.Invoke(instance);
        }
    }

    public void Remove(T instance)
    {
        if (activePool.Contains(instance))
        {
            activePool.Remove(instance);
        }

        instance.transform.SetParent(transform);
        instance.gameObject.SetActive(false);
        inActivePool.Enqueue(instance);
    }
}
