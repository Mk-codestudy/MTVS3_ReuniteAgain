using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator; // ���콺 �ε������͸� ��Ÿ���� ���� ������Ʈ
    [SerializeField]
    InputMGR inputMGR; // ����� �Է��� �����ϴ� Ŭ����
    [SerializeField]
    Grid grid; // ����Ƽ�� �׸��� �ý���, ���� ��ǥ�� �׸��� ��ǥ�� ��ȯ�ϴ� �� ���

    [SerializeField]
    ObjectsDatabaseSO database; // ��ġ�� ��ü�� �����͸� ������ �����ͺ��̽�

    [SerializeField]
    GameObject gridVisualization; // �׸��� �ð�ȭ�� ���� ������Ʈ

    private GridData floorData, furnitureData; // �ٴڰ� ���� �����͸� �����ϴ� Ŭ����

    [SerializeField]
    PreviewSystem preview; // ��ü ��ġ �� �̸����� ����� �����ϴ� �ý���

    Vector3Int lastDetectedPosition = Vector3Int.zero; // ���������� ������ �׸��� ��ġ

    [SerializeField]
    private ObjectPlacer objectPlacer; // ��ü�� �׸��忡 ��ġ�ϴ� Ŭ����

    IBuildingState buildingState; // ��ü ��ġ ���¸� �����ϴ� �������̽�

    private void Start()
    {
        StopPlacement(); // �ʱ�ȭ �� ��ġ�� ����
        floorData = new(); // �ٴ� ������ �ʱ�ȭ
        furnitureData = new(); // ���� ������ �ʱ�ȭ
    }

    // ��ġ�� �����ϴ� �Լ�, ID�� ���� ��ġ�� ��ü ����
    public void startPlacement(int ID)
    {
        StopPlacement(); // ���� ��ġ�� ����
        gridVisualization.SetActive(true); // �׸��� �ð�ȭ Ȱ��ȭ
        buildingState = new PlacementState(ID,
                                           grid,
                                           preview,
                                           database,
                                           floorData,
                                           furnitureData,
                                           objectPlacer); // ���ο� ��ġ ���� �ʱ�ȭ
        inputMGR.OnClicked += PlaceStructure; // Ŭ�� �̺�Ʈ�� ��ġ �Լ� ���
        inputMGR.OnExit += StopPlacement; // ������ �̺�Ʈ�� ��ġ ���� �Լ� ���
    }

    // ���콺 Ŭ�� �� ��ü�� �׸��� ���� ��ġ�ϴ� �Լ�
    private void PlaceStructure()
    {
        if (inputMGR.isPointerOverUI()) // ���� ���콺�� UI ���� �ִٸ� ��ġ�� ����
        {
            return;
        }
        Vector3 mousePosition = inputMGR.GetSelectedMapPosition(); // ���콺 ��ġ�� �� ��ǥ�� ��ȯ
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); // ���� ��ǥ�� �׸��� ��ǥ�� ��ȯ

        buildingState.OnAction(gridPosition); // ��ġ ���¿����� ���� ����
    }

    // ��ġ�� �����ϴ� �Լ�
    private void StopPlacement()
    {
        if (buildingState == null) // ��ġ ���°� ������ ����
        {
            return;
        }
        gridVisualization.SetActive(false); // �׸��� �ð�ȭ ��Ȱ��ȭ
        buildingState.EndState(); // ��ġ ���� ����
        inputMGR.OnClicked -= PlaceStructure; // Ŭ�� �̺�Ʈ���� ��ġ �Լ� ����
        inputMGR.OnExit -= StopPlacement; // ������ �̺�Ʈ���� ��ġ ���� �Լ� ����
        lastDetectedPosition = Vector3Int.zero; // ������ ������ ��ġ �ʱ�ȭ
        buildingState = null; // ���� ��ġ ���� �ʱ�ȭ
    }

    // �� �����Ӹ��� �׸��� ��ǥ ���� �� ��ġ ���� ������Ʈ
    private void Update()
    {
        if (buildingState == null) // ��ġ ���°� ������ ����
            return;
        Vector3 mousePosition = inputMGR.GetSelectedMapPosition(); // ���콺 ��ġ�� ������
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); // ���콺 ��ġ�� �׸��� ��ǥ�� ��ȯ
        if (lastDetectedPosition != gridPosition) // ������ ������ ��ġ�� ���� ��ġ�� �ٸ���
        {
            buildingState.UpdateState(gridPosition); // ��ġ ���� ����
            lastDetectedPosition = gridPosition; // ������ ��ġ ������Ʈ
        }
    }
}
