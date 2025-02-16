using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
public class RollButtonHandler : MonoBehaviour
{
    public UnityAction OnButtonClicked;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI diceText;

    public void OnRollButton()
    {
        OnButtonClicked?.Invoke();
        SetButtonStatus(false);
    }

    public void SetDiceText(int dice)
    {
        diceText.text = dice.ToString();
        if (dice == 0)
        {
            diceText.text = "";
        }
    }

    public void SetButtonStatus(bool isActive)
    {
        button.interactable = isActive;
    }
}
