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
    [SerializeField] SkillTreeLines line;
    [SerializeField] TextMeshProUGUI skillPointsView;
    [SerializeField] GameObject skillViewsPrefab;
    [SerializeField] GameObject skillLinesPrefab;

    [HideInInspector] [SerializeField] GameObject skillViewsParent;
    [HideInInspector] [SerializeField] GameObject skillLinesParent;

    
    [SerializeField] private int skillPoints;

    [HideInInspector][SerializeField] private List<SpawnedSkill> spawnedSkills = new List<SpawnedSkill>();

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
                var newView = Instantiate(skillUI, skillViewsParent.transform);
                newView.SetUp(skill, tree);
                spawnedSkills.Add(new SpawnedSkill(skill, newView));
                BuildChildren(skill, skillViewsParent.transform);
            }
        }
        Canvas.ForceUpdateCanvases();
        BuildLines();
    }

    private void BuildLines()
    {
        foreach (var skill in spawnedSkills)
        {
            foreach (var child in tree.GetChildren(skill.skill))
            {
                var newLine = Instantiate(line, skillLinesParent.transform);
                newLine.Setup(skill.view.GetConnectStartPosition(), GetView(child).GetConnectEndPosition());
            }
        }
    }

    private void BuildChildren(SkillTreeNode parentSkillNode, Transform group)
    {
        if (parentSkillNode.GetChildren().Count == 0) { return; }
        
        foreach (var child in tree.GetChildren(parentSkillNode))
        {
            SkillView newSkill;
            if (!IsSpawned(child)) 
            {
                newSkill = Instantiate(skillUI, group);
                newSkill.SetUp(child, tree);
                spawnedSkills.Add(new SpawnedSkill(child, newSkill));
            }
        }
        foreach (var child in tree.GetChildren(parentSkillNode))
        {
            BuildChildren(child, group);
        }
        Canvas.ForceUpdateCanvases();
    }

    private bool IsSpawned(SkillTreeNode skill)
    {
        foreach(var spawnedSkill in spawnedSkills)
        {
            if(spawnedSkill.skill == skill)
            {
                return true;
            }
        }
        return false;
    }

    private SkillView GetView(SkillTreeNode skill)
    {
        foreach (var spawnedSkill in spawnedSkills)
        {
            if (spawnedSkill.skill == skill)
            {
                return spawnedSkill.view;
            }
        }
        return null;
    }

    private void ResetView()
    {
        if (skillViewsParent != null) { DestroyImmediate(skillViewsParent.gameObject); }
        if (skillLinesParent != null) { DestroyImmediate(skillLinesParent.gameObject); }

        spawnedSkills.Clear();

        skillLinesParent = Instantiate(skillLinesPrefab, transform);
        skillViewsParent = Instantiate(skillViewsPrefab, transform);
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
