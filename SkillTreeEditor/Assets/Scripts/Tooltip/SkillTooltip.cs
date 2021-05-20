using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI cost;

    private SkillTreeNode skill;

    public void SetUp(SkillTreeNode skill)
    {
        this.skill = skill;
        title.text = skill.GetSkillName();
        description.text = "In work";
        cost.text = "Cost: " + skill.GetCosts()[skill.GetCurrentLevel()].GetCost().ToString();
    }

    public void OnButtonPress()
    {
        skill.UnlockNextLevel();
    }
}
