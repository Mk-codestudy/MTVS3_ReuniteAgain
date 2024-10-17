using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] // Unity 에디터에서 이 ScriptableObject를 생성할 수 있게 해주는 속성
public class ObjectsDatabaseSO : ScriptableObject
{
    public List<ObjectData> objectData = new List<ObjectData>(); // ObjectData 객체들의 리스트
}

[Serializable] // 이 클래스를 직렬화 가능하게 만드는 속성
public class ObjectData // 객체 데이터를 정의하는 클래스
{
    // 나중에는 서버에 저장해야할 것들
    // https://youtu.be/i9W1kqUinIs?si=1JJWg8ybyXCi-vbo 영상 그대로 구현됨

    [field: SerializeField] // 이 필드를 Unity Inspector에서 표시하고 직렬화
    public string Name { get; private set; } // 객체의 이름

    [field: SerializeField]
    public int ID { get; private set; } // 객체의 고유 ID

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one; // 객체의 크기, 기본값은 1x1

    [field: SerializeField]
    public GameObject Prefab { get; private set; } // 객체와 연관된 프리팹
}