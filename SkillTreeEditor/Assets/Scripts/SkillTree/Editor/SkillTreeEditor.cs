using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class SkillTreeEditor : EditorWindow
{
    private SkillTree selectedSkillTree;
    private Vector2 scrollPosition;

    private SkillTreeNode nodeToDrag;
    private SkillTreeNode nodeToConnect;
    private int toolbarInt = -1;

    private bool draggingCanvas;
    private Vector2 draggingOffset;

    const float CanvasSize = 4000;
    private const float BackgroundSize = 50;

    [MenuItem("Window/Skill Tree Editor")]
    public static void ShowEditorWindow()
    {
        GetWindow(typeof(SkillTreeEditor), false, "SkillTreeEditor");
    }

    [OnOpenAsset(1)]
    public static bool OnOpenAsset(int instanceId, int line)
    {
        var asset = EditorUtility.InstanceIDToObject(instanceId) as SkillTree;
        if (asset != null)
        {
            ShowEditorWindow();
            return true;
        }
        return false;
    }

    public static void CreateNewSkillTree(string name)
    {
        var newSkillTree = CreateInstance<SkillTree>();
        AssetDatabase.CreateAsset(newSkillTree, "Assets/Game/SkillTrees/" + name + ".asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newSkillTree;
    }

    private void OnEnable()
    {
        Selection.selectionChanged += OnSelectionChange;
        toolbarInt = -1;
    }

    private void OnSelectionChange()
    {
        var selected = Selection.activeObject as SkillTree;
        if (selected != null)
        {
            selectedSkillTree = selected;
            Repaint();
        }
    }

    private void OnGUI()
    {
        if (selectedSkillTree)
        {
            ProcessEvents();
            Draw();
        }
        else
        {
            toolbarInt = GUILayout.Toolbar(toolbarInt, new string[] { "New SkillTree" }, SkillTreeEditorStyles.GetToolBarStyle()); ;
            HandleToolbarEvents();
        }
    }

    private void ProcessEvents()
    {
        if (Event.current.type == EventType.MouseDown && nodeToDrag == null && Event.current.button == 0)
        {
            nodeToDrag = GetNodeAtMousePosition(Event.current.mousePosition + scrollPosition);
            if (nodeToDrag != null)
            {
                draggingOffset = nodeToDrag.GetRect().position - Event.current.mousePosition;
                Selection.activeObject = nodeToDrag;
            }
            else
            {
                draggingCanvas = true;
                draggingOffset = Event.current.mousePosition + scrollPosition;
                Selection.activeObject = selectedSkillTree;
            }
        }
        else if (Event.current.type == EventType.MouseDrag)
        {
            if (nodeToDrag != null)
            {
                nodeToDrag.SetRectPosition(Event.current.mousePosition + draggingOffset);
                GUI.changed = true;
            }
            else if (draggingCanvas)
            {
                scrollPosition = draggingOffset - Event.current.mousePosition;
                GUI.changed = true;
            }
        }
        else if (Event.current.type == EventType.MouseUp && nodeToDrag != null)
        {
            nodeToDrag = null;
            draggingCanvas = false;
        }
        else if (Event.current.type == EventType.MouseUp && Event.current.button == 1 && nodeToConnect == null)
        {
            GenericMenu menu = new GenericMenu();
            if (GetNodeAtMousePosition(Event.current.mousePosition + scrollPosition) != null)
            {
                AddMenuItem(menu, "Delete Skill", OnDeleteSkillButtonPress, Event.current.mousePosition);
            }
            else
            {
                AddMenuItem(menu, "New Skill", OnNewSkillButtonPress, Event.current.mousePosition);
            }

            menu.ShowAsContext();
        }
        else if(Event.current.type == EventType.MouseUp && Event.current.button == 1 && nodeToConnect)
        {
            nodeToConnect = null;
        }
        HandleToolbarEvents();
    }

    private void HandleToolbarEvents()
    {
        switch (toolbarInt)
        {
            case 0:
                SkillTreeCreationWindow.ShowEditorWindow();
                toolbarInt = -1;
                break;
            case 1:
                ConfirmationPopUp.ShowEditorWindow("Do you want to delete this Skill Tree?", () => 
                {
                    DeleteSkillTree(selectedSkillTree.name);
                });
                toolbarInt = -1;
                break;
        }
    }

    private void DeleteSkillTree(string name)
    {
        if (!selectedSkillTree) { return; }
        var assetToDelete = AssetDatabase.FindAssets(name);
        AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(assetToDelete[0]));
    }

    private void AddMenuItem(GenericMenu menu, string name, GenericMenu.MenuFunction2 function, object mousePosition)
    {
        menu.AddItem(new GUIContent(name), false, function, mousePosition);
    }

    private void OnNewSkillButtonPress(object mousePosition)
    {
        selectedSkillTree.CreateNode((Vector2)mousePosition + scrollPosition);
    }

    private void OnDeleteSkillButtonPress(object mousePosition)
    {
        selectedSkillTree.DeleteNode(GetNodeAtMousePosition((Vector2)mousePosition + scrollPosition));
    }

    private SkillTreeNode GetNodeAtMousePosition(Vector2 currentMousePosition)
    {
        SkillTreeNode returnNode = null;
        foreach (var node in selectedSkillTree.GetNodes())
        {
            if (node.GetRect().Contains(currentMousePosition))
            {
                returnNode = node;
            }
        }
        return returnNode;
    }

    private void Draw()
    {
        toolbarInt = GUILayout.Toolbar(toolbarInt, new string[] { "New SkillTree", "Delete" }, SkillTreeEditorStyles.GetToolBarStyle());
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        Rect rect = GUILayoutUtility.GetRect(CanvasSize, CanvasSize);
        Texture2D texture2D = Resources.Load("background") as Texture2D;
        Rect texCoords = new Rect(0, 0, CanvasSize / BackgroundSize, CanvasSize / BackgroundSize);

        GUI.DrawTextureWithTexCoords(rect, texture2D, texCoords);

        foreach (var node in selectedSkillTree.GetNodes())
        {
            DrawConnections(node);
        }

        foreach (var node in selectedSkillTree.GetNodes())
        {
            DrawNode(node);
        }

        if (nodeToConnect)
        {
            DrawConnectionWithMouse(nodeToConnect, Event.current.mousePosition);
            Repaint();
        }

        EditorGUILayout.EndScrollView();
    }
    private void DrawNode(SkillTreeNode node)
    {
        GUIStyle style = node.GetNodeStyle();
        DrawConnectButtons(node);

        GUILayout.BeginArea(node.GetRect(), style);

        var result = (Skill)EditorGUILayout.ObjectField(node.GetSkill(), typeof(Skill), false);
        node.SetSkill(result);

        EditorGUILayout.LabelField(node.GetSkillName(), SkillTreeEditorStyles.GetLableStyle());

        GUILayout.BeginHorizontal(SkillTreeEditorStyles.GetLableStyle());

        if(node.GetSkill() != null)
        {
            GUILayout.Box(node.GetSkill().GetIcon(), GUILayout.Height(50), GUILayout.Width(50));
        }
        else
        {
            GUILayout.Box((Texture2D)null, GUILayout.Height(50), GUILayout.Width(50));
        }

        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    private void DrawConnectButtons(SkillTreeNode node)
    {
        var buttonHeight = node.GetRect().height / 5;
        var buttonWidth = node.GetRect().width / 8;
        
        if (GUI.Button(new Rect(node.GetRect().xMax, node.GetRect().yMin + node.GetRect().height / 2 - buttonHeight/2, buttonWidth, buttonHeight), ""))
        {
            if (nodeToConnect != null)
            {
                return;
            }
            else
            {
                nodeToConnect = node;
            }
        }

        if (GUI.Button(new Rect(node.GetRect().xMin - buttonWidth, node.GetRect().yMin + node.GetRect().height / 2 - buttonHeight/2, buttonWidth, buttonHeight), ""))
        {
            if (nodeToConnect != null && nodeToConnect != node)
            {
                nodeToConnect.AddChild(node.name);
                nodeToConnect = null;
            }   
        }
    }

    private void DrawConnections(SkillTreeNode node)
    {
        Vector3 startPosition = new Vector3(node.GetRect().xMax, node.GetRect().center.y);
        foreach (var childNode in selectedSkillTree.GetChildren(node))
        {
            Vector3 childPosition = new Vector3(childNode.GetRect().xMin, childNode.GetRect().center.y);
            Vector3 controlPointOffset = childPosition - startPosition;
            controlPointOffset.y = 0;
            Handles.DrawBezier(
                startPosition, childPosition,
                startPosition + controlPointOffset, childPosition - controlPointOffset,
                Color.blue, null, 5f);
        }
    }

    private void DrawConnectionWithMouse(SkillTreeNode node, Vector3 mousePosition)
    {
        Vector3 startPosition = new Vector3(node.GetRect().xMax, node.GetRect().center.y);
        Vector3 controlPointOffset = mousePosition - startPosition;
        controlPointOffset.y = 0;
        Handles.DrawBezier(startPosition, mousePosition, startPosition + controlPointOffset, mousePosition - controlPointOffset,
                Color.blue, null, 5f);
    }
}
