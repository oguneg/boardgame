using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CurrencyManager currencyManager;
    [SerializeField] private PlayerController player;
    [SerializeField] private TrackManager trackManager;
    private const string levelIndexSaveKey = "BoardGame.Level";
    private int levelIndex;
    int moves = 0;

    private void Start()
    {
        levelIndex = PlayerPrefs.GetInt(levelIndexSaveKey);
        trackManager.LoadTrack(levelIndex);
        player.OnMovementEnded += OnTileLanded;
        player.OnMoved += OnPlayerMoved;
    }

    public void Roll()
    {
        moves = Random.Range(1, 7);
        uiManager.UpdateDiceText(moves);
        player.Move(moves);
    }

    private void CompleteTurn()
    {
        uiManager.EnableRoll();
    }

    private void OnPlayerMoved(int remainingSteps)
    {
        uiManager.UpdateDiceText(remainingSteps);
    }

    private void OnTileLanded(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.Empty:
            case TileType.Start:
                CompleteTurn();
                break;
            case TileType.Minigame:
                TriggerMinigame();
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) TriggerMinigame();
    }

    private void TriggerMinigame()
    {
        if (Random.value > 0.5f)
        {
            uiManager.StartFlagQuiz();
        }
        else
        {
            uiManager.StartTextQuiz();
        }
    }

    public void CompleteQuiz(bool isSuccess)
    {
        if (isSuccess)
        {
            currencyManager.AddCurrency(100);
        }
        CompleteTurn();
    }

    public void NextLevel()
    {
        PlayerPrefs.SetInt(levelIndexSaveKey, levelIndex + 1);
        SceneHandler.Reload();
    }
}