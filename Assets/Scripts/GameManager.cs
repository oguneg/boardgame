using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerController player;
    int moves = 0;

    private void Start()
    {
        player.OnMovementEnded += OnTileLanded;
    }

    public void Roll()
    {
        moves = Random.Range(1, 7);
        player.Move(moves);
    }

    private void CompleteTurn()
    {
        uiManager.EnableRoll();
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

    public void CompleteQuiz()
    {
        CompleteTurn();
    }
}