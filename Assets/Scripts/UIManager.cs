using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private RollButtonHandler rollButton;
    [SerializeField] private FlagQuizHandler flagQuizHandler;
    private void Start()
    {
        rollButton.OnButtonClicked += OnRollButton;
        flagQuizHandler.OnQuizComplete+=OnQuizComplete;
    }

    private void OnRollButton()
    {
        gameManager.Roll();
    }

    public void EnableRoll()
    {
        rollButton.SetDiceText(0);
        rollButton.SetButtonStatus(true);
    }

    public void StartFlagQuiz()
    {
        flagQuizHandler.StartQuiz();
    }

    public void OnQuizComplete(bool isSuccess)
    {
        flagQuizHandler.gameObject.SetActive(false);
        gameManager.CompleteQuiz();
    }

    public void UpdateDiceText(int remainingSteps)
    {
        rollButton.SetDiceText(remainingSteps);
    }
}
