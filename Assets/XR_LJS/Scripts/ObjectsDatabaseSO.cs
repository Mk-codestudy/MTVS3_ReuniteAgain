using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] // Unity �����Ϳ��� �� ScriptableObject�� ������ �� �ְ� ���ִ� �Ӽ�
public class ObjectsDatabaseSO : ScriptableObject
{
    public List<ObjectData> objectData; // ObjectData ��ü���� ����Ʈ
    internal object objectsData; // ���������� ���Ǵ� ��ü ������ (���� ������ �ʴ� ������ ����)
}

[Serializable] // �� Ŭ������ ����ȭ �����ϰ� ����� �Ӽ�
public class ObjectData // ��ü �����͸� �����ϴ� Ŭ����
{
    // ���߿��� ������ �����ؾ��� �͵�
    // https://youtu.be/i9W1kqUinIs?si=1JJWg8ybyXCi-vbo ���� �״�� ������

    public int MyProperty { get; set; } // ��� ������ ��Ȯ���� ���� ������Ƽ

    [field: SerializeField] // �� �ʵ带 Unity Inspector���� ǥ���ϰ� ����ȭ
    public string Name { get; private set; } // ��ü�� �̸�

    [field: SerializeField]
    public int ID { get; private set; } // ��ü�� ���� ID

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one; // ��ü�� ũ��, �⺻���� 1x1

    [field: SerializeField]
    public GameObject Prefab { get; private set; } // ��ü�� ������ ������
}