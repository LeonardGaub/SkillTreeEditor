using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillTreeConnection))]
public class SkillConnectionInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SkillTreeConnection skillTreeConnection = (SkillTreeConnection)target;

        GUILayout.Label("Conditions: ");
        for (int i = 0; i < skillTreeConnection.GetConditions().Count; i++)
        {
            GUILayout.BeginHorizontal();
            
            var condition = skillTreeConnection.GetConditions()[i];
            var type = EditorGUILayout.EnumPopup(condition.GetConditionType(), GUILayout.Width(200));
            
            condition.SetType(type);
            if (condition.GetConditionType() == SkillTreeCondition.ConditionType.parentCertainLevel)
            {
                condition.SetRequiredLevel(EditorGUILayout.IntField(condition.GetRequiredLevel(), GUILayout.Width(200)));
            }

            if (i == skillTreeConnection.GetConditions().Count - 1)
            {
                if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    skillTreeConnection.RemoveCondition(i);
                }
            }
            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("+", GUILayout.Width(25), GUILayout.Height(25)))
        {
            skillTreeConnection.AddNewCondition();
        }
    }
}
