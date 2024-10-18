using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager; // �Է� ������
    [SerializeField]
    private Grid grid; // ����Ƽ �׸��� �ý���

    [SerializeField]
    private ObjectsDatabaseSO database; // ������Ʈ �����ͺ��̽�

    [SerializeField]
    private GameObject gridVisualization; // �׸��� �ð�ȭ ������Ʈ

    private GridData floorData, furnitureData; // �ٴڰ� ���� ������

    [SerializeField]
    private PreviewSystem preview; // ������ �ý���

    private Vector3Int lastDetectedPosition = Vector3Int.zero; // ���������� ������ �׸��� ��ġ

    [SerializeField]
    private ObjectPlacer objectPlacer; // ������Ʈ ��ġ��

    IBuildingState buildingState; // ���� �Ǽ� ����

    private void Start()
    {
        gridVisualization.SetActive(false); // �ʱ⿡ �׸��� �ð�ȭ ��Ȱ��ȭ
        floorData = new();
        furnitureData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement(); // ���� ��ġ �۾� ����
        gridVisualization.SetActive(true); // �׸��� �ð�ȭ Ȱ��ȭ
        buildingState = new PlacementState(ID,
                                           grid,
                                           preview,
                                           database,
                                           floorData,
                                           furnitureData,
                                           objectPlacer); // ���ο� ��ġ ���� ����
        inputManager.OnClicked += PlaceStructure; // Ŭ�� �̺�Ʈ�� ������ ��ġ �޼��� ����
        inputManager.OnExit += StopPlacement; // ���� �̺�Ʈ�� ��ġ ���� �޼��� ����
    }

    public void StartRemoving()
    {
        StopPlacement(); // ���� �۾� ����
        gridVisualization.SetActive(true); // �׸��� �ð�ȭ Ȱ��ȭ
        buildingState = new RemovingState(grid, preview, floorData, furnitureData, objectPlacer); // ���ο� ���� ���� ����
        inputManager.OnClicked += PlaceStructure; // Ŭ�� �̺�Ʈ�� ������ ��ġ(���⼭�� ����) �޼��� ����
        inputManager.OnExit += StopPlacement; // ���� �̺�Ʈ�� ��ġ ���� �޼��� ����
    }

    private void PlaceStructure()
    {
        // UI ���� �ִ��� Ȯ���ϴ� �ڵ� (���� �ּ� ó����)
        //if (inputManager.IsPointerOverUI())
        //{
        //    return;
        //}
        Vector3 mousePosition = inputManager.GetSelectedMapPosition(); // ���콺 ��ġ ��������
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); // ���� ��ǥ�� �׸��� ��ǥ�� ��ȯ

        buildingState.OnAction(gridPosition); // ���� ���¿� ���� �׼� ����
    }

    // ��ġ ��ȿ�� �˻� �޼��� (���� �ּ� ó����)
    //private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    //{
    //    GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? 
    //        floorData : 
    //        furnitureData;

    //    return selectedData.CanPlaceObejctAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    //}

    private void StopPlacement()
    {
        if (buildingState == null)
            return;
        gridVisualization.SetActive(false); // �׸��� �ð�ȭ ��Ȱ��ȭ
        buildingState.EndState(); // ���� ���� ����
        inputManager.OnClicked -= PlaceStructure; // Ŭ�� �̺�Ʈ���� ������ ��ġ �޼��� ����
        inputManager.OnExit -= StopPlacement; // ���� �̺�Ʈ���� ��ġ ���� �޼��� ����
        lastDetectedPosition = Vector3Int.zero; // ������ ���� ��ġ �ʱ�ȭ
        buildingState = null; // �Ǽ� ���� �ʱ�ȭ
    }

    private void Update()
    {
        if (buildingState == null)
            return;
        Vector3 mousePosition = inputManager.GetSelectedMapPosition(); // ���콺 ��ġ ��������
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); // ���� ��ǥ�� �׸��� ��ǥ�� ��ȯ
        if (lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition); // �׸��� ��ġ�� ����Ǿ��� �� ���� ������Ʈ
            lastDetectedPosition = gridPosition; // ������ ���� ��ġ ������Ʈ
        }
    }
}