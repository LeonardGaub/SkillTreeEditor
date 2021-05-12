using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeCondition
{
    private ConditionType type;

    public bool isFullfilled(SkillTreeConnection connection) 
    {
        switch (type)
        {
            case ConditionType.parentUnlocked:
                if (connection.GetParent().isUnlocked())
                {
                    return true;
                }
                return false;
        }
        return false;
    }

    public enum ConditionType
    {
        parentUnlocked,
        parentCertainLevel
    }
}