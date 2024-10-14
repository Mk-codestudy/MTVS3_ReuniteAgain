using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new List<GameObject>();

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab, position, Quaternion.identity);
        placedGameObjects.Add(newObject);
        Debug.Log($"오브젝트가 배치되었습니다: 위치 {position}, 인덱스 {placedGameObjects.Count - 1}");
        return placedGameObjects.Count - 1;
    }
}