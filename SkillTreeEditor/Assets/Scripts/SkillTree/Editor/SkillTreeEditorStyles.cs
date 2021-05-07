using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillTreeEditorStyles
{ 
    public static GUIStyle GetLableStyle()
    {
        GUIStyle style = new GUIStyle(EditorStyles.textField);
        style.normal.background = Texture2D.blackTexture;
        return style;
    }

    public static GUIStyle GetToolBarStyle()
    {
        var style = new GUIStyle(EditorStyles.toolbar);
        //style.normal.background = Texture2D.redTexture;
        return null;
    }
}
