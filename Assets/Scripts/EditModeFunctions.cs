using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditModeFunctions : EditorWindow
{
    [MenuItem("Window/Edit Mode Functions")]
    public static void ShowWindow()
    {
        GetWindow<EditModeFunctions>("Edit Mode Functions");
    }

    private void OnGUI()
    {
        GameDesignerDialogFriend gdFriend = ScriptableObject.CreateInstance<GameDesignerDialogFriend>();

        if (GUILayout.Button("Run Function"))
        {
            gdFriend.Load();
            AssetDatabase.CreateAsset(gdFriend, "Assets/Scripts/ScriptableObjects.asset");
        }
    }
}
