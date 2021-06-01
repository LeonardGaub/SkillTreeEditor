using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillTree : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] List<SkillTreeNode> nodes = new List<SkillTreeNode>();

    Dictionary<string, SkillTreeNode> nodeLookup = new Dictionary<string, SkillTreeNode>();

    private void OnValidate()
    {
        nodeLookup.Clear();
        foreach (var node in GetNodes())
        {
            nodeLookup.Add(node.name, node);
        }
    }

    public IEnumerable<SkillTreeNode> GetNodes()
    {
        return nodes;
    }

    public IEnumerable<SkillBase> GetAllUnlockedSkills()
    {
        foreach(var skill in GetNodes())
        {
            if (skill.IsUnlocked())
            {
                yield return skill.GetSkill();
            }
        }
    }

    public SkillTreeNode GetRootNode()
    {
        return nodes[0];
    }

    public IEnumerable<SkillTreeNode> GetAllChildren(SkillTreeNode currentNode)
    {
        foreach (var node in GetChildren(currentNode))
        {
             yield return node;
        }
    }

    public IEnumerable<SkillTreeNode> GetChildren(SkillTreeNode parentNode)
    {
        foreach (var id in parentNode.GetChildren())
        {
            if (nodeLookup.ContainsKey(id))
            {
                yield return nodeLookup[id];
            }
        }
    }

    public IEnumerable<SkillTreeNode> GetParents(SkillTreeNode childNode)
    {
        foreach (var id in childNode.GetParents())
        {
            if (nodeLookup.ContainsKey(id))
            {
                yield return nodeLookup[id];
            }
        }
    }

    public bool isUnlockable(SkillTreeNode node)
    {
        foreach(var connection in node.GetConnections())
        {
            if (!connection.isFullfilled())
            {
                return false;
            }
        }
        return true;
    }

    public void UnlockSkill(SkillTreeNode node)
    {
        node.UnlockNextLevel();
    }

#if UNITY_EDITOR
    public void CreateNode(Vector2 mousePosition)
    {
        SkillTreeNode newNode = MakeNode(mousePosition);
        Undo.RegisterCreatedObjectUndo(newNode, "Create New Node");
        Undo.RecordObject(this, "Creating Skill Tree Node");
        AddNewNode(newNode);
    }

    public void DeleteNode(SkillTreeNode selectedNode)
    {
        Undo.RecordObject(this, "Deleting Skill Tree Node");
        nodes.Remove(selectedNode);
        OnValidate();
        CleanUpDependencies(selectedNode);
        Undo.DestroyObjectImmediate(selectedNode);
    }

    private SkillTreeNode MakeNode(Vector2 mousePosition)
    {
        SkillTreeNode newNode = CreateInstance<SkillTreeNode>();
        newNode.name = System.Guid.NewGuid().ToString();
        newNode.SetRectPosition(mousePosition);

        return newNode;
    }

    private void AddNewNode(SkillTreeNode newNode)
    {
        nodes.Add(newNode);
        AssetDatabase.AddObjectToAsset(newNode, this);
        OnValidate();
    }

    private void CleanUpDependencies(SkillTreeNode selectedNode)
    {
        foreach (var node in GetNodes())
        {
            selectedNode.RemoveConnection(node.name);
            node.RemoveConnection(selectedNode.name);
            node.RemoveChild(selectedNode.name);
            node.RemoveParent(selectedNode.name);
        }
    }

   
#endif

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        if (AssetDatabase.GetAssetPath(this) != "")
        {
            foreach (SkillTreeNode node in GetNodes())
            {
                if (!AssetDatabase.Contains(node))
                {
                    AssetDatabase.AddObjectToAsset(node, this);
                }
            }
        }
#endif
    }

    public void OnAfterDeserialize()
    {
    }
}
