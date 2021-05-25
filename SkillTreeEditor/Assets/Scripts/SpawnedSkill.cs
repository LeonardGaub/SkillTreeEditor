using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpawnedSkill
{
    [SerializeField] public SkillTreeNode skill;
    [SerializeField] public SkillView view;

    public SpawnedSkill(SkillTreeNode skill, SkillView view)
    {
        this.skill = skill;
        this.view = view;
    }

    public void ResetLevel()
    {
        if (skill == null) { return; }
        skill.ResetLevel();
    }
}
