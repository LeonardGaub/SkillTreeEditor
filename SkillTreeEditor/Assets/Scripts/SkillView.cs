using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] private Image skillIcon;

    [HideInInspector][SerializeField] private SkillTreeNode skill;

    private void Start()
    {
        DisplayLevelText();
    }

    public SkillTreeNode GetSkill()
    {
        return skill;
    }
    public void SetUp(SkillTreeNode skill)
    {
        this.skill = skill;
        GetComponent<RectTransform>().localPosition = new Vector3(skill.GetRect().x, -skill.GetRect().y, 0);
        skillNameText.text = skill.GetSkillName();
        skillIcon.sprite = skill.GetIcon();
        skill.ResetLevel();
        DisplayLevelText();
    }

    private void DisplayLevelText()
    {
        int currentLevel = skill.GetCurrentLevel();

        if(IsMaxLevel(currentLevel))
        {
            levelText.text = currentLevel.ToString() + "(Max)";
            GetComponent<Button>().enabled = false;
            return;
        }

        levelText.text = currentLevel.ToString() + "/" + skill.GetMaxLevel().ToString();
    }

    private bool IsMaxLevel(int current)
    {
        if(current >= skill.GetMaxLevel())
        {
            return true;
        }
        return false;
    }

    public void OnButtonPress()
    {
        if(IsMaxLevel(skill.GetCurrentLevel())) { return; }
        FindObjectOfType<SkillTreeView>().UnlockSkill(skill);
        DisplayLevelText();
    }
}
