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
        Debug.Log($"������Ʈ�� ��ġ�Ǿ����ϴ�: ��ġ {position}, �ε��� {placedGameObjects.Count - 1}");
        return placedGameObjects.Count - 1;
    }
}