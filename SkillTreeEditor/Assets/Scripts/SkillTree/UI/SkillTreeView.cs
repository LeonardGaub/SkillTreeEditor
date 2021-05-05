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
            if (skill.GetParents().Count == 0)
            {
                //var root = Instantiate(treeParent, this.transform);
                //root.transform.position = Vector2.zero;
                var newSkill = Instantiate(skillUI, this.transform);
                newSkill.GetComponent<RectTransform>().position = new Vector3(skill.GetRect().x, skill.GetRect().y, 0);
                newSkill.SetUp(skill.GetSkillName(), skill.GetIcon());

                BuildChildren(skill, this.transform);
            }
        }
        Canvas.ForceUpdateCanvases();
    }

    private void BuildChildren(SkillTreeNode skillNode, Transform group)
    {
        if (skillNode.GetChildren().Count == 0) { return; }
        foreach(var child in tree.GetChildren(skillNode))
        {
            var newSkill = Instantiate(skillUI, group);
            newSkill.GetComponent<RectTransform>().position = new Vector3(child.GetRect().x, child.GetRect().y, 0);
            newSkill.SetUp(child.GetSkillName(), child.GetIcon());
        }
        foreach (var child in tree.GetChildren(skillNode))
        {
            BuildChildren(child, group);
        }
    }
}
