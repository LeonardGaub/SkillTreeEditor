using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private Image skillIcon;

    public void SetUp(string name, Sprite icon)
    {
        skillNameText.text = name;
        Debug.Log(icon);
        skillIcon.sprite = icon;
    }
}
