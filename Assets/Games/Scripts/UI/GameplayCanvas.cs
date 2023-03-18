using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using Game.Events;

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
        Vector3 initPos = button.transform.position;
        Sequence openingSequence = DOTween.Sequence();
        button.transform.position = startPoint.position;

        openingSequence.Append(button.transform.DOMove(initPos, animationSpeed));
    }

    private void Close(Button button)
    {
        Vector3 initPos = button.transform.position;
        button.transform.DOMove(startPoint.position, animationSpeed).OnComplete(() =>
        {
            button.transform.position = initPos;
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
