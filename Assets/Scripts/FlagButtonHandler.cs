using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FlagButtonHandler : MonoBehaviour
{
    [SerializeField] private Image flagImage, frame;
    public UnityAction<int> OnFlagButtonClicked;
    private int index;

    public void SetValues(int index, Sprite flagSprite)
    {
        HideFrame();
        this.index = index;
        flagImage.sprite = flagSprite;
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
        OnFlagButtonClicked?.Invoke(index);
    }
}