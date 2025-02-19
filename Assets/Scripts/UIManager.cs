using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private RollButtonHandler rollButton;
    [SerializeField] private QuizHandler flagQuizHandler, textQuizHandler;
    private QuizHandler activeQuiz;
    private void Start()
    {
        rollButton.OnButtonClicked += OnRollButton;
        flagQuizHandler.OnQuizComplete += OnQuizComplete;
        textQuizHandler.OnQuizComplete += OnQuizComplete;
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
        activeQuiz = flagQuizHandler;
    }

    public void StartTextQuiz()
    {
        textQuizHandler.StartQuiz();
        activeQuiz = textQuizHandler; ;
    }

    public void OnQuizComplete(bool isSuccess)
    {
        activeQuiz.gameObject.SetActive(false);
        activeQuiz = null;
        gameManager.CompleteQuiz(isSuccess);
    }

    public void UpdateDiceText(int remainingSteps)
    {
        rollButton.SetDiceText(remainingSteps);
    }
}
