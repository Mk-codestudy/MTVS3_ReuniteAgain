using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObject = new(); // ��ġ�� ���� ������Ʈ���� �����ϴ� ����Ʈ

    // ������Ʈ�� ��ġ�ϰ� �ش� ������Ʈ�� �ε����� ��ȯ�ϴ� �޼���
    public int Placeobject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab); // ���������κ��� �� ������Ʈ ����
        newObject.transform.position = position; // ������Ʈ ��ġ ����
        placedGameObject.Add(newObject); // ������ ������Ʈ�� ����Ʈ�� �߰�
        return placedGameObject.Count - 1; // ��ġ�� ������Ʈ�� �ε��� ��ȯ (����Ʈ�� ������ �ε���)
    }
}