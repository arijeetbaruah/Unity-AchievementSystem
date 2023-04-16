using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Game.Events;
using Game.Service;
using UnityEngine.UI;

public class EventReporter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI txt;
    [SerializeField]
    private Image bg;
    private Coroutine coroutine;

    private Queue<string> strQ = new Queue<string>();

    private void OnEnable()
    {
        ServiceRegistry.Get<EventManager>().AddListener<ReportStatusEvent>(ReportStatusEvent);
    }

    private void OnDisable ()
    {
        ServiceRegistry.Get<EventManager>().RemoveListener<ReportStatusEvent>(ReportStatusEvent);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return))
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            Close();
        }

        if(!bg.enabled && strQ.Count > 0)
        {
            ShowText(strQ.Dequeue());
        }
    }

    public void Open()
    {
        bg.enabled = true;
    }

    public void Close()
    {
        bg.enabled = false;
        txt.SetText(string.Empty);
    }

    private void ReportStatusEvent(ReportStatusEvent reportEvent)
    {
        ShowText($"{reportEvent.characterID} is {reportEvent.status.ToString()}");
    }

    private void ShowText(string msg)
    {
        if (coroutine != null)
        {
            strQ.Enqueue(msg);
            return;
        }

        coroutine = StartCoroutine(ShowTxtCoroutine(msg));
    }

    private IEnumerator ShowTxtCoroutine(string msg)
    {
        Open();
        txt.SetText(msg);
        yield return new WaitForSeconds(1.5f);
        Close();
    }
}

public class ReportStatusEvent : GameEvent
{
    public string characterID;
    public CombatStatus status;

    public ReportStatusEvent(string characterID, CombatStatus status)
    {
        this.characterID = characterID;
        this.status = status;
    }
}
