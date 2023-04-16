using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaidalMenu : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> buttons;

    [SerializeField]
    private float radius;
    [SerializeField]
    private float duration = 1;

    private float currentRadius;

    private void Rearrange()
    {
        float radiansOfSeperation = (Mathf.PI * 2) / buttons.Count;

        for (int i = 0; i < buttons.Count; i++)
        {
            float x = Mathf.Sin(radiansOfSeperation * i) * currentRadius;
            float y = Mathf.Cos(radiansOfSeperation * i) * currentRadius;

            buttons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);

        DOTween.To(val => currentRadius = val, 0, radius, duration)
            .OnUpdate(() =>
            {
                Rearrange();
            });
    }

    public void Close()
    {
        DOTween.To(val => currentRadius = val, radius, 0, duration)
            .OnUpdate(() =>
            {
                Rearrange();
            })
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }

    private void OnValidate()
    {
        currentRadius = radius;
        Rearrange();
    }
}
