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
        //Destroys old SkillTree
        foreach(Transform child in this.transform)
        {
            DestroyImmediate(child.gameObject);
        }

        if(tree.GetNodes() == null) { return; }


        foreach(var skill in tree.GetNodes())
        {
            if (skill.GetParents().Count == 0)
            {
                var root = Instantiate(treeParent, this.transform);
                var newSkill = Instantiate(skillUI, root);
                newSkill.SetUp(skill.GetSkillName(), skill.GetIcon());

                BuildChildren(skill, root);
            }
        }
        Canvas.ForceUpdateCanvases();
    }

    private void BuildChildren(SkillTreeNode skillNode, Transform group)
    {
        if (skillNode.GetChildren().Count == 0) { return; }
        var newGroup = Instantiate(skillGroup, group);
        foreach(var child in tree.GetChildren(skillNode))
        {
            var newSkill = Instantiate(skillUI, newGroup);
            newSkill.SetUp(child.GetSkillName(), child.GetIcon());
        }
        foreach (var child in tree.GetChildren(skillNode))
        {
            BuildChildren(child, group);
        }

    }
}
