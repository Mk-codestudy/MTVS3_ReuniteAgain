using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new(); // ��ġ�� ���� ������Ʈ���� �����ϴ� ����Ʈ

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        // �������� �ν��Ͻ�ȭ�Ͽ� ������ ��ġ�� ������Ʈ�� ��ġ�ϴ� �޼���
        GameObject newObject = Instantiate(prefab); // �������� �����Ͽ� �� ������Ʈ ����
        newObject.transform.position = position; // �� ������Ʈ�� ��ġ ����
        placedGameObjects.Add(newObject); // ������ ������Ʈ�� ����Ʈ�� �߰�
        return placedGameObjects.Count - 1; // ��ġ�� ������Ʈ�� �ε��� ��ȯ (����Ʈ�� ������ �ε���)
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        // ������ �ε����� ���� ������Ʈ�� �����ϴ� �޼���
        if (placedGameObjects.Count <= gameObjectIndex
            || placedGameObjects[gameObjectIndex] == null)
            return; // �ε����� ��ȿ���� �ʰų� �̹� ���ŵ� ��� �޼��� ����

        Destroy(placedGameObjects[gameObjectIndex]); // ���� ������Ʈ �ı�
        placedGameObjects[gameObjectIndex] = null; // ����Ʈ���� �ش� �ε����� null�� ���� (������ �������� �ʰ� null�� ǥ��)
    }
}