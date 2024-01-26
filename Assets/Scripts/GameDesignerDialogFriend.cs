using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDesignerDialogDesigner", menuName = "Scripts/ScriptableObjects", order = 1)]
public class GameDesignerDialogFriend : ScriptableObject
{
    [Foldout("pas toucher GD")]
    [SerializeField] TextAsset csv;

    //[Button("Load CSV")] private void Load() { }


    [Header("SPEAKER 1 IS THE FIRST ONE TO TALK, FROM THERE IT SWAPS BETWEEN EACH SENTENCE")]
    [Header("INPUT THE PLAIN NAME OF THE SPEAKER, EXAMPLE : NICOLAS")]
    [Space(30)]
    public string Speaker1;
    public string Speaker2;

    [Space(30)]
    [Header("PLEASE INPUT THE KEYS FOR THE SENTENCES")]
    [Space(30)]
    //public KEYS[] SentencesID;
    [Options("options")] public string selection;
    public List<String> _keysID = new List<string>();
    

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

    private void AddIDsToEnum()
    {
        Debug.Log(rowList.Count);
        
        foreach (Row row in rowList)
        {
            _keysID.Add(row.ID);
        }
    }

}
