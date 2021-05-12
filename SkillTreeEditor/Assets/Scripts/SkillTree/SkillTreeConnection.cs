using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillTreeConnection
{
    [SerializeField] private SkillTreeNode parent;
    [SerializeField] private SkillTreeNode child;
    private List<SkillTreeCondition> conditions = new List<SkillTreeCondition>();


    public SkillTreeConnection(SkillTreeNode parent, SkillTreeNode child)
    {
        this.parent = parent;
        this.child = child;
    }

    public SkillTreeNode GetChild()
    {
        return child;
    }

    public SkillTreeNode GetParent()
    {
        return parent;
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
}