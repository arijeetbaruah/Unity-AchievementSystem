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

public class AIHPHUD : MonoBehaviour
{
    [SerializeField]
    private CharacterDetails characterDetails;
    [SerializeField]
    private Image bg;
    [SerializeField]
    private TextMeshProUGUI playerName;
    [SerializeField]
    private HPBar hpBar;
    [SerializeField]
    private Color lowHPColor;
    [SerializeField]
    private Color normalHPColor;
    [SerializeField]
    private AssetReference statusIcon;
    [SerializeField]
    private Transform statusContent;

    private Dictionary<CombatStatus, GameObject> activeStatus = new Dictionary<CombatStatus, GameObject>();

    private void Start()
    {
        hpBar?.SetHP(characterDetails.currentHP, characterDetails.Stats.Stats.maxHP);
        playerName.SetText(characterDetails.characterID);
    }

    private void OnEnable()
    {
        ServiceRegistry.Get<EventManager>().AddListener<PlayerUpdateHP>(UpdatePlayerHP);
        ServiceRegistry.Get<EventManager>().AddListener<UpdateStatusEffect>(UpdateStatusEffect);
    }

    public void UpdateStatusEffect(UpdateStatusEffect statusEffect)
    {
        if (statusEffect.characterID == characterDetails.characterID)
        {
            UpdateStatusIcon();
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
            }
            else
            {
                bg.color = normalHPColor;
            }
        }
    }

    public void UpdateStatusIcon()
    {
        var dic = StatusIconSO.Instance.statusIcons.ToDictionary(s => s.status, s => s.texture);

        CombatStatus[] statues = (CombatStatus[])System.Enum.GetValues(typeof(CombatStatus));

        foreach (CombatStatus statue in statues)
        {
            if (statue == CombatStatus.None)
            {
                continue;
            }

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
}
