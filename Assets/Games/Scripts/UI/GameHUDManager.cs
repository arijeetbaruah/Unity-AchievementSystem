using Game.Events;
using Game.Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHUDManager : MonoBehaviour
{
    [SerializeField]
    private PlayerHPHUD playerHPHUDPrefab;
    [SerializeField]
    private Transform content;

    private Dictionary<string, PlayerHPHUD> playerHPDictionary = new Dictionary<string, PlayerHPHUD>();

    private void OnEnable()
    {
        playerHPDictionary = new Dictionary<string, PlayerHPHUD>();

        ServiceRegistry.Get<EventManager>().AddListener<CreatePlayerHUD>(CreatePlayerHUD);
    }

    private void OnDisable()
    {
        ServiceRegistry.Get<EventManager>().RemoveListener<CreatePlayerHUD>(CreatePlayerHUD);
    }

    private void CreatePlayerHUD(CreatePlayerHUD createPlayerHUD)
    {
        CreateHUD(createPlayerHUD.characterDetails);
    }

    public void CreateHUD(CharacterDetails characterDetails)
    {
        PlayerHPHUD newHUD = Instantiate(playerHPHUDPrefab, content);
        newHUD.SetCharacterStats(characterDetails);

        playerHPDictionary.Add(characterDetails.characterID, newHUD);
    }

    public void RemoveHUD(string characterID)
    {
        if (playerHPDictionary.TryGetValue(characterID, out PlayerHPHUD playerHPHUD))
        {
            Destroy(playerHPHUD.gameObject);
            playerHPDictionary.Remove(characterID);
        }
    }
}
