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
        // 완료하기 버튼을 클릭하면 커스텀 패러미터 수치가 json으로 저장된다.
        SaveCustomData(bsm.parameters, bsm.colorHex);
    }
    
    void SaveCustomData(float[] parameter, string color) 
    {
        CharacterData character = LoadCharacterData();



        CustomData newCustomData = new CustomData
        {
            pmtr = parameter,
            colHex = color
        };
        character.data.Add(newCustomData);

        // Json변환 및 저장
        string json = JsonUtility.ToJson(character);
        System.IO.File.WriteAllText(saveFilePath, json);
    }

    private CharacterData LoadCharacterData() 
    {
        return new CharacterData(); // 데이터를 새로 생성
    }
}

[System.Serializable]
public class CustomData
{
    public float[] pmtr = new float[6]; // 슬라이더 수치값
    public string colHex; // 색상 hex코드
}

[System.Serializable]
public class CharacterData 
{
    public List<CustomData> data = new List<CustomData>(); // 커스텀 정보들 저장
}
