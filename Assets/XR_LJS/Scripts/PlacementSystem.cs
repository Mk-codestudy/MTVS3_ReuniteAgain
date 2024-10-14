using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI ��Ҹ� ����ϱ� ���� UnityEngine.UI�� �߰�

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator; // ���콺 ��ġ�� ǥ���� ���� ������Ʈ
    [SerializeField] private InputMGR inputMGR; // �Է� ������
    [SerializeField] private Grid grid; // ����Ƽ �׸��� �ý���
    [SerializeField] private ObjectsDatabaseSO database; // ��ġ ������ ������Ʈ �����ͺ��̽�
    [SerializeField] private GameObject gridVisualization; // �׸��带 �ð������� ǥ���� ���� ������Ʈ
    [SerializeField] private PreviewSystem preview; // ��ġ �̸����� �ý���
    [SerializeField] private ObjectPlacer objectPlacer; // ���� ������Ʈ ��ġ�� ����ϴ� Ŭ����
    [SerializeField] private Button startPlacementButton; // ��ġ ��带 ����/�����ϴ� ��ư

    private GridData floorData, furnitureData; // �ٴڰ� ������ ��ġ ������
    private Vector3Int lastDetectedPosition = Vector3Int.zero; // ���������� ������ �׸��� ��ġ
    private IBuildingState buildingState; // ���� �Ǽ� ����
    private bool isInPlacementMode = false; // ���� ��ġ ��� ����

    private void Start()
    {
        InitializeComponents(); // ������Ʈ �ʱ�ȭ
        floorData = new GridData(); // �ٴ� ������ �ʱ�ȭ
        furnitureData = new GridData(); // ���� ������ �ʱ�ȭ
        gridVisualization.SetActive(false);
        preview.gameObject.SetActive(false); // �ʱ⿡ ������ ��Ȱ��ȭ

        // ��ġ ��� ����/���� ��ư ����
        if (startPlacementButton != null)
        {
            startPlacementButton.onClick.AddListener(() => TogglePlacementMode(0)); // ��ư�� �Ҵ�� ������Ʈ 
        }
        else
        {
            Debug.LogError("�ν����Ϳ��� ���� ��ġ ��ư�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    //�ʿ��� ������Ʈ���� ��� �Ҵ�Ǿ����� Ȯ��
    private void InitializeComponents()
    {
        if (inputMGR == null || grid == null || database == null ||
            gridVisualization == null || preview == null || objectPlacer == null)
        {
            Debug.LogError("�ν����Ϳ��� �ϳ� �̻��� ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
            enabled = false; // ������Ʈ ��Ȱ��ȭ
        }
    }

    // ��ġ ��� ��ȯ (�ѱ�/����)
    public void TogglePlacementMode(int objectId)
    {
        if (isInPlacementMode)
        {
            StopPlacement(); // ��ġ ��� ����
        }
        else
        {
            StartPlacement(objectId); // ��ġ ��� ����
        }
        isInPlacementMode = !isInPlacementMode; // ���� ��ȯ
    }
    public void ChangeSelectedObject(int objectId)
    {
        if (!isInPlacementMode)
        {
            StartPlacement(objectId); // ��ġ ��尡 �ƴϸ� ����
        }
        else
        {
            UpdatePlacementObject(objectId); // �̹� ��ġ ���� ������Ʈ�� ������Ʈ
        }
    }
    // ��ġ ������Ʈ ������Ʈ �޼���
    private void UpdatePlacementObject(int id)
    {
        if (database.objectData.Count == 0)
        {
            Debug.LogError("������Ʈ �����ͺ��̽��� ����ֽ��ϴ�!");
            return;
        }

        buildingState.EndState(); // ���� ���� ����
        buildingState = new PlacementState(id, grid, preview, database, floorData, furnitureData, objectPlacer);
        Debug.Log($"���õ� ������Ʈ�� ����Ǿ����ϴ�. ���õ� ������Ʈ ID: {id}"); // �� ���� ����
    }

    private void StartPlacement(int id)
    {
        if (database.objectData.Count == 0)
        {
            Debug.LogError("������Ʈ �����ͺ��̽��� ����ֽ��ϴ�!");
            return;
        }

        Debug.Log($"���õ� ������Ʈ�� ����Ǿ����ϴ�. ���õ� ������Ʈ ID: {id}");
        gridVisualization.SetActive(true);
        preview.gameObject.SetActive(true);
        buildingState = new PlacementState(id, grid, preview, database, floorData, furnitureData, objectPlacer);
        inputMGR.OnClicked += PlaceStructure;
        inputMGR.OnExit += StopPlacement;
        isInPlacementMode = true;
    }

    // ������ ��ġ
    private void PlaceStructure()
    {
        //bool isOverUI = inputMGR.IsPointerOverUI();
       // Debug.Log($"PlaceStructure: IsPointerOverUI = {isOverUI}");
       // if (isOverUI)
       // {
       //     Debug.Log("UI �������� ������Ʈ�� ��ġ�� �� �����ϴ�.");
       //     return;
       // }
        Vector3 mousePosition = inputMGR.GetSelectedMapPosition(); // ���콺 ��ġ ��������
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); // ���� ��ǥ�� �׸��� ��ǥ�� ��ȯ

        buildingState.OnAction(gridPosition); // ���� ���¿� ���� ��ġ �׼� ����
        Debug.Log($"�׸��� ��ġ {gridPosition}�� ������Ʈ ��ġ �õ�");
    }

    // ��ġ ��� ����
    private void StopPlacement()
    {
        if (buildingState == null) return;

        gridVisualization.SetActive(false); // �׸��� �ð�ȭ ��Ȱ��ȭ
        preview.gameObject.SetActive(false); // ������ ��Ȱ��ȭ
        buildingState.EndState(); // ���� ���� ����
        inputMGR.OnClicked -= PlaceStructure; // Ŭ�� �̺�Ʈ���� ������ ��ġ �Լ� ����
        inputMGR.OnExit -= StopPlacement; // ���� �̺�Ʈ���� ��ġ ���� �Լ� ����
        buildingState = null; // ���� �ʱ�ȭ
        Debug.Log("��ġ ��尡 ����Ǿ����ϴ�.");
    }

    // �� �����Ӹ��� ����Ǵ� ������Ʈ �Լ�
    private void Update()
    {
        if (buildingState == null) return;

        Vector3 mousePosition = inputMGR.GetSelectedMapPosition(); // ���� ���콺 ��ġ ��������
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); // �׸��� ��ǥ�� ��ȯ

        if (lastDetectedPosition != gridPosition) // ��ġ�� ����Ǿ��� ���� ������Ʈ
        {
            buildingState.UpdateState(gridPosition); // ���� ������Ʈ
            lastDetectedPosition = gridPosition; // ������ ���� ��ġ ����
        }
    }

    
}