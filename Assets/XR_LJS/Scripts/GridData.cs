using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridData
{
    // ��ġ�� ��ü���� ������ �����ϴ� ��ųʸ�
    Dictionary<Vector3Int, PlacementData> PlacedObjects = new();

    // �׸��忡 ��ü�� �߰��ϴ� �޼���
    public void AddObjectAt(Vector3Int gridPosition,
                            Vector2Int objectSize,
                            int ID,
                            int placedObjectIndex)
    {
        // ��ü�� ������ ��� ��ġ�� ���
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);
        foreach (var pos in positionToOccupy)
        {
            // �̹� �ش� ��ġ�� ��ü�� ������ ���� �߻�
            if (PlacedObjects.ContainsKey(pos))
                throw new Exception($"Dictionary already contains this cell position {pos}");
            PlacedObjects[pos] = data;
        }
    }

    // ��ü�� ������ ��� �׸��� ��ġ�� ����ϴ� �޼���
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

    // Ư�� ��ġ�� ��ü�� ��ġ�� �� �ִ��� Ȯ���ϴ� �޼���
    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {
            // �̹� �ش� ��ġ�� ��ü�� ������ false ��ȯ
            if (PlacedObjects.ContainsKey(pos))
                return false;
        }
        return true;
    }
}

// ��ġ�� ��ü�� �����͸� �����ϴ� Ŭ����
public class PlacementData
{
    // ��ü�� �����ϰ� �ִ� ��� �׸��� ��ġ
    public List<Vector3Int> occupiedPositions;

    // ��ü�� ���� ID
    public int ID { get; private set; }
    // ��ġ�� ��ü�� �ε���
    public int placeObjectIndex { get; private set; }

    // ������
    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placeObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        this.placeObjectIndex = placeObjectIndex;
    }
}