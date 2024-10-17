using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] // Unity �����Ϳ��� �� ScriptableObject�� ������ �� �ְ� ���ִ� �Ӽ�
public class ObjectsDatabaseSO : ScriptableObject
{
    public List<ObjectData> objectData = new List<ObjectData>(); // ObjectData ��ü���� ����Ʈ
}

[Serializable] // �� Ŭ������ ����ȭ �����ϰ� ����� �Ӽ�
public class ObjectData // ��ü �����͸� �����ϴ� Ŭ����
{
    // ���߿��� ������ �����ؾ��� �͵�
    // https://youtu.be/i9W1kqUinIs?si=1JJWg8ybyXCi-vbo ���� �״�� ������

    [field: SerializeField] // �� �ʵ带 Unity Inspector���� ǥ���ϰ� ����ȭ
    public string Name { get; private set; } // ��ü�� �̸�

    [field: SerializeField]
    public int ID { get; private set; } // ��ü�� ���� ID

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one; // ��ü�� ũ��, �⺻���� 1x1

    [field: SerializeField]
    public GameObject Prefab { get; private set; } // ��ü�� ������ ������
}