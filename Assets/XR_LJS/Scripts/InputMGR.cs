using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera; // 씬 카메라 참조

    private Vector3 lastPosition; // 마지막으로 감지된 위치

    [SerializeField]
    private LayerMask placementLayermask; // 배치 가능한 레이어 마스크

    public event Action OnClicked, OnExit; // 클릭 및 종료 이벤트

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnClicked?.Invoke(); // 마우스 왼쪽 버튼 클릭 시 OnClicked 이벤트 발생

        if (Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke(); // ESC 키 입력 시 OnExit 이벤트 발생
    }

    // UI 요소 위에 마우스가 있는지 확인하는 메서드 (현재 주석 처리됨)
    //public bool IsPointerOverUI()
    //    => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition; // 현재 마우스 위치 가져오기
        mousePos.z = sceneCamera.nearClipPlane; // 카메라의 근평면(near plane)으로 Z 좌표 설정

        Ray ray = sceneCamera.ScreenPointToRay(mousePos); // 마우스 위치에서 레이 생성
        RaycastHit hit;

        // 레이캐스트를 통해 배치 가능한 표면 감지
        if (Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            lastPosition = hit.point; // 레이캐스트 히트 지점을 마지막 위치로 저장
        }

        return lastPosition; // 마지막으로 감지된 위치 반환
    }
}