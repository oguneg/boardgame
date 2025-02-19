using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class TextButtonHandler : MonoBehaviour
{
    [SerializeField] private Image frame;
    [SerializeField] private TextMeshProUGUI answerText;
    public UnityAction<int> OnTextButtonClicked;
    private int index;

    public void SetValues(int index, string answer)
    {
        HideFrame();
        answerText.text = answer;
        this.index = index;
    }

    private void HideFrame()
    {
        frame.gameObject.SetActive(false);
    }

    public void ShowFrame(bool isCorrect)
    {
        frame.gameObject.SetActive(true);
        frame.color = isCorrect ? Color.white : Color.red;
    }

    public void OnButtonClicked()
    {
        OnTextButtonClicked?.Invoke(index);
    }
}