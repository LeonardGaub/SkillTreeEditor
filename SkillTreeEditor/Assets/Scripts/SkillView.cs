using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private Image skillIcon;

    public void SetUp(string name, Sprite icon, Vector3 position, bool unlockable)
    {
        GetComponent<RectTransform>().position = position;
        skillNameText.text = name;
        skillIcon.sprite = icon;

        //Testing
        GetComponent<Button>().enabled = unlockable;
    }
}
