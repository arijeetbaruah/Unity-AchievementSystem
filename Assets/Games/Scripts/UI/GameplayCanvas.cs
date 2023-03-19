using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using Game.Events;
using Unity.VisualScripting;

public class GameplayCanvas : MonoBehaviour
{
    [SerializeField]
    private Button attackButton;
    [SerializeField]
    private Button magicButton;
    [SerializeField]
    private Button itemButton;
    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private float animationSpeed = 1;

    private System.Collections.Generic.Dictionary<string, Vector3> initPosition = new System.Collections.Generic.Dictionary<string, Vector3>();

    private void Start()
    {
        attackButton.onClick.AddListener(() =>
        {
            Close(attackButton);
            Close(magicButton);
            Close(itemButton);
        });
    }

    private void OnEnable()
    {
        attackButton.onClick.AddListener(ButtonCallback<AttackButtonClickEvent>);
        magicButton.onClick.AddListener(ButtonCallback<MagicButtonClickEvent>);
        itemButton.onClick.AddListener(ButtonCallback<ItemButtonClickEvent>);
    }

    private void OnDisable()
    {
        attackButton.onClick.RemoveListener(ButtonCallback<AttackButtonClickEvent>);
        magicButton.onClick.RemoveListener(ButtonCallback<MagicButtonClickEvent>);
        itemButton.onClick.RemoveListener(ButtonCallback<ItemButtonClickEvent>);
    }

    private void ButtonCallback<T>() where T : GameEvent, new()
    {
        EventManager.Trigger<T>(new T());
    }

    public void OpenAll()
    {
        gameObject.SetActive(true);

        Open(attackButton);
        Open(magicButton);
        Open(itemButton);
    }

    public void CloseAll()
    {
        Close(attackButton);
        Close(magicButton);
        Close(itemButton);
    }

    private void Open(Button button)
    {
        if (!initPosition.ContainsKey(button.name))
        {
            initPosition.Add(button.name, button.transform.position);
        }

        DG.Tweening.Sequence openingSequence = DOTween.Sequence();
        button.transform.position = startPoint.position;

        openingSequence.Append(button.transform.DOMove(initPosition[button.name], animationSpeed));
    }

    private void Close(Button button)
    {
        if (!initPosition.ContainsKey(button.name))
        {
            initPosition.Add(button.name, button.transform.position);
        }

        button.transform.DOMove(startPoint.position, animationSpeed).OnComplete(() =>
        {
            gameObject.SetActive(false);
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
