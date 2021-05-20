using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillTreeNode : ScriptableObject
{
    [SerializeField] private Skill skill;
    [SerializeField] private List<SkillTreeConnection> connections = new List<SkillTreeConnection>();
    [HideInInspector][SerializeField] private List<SkillCost> costs = new List<SkillCost>();
    [HideInInspector][SerializeField] private Rect rect = new Rect(0, 0, 75, 95);

    private bool unlocked = false;
    private int currentLevel;
    private List<string> children = new List<string>();
    private List<string> parents = new List<string>();

    public static Action OnSkillUpdated;

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

    public List<SkillCost> GetCosts()
    {
        if(costs.Count == 0) { costs.Add(new SkillCost(1, 3)); }
        return costs;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
    public int GetMaxLevel()
    {
        return GetCosts().Count;
    }
    public bool IsUnlocked()
    {
        return unlocked;
    }

    public bool isCertainLevelUnlocked(int level)
    {
        if(currentLevel >= level)
        {
            return true;
        }
        return false;
    }

    public void SetUnlocked(bool value)
    {
        unlocked = value;
    }

    public void UnlockNextLevel()
    {
        SetUnlocked(true);
        if (currentLevel < costs.Count)
        {
            currentLevel++;
            Debug.Log("current: " + currentLevel + "," + "Max: " + GetCosts().Count.ToString());    
        }
    }

    public void ResetLevel()
    {
        unlocked = false;
        currentLevel = 0;
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

    public void SetCosts(int level, int newCost)
    {
        GetCostAtLevel(level).SetCost(newCost);
        Debug.Log(level + "gets " + newCost);
    }
    public void AddNewCost(int level, int cost)
    {
        if (ContainsLevel(level)) { return; }
        costs.Add(new SkillCost(level,cost));
    }
    public void RemoveCost(int level)
    {
        costs.Remove(GetCostAtLevel(level));
    }
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
            OnSkillUpdated?.Invoke();
            EditorUtility.SetDirty(this);
        }
    }

    public void AddChild(string newChild)
    {
        Undo.RecordObject(this, "Add Child");
        children.Add(newChild);
        EditorUtility.SetDirty(this);
    }
    private void AddConnection(SkillTreeConnection connection)
    {
        Undo.RecordObject(this, "Add Connection");
        connections.Add(connection);
        EditorUtility.SetDirty(this);
    }
    public void MakeConnection(SkillTreeNode parent, SkillTreeNode child)
    {
        SkillTreeConnection newConnection = CreateInstance<SkillTreeConnection>();
        newConnection = new SkillTreeConnection(parent, child);
        newConnection.name = parent.GetSkillName() + "-" + child.GetSkillName();
        newConnection.AddNewCondition();
        //AssetDatabase.CreateAsset(newConnection, "Assets/Game/SkillTrees/Resources/SkillTreeConnections/" + newConnection.name + ".asset");
        AssetDatabase.AddObjectToAsset(newConnection, this);
        Undo.RegisterCreatedObjectUndo(newConnection, "Create New Connection");
        Undo.RecordObject(this, "Creating Skill Tree Connection");
        AddConnection(newConnection);
    }
    public void RemoveConnection(string removedNode)
    {
        Undo.RecordObject(this, "Remove Connection");
        foreach(var connection in connections)
        {
            if(removedNode == connection.GetParent().name || removedNode == connection.GetChild().name)
            {
                connections.Remove(connection);
                connection.RemoveAction();
                Undo.DestroyObjectImmediate(connection);
                break;
            }
        }
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
  
    private bool ContainsLevel(int level)
    {
        foreach(var cost in costs)
        {
            if(cost.GetLevel() == level)
            {
                return true;
            }
        }

        return false;
    }
    private SkillCost GetCostAtLevel(int level)
    {
        foreach(var cost in costs)
        {
            if(cost.GetLevel() == level)
            {
                return cost;
            }
        }
        return null;
    }

#endif
}
