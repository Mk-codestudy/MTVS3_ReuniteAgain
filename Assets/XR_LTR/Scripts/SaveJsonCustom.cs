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
        SaveCustomData(bsm.parameters, bsm.colorEye, bsm.colorNose);
    }
    
    void SaveCustomData(float[] parameter, List<float> color1, List<float> color2) 
    {
        // ������ ������ ������ ����
        CharacterData character = LoadCharacterData();

        CustomData newCustomData = new CustomData
        {
            pmtr = parameter,
            eyeCol = color1,
            noseCol = color2
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
    public float[] pmtr = new float[6]; // �����̴� ��ġ��
    public List<float> eyeCol = new List<float>();
    public List<float> noseCol = new List<float>();
}

[System.Serializable]
public class CharacterData 
{
    public List<CustomData> data = new List<CustomData>(); // Ŀ���� ������ ����
}
