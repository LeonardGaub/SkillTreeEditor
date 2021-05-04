﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Tree", menuName = "Skill Tree")]
public class SkillTree : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] List<SkillTreeNode> nodes = new List<SkillTreeNode>();
    [SerializeField] Vector2 newNodeOffset = new Vector2(300, 0);

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
#if UNITY_EDITOR
    public void CreateNode(SkillTreeNode parentNode)
    {
        SkillTreeNode newNode = MakeNode(parentNode);
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

    private SkillTreeNode MakeNode(SkillTreeNode parentNode)
    {
        SkillTreeNode newNode = CreateInstance<SkillTreeNode>();
        newNode.name = System.Guid.NewGuid().ToString();

        if (parentNode != null)
        {
            parentNode.AddChild(newNode.name);
            newNode.AddParent(parentNode.name);
            newNode.SetRectPosition(parentNode.GetRect().position + newNodeOffset);
        }

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
            node.RemoveChild(selectedNode.name);
            node.RemoveParent(selectedNode.name);
        }
    }

   
#endif

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        if (nodes.Count == 0)
        {
            SkillTreeNode firstNode = MakeNode(null);
            AddNewNode(firstNode);
        }
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