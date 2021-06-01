using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillTreeCreationWindow : EditorWindow
{
    const float CanvasSize = 1000;
    private const float BackgroundSize = 50;
    private static Action<string> onConfirmPressed;

    private string input;
    public static void ShowEditorWindow(Action<string> onConfirm)
    {
        SkillTreeCreationWindow window = CreateInstance<SkillTreeCreationWindow>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
        onConfirmPressed = onConfirm;
        window.ShowPopup();
    }

    private void OnGUI()
    {
        ProcessEvents();
        GUILayout.Label("Enter Name");

        input = GUILayout.TextField(input);

        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Create")) { onConfirmPressed.Invoke(input); ; Close(); }
        if (GUILayout.Button("Cancel")) { Close(); }
        GUILayout.EndHorizontal();

        Rect rect = GUILayoutUtility.GetRect(CanvasSize, CanvasSize / 2);
        Texture2D texture2D = Texture2D.blackTexture;
        Rect texCoords = new Rect(0, 0, CanvasSize / BackgroundSize, CanvasSize / BackgroundSize);

        GUI.DrawTextureWithTexCoords(rect, texture2D, texCoords);
    }

    private void ProcessEvents()
    {
        if(Event.current.keyCode == KeyCode.Return)
        {
            onConfirmPressed.Invoke(input); 
            Close();
        }
    }
}
