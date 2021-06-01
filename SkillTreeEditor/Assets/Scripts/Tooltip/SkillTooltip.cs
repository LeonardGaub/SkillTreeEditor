using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI requirements;
    [SerializeField] TextMeshProUGUI cost;
    [SerializeField] TextMeshProUGUI effects;
    [SerializeField] GameObject requirementsParent;


    private SkillTreeNode skill;

    public void SetUp(SkillTreeNode skill)
    {
        this.skill = skill;
        title.text = skill.GetSkillName();
        description.text = skill.GetSkillDescription();
        effects.text = skill.GetSkill().GetEffect();
        DisplayRequirements();
        DisplayCostText();
    }

    private void DisplayRequirements()
    {
        requirementsParent.SetActive(true);
        requirements.text = "";
        foreach(var connection in skill.GetConnections())
        {
            if (!connection.isFullfilled())
            {
                foreach(var condition in connection.GetConditions())
                {
                    requirements.text += condition.GetRequirementsText(connection.GetParent()) + "\n";
                }
            }
        }
        if(requirements.text == "")
        {
            requirementsParent.SetActive(false);
        }
    }

    private void DisplayCostText()
    {
        if(skill.GetCosts().Count > skill.GetCurrentLevel())
        {
            cost.text = "Cost: " + skill.GetCosts()[skill.GetCurrentLevel()].GetCost().ToString();
        }
        else
        {
            cost.text = "Max Level";
        }
    }
}
