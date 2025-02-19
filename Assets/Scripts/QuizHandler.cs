using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public abstract class QuizHandler : MonoBehaviour
{
    [SerializeField] protected QuestionManager questionManager;
    [SerializeField] protected TextMeshProUGUI questionText;

    [SerializeField] protected Transform portrait, speechBubble;
    [SerializeField] protected CanvasGroup cg;
    [SerializeField] protected QuizTimer timer;

    protected QuizQuestion question;
    public UnityAction<bool> OnQuizComplete;

    protected virtual void Start()
    {
        timer.OnTimeRunOut += OnTimeRunOut;
    }

    protected void ShowResultText(bool isCorrect)
    {
        Sequence textSeq = DOTween.Sequence();
        textSeq.Append(speechBubble.transform.DOScale(0, 0.2f).SetEase(Ease.InBack));
        textSeq.AppendInterval(0.2f);
        textSeq.AppendCallback(() => questionText.text = isCorrect ? "Correct!" : "Wrong answer :(");
        textSeq.Append(speechBubble.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack));
    }

    protected IEnumerator EndQuizAfterDelay(bool isCorrect)
    {
        cg.DOFade(0, 0.2f).SetDelay(1.8f);
        yield return new WaitForSeconds(2f);
        OnQuizComplete?.Invoke(isCorrect);
    }


    protected abstract void OnTimeRunOut();
    public abstract void StartQuiz();
}
