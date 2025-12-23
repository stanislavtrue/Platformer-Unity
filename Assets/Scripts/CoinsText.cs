using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CoinsText : MonoBehaviour
{
    public static int coins;
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        coins = 0;
    }
    private void Update()
    {
        text.text = coins.ToString();
        WinGame();
    }

    private void WinGame()
    {
        if (coins == 20)
        {
            GameManager.Instance.WinGame();
        }
    }
}
