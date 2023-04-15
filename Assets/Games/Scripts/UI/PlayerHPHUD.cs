using DG.Tweening;
using Game.Events;
using Game.Service;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.IO.Archive;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class PlayerHPHUD : MonoBehaviour
{
    private CharacterDetails characterDetails;

    [SerializeField]
    private TextMeshProUGUI playerName;
    [SerializeField]
    private Image bg;
    [SerializeField]
    private Image chargeImg;
    [SerializeField]
    private HPBar hpBar;
    [SerializeField]
    private HPBar manaBar;
    [SerializeField]
    private HPBar maxBar;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Color lowHPColor;
    [SerializeField]
    private Color normalHPColor;
    [SerializeField]
    private AssetReference statusIcon;
    [SerializeField]
    private Transform statusContent;

    private Dictionary<CombatStatus, GameObject> activeStatus;

    private void OnEnable()
    {
        ServiceRegistry.Get<EventManager>().AddListener<PlayerUpdateHP>(UpdatePlayerHP);
        ServiceRegistry.Get<EventManager>().AddListener<PlayerUpdateMana>(UpdatePlayerMana);
        ServiceRegistry.Get<EventManager>().AddListener<PlayerUpdateCharge>(UpdatePlayerCharge);
        ServiceRegistry.Get<EventManager>().AddListener<ResetPlayerCharge>(ResetPlayerCharge);
        ServiceRegistry.Get<EventManager>().AddListener<UpdateStatusEffect>(UpdateStatusEffect);
    }

    public void SetCharacterStats(CharacterDetails characterDetails)
    {
        this.characterDetails = characterDetails;
        var characterStats = characterDetails.Stats;

        playerName.SetText(characterDetails.characterID);
        hpBar.SetHP(characterDetails.currentHP, characterStats.Stats.maxHP);
        manaBar.SetHP(characterDetails.currentMana, characterStats.Stats.maxMana);
        maxBar.SetHP(characterDetails.currentMax, characterStats.Stats.maxCharge);
    }

    public void UpdateStatusEffect(UpdateStatusEffect statusEffect)
    {
        if (statusEffect.characterID == characterDetails.characterID)
        {
            UpdateStatusIcon();
        }
    }

    public void UpdateStatusIcon()
    {
        var dic = StatusIconSO.Instance.statusIcons.ToDictionary(s => s.status, s => s.texture);

        CombatStatus[] statues = (CombatStatus[]) System.Enum.GetValues(typeof(CombatStatus));

        foreach (CombatStatus statue in statues)
        {
            if (characterDetails.StatusEffect.Contains(statue))
            {
                if (dic.TryGetValue(statue, out var icon))
                {
                    if (activeStatus.TryGetValue(statue, out GameObject iconGO))
                    {
                        iconGO.SetActive(true);
                    }
                    else
                    {
                        statusIcon.InstantiateAsync(statusContent).Completed += handler2 =>
                        {
                            Image img = handler2.Result.GetComponentInChildren<Image>();
                            activeStatus.Add(statue, handler2.Result);
                            img.sprite = icon;
                        };
                    }
                }
            }
            else if (activeStatus.TryGetValue(statue, out GameObject icon))
            { 
                icon.gameObject.SetActive(false);
            }
        }
    }

    public void ResetPlayerCharge(ResetPlayerCharge resetPlayerCharge)
    {
        if (resetPlayerCharge.playerID == characterDetails.characterID)
        {
            maxBar.SetHP(0, characterDetails.Stats.Stats.maxCharge);
        }
    }

    public void UpdatePlayerCharge(PlayerUpdateCharge playerUpdateCharge)
    {
        if (playerUpdateCharge.playerID == characterDetails.characterID)
        {
            maxBar.SetHP(playerUpdateCharge.amount, characterDetails.Stats.Stats.maxCharge);

            if (playerUpdateCharge.amount == characterDetails.Stats.Stats.maxCharge)
            {
                Sequence sequence = DOTween.Sequence();
                sequence.Append(chargeImg.DOColor(Color.red, 1));
                sequence.Append(chargeImg.DOColor(Color.blue, 1));
                sequence.Append(chargeImg.DOColor(Color.green, 1));
                sequence.Append(chargeImg.DOColor(Color.yellow, 1));
                sequence.SetLoops(99);
            }
        }
    }

    public void UpdatePlayerHP(PlayerUpdateHP playerUpdateHP)
    {
        if (playerUpdateHP.playerID == characterDetails.characterID)
        {
            hpBar.SetHP(playerUpdateHP.amount, characterDetails.Stats.Stats.maxHP);

            if (playerUpdateHP.amount == 0)
            {
                bg.color = lowHPColor;
                canvasGroup.alpha = 0.5f;
            }
            else
            {
                bg.color = normalHPColor;
                canvasGroup.alpha = 1f;
            }
        }
    }

    public void UpdatePlayerMana(PlayerUpdateMana playerUpdateMana)
    {
        if (playerUpdateMana.playerID == characterDetails.characterID)
        {
            manaBar.SetHP(playerUpdateMana.amount, characterDetails.Stats.Stats.maxHP);
        }
    }
}
