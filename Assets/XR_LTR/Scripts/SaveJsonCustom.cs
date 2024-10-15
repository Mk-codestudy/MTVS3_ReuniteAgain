using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SaveJsonCustom : MonoBehaviour
{
    public string saveFilePath;
    public BlendShapeMgr bsm;

    void Start()
    {
        saveFilePath = Application.persistentDataPath + "/customData.json";
    }

    public void saveButton() 
    {
        // �Ϸ��ϱ� ��ư�� Ŭ���ϸ� Ŀ���� �з����� ��ġ�� json���� ����ȴ�.
    }
    
    void SaveCustomData(int index, float parameter, Color color) 
    {
        CharacterData character = LoadCharacterData();

        CustomData newCustomData = new CustomData
        {
            idx = index,
            pmtr = parameter,
            col = color,
        };
        character.data.Add(newCustomData);

        // Json��ȯ �� ����
        string json = JsonUtility.ToJson(character);
        System.IO.File.WriteAllText(saveFilePath, json);
    }

    private CharacterData LoadCharacterData() 
    {
        return new CharacterData(); // �����͸� ���� ����
    }
}

[System.Serializable]
public class CustomData
{
    public int idx; // ���������
    public float pmtr; // �����̴� ��ġ��
    public Color col; // �÷���
}

[System.Serializable]
public class CharacterData 
{
    public List<CustomData> data = new List<CustomData>(); // Ŀ���� ������ ����
}
