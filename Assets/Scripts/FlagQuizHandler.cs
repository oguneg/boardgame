using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Events;

public class FlagQuizHandler : MonoBehaviour
{
    [SerializeField] private List<FlagButtonHandler> flagButtons;
    [SerializeField] private QuestionManager questionManager;
    [SerializeField] private TextMeshProUGUI questionText, timerText;

    [SerializeField] private Transform portrait, speechBubble, timerBG;
    [SerializeField] private CanvasGroup cg;

    private const int ANSWER_TIME = 10;

    private Coroutine timerRoutine;
    private QuizQuestion question;
    public UnityAction<bool> OnQuizComplete;
    private void Start()
    {
        SubscribeToButtons();
    }

    public void StartQuiz()
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
        timerBG.localScale = portrait.localScale = speechBubble.localScale = Vector3.zero;
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
        timerRoutine = StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        int timer = ANSWER_TIME;
        timerText.text = timer.ToString();
        timerBG.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
        var wfs = new WaitForSeconds(1f);
        while (timer > 0)
        {
            yield return wfs;
            timer--;
            timerText.transform.DOScale(1.3f, 0.1f).SetEase(Ease.InOutBack).SetLoops(2, LoopType.Yoyo);
            timerText.text = timer.ToString();
        }
        RunOutOfTime();
    }

    private void SubscribeToButtons()
    {
        foreach (var element in flagButtons)
        {
            element.OnFlagButtonClicked += OnAnswerButtonClicked;
        }
    }

    private void RunOutOfTime()
    {
        OnAnswerButtonClicked(-1);
    }

    private void EndTimer()
    {
        StopCoroutine(timerRoutine);
        timerRoutine = null;
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
        StartCoroutine(EndQuizAfterDelay(isCorrect));
    }

    private IEnumerator EndQuizAfterDelay(bool isCorrect)
    {
        cg.DOFade(0, 0.2f).SetDelay(1.8f);
        yield return new WaitForSeconds(2f);
        OnQuizComplete?.Invoke(isCorrect);
    }
}
