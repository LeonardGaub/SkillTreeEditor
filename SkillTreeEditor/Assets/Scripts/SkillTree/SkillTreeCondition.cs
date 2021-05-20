using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillTreeCondition
{
    [SerializeField] private ConditionType type;
    [SerializeField] private int requiredLevel;

    public ConditionType GetConditionType()
    {
        return type;
    }

    public int GetRequiredLevel()
    {
        return requiredLevel;
    }

    public bool isFullfilled(SkillTreeConnection connection) 
    {
        switch (type)
        {
            case ConditionType.parentUnlocked:
                if (connection.GetParent().IsUnlocked())
                {
                    return true;
                }
                return false;
            case ConditionType.parentCertainLevel:
                if (connection.GetParent().isCertainLevelUnlocked(2))
                {
                    return true;
                }
                return false;
        }
        return false;
    }

    public void SetRequiredLevel(int level)
    {
        requiredLevel = level;
    }

    public void SetType(Enum type)
    {
        this.type = (ConditionType)type;
    }

    public enum ConditionType
    {
        parentUnlocked,
        parentCertainLevel
    }
}