using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObject = new(); // 배치된 게임 오브젝트들을 저장하는 리스트

    // 오브젝트를 배치하고 해당 오브젝트의 인덱스를 반환하는 메서드
    public int Placeobject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab); // 프리팹으로부터 새 오브젝트 생성
        newObject.transform.position = position; // 오브젝트 위치 설정
        placedGameObject.Add(newObject); // 생성된 오브젝트를 리스트에 추가
        return placedGameObject.Count - 1; // 배치된 오브젝트의 인덱스 반환 (리스트의 마지막 인덱스)
    }
}