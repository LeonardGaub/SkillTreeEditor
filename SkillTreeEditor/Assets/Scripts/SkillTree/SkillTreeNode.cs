using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillTreeNode : ScriptableObject
{
    private List<string> children = new List<string>();
    private List<string> parents = new List<string>();
    [SerializeField] private List<SkillTreeConnection> connections = new List<SkillTreeConnection>();
    [SerializeField] private bool unlocked = false;
    [SerializeField] private Skill skill;
    private Rect rect = new Rect(0, 0, 100, 110);

    public Rect GetRect()
    {
        return rect;
    }

    public string GetSkillName()
    {
        return skill != null ? skill.name: "";
    }

    public List<string> GetChildren()
    {
        return children;
    }

    public List<string> GetParents()
    {
        return parents;
    }

    public bool isUnlocked()
    {
        return unlocked;
    }

    public void SetUnlocked(bool value)
    {
        unlocked = value;
    }

    public Skill GetSkill()
    {
        return skill;
    }

    public Sprite GetIcon()
    {
        var iconSprite = Sprite.Create(skill.GetIcon(), new Rect(0, 0, skill.GetIcon().width, skill.GetIcon().height), new Vector2(0.5f, 0.5f));
        return iconSprite;
    }


    public GUIStyle GetNodeStyle()
    {
        GUIStyle nodeStyle = new GUIStyle();

        nodeStyle.normal.background = Texture2D.whiteTexture;
        return nodeStyle;
    }

    public List<SkillTreeConnection> GetConnections()
    {
        return connections;
    }


#if UNITY_EDITOR
    public void SetRectPosition(Vector2 pos)
    {
        Undo.RecordObject(this, "Dragging Skill Node");
        rect.position = pos;
        EditorUtility.SetDirty(this);
    }

    public void SetSkill(Skill skill)
    {
        if(skill != this.skill)
        {
            Undo.RecordObject(this, "Setting new Skill");
            this.skill = skill;
            EditorUtility.SetDirty(this);
        }
    }

    public void AddChild(string newChild)
    {
        Undo.RecordObject(this, "Add Child");
        children.Add(newChild);
        EditorUtility.SetDirty(this);
    }

    public void AddConnection(SkillTreeNode parent)
    {
        Undo.RecordObject(this, "Add Connection");
        connections.Add(new SkillTreeConnection(parent, this));
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
