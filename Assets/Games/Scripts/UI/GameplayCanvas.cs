using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using Game.Events;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameplayCanvas : MonoBehaviour
{
    [SerializeField]
    private Button attackButton;
    [SerializeField]
    private Button magicButton;
    [SerializeField]
    private Button itemButton;
    [SerializeField]
    private Button superButton;
    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private float animationSpeed = 1;

    private System.Collections.Generic.Dictionary<string, Vector3> initPosition = new System.Collections.Generic.Dictionary<string, Vector3>();

    private List<Button> buttons => new List<Button>()
    {
        attackButton, magicButton, itemButton, superButton
    };

    public Button SuperButton => superButton;

    private void OnEnable()
    {
        attackButton.onClick.AddListener(ButtonCallback<AttackButtonClickEvent>);
        magicButton.onClick.AddListener(ButtonCallback<MagicButtonClickEvent>);
        itemButton.onClick.AddListener(ButtonCallback<ItemButtonClickEvent>);
        superButton.onClick.AddListener(ButtonCallback<SuperAttackButtonClickEvent>);
    }

    private void OnDisable()
    {
        attackButton.onClick.RemoveListener(ButtonCallback<AttackButtonClickEvent>);
        magicButton.onClick.RemoveListener(ButtonCallback<MagicButtonClickEvent>);
        itemButton.onClick.RemoveListener(ButtonCallback<ItemButtonClickEvent>);
        superButton.onClick.RemoveListener(ButtonCallback<SuperAttackButtonClickEvent>);
    }

    private void ButtonCallback<T>() where T : GameEvent, new()
    {
        EventManager.Trigger<T>(new T());
    }

    public void Initialized()
    {
        foreach(var button in buttons)
        {
            if (!initPosition.ContainsKey(button.name))
            {
                initPosition.Add(button.name, button.transform.position);
            }
            button.transform.position = startPoint.position;
        }
    }

    public void OpenAll()
    {
        Initialized();

        gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        foreach(var button in buttons)
        {
            Open(button);
        }
    }

    public void CloseAll()
    {
        foreach (var button in buttons)
        {
            Close(button);
        }
    }

    private void Open(Button button)
    {
        button.gameObject.SetActive(true);
        button.transform.DOMove(initPosition[button.name], animationSpeed);
    }

    private void Close(Button button)
    {
        button.transform.DOMove(startPoint.position, animationSpeed).OnComplete(() =>
        {
            button.gameObject.SetActive(false);
        });
    }
}

public class AttackButtonClickEvent : GameEvent
{

}

public class MagicButtonClickEvent : GameEvent
{

}

public class ItemButtonClickEvent : GameEvent
{

}

public class SuperAttackButtonClickEvent : GameEvent
{

}
