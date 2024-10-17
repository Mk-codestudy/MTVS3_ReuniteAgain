using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1; // 제거할 게임 오브젝트의 인덱스
    Grid grid; // 그리드 시스템 참조
    PreviewSystem previewSystem; // 미리보기 시스템 참조
    GridData floorData; // 바닥 데이터 참조
    GridData furnitureData; // 가구 데이터 참조
    ObjectPlacer objectPlacer; // 오브젝트 배치기 참조


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

        previewSystem.StartShowingRemovePreview(); // 제거 미리보기 시작
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview(); // 미리보기 중지
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null; // 제거할 데이터 선택을 위한 변수
        if (furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            selectedData = furnitureData; // 가구를 제거할 수 있는 경우
        }
        else if (floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            selectedData = floorData; // 바닥을 제거할 수 있는 경우
        }

        if (selectedData == null)
        {
            return; // 제거할 수 있는 오브젝트가 없는 경우
        }
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition); // 제거할 오브젝트의 인덱스 가져오기
            if (gameObjectIndex == -1)
                return; // 유효하지 않은 인덱스인 경우
            selectedData.RemoveObjectAt(gridPosition); // 그리드 데이터에서 오브젝트 제거
            objectPlacer.RemoveObjectAt(gameObjectIndex); // 실제 게임 오브젝트 제거
        }
        Vector3 cellPosition = grid.CellToWorld(gridPosition); // 그리드 위치를 월드 좌표로 변환
        previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition)); // 미리보기 위치 및 유효성 업데이트
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one) &&
            floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one)); // 가구와 바닥 모두 배치 가능한 경우 제거 불가능
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition); // 현재 위치의 선택 유효성 확인
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity); // 미리보기 시스템 업데이트
    }
}
