using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeView : MonoBehaviour
{
    [SerializeField] SkillTree tree;
    [SerializeField] SkillView skillUI;
    [SerializeField] TextMeshProUGUI skillPointsView;
    [SerializeField] private int skillPoints;

    [HideInInspector][SerializeField] private List<SkillTreeNode> spawnedSkills = new List<SkillTreeNode>();

    private void Awake()
    {
        foreach (var skill in spawnedSkills)
        {
            skill.ResetLevel();
        }
        UpdateSkillPointsView(skillPoints);
    }  
    
    public void GenerateUI()
    {
        if(tree.GetNodes() == null) { return; }

        ResetView();

        foreach (var skill in tree.GetNodes())
        {
            if (skill.GetConnections().Count == 0)
            {
                var newSkill = Instantiate(skillUI, this.transform);
                newSkill.SetUp(skill);
                spawnedSkills.Add(skill);
                BuildChildren(skill, this.transform);
            }
        }
        Canvas.ForceUpdateCanvases();
    }

    private void BuildChildren(SkillTreeNode skillNode, Transform group)
    {
        if (skillNode.GetChildren().Count == 0) { return; }
        
        foreach (var child in tree.GetChildren(skillNode))
        {
            if (spawnedSkills.Contains(child)) { continue; }
            
            var newSkill = Instantiate(skillUI, group);
            spawnedSkills.Add(child);
            newSkill.SetUp(child);
        }
        foreach (var child in tree.GetChildren(skillNode))
        {
            BuildChildren(child, group);
        }
        Canvas.ForceUpdateCanvases();
    }

    private void ResetView()
    {
        spawnedSkills.Clear();
    }

    private void UpdateSkillPointsView(int skillPoints)
    {
        skillPointsView.text = "Skillpoints : " + skillPoints;
    }

    public void UnlockSkill(SkillTreeNode skill)
    {
        if (tree.isUnlockable(skill))
        {
            var nextLevelCost = skill.GetCosts()[skill.GetCurrentLevel()].GetCost();
            if (skillPoints < nextLevelCost)
            {
                Debug.Log("Not enough Skill Points");
                return;
            }
            skillPoints -= nextLevelCost;
            UpdateSkillPointsView(skillPoints);

            tree.UnlockSkill(skill);
        }
        else
        {
            Debug.LogError("Requirements not met");
        }
    }
}
