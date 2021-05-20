using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillTreeNode))]
public class SkillTreeNodeInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.BeginVertical();
        EditorGUILayout.Space();
        GUILayout.Label("Costs: ");
        SkillTreeNode skillTreeNode = (SkillTreeNode)target;
        for (int i = 0; i < skillTreeNode.GetCosts().Count; i++)
        {
            GUILayout.BeginHorizontal();
            int cost = EditorGUILayout.IntField("Level " + (i + 1).ToString() + ":", skillTreeNode.GetCosts()[i].GetCost(), GUILayout.Width(200));
            
            //Update Costs only when there is a new value
            if(cost != skillTreeNode.GetCosts()[i].GetCost())
            {
                skillTreeNode.SetCosts(i + 1, cost);
            }

            //Delete Button that is only shown at the last Entry 
            if (i == skillTreeNode.GetCosts().Count -1 && i != 0)
            {
                if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    skillTreeNode.RemoveCost(i + 1);
                }
            }
            GUILayout.EndHorizontal();
        }

        if(GUILayout.Button("+", GUILayout.Width(25), GUILayout.Height(25)))
        {
            skillTreeNode.AddNewCost(skillTreeNode.GetCosts().Count + 1, 1);
        }
        GUILayout.EndVertical();
    }
}
