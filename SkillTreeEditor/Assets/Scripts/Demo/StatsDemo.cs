using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsDemo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI strText;
    [SerializeField] private TextMeshProUGUI intText;
    [SerializeField] private TextMeshProUGUI dexText;

    private void Awake()
    {
        PlayerDemo.OnStatsUpdated += UpdateStats;
    }

    private void UpdateStats(int str, int dex, int inte)
    {
        strText.text = "Strength: " + str.ToString();
        dexText.text = "Dexterity: " + dex.ToString();
        intText.text = "Intelligence: " + inte.ToString();
    }
}
