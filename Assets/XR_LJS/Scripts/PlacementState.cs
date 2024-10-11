using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1; // ���õ� ������Ʈ�� �ε���
    int ID; // ������Ʈ ID
    Grid grid; // �׸��� �ý���
    PreviewSystem previewSystem; // ������ �ý���
    ObjectsDatabaseSO database; // ������Ʈ �����ͺ��̽�
    GridData floorData; // �ٴ� ������
    GridData furnitureData; // ���� ������
    ObjectPlacer objectPlacer; // ������Ʈ ��ġ��

    // ������
    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectsDatabaseSO database,
                          GridData floorData,
                          GridData furnitureData,
                          ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        // ID�� �ش��ϴ� ������Ʈ �ε��� ã��
        selectedObjectIndex = database.objectData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            // ������ ����
            previewSystem.StartShowingPlacementPreview(
                database.objectData[selectedObjectIndex].Prefab,
                database.objectData[selectedObjectIndex].Size);
        }
        else
            Debug.LogWarning($"No object with ID {ID} found. Using default object.");
        selectedObjectIndex = 0; // �⺻ ������Ʈ ���
    }

    // ���� ���� �� ȣ��
    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    // �׼� ���� (������Ʈ ��ġ)
    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
            return;

        // ������Ʈ ��ġ
        int index = objectPlacer.Placeobject(database.objectData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));

        // ���õ� ������ (�ٴ� �Ǵ� ����)
        GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ?
            floorData :
            furnitureData;

        // �׸��� �����Ϳ� ������Ʈ �߰�
        selectedData.AddObjectAt(gridPosition,
            database.objectData[selectedObjectIndex].Size,
            database.objectData[selectedObjectIndex].ID,
            index);

        // ������ ������Ʈ
        previewSystem.updatePostion(grid.CellToWorld(gridPosition), false);
    }

    // ��ġ ���� ���� Ȯ��
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedobjectIndex)
    {
        GridData selectedData = database.objectData[selectedobjectIndex].ID == 0 ?
            floorData :
            furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.objectData[selectedobjectIndex].Size);
    }

    // ���� ������Ʈ (������ ��ġ ����)
    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        previewSystem.updatePostion(grid.CellToWorld(gridPosition), placementValidity);
    }
}