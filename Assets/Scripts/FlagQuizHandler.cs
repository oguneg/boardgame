using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Events;

public class FlagQuizHandler : QuizHandler
{
    [SerializeField] private List<FlagButtonHandler> flagButtons;

    protected override void Start()
    {
        base.Start();
        SubscribeToButtons();
    }

    public override void StartQuiz()
    {
        question = questionManager.GetFlagQuestion();
        questionText.text = question.Question;
        for (int i = 0; i < 4; i++)
        {
            flagButtons[i].SetValues(i, questionManager.GetFlag(question.Answers[i].ImageID));
        }
        Appear();
    }

    private void Appear()
    {
        cg.alpha = 0;
        timer.HideTimer();
        portrait.localScale = speechBubble.localScale = Vector3.zero;
        foreach (var element in flagButtons)
        {
            element.transform.localScale = Vector3.zero;
        }
        gameObject.SetActive(true);
        cg.DOFade(1, 0.3f).SetDelay(0.2f);
        portrait.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetDelay(0.7f);
        speechBubble.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetDelay(1.1f);
        foreach (var element in flagButtons)
        {
            element.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetDelay(Random.Range(0, 0.35f) + 1.7f);
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
        foreach (var element in flagButtons)
        {
            element.OnFlagButtonClicked += OnAnswerButtonClicked;
        }
    }

    private void EndTimer()
    {
        timer.StopTimer();
    }

    private void OnAnswerButtonClicked(int index)
    {
        EndTimer();
        bool isCorrect = index == question.CorrectAnswerIndex;
        if (!isCorrect && index != -1)
        {
            flagButtons[index].ShowFrame(false);
        }
        flagButtons[question.CorrectAnswerIndex].ShowFrame(true);
        ShowResultText(isCorrect);
        StartCoroutine(EndQuizAfterDelay(isCorrect));
    }
}
