using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator;
    [SerializeField]
    InputMGR inputMGR;
    [SerializeField]
    Grid grid;

    [SerializeField]
    ObjectsDatabaseSO database;

    [SerializeField]
    GameObject gridVisualization;

    private GridData floorData, furnitureData;


    [SerializeField]
    PreviewSystem preview;

    Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    IBuildingState buildingState;

    private void Start()
    {
        StopPlacemet();
        floorData = new();
        furnitureData = new();
    }

    public void startPlacement(int ID) //https://youtu.be/i9W1kqUinIs?si=3vk3xC-v2FYZCUAW 
    {
        StopPlacemet();
        gridVisualization.SetActive(true);
        buildingState = new PlacementState(ID,
                                           grid,
                                           preview,
                                           database,
                                           floorData,
                                           furnitureData,
                                           objectPlacer);
        inputMGR.OnClicked += PlaceStructure;
        inputMGR.OnExit += StopPlacemet;
    }

    // 그리드 위에 있을 때 마우스 버튼을 누르면 새 객체를 인스턴스화 하고 객체의 위치에 배치하는 코드
    private void PlaceStructure()
    {
        if (inputMGR.isPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputMGR.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);
    }

    //private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedobjectIndex)
    //{
    //    GridData selectedData = database.objectData[selectedobjectIndex].ID == 0 ? 
    //        floorData : 
    //        furnitureData;

    //    return selectedData.CanPlaceObjectAt(gridPosition, database.objectData[selectedobjectIndex].Size);
    //}

    private void StopPlacemet()
    {
        if (buildingState == null)
        {
            return;
        }
        gridVisualization.SetActive(false);
        buildingState.EndState();
        inputMGR.OnClicked -= PlaceStructure;
        inputMGR.OnExit -= StopPlacemet;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null)
            return;
        Vector3 mousePosition = inputMGR.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        if (lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }


    }
}
