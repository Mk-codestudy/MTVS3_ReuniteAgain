using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1; // ������ ���� ������Ʈ�� �ε���
    Grid grid; // �׸��� �ý��� ����
    PreviewSystem previewSystem; // �̸����� �ý��� ����
    GridData floorData; // �ٴ� ������ ����
    GridData furnitureData; // ���� ������ ����
    ObjectPlacer objectPlacer; // ������Ʈ ��ġ�� ����


    public RemovingState(Grid grid,
                         PreviewSystem previewSystem,
                         GridData floorData,
                         GridData furnitureData,
                         ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        previewSystem.StartShowingRemovePreview(); // ���� �̸����� ����
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview(); // �̸����� ����
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null; // ������ ������ ������ ���� ����
        if (furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            selectedData = furnitureData; // ������ ������ �� �ִ� ���
        }
        else if (floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            selectedData = floorData; // �ٴ��� ������ �� �ִ� ���
        }

        if (selectedData == null)
        {
            return; // ������ �� �ִ� ������Ʈ�� ���� ���
        }
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition); // ������ ������Ʈ�� �ε��� ��������
            if (gameObjectIndex == -1)
                return; // ��ȿ���� ���� �ε����� ���
            selectedData.RemoveObjectAt(gridPosition); // �׸��� �����Ϳ��� ������Ʈ ����
            objectPlacer.RemoveObjectAt(gameObjectIndex); // ���� ���� ������Ʈ ����
        }
        Vector3 cellPosition = grid.CellToWorld(gridPosition); // �׸��� ��ġ�� ���� ��ǥ�� ��ȯ
        previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition)); // �̸����� ��ġ �� ��ȿ�� ������Ʈ
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) &&
            floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one)); // ������ �ٴ� ��� ��ġ ������ ��� ���� �Ұ���
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition); // ���� ��ġ�� ���� ��ȿ�� Ȯ��
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity); // �̸����� �ý��� ������Ʈ
    }
}
