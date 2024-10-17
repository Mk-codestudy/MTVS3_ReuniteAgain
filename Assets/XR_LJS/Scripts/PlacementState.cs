using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1; // ���õ� ������Ʈ�� �ε���
    int ID; // ������Ʈ�� ���� ID
    Grid grid; // Unity�� Grid ������Ʈ ����
    PreviewSystem previewSystem; // �̸����� �ý��� ����
    ObjectsDatabaseSO database; // ������Ʈ �����ͺ��̽� ����
    GridData floorData; // �ٴ� ������ ����
    GridData furnitureData; // ���� ������ ����
    ObjectPlacer objectPlacer; // ������Ʈ ��ġ �ý��� ����


    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectsDatabaseSO database,
                          GridData floorData,
                          GridData furnitureData,
                          ObjectPlacer objectPlacer)
    {
        // ������: ��� �ʿ��� ������ �����͸� �ʱ�ȭ
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        // �����ͺ��̽����� �־��� ID�� �ش��ϴ� ������Ʈ�� �ε����� ã��
        selectedObjectIndex = database.objectData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            // ��ȿ�� �ε����� ã�Ҵٸ� �̸����� ����
            previewSystem.StartShowingPlacementPreview(
                database.objectData[selectedObjectIndex].Prefab,
                database.objectData[selectedObjectIndex].Size);
        }
        else
            // ��ȿ�� �ε����� ã�� ���ߴٸ� ���� �߻�
            throw new System.Exception($"No object with ID {iD}");

    }

    public void EndState()
    {
        // ���� ���� �� �̸����� ����
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        // ����� �׼�(��: Ŭ��) �� ȣ��Ǵ� �޼���

        // ���� �׸��� ��ġ�� ������Ʈ ��ġ ���� ���� Ȯ��
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
        {
            return; // ��ġ �Ұ����ϸ� �޼��� ����
        }

        // ������Ʈ ���� ��ġ
        int index = objectPlacer.PlaceObject(database.objectData[selectedObjectIndex].Prefab,
            grid.CellToWorld(gridPosition));

        // �ٴ����� ���������� ���� ������ GridData ����
        GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ?
            floorData :
            furnitureData;

        // ���õ� GridData�� ������Ʈ ���� �߰�
        selectedData.AddObjectAt(gridPosition,
            database.objectData[selectedObjectIndex].Size,
            database.objectData[selectedObjectIndex].ID,
            index);

        // �̸����� ��ġ ������Ʈ (��ġ �Ŀ��� ��ȿ���� ������ ǥ��)
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        // ������Ʈ ��ġ ���� ���θ� Ȯ���ϴ� �޼���

        // �ٴ����� ���������� ���� ������ GridData ����
        GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ?
            floorData :
            furnitureData;

        // ���õ� ��ġ�� ������Ʈ ��ġ ���� ���� ��ȯ
        return selectedData.CanPlaceObjectAt(gridPosition, database.objectData[selectedObjectIndex].Size);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        // ���� ������Ʈ (��: ���콺 �̵� �� ȣ��)

        // ���� �׸��� ��ġ�� ��ġ ���� ���� Ȯ��
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        // �̸����� �ý��� ������Ʈ
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
}