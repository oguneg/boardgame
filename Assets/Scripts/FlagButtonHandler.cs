using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FlagButtonHandler : MonoBehaviour
{
    [SerializeField] private Image flagImage;
    public UnityAction<int> OnFlagButtonClicked;
    private int index;

    public void SetValues(int index, Sprite flagSprite)
    {
        this.index = index;
        flagImage.sprite = flagSprite;
    }

    public void OnButtonClicked()
    {
        OnFlagButtonClicked?.Invoke(index);
    }
}