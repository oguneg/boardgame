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
    [SerializeField] private TextMeshProUGUI questionText;
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
        gameObject.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            flagButtons[i].SetValues(i, questionManager.GetFlag(question.Answers[i].ImageID));
        }
    }

    private void SubscribeToButtons()
    {
        foreach (var element in flagButtons)
        {
            element.OnFlagButtonClicked += OnAnswerButtonClicked;
        }
    }

    private void OnAnswerButtonClicked(int index)
    {
        OnQuizComplete?.Invoke(index==question.CorrectAnswerIndex);
    }
}
