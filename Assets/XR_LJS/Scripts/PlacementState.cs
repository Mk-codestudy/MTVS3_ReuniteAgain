using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class PlacementState : IBuildingState
{
    int currentRotation = 0; // 회전각도 0
    private int selectedObjectIndex = -1; // 선택된 오브젝트의 인덱스
    int ID; // 오브젝트의 고유 ID
    Grid grid; // Unity의 Grid 컴포넌트 참조
    PreviewSystem previewSystem; // 미리보기 시스템 참조
    ObjectsDatabaseSO database; // 오브젝트 데이터베이스 참조
    GridData floorData; // 바닥 데이터 참조
    GridData furnitureData; // 가구 데이터 참조
    ObjectPlacer objectPlacer; // 오브젝트 배치 시스템 참조


    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectsDatabaseSO database,
                          GridData floorData,
                          GridData furnitureData,
                          ObjectPlacer objectPlacer)
    {
        // 생성자: 모든 필요한 참조와 데이터를 초기화
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;
        this.currentRotation = 0;

        // 데이터베이스에서 주어진 ID에 해당하는 오브젝트의 인덱스를 찾음
        selectedObjectIndex = database.objectData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            // 유효한 인덱스를 찾았다면 미리보기 시작
            previewSystem.StartShowingPlacementPreview(
                database.objectData[selectedObjectIndex].Prefab,
                database.objectData[selectedObjectIndex].Size);
           
        }
        else
            // 유효한 인덱스를 찾지 못했다면 예외 발생
            throw new System.Exception($"No object with ID {iD}");

    }
    public int GetCurrentRotation()
    {
        return  currentRotation; // 현재 회전 각도 반환
    }
    public void ResetRotation()
    {
        currentRotation = 0; // 회전 각도를 0으로 초기화
                             // previewSystem.UpdateRotation(currentRotation); // 프리뷰 시스템의 회전 업데이트
       // Rotate();
       // UpdateRotation();
    }

    public void EndState()
    {
        // 상태 종료 시 미리보기 중지
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        // 사용자 액션(예: 클릭) 시 호출되는 메서드

        // 현재 그리드 위치에 오브젝트 배치 가능 여부 확인
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
        {
            return; // 배치 불가능하면 메서드 종료
        }

        Vector3 worldPosition = grid.CellToWorld(gridPosition);
        Quaternion rotation = Quaternion.Euler(0,currentRotation,0);


        // 오브젝트 실제 배치
        int index = objectPlacer.PlaceObject(database.objectData[selectedObjectIndex].Prefab,
            worldPosition,
            rotation);

        // 바닥인지 가구인지에 따라 적절한 GridData 선택
        GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ?
            floorData :
            furnitureData;

        // 선택된 GridData에 오브젝트 정보 추가
        selectedData.AddObjectAt(gridPosition,
            database.objectData[selectedObjectIndex].Size,
            database.objectData[selectedObjectIndex].ID,
            index,
            currentRotation);

        // 미리보기 위치 업데이트 (배치 후에는 유효하지 않음을 표시)
        previewSystem.UpdatePosition(worldPosition, false);
        
    }


    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        // 오브젝트 배치 가능 여부를 확인하는 메서드

        // 바닥인지 가구인지에 따라 적절한 GridData 선택
        GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ?
            floorData :
            furnitureData;

        // 선택된 위치에 오브젝트 배치 가능 여부 반환
        return selectedData.CanPlaceObjectAt(gridPosition, database.objectData[selectedObjectIndex].Size);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        // 상태 업데이트 (예: 마우스 이동 시 호출)

        // 현재 그리드 위치의 배치 가능 여부 확인
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        // 미리보기 시스템 업데이트
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
       
    }
    public void Rotate()
    {
        currentRotation = (currentRotation - 90) % 180;
        previewSystem.UpdateRotation(currentRotation);
      //  Debug.Log($"Rotated to {currentRotation} degrees12");
    }

    private void UpdateRotation()
    {
        previewSystem.UpdateRotation(currentRotation);
     //   Debug.Log($"Preview rotation updated to {currentRotation} degrees");
    }
}