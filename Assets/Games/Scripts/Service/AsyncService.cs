using Game.Events;
using Game.Service;
using System.Collections;
using UnityEngine;

public class AsyncService : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        ServiceRegistry.Get<EventManager>().AddListener<AsyncEvent>(OnAsyncEvent);
    }

    public void OnAsyncEvent(AsyncEvent @event)
    {
        StartCoroutine(@event.corutine);
    }
}

public class AsyncEvent : GameEvent
{
    public IEnumerator corutine;

    public AsyncEvent(IEnumerator corutine)
    {
        this.corutine = corutine;
    }
}
