using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CurrencyManager currencyManager;
    [SerializeField] private PlayerController player;
    int moves = 0;

    private void Start()
    {
        player.OnMovementEnded += OnTileLanded;
        player.OnMoved+=OnPlayerMoved;
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

    private void TriggerMinigame()
    {
        uiManager.StartFlagQuiz();
    }

    public void CompleteQuiz(bool isSuccess)
    {
        if(isSuccess)
        {
            currencyManager.AddCurrency(100);
        }
        CompleteTurn();
    }
}