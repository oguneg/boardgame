using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    private int coinAmount = 0;
    public void AddCurrency(int amount)
    {
        coinAmount+=amount;
        UpdateText();
    }

    private void UpdateText()
    {
        coinText.transform.DOKill();
        coinText.transform.localScale = Vector3.one;
        coinText.text = $"{coinAmount}";
        coinText.transform.DOScale(1.2f,0.1f).SetEase(Ease.InOutBack).SetLoops(2,LoopType.Yoyo);
    }
}
