using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Events;

public class QuizTimer : MonoBehaviour
{
    [SerializeField] private Transform bg;
    [SerializeField] private TextMeshProUGUI timerText;
    private const int ANSWER_TIME = 10;
    private Coroutine timerRoutine;
    public UnityAction OnTimeRunOut;

    public void HideTimer()
    {
        bg.transform.localScale = Vector3.zero;
    }

    public void StartTimer()
    {
        timerRoutine = StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        int timer = ANSWER_TIME;
        timerText.text = timer.ToString();
        bg.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
        var wfs = new WaitForSeconds(1f);
        while (timer > 0)
        {
            yield return wfs;
            timer--;
            timerText.transform.DOScale(1.3f, 0.1f).SetEase(Ease.InOutBack).SetLoops(2, LoopType.Yoyo);
            timerText.text = timer.ToString();
        }
        OnTimeRunOut?.Invoke();
    }

    public void StopTimer()
    {
        StopCoroutine(timerRoutine);
        timerRoutine = null;
    }
}
