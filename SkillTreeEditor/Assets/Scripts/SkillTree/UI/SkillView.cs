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

    private static float xOffset = 10;

    [HideInInspector][SerializeField] private SkillTreeNode skill;
    [HideInInspector][SerializeField] private SkillTreeView view;

    private void Start()
    {
        skill.ResetLevel();
        DisplayLevelText();
        SkillTreeView.OnSkillUnlocked += SetUnlockable;
    }

    public SkillTreeNode GetSkill()
    {
        return skill;
    }
    public void SetUp(SkillTreeNode skill, SkillTreeView view)
    {
        this.skill = skill;
        this.view = view;
        GetComponent<RectTransform>().localPosition = new Vector3(skill.GetRect().x * 1.2f, -skill.GetRect().y * 1.2f, 0);
        skillNameText.text = skill.GetSkillName();
        skillIcon.sprite = skill.GetIcon();
        
        SetUnlockable();
        DisplayLevelText();
    }

    public Vector2 GetConnectStartPosition()
    {
        Vector2 pos = new Vector2();
        RectTransform rect = GetComponent<RectTransform>();
        pos.x = rect.localPosition.x + rect.rect.width - xOffset;
        pos.y = rect.localPosition.y - rect.rect.height / 2;

        return pos;
    }

    public Vector2 GetConnectEndPosition()
    {
        Vector2 pos = new Vector2();
        RectTransform rect = GetComponent<RectTransform>();
        pos.x = rect.localPosition.x + xOffset;
        pos.y = rect.localPosition.y - rect.rect.height / 2;

        return pos;
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
        view.UnlockSkill(skill);
        
        DisplayLevelText();
    }

    public void SetUnlockable()
    {
        GetComponent<Button>().interactable = view.GetTree().isUnlockable(skill);
    }

    private void OnDestroy()
    {
        SkillTreeView.OnSkillUnlocked -= SetUnlockable;
    }
}
