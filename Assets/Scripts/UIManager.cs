using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private RollButtonHandler rollButton;
    private void Start()
    {
        rollButton.OnButtonClicked += OnRollButton;
    }

    private void OnRollButton()
    {
        gameManager.Roll();
    }

    public void EnableRoll()
    {
        rollButton.SetDiceText(0);
        rollButton.SetButtonStatus(true);
    }
}
