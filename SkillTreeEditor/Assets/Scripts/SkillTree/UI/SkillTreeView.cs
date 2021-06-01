using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeView : MonoBehaviour
{
    [SerializeField] SkillTree tree;
    [SerializeField] SkillView skillView;
    [SerializeField] SkillTreeLines line;
    [SerializeField] TextMeshProUGUI skillPointsView;
    [SerializeField] GameObject skillViewsPrefab;
    [SerializeField] GameObject skillLinesPrefab;

    [HideInInspector][SerializeField] GameObject skillViewsParent;
    [HideInInspector][SerializeField] GameObject skillLinesParent;

    
    [SerializeField] private int skillPoints;

    [HideInInspector][SerializeField] private List<SpawnedSkill> spawnedSkills = new List<SpawnedSkill>();

    public static Action OnSkillUnlocked;

    private void Awake()
    {
        foreach (var skill in spawnedSkills)
        {
            skill.ResetLevel();
        }
        UpdateSkillPointsView(skillPoints);

    }  
    public SkillTree GetTree()
    {
        if(tree == null) { return null;}
        return tree;
    }

    public void GenerateUI()
    {
        if(tree.GetNodes() == null) { return; }

        Reset();

        foreach (var skill in tree.GetNodes())
        {
            if (skill.GetConnections().Count == 0)
            {
                var newView = Instantiate(skillView, skillViewsParent.transform);
                newView.SetUp(skill, this);
                spawnedSkills.Add(new SpawnedSkill(skill, newView));
                BuildChildren(skill, skillViewsParent.transform);
            }
        }
        Canvas.ForceUpdateCanvases();
        BuildLines();
    }

    public void BuildLines()
    {
        ResetLines();
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
                newSkill = Instantiate(skillView, group);
                newSkill.SetUp(child, this);
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

    private void Reset()
    {
        ResetLines();
        spawnedSkills.Clear();
        ResetViews();
    }

    private void ResetLines()
    {
        skillLinesParent = GameObject.Find("SkillLinesParent(Clone)");
        if (skillLinesParent != null) { DestroyImmediate(skillLinesParent.gameObject); }
        skillLinesParent = Instantiate(skillLinesPrefab, transform);
        skillLinesParent.transform.SetSiblingIndex(0);
    }

    private void ResetViews()
    {
        skillViewsParent = GameObject.Find("SkillViewsParent(Clone)");
        if (skillViewsParent != null) { DestroyImmediate(skillViewsParent.gameObject); }
        skillViewsParent = Instantiate(skillViewsPrefab, transform);
        skillViewsParent.transform.SetSiblingIndex(1);
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
            OnSkillUnlocked?.Invoke();
            var tooltip = FindObjectOfType<SkillTooltip>();
            if (tooltip) { tooltip.SetUp(skill); }
        }
        else
        {
            Debug.LogError("Requirements not met");
        }
    }
}
