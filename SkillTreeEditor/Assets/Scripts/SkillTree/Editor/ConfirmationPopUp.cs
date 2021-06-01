using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConfirmationPopUp : EditorWindow
{
    const float CanvasSize = 1000;
    private const float BackgroundSize = 50;

    private static Action onConfirm;
    private static string popUpMessage;

    public static void ShowEditorWindow(string message, Action callback)
    {
        ConfirmationPopUp window = CreateInstance<ConfirmationPopUp>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
        onConfirm = callback;
        popUpMessage = message;
        window.ShowPopup();
    }

    private void OnGUI()
    {
        GUILayout.Label(popUpMessage);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Yes")) { Close(); onConfirm.Invoke();  }
        if (GUILayout.Button("No")) { Close(); }
        GUILayout.EndHorizontal();

        Rect rect = GUILayoutUtility.GetRect(CanvasSize, CanvasSize / 2);
        Texture2D texture2D = Texture2D.blackTexture;
        Rect texCoords = new Rect(0, 0, CanvasSize / BackgroundSize, CanvasSize / BackgroundSize);

        GUI.DrawTextureWithTexCoords(rect, texture2D, texCoords);
    }
}
