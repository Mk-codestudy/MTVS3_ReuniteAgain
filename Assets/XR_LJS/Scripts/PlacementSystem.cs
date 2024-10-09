using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    GameObject mouseIndicator, cellIndicator;
    [SerializeField]
    InputMGR inputMGR;
    [SerializeField]
    Grid grid;

    [SerializeField]
    ObjectsDatabaseSO database;
    int selectedobjectIndex = -1;

    [SerializeField]
    GameObject gridVisualization;

    private GridData floorData, furnitureData;

    private Renderer previewRenderer;

    private List<GameObject> placedGameObject = new();

    private void Start()
    {
        StopPlacemet();
        floorData = new();
        furnitureData = new();
        previewRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void startPlacement(int ID) //https://youtu.be/i9W1kqUinIs?si=3vk3xC-v2FYZCUAW 
    {
       // StopPlacemet();
        // 데이타 베이스 찾기 람다식으로 인덱스 찾아서 ID를 찾아 반환
        selectedobjectIndex = database.objectData.FindIndex(data => data.ID == ID);
        if (selectedobjectIndex < 0)
        {
            Debug.LogError($"No ID Found {ID}");
            return;
        }
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        inputMGR.OnClicked += PlaceStructure;
        inputMGR.OnExit += StopPlacemet;
    }

    // 그리드 위에 있을 때 마우스 버튼을 누르면 새 객체를 인스턴스화 하고 객체의 위치에 배치하는 코드
    private void PlaceStructure() 
    {
        if(inputMGR.isPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputMGR.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedobjectIndex);
        if (placementValidity == false)
            return;
        // 선택 해제된 객체 인덱스를 선택하고 도트 프리팹을 추가할 것이므로 섹터 인덱스를 교체하여 게임 객체 프리팹 서버를 가져오는 방법
        GameObject newObject = Instantiate(database.objectData[selectedobjectIndex].Prefab);        
        newObject.transform.position = grid.CellToWorld(gridPosition);
        placedGameObject.Add(newObject);
        GridData selectedData = database.objectData[selectedobjectIndex].ID == 0 ?
            floorData :
            furnitureData;
        selectedData.AddObjectAt(gridPosition,
            database.objectData[selectedobjectIndex].Size,
            database.objectData[selectedobjectIndex].ID,
            placedGameObject.Count - 1);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedobjectIndex)
    {
        GridData selectedData = database.objectData[selectedobjectIndex].ID == 0 ? 
            floorData : 
            furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.objectData[selectedobjectIndex].Size);
    }

    private void StopPlacemet()
    {
        selectedobjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputMGR.OnClicked -= PlaceStructure;
        inputMGR.OnExit -= StopPlacemet;
    }

    private void Update()
    {
        if (selectedobjectIndex < 0)  
            return; 
        Vector3 mousePosition = inputMGR.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedobjectIndex);
        previewRenderer.material.color = placementValidity ? Color.yellow : Color.red;

        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);

    }
}
