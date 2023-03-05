using Game.Events;
using Game.Logger;
using Game.Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent : GameEvent
{
    public TestEvent()
    {
    }
}

public class Test : MonoBehaviour
{
    EventManager eventManager => ServiceRegistry.Get<EventManager>();

    private void OnEnable()
    {
        eventManager.AddListener<TestEvent>(@event => { });
    }

    private void OnDisable()
    {
        eventManager.RemoveListener<TestEvent>(@event => { });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            eventManager.TriggerEvent(new TestEvent());
        }
    }
}
