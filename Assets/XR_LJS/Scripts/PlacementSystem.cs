using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementSystem : MonoBehaviour 
{
    [SerializeField]
    private InputManager inputManager; // 입력 관리자
    [SerializeField]
    private Grid grid; // 유니티 그리드 시스템

    [SerializeField]
    private ObjectsDatabaseSO database; // 오브젝트 데이터베이스

    [SerializeField]
    private GameObject gridVisualization; // 그리드 시각화 오브젝트

    private GridData floorData, furnitureData; // 바닥과 가구 데이터

    [SerializeField]
    private PreviewSystem preview; // 프리뷰 시스템

    private Vector3Int lastDetectedPosition = Vector3Int.zero; // 마지막으로 감지된 그리드 위치

    [SerializeField]
    private ObjectPlacer objectPlacer; // 오브젝트 배치기

    IBuildingState buildingState; // 현재 건설 상태

    int rotation = 0;
    // private int currentRotation;

    public void Start()
    {
        gridVisualization.SetActive(false); // 초기에 그리드 시각화 비활성화
        floorData = new(); // 바닥 데이터 초기화
        furnitureData = new(); // 가구 데이터 초기화
     
    }

    public void StartPlacement(int ID) // 배치를 시작할떄 호출이 안되고있음
    {
        StopPlacement(); // 이전 배치 작업 중지
        gridVisualization.SetActive(true); // 그리드 시각화 활성화
        buildingState = new PlacementState(ID,
                                           grid,
                                           preview,
                                           database,
                                           floorData,
                                           furnitureData,
                                           objectPlacer); // 새로운 배치 상태 생성
        if (buildingState is PlacementState placementState)
        {
            //    return;
            // buildingState = new RemovingState(placementState);
            // currentRotation = 0; // 회전 각도를 0으로 초기화
            placementState.ResetRotation();
          //  Debug.Log("Placement started, rotation reset"); // 디버그 로그 추가 ------- 이게 호출이 안됨
        }
       
        inputManager.OnClicked += PlaceStructure; // 클릭 이벤트에 구조물 배치 메서드 연결
        inputManager.OnExit += StopPlacement; // 종료 이벤트에 배치 중지 메서드 연결
        inputManager.OnRotate += RotateStructure;
     
    }

    public void StartRemoving()
    {
        StopPlacement(); // 이전 작업 중지
        gridVisualization.SetActive(true); // 그리드 시각화 활성화
        buildingState = new RemovingState(grid, preview, floorData, furnitureData, objectPlacer); // 새로운 제거 상태 생성
        inputManager.OnClicked += PlaceStructure; // 클릭 이벤트에 구조물 배치(여기서는 제거) 메서드 연결
        inputManager.OnExit += StopPlacement; // 종료 이벤트에 배치 중지 메서드 연결
    }

    public void PlaceStructure()
    {
        // UI 위에 있는지 확인하는 코드 (현재 주석 처리됨)
        //if (inputManager.IsPointerOverUI())
        //{
        //    return;
        //}
        Vector3 mousePosition = inputManager.GetSelectedMapPosition(); // 마우스 위치 가져오기
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); // 월드 좌표를 그리드 좌표로 변환

        buildingState.OnAction(gridPosition); // 현재 상태에 따라 액션 수행
    }

    // 배치 유효성 검사 메서드 (현재 주석 처리됨)
    //private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    //{
    //    GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? 
    //        floorData : 
    //        furnitureData;

    //    return selectedData.CanPlaceObejctAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    //}

    public void RotateStructure()
    {
        if (buildingState != null && buildingState is PlacementState placementState)
        {
            placementState.Rotate(); // PlacementState인 경우 회전 수행
        }
    }

    private void StopPlacement()
    {
        if (buildingState == null)
            return;
        gridVisualization.SetActive(false); // 그리드 시각화 비활성화
        preview.gameObject.SetActive(false);

        if (buildingState is PlacementState placementState)
        {
            placementState.ResetRotation();
        }

        buildingState.EndState(); // 현재 상태 종료
        inputManager.OnClicked -= PlaceStructure; // 클릭 이벤트에서 구조물 배치 메서드 제거
        inputManager.OnExit -= StopPlacement; // 종료 이벤트에서 배치 중지 메서드 제거
        lastDetectedPosition = Vector3Int.zero; // 마지막 감지 위치 초기화
        buildingState = null; // 건설 상태 초기화
        inputManager.OnRotate -= RotateStructure;
        
    }

    private void Update()
    {
        if (buildingState == null)
            return;
        Vector3 mousePosition = inputManager.GetSelectedMapPosition(); // 마우스 위치 가져오기
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); // 월드 좌표를 그리드 좌표로 변환
        if (lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition); // 그리드 위치가 변경되었을 때 상태 업데이트
            lastDetectedPosition = gridPosition; // 마지막 감지 위치 업데이트
        }
    }
}