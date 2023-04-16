using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField]
    private Image bar;
    [SerializeField]
    private Image backBar;
    [SerializeField]
    private Color startColor;
    [SerializeField]
    private Color endColor;
    [SerializeField]
    private float lerpAt = 20;
    [SerializeField]
    private TextMeshProUGUI amountTxt;

    public void Awake()
    {
        bar.color = startColor;
    }

    public void SetHP(float currentValue, float maxValue)
    {
        float percent = currentValue / maxValue;
        bar.fillAmount = percent;
        backBar.DOFillAmount(percent, 2);
        amountTxt.SetText($"{currentValue}/{maxValue}");

        if (percent <= lerpAt)
        {
            bar.color = Color.Lerp(endColor, startColor, (percent - 0.1f) / lerpAt);
        }
    }
}
