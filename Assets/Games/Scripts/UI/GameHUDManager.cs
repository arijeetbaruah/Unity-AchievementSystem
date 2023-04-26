using DG.Tweening;
using Game.Events;
using Game.Service;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameHUDManager : MonoBehaviour
{
    [SerializeField]
    private PlayerHPHUD playerHPHUDPrefab;
    [SerializeField]
    private Transform content;
    [SerializeField]
    private Transform critTxt;
    [SerializeField]
    private Transform oneMoreTxt;
    [SerializeField]
    private TextMeshProUGUI youWinTxt;

    private Dictionary<string, PlayerHPHUD> playerHPDictionary = new Dictionary<string, PlayerHPHUD>();
    private bool isShowingTxt = false;

    private void OnEnable()
    {
        playerHPDictionary = new Dictionary<string, PlayerHPHUD>();

        ServiceRegistry.Get<EventManager>().AddListener<CreatePlayerHUD>(CreatePlayerHUD);
        ServiceRegistry.Get<EventManager>().AddListener<CritEvent>(EnableCritTxt);
        ServiceRegistry.Get<EventManager>().AddListener<OneMoreEvent>(EnableOneMoreTxt);
        ServiceRegistry.Get<EventManager>().AddListener<IsShowingTextEvent>(IsShowingTextEvent);
        ServiceRegistry.Get<EventManager>().AddListener<GameOverEvents>(GameOverEvents);
    }

    private void OnDisable()
    {
        ServiceRegistry.Get<EventManager>().RemoveListener<CreatePlayerHUD>(CreatePlayerHUD);
        ServiceRegistry.Get<EventManager>().RemoveListener<CritEvent>(EnableCritTxt);
        ServiceRegistry.Get<EventManager>().RemoveListener<OneMoreEvent>(EnableOneMoreTxt);
        ServiceRegistry.Get<EventManager>().RemoveListener<IsShowingTextEvent>(IsShowingTextEvent);
        ServiceRegistry.Get<EventManager>().RemoveListener<GameOverEvents>(GameOverEvents);
    }

    private void CreatePlayerHUD(CreatePlayerHUD createPlayerHUD)
    {
        CreateHUD(createPlayerHUD.characterDetails);
    }

    private void GameOverEvents(GameOverEvents @event)
    {
        youWinTxt.gameObject.SetActive(true);
        youWinTxt.SetText(@event.txt);
    }

    public void IsShowingTextEvent(IsShowingTextEvent @event)
    {
        @event.callback.Invoke(isShowingTxt);
    }

    public void EnableCritTxt(CritEvent @event)
    {
        critTxt.gameObject.SetActive(true);
        isShowingTxt = true;

        Sequence critSequence = DOTween.Sequence();
        critSequence.AppendInterval(2);
        critSequence.AppendCallback(() =>
        {
            critTxt.gameObject.SetActive(false);
        });
        critSequence.AppendInterval(2);
        critSequence.AppendCallback(() =>
        {
            isShowingTxt = false;
        });
    }

    public void EnableOneMoreTxt(OneMoreEvent @event)
    {
        oneMoreTxt.gameObject.SetActive(true);
        isShowingTxt = true;

        Sequence critSequence = DOTween.Sequence();
        critSequence.AppendInterval(2);
        critSequence.AppendCallback(() =>
        {
            oneMoreTxt.gameObject.SetActive(false);
        });
        critSequence.AppendInterval(2);
        critSequence.AppendCallback(() =>
        {
            isShowingTxt = false;
        });
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
