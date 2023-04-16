using UnityEngine;
using UnityEngine.UI;

public class ItemTabButton : MonoBehaviour
{
    public ItemCategory category;

    [SerializeField]
    private Image bg;
    [SerializeField]
    private Button btn;
    [SerializeField]
    private Color activeColor;
    [SerializeField]
    private Color inactiveColor;

    public System.Action<ItemCategory> OnClick;

    private bool isActive;
    public bool IsActive {
        get => isActive;
        set
        {
            isActive = value;
            UpdateBtn();
        }
    }

    public void Start()
    {
        btn.onClick.AddListener(() => OnClick?.Invoke(category));
        UpdateBtn();
    }

    public void UpdateBtn()
    {
        bg.color = isActive ? activeColor : inactiveColor;
    }
}
