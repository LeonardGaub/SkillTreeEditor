using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillTreeView))]
public class SkillTreeViewInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SkillTreeView skillTreeView = (SkillTreeView)target;
        if(skillTreeView.GetTree() == null) { return; }
        if (GUILayout.Button("Generate UI"))
        {
            skillTreeView.GenerateUI();
        }
        if(GUILayout.Button("Update UI Connections"))
        {
            skillTreeView.BuildLines();
        }
    }
}
