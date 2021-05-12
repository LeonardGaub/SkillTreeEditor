using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeView : MonoBehaviour
{
    [SerializeField] SkillTree tree;
    [SerializeField] SkillView skillUI;
    [SerializeField] RectTransform treeParent;
    [SerializeField] RectTransform skillGroup;

    public void GenerateUI()
    {
        if(tree.GetNodes() == null) { return; }

        foreach(var skill in tree.GetNodes())
        {
            if (skill.GetConnections().Count == 0)
            {
                var newSkill = Instantiate(skillUI, this.transform);
                newSkill.SetUp(skill.GetSkillName(), skill.GetIcon(), new Vector3(skill.GetRect().x, skill.GetRect().y, 0), true);
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
            var newSkill = Instantiate(skillUI, group);
            newSkill.SetUp(child.GetSkillName(), child.GetIcon(), new Vector3(child.GetRect().x, child.GetRect().y, 0), tree.isUnlockable(child));
        }
        foreach (var child in tree.GetChildren(skillNode))
        {
            BuildChildren(child, group);
        }
        Canvas.ForceUpdateCanvases();
    }
}
