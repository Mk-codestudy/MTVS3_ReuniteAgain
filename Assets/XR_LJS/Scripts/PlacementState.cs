using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1; // 선택된 오브젝트의 인덱스
    int ID; // 오브젝트 ID
    Grid grid; // 그리드 시스템
    PreviewSystem previewSystem; // 프리뷰 시스템
    ObjectsDatabaseSO database; // 오브젝트 데이터베이스
    GridData floorData; // 바닥 데이터
    GridData furnitureData; // 가구 데이터
    ObjectPlacer objectPlacer; // 오브젝트 배치기

    // 생성자
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

        // ID에 해당하는 오브젝트 인덱스 찾기
        selectedObjectIndex = database.objectData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            // 프리뷰 시작
            previewSystem.StartShowingPlacementPreview(
                database.objectData[selectedObjectIndex].Prefab,
                database.objectData[selectedObjectIndex].Size);
        }
        else
            Debug.LogWarning($"No object with ID {ID} found. Using default object.");
        selectedObjectIndex = 0; // 기본 오브젝트 사용
    }

    // 상태 종료 시 호출
    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    // 액션 수행 (오브젝트 배치)
    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
            return;

        // 오브젝트 배치
        int index = objectPlacer.Placeobject(database.objectData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));

        // 선택된 데이터 (바닥 또는 가구)
        GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ?
            floorData :
            furnitureData;

        // 그리드 데이터에 오브젝트 추가
        selectedData.AddObjectAt(gridPosition,
            database.objectData[selectedObjectIndex].Size,
            database.objectData[selectedObjectIndex].ID,
            index);

        // 프리뷰 업데이트
        previewSystem.updatePostion(grid.CellToWorld(gridPosition), false);
    }

    // 배치 가능 여부 확인
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedobjectIndex)
    {
        GridData selectedData = database.objectData[selectedobjectIndex].ID == 0 ?
            floorData :
            furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.objectData[selectedobjectIndex].Size);
    }

    // 상태 업데이트 (프리뷰 위치 갱신)
    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        previewSystem.updatePostion(grid.CellToWorld(gridPosition), placementValidity);
    }
}