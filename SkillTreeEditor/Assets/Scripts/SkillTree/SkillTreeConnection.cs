using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillTreeConnection : ScriptableObject
{
    [HideInInspector][SerializeField] private SkillTreeNode parent;
    [HideInInspector][SerializeField] private SkillTreeNode child;
    [HideInInspector][SerializeField] private List<SkillTreeCondition> conditions = new List<SkillTreeCondition>();

    public SkillTreeConnection(SkillTreeNode parent, SkillTreeNode child)
    {
        this.parent = parent;
        this.child = child;
        SkillTreeNode.OnSkillUpdated += RefreshName;
    }

    public SkillTreeNode GetChild()
    {
        return child;
    }

    public SkillTreeNode GetParent()
    {
        return parent;
    }

    public List<SkillTreeCondition> GetConditions()
    {
        return conditions;
    }

    public void AddNewCondition()
    {
        conditions.Add(new SkillTreeCondition());
    }

    public void RemoveCondition(int index)
    {
        conditions.RemoveAt(index);
    }

    public bool isFullfilled()
    {
        foreach(var condition in conditions)
        {
            if (!condition.isFullfilled(this))
            {
                return false;
            }
        }
        return true;
    }

    public void RemoveAction()
    {
        SkillTreeNode.OnSkillUpdated -= RefreshName;
    }

    public void RefreshName()
    {
        this.name = parent.GetSkillName() + "-" + child.GetSkillName();
    }
}