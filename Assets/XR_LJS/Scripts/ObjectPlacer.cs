using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObject = new List<GameObject>(); // ��ġ�� ���� ������Ʈ���� �����ϴ� ����Ʈ

    // ������Ʈ�� ��ġ�ϰ� �ش� ������Ʈ�� �ε����� ��ȯ�ϴ� �޼���
    public int Placeobject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab,position, Quaternion.identity); // ���������κ��� �� ������Ʈ ����
        placedGameObject.Add(newObject); // ������ ������Ʈ�� ����Ʈ�� �߰�
        Debug.Log($"Object Placed at Position: {position}");
        return placedGameObject.Count - 1; // ��ġ�� ������Ʈ�� �ε��� ��ȯ (����Ʈ�� ������ �ε���)
    }
}