using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextQuizHandler : QuizHandler
{
    [SerializeField] private List<TextButtonHandler> answerButtons;
    [SerializeField] private Image questionImage;
    [SerializeField] private Transform imageBG;
    private bool isAnswered;
    protected override void Start()
    {
        base.Start();
        SubscribeToButtons();
    }

    public override void StartQuiz()
    {
        question = questionManager.GetTextQuestion();
        questionText.text = question.Question;
        questionImage.sprite = questionManager.GetQuestionImage(question.CustomImageID);
        for (int i = 0; i < 4; i++)
        {
            answerButtons[i].SetValues(i, question.Answers[i].Text);
        }
        Appear();
    }

    private void Appear()
    {
        cg.alpha = 0;
        isAnswered = false;
        timer.HideTimer();
        imageBG.localScale = portrait.localScale = speechBubble.localScale = Vector3.zero;
        foreach (var element in answerButtons)
        {
            element.transform.localScale = Vector3.zero;
        }
        gameObject.SetActive(true);
        cg.DOFade(1, 0.3f).SetDelay(0.2f);
        portrait.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetDelay(0.7f);
        speechBubble.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetDelay(1.1f);
        imageBG.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetDelay(1.5f);
        foreach (var element in answerButtons)
        {
            element.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetDelay(Random.Range(0, 0.35f) + 1.9f);
        }
        DOVirtual.DelayedCall(2f, StartTimer);
    }

    private void StartTimer()
    {
        timer.StartTimer();
    }

    protected override void OnTimeRunOut()
    {
        OnAnswerButtonClicked(-1);
    }


    private void SubscribeToButtons()
    {
        foreach (var element in answerButtons)
        {
            element.OnTextButtonClicked += OnAnswerButtonClicked;
        }
    }

    private void EndTimer()
    {
        timer.StopTimer();
    }

    private void OnAnswerButtonClicked(int index)
    {
        if (isAnswered) return;
        isAnswered = true;
        EndTimer();
        bool isCorrect = index == question.CorrectAnswerIndex;
        if (!isCorrect && index != -1)
        {
            answerButtons[index].ShowFrame(false);
        }
        answerButtons[question.CorrectAnswerIndex].ShowFrame(true);
        ShowResultText(isCorrect);
        StartCoroutine(EndQuizAfterDelay(isCorrect));
    }
}
