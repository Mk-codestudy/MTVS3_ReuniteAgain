using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator; // 마우스 인디케이터를 나타내는 게임 오브젝트
    [SerializeField]
    InputMGR inputMGR; // 사용자 입력을 관리하는 클래스
    [SerializeField]
    Grid grid; // 유니티의 그리드 시스템, 월드 좌표를 그리드 좌표로 변환하는 데 사용

    [SerializeField]
    ObjectsDatabaseSO database; // 배치할 객체의 데이터를 저장한 데이터베이스

    [SerializeField]
    GameObject gridVisualization; // 그리드 시각화용 게임 오브젝트

    private GridData floorData, furnitureData; // 바닥과 가구 데이터를 저장하는 클래스

    [SerializeField]
    PreviewSystem preview; // 객체 배치 전 미리보기 기능을 제공하는 시스템

    Vector3Int lastDetectedPosition = Vector3Int.zero; // 마지막으로 감지된 그리드 위치

    [SerializeField]
    private ObjectPlacer objectPlacer; // 객체를 그리드에 배치하는 클래스

    IBuildingState buildingState; // 객체 배치 상태를 관리하는 인터페이스

    private void Start()
    {
        StopPlacement(); // 초기화 시 배치를 중지
        floorData = new(); // 바닥 데이터 초기화
        furnitureData = new(); // 가구 데이터 초기화
    }

    // 배치를 시작하는 함수, ID를 통해 배치할 객체 선택
    public void startPlacement(int ID)
    {
        StopPlacement(); // 기존 배치를 중지
        gridVisualization.SetActive(true); // 그리드 시각화 활성화
        buildingState = new PlacementState(ID,
                                           grid,
                                           preview,
                                           database,
                                           floorData,
                                           furnitureData,
                                           objectPlacer); // 새로운 배치 상태 초기화
        inputMGR.OnClicked += PlaceStructure; // 클릭 이벤트에 배치 함수 등록
        inputMGR.OnExit += StopPlacement; // 나가기 이벤트에 배치 중지 함수 등록
    }

    // 마우스 클릭 시 객체를 그리드 위에 배치하는 함수
    private void PlaceStructure()
    {
        if (inputMGR.isPointerOverUI()) // 만약 마우스가 UI 위에 있다면 배치를 중지
        {
            return;
        }
        Vector3 mousePosition = inputMGR.GetSelectedMapPosition(); // 마우스 위치를 맵 좌표로 변환
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); // 월드 좌표를 그리드 좌표로 변환

        buildingState.OnAction(gridPosition); // 배치 상태에서의 동작 수행
    }

    // 배치를 중지하는 함수
    private void StopPlacement()
    {
        if (buildingState == null) // 배치 상태가 없으면 리턴
        {
            return;
        }
        gridVisualization.SetActive(false); // 그리드 시각화 비활성화
        buildingState.EndState(); // 배치 상태 종료
        inputMGR.OnClicked -= PlaceStructure; // 클릭 이벤트에서 배치 함수 제거
        inputMGR.OnExit -= StopPlacement; // 나가기 이벤트에서 배치 중지 함수 제거
        lastDetectedPosition = Vector3Int.zero; // 마지막 감지된 위치 초기화
        buildingState = null; // 현재 배치 상태 초기화
    }

    // 매 프레임마다 그리드 좌표 갱신 및 배치 상태 업데이트
    private void Update()
    {
        if (buildingState == null) // 배치 상태가 없으면 리턴
            return;
        Vector3 mousePosition = inputMGR.GetSelectedMapPosition(); // 마우스 위치를 가져옴
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); // 마우스 위치를 그리드 좌표로 변환
        if (lastDetectedPosition != gridPosition) // 마지막 감지된 위치와 현재 위치가 다르면
        {
            buildingState.UpdateState(gridPosition); // 배치 상태 갱신
            lastDetectedPosition = gridPosition; // 마지막 위치 업데이트
        }
    }
}
