using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillTreeNode : ScriptableObject
{
    [SerializeField] private string skillName;
    [SerializeField] private List<string> children = new List<string>();
    [SerializeField] private List<string> parents = new List<string>();
    [SerializeField] private Sprite icon;
    [SerializeField] private Rect rect = new Rect(0, 0, 100, 150);

    public Rect GetRect()
    {
        return rect;
    }

    public string GetSkillName()
    {
        return skillName;
    }

    public List<string> GetChildren()
    {
        return children;
    }

    public List<string> GetParents()
    {
        return parents;
    }


    public Sprite GetIcon()
    {
        Debug.Log(icon);
        return icon;
    }


    public GUIStyle GetNodeStyle()
    {
        GUIStyle nodeStyle = new GUIStyle();

        nodeStyle.normal.background = Texture2D.whiteTexture;
        return nodeStyle;
    }


#if UNITY_EDITOR
    public void SetRectPosition(Vector2 pos)
    {
        Undo.RecordObject(this, "Dragging Skill Node");
        rect.position = pos;
        EditorUtility.SetDirty(this);
    }


    public void SetIcon(Sprite icon)
    {
        if (icon != this.icon)
        {
            Undo.RecordObject(this, "Setting new Icon");
            this.icon = icon;
            EditorUtility.SetDirty(this);
        }
    }
    public void SetText(string newName)
    {
        if (newName != skillName)
        {
            Undo.RecordObject(this, "Setting new Text");
            skillName = newName;
            EditorUtility.SetDirty(this);
        }
    }

    public void AddChild(string newChild)
    {
        Undo.RecordObject(this, "Add Child");
        children.Add(newChild);
        EditorUtility.SetDirty(this);
    }

    public void RemoveChild(string child)
    {
        Undo.RecordObject(this, "Remove Child");
        children.Remove(child);
        EditorUtility.SetDirty(this);
    }

    public void AddParent(string newParent)
    {
        Undo.RecordObject(this, "Add Parent");
        parents.Add(newParent);
        EditorUtility.SetDirty(this);
    }

    public void RemoveParent(string parent)
    {
        Undo.RecordObject(this, "Remove Parent");
        parents.Remove(parent);
        EditorUtility.SetDirty(this);
    }

#endif
}
