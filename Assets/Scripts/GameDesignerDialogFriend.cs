using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "GameDesignerDialogDesigner", menuName = "Scripts/ScriptableObjects", order = 1)]
public class GameDesignerDialogFriend : ScriptableObject
{
    [Header("Change file name  : Write name then click button at the bottom")]

    [SerializeField] private string dialogName;
    [Button("Update File Name")]
    public void ChangeFileName()
    {
        AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(this), dialogName);
    }
    [HorizontalLine(color: EColor.Blue)]

    [Header("SPEAKER 1 IS THE FIRST ONE TO TALK, FROM THERE IT SWAPS BETWEEN EACH SENTENCE")]
    [Header("INPUT THE PLAIN NAME OF THE SPEAKER, EXAMPLE : NICOLAS")]
    [Space(30)]
    public string Speaker1;
    public string Speaker2;
    [HorizontalLine(color: EColor.Blue)]

    [Space(30)]
    [Header("PLEASE INPUT THE KEYS FOR THE SENTENCES")]
    public string[] SentencesKEYS;
    
    [Space(30)]
    [ReadOnly] public List<String> _availableKeys = new List<string>();
    [HorizontalLine(color: EColor.Red)]

    [Foldout("pas toucher GD")]
    [SerializeField] TextAsset csv;

    public class Row
    {
        public string ID;
        public string KEY;
        public string FR;
        public string EN;
        public string SP;
    }

    public List<Row> rowList = new();
     

    public enum KEYS
    {
        
    }

    [Button("Populate list with CSV keys")]
    public void Load()
    {
        csv = (TextAsset)AssetDatabase.LoadAssetAtPath("Assets/MyData/Sentences1.csv", typeof(TextAsset));

        rowList.Clear();
        string[][] grid = CsvParser2.Parse(csv.text);
        for (int i = 1; i < grid.Length; i++)
        {
            Row row = new Row();
            row.ID = grid[i][0];
            row.KEY = grid[i][1];
            row.FR = grid[i][2];
            row.EN = grid[i][3];
            row.SP = grid[i][4];

            rowList.Add(row);
        }
        AddIDsToEnum();
    }

    [Button("Clear List")]
    public void ClearList()
    {
        _availableKeys.Clear();
    }

    private void AddIDsToEnum()
    {   
        foreach (Row row in rowList)
        {
            if (!_availableKeys.Contains(row.KEY))
            {
                _availableKeys.Add(row.KEY);
            }
        }
    }
}
