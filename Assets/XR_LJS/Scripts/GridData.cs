using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridData
{
    // 배치된 객체들의 정보를 저장하는 딕셔너리
    Dictionary<Vector3Int, PlacementData> PlacedObjects = new Dictionary<Vector3Int, PlacementData>();

    // 그리드에 객체를 추가하는 메서드
    public void AddObjectAt(Vector3Int gridPosition,
                            Vector2Int objectSize,
                            int ID,
                            int placedObjectIndex,
                            int rotation)
    {
        // 객체가 차지할 모든 위치를 계산
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex, rotation);
        foreach (var pos in positionToOccupy)
        {
            // 이미 해당 위치에 객체가 있으면 예외 발생
            if (PlacedObjects.ContainsKey(pos))
                throw new Exception($"Dictionary already contains this cell position {pos}");
            PlacedObjects[pos] = data;
        }
    }

    // 객체가 차지할 모든 그리드 위치를 계산하는 메서드
    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    // 특정 위치에 객체를 배치할 수 있는지 확인하는 메서드
    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {
            // 이미 해당 위치에 객체가 있으면 false 반환
            if (PlacedObjects.ContainsKey(pos))
                return false;
        }
        return true;
    }

    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if(PlacedObjects.ContainsKey(gridPosition) == false)
            return -1; // 해당 위치에 객체가 없으면 -1 반환
        return PlacedObjects[gridPosition].placeObjectIndex; // 객체의 인덱스 반환
    }
    // 그리드 위치에 있는 객체를 제거하는 메서드
    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (var pos in PlacedObjects[gridPosition].occupiedPositions)
        {
            PlacedObjects.Remove(pos); // 객체가 차지하고 있던 모든 위치에서 객체 정보 제거
        }
    }
}

// 배치된 객체의 데이터를 저장하는 클래스
public class PlacementData
{
    // 객체가 차지하고 있는 모든 그리드 위치
    public List<Vector3Int> occupiedPositions;

    // 객체의 고유 ID
    public int ID { get; private set; }
    // 배치된 객체의 인덱스
    public int placeObjectIndex { get; private set; }
    // 객체의 회전 각도
    public int Rotation {  get; private set; } 

    // 생성자
    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placeObjectIndex, int rotation)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        this.placeObjectIndex = placeObjectIndex;
        Rotation = rotation;

    }
}