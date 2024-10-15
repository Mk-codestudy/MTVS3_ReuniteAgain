using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new(); // 배치된 게임 오브젝트들을 저장하는 리스트

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        // 프리팹을 인스턴스화하여 지정된 위치에 오브젝트를 배치하는 메서드
        GameObject newObject = Instantiate(prefab); // 프리팹을 복제하여 새 오브젝트 생성
        newObject.transform.position = position; // 새 오브젝트의 위치 설정
        placedGameObjects.Add(newObject); // 생성된 오브젝트를 리스트에 추가
        return placedGameObjects.Count - 1; // 배치된 오브젝트의 인덱스 반환 (리스트의 마지막 인덱스)
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        // 지정된 인덱스의 게임 오브젝트를 제거하는 메서드
        if (placedGameObjects.Count <= gameObjectIndex
            || placedGameObjects[gameObjectIndex] == null)
            return; // 인덱스가 유효하지 않거나 이미 제거된 경우 메서드 종료

        Destroy(placedGameObjects[gameObjectIndex]); // 게임 오브젝트 파괴
        placedGameObjects[gameObjectIndex] = null; // 리스트에서 해당 인덱스를 null로 설정 (실제로 제거하지 않고 null로 표시)
    }
}