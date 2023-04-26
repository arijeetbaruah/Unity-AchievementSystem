using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private List<UIPop> availablePop = new List<UIPop>();
    public Dictionary<Type, UIPop> availablePopDic = new Dictionary<Type, UIPop>();
    public UIPop activePop = null;

    public Queue<KeyValuePair<Type, Action<UIPop>>> popupQ = new Queue<KeyValuePair<Type, Action<UIPop>>>();

    private void Start()
    {
        availablePopDic = availablePop.ToDictionary(pop => pop.GetType());
    }

    private void LateUpdate()
    {
        if (popupQ.Count > 0 && !activePop.gameObject.activeSelf)
        {
            var popupDetails = popupQ.Dequeue();
            ShowPopup(popupDetails.Key, popupDetails.Value);
        }
    }

    public void QueuePopup<T>(Action<T> callback) where T : UIPop
    {
        popupQ.Enqueue(new KeyValuePair<Type, Action<UIPop>>(typeof(T), (Action<UIPop>) callback));
    }

    public bool ShowPopup(Type type, Action<UIPop> callback)
    {
        if (availablePopDic.TryGetValue(type, out UIPop popup))
        {
            callback?.Invoke(popup);
            popup.gameObject.SetActive(true);
            activePop = popup;
            return true;
        }

        return false;
    }

    public bool ShowPopup<T>(Action<T> callback) where T : UIPop
    {
        Type type = typeof(T);

        if (availablePopDic.TryGetValue(type, out UIPop popup))
        {
            
            callback?.Invoke((T) popup);
            popup.gameObject.SetActive(true);
            activePop = popup;
            return true;
        }
        
        return false;
    }

    public void HidePopup<T>()
    {
        Type type = typeof(T);

        if (availablePopDic.TryGetValue(type, out UIPop popup))
        {
            popup.gameObject.SetActive(false);
        }
    }
}
