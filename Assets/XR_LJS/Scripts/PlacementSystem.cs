using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 요소를 사용하기 위해 UnityEngine.UI를 추가

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator; // 마우스 위치를 표시할 게임 오브젝트
    [SerializeField] private InputMGR inputMGR; // 입력 관리자
    [SerializeField] private Grid grid; // 유니티 그리드 시스템
    [SerializeField] private ObjectsDatabaseSO database; // 배치 가능한 오브젝트 데이터베이스
    [SerializeField] private GameObject gridVisualization; // 그리드를 시각적으로 표시할 게임 오브젝트
    [SerializeField] private PreviewSystem preview; // 배치 미리보기 시스템
    [SerializeField] private ObjectPlacer objectPlacer; // 실제 오브젝트 배치를 담당하는 클래스
    [SerializeField] private Button startPlacementButton; // 배치 모드를 시작/종료하는 버튼

    private GridData floorData, furnitureData; // 바닥과 가구의 배치 데이터
    private Vector3Int lastDetectedPosition = Vector3Int.zero; // 마지막으로 감지된 그리드 위치
    private IBuildingState buildingState; // 현재 건설 상태
    private bool isInPlacementMode = false; // 현재 배치 모드 상태

    private void Start()
    {
        InitializeComponents(); // 컴포넌트 초기화
        floorData = new GridData(); // 바닥 데이터 초기화
        furnitureData = new GridData(); // 가구 데이터 초기화
        gridVisualization.SetActive(false);
        preview.gameObject.SetActive(false); // 초기에 프리뷰 비활성화

        // 배치 모드 시작/종료 버튼 설정
        if (startPlacementButton != null)
        {
            startPlacementButton.onClick.AddListener(() => TogglePlacementMode(0)); // 버튼에 할당된 오브젝트 
        }
        else
        {
            Debug.LogError("인스펙터에서 시작 배치 버튼이 할당되지 않았습니다.");
        }
    }

    //필요한 컴포넌트들이 모두 할당되었는지 확인
    private void InitializeComponents()
    {
        if (inputMGR == null || grid == null || database == null ||
            gridVisualization == null || preview == null || objectPlacer == null)
        {
            Debug.LogError("인스펙터에서 하나 이상의 컴포넌트가 할당되지 않았습니다.");
            enabled = false; // 컴포넌트 비활성화
        }
    }

    // 배치 모드 전환 (켜기/끄기)
    public void TogglePlacementMode(int objectId)
    {
        if (isInPlacementMode)
        {
            StopPlacement(); // 배치 모드 종료
        }
        else
        {
            StartPlacement(objectId); // 배치 모드 시작
        }
        isInPlacementMode = !isInPlacementMode; // 상태 전환
    }
    public void ChangeSelectedObject(int objectId)
    {
        if (!isInPlacementMode)
        {
            StartPlacement(objectId); // 배치 모드가 아니면 시작
        }
        else
        {
            UpdatePlacementObject(objectId); // 이미 배치 모드면 오브젝트만 업데이트
        }
    }
    // 배치 오브젝트 업데이트 메서드
    private void UpdatePlacementObject(int id)
    {
        if (database.objectData.Count == 0)
        {
            Debug.LogError("오브젝트 데이터베이스가 비어있습니다!");
            return;
        }

        buildingState.EndState(); // 현재 상태 종료
        buildingState = new PlacementState(id, grid, preview, database, floorData, furnitureData, objectPlacer);
        Debug.Log($"선택된 오브젝트가 변경되었습니다. 선택된 오브젝트 ID: {id}"); // 새 상태 생성
    }

    private void StartPlacement(int id)
    {
        if (database.objectData.Count == 0)
        {
            Debug.LogError("오브젝트 데이터베이스가 비어있습니다!");
            return;
        }

        Debug.Log($"선택된 오브젝트가 변경되었습니다. 선택된 오브젝트 ID: {id}");
        gridVisualization.SetActive(true);
        preview.gameObject.SetActive(true);
        buildingState = new PlacementState(id, grid, preview, database, floorData, furnitureData, objectPlacer);
        inputMGR.OnClicked += PlaceStructure;
        inputMGR.OnExit += StopPlacement;
        isInPlacementMode = true;
    }

    // 구조물 배치
    private void PlaceStructure()
    {
        //bool isOverUI = inputMGR.IsPointerOverUI();
       // Debug.Log($"PlaceStructure: IsPointerOverUI = {isOverUI}");
       // if (isOverUI)
       // {
       //     Debug.Log("UI 위에서는 오브젝트를 배치할 수 없습니다.");
       //     return;
       // }
        Vector3 mousePosition = inputMGR.GetSelectedMapPosition(); // 마우스 위치 가져오기
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); // 월드 좌표를 그리드 좌표로 변환

        buildingState.OnAction(gridPosition); // 현재 상태에 따라 배치 액션 수행
        Debug.Log($"그리드 위치 {gridPosition}에 오브젝트 배치 시도");
    }

    // 배치 모드 중지
    private void StopPlacement()
    {
        if (buildingState == null) return;

        gridVisualization.SetActive(false); // 그리드 시각화 비활성화
        preview.gameObject.SetActive(false); // 프리뷰 비활성화
        buildingState.EndState(); // 현재 상태 종료
        inputMGR.OnClicked -= PlaceStructure; // 클릭 이벤트에서 구조물 배치 함수 제거
        inputMGR.OnExit -= StopPlacement; // 종료 이벤트에서 배치 중지 함수 제거
        buildingState = null; // 상태 초기화
        Debug.Log("배치 모드가 종료되었습니다.");
    }

    // 매 프레임마다 실행되는 업데이트 함수
    private void Update()
    {
        if (buildingState == null) return;

        Vector3 mousePosition = inputMGR.GetSelectedMapPosition(); // 현재 마우스 위치 가져오기
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); // 그리드 좌표로 변환

        if (lastDetectedPosition != gridPosition) // 위치가 변경되었을 때만 업데이트
        {
            buildingState.UpdateState(gridPosition); // 상태 업데이트
            lastDetectedPosition = gridPosition; // 마지막 감지 위치 갱신
        }
    }

    
}