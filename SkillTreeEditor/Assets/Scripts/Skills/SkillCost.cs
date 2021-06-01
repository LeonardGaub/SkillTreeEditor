using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillCost
{
    [SerializeField] private int level;
    [SerializeField] private int cost;

    public SkillCost(int level, int cost)
    {
        this.level = level;
        this.cost = cost;
    }

    public int GetCost()
    {
        return cost;
    }

    public int GetLevel()
    {
        return level;
    }

    public void SetCost(int newCost)
    {
        cost = newCost;
    }
}
