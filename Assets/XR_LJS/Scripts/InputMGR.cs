using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputMGR : MonoBehaviour
{
    [SerializeField]
    Camera sceneCamera; // 씬 카메라 참조

    Vector3 lastPosition; // 마지막으로 선택된 위치를 저장하는 변수

    [SerializeField]
    LayerMask placementLayermask; // 배치 가능한 레이어 마스크

    public event Action OnClicked, OnExit; // 클릭 및 종료 이벤트

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //개신기함 물음표가 안전하게 호출하는 방법임 
            // 작업 이벤트가 발생하고 무언가가 수신되고 있는 경우
            // 일부 메서드가 할당되어 해당버튼이 클릭되었고 어떤 일이 발생해야 한다는 것을 이 메서드에 알림
            // 널 조건부 연산자(?.)를 사용하여 OnClicked 이벤트가 null이 아닐 때만 호출
            OnClicked?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Escape 키가 눌렸을 때 OnExit 이벤트 호출
            OnExit?.Invoke();
        }
    }

    public bool isPointerOverUI()
        // 람다식 이벤트 시스템 c#통해 불러와서 true / false 반환 
        // UI 요소 위에 포인터가 있는지 확인하는 메서드
        => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition; // 마우스 위치 가져오기
        mousePos.z = sceneCamera.nearClipPlane; // 카메라의 근평면 거리로 z값 설정
        Ray ray = sceneCamera.ScreenPointToRay(mousePos); // 마우스 위치에서 레이 생성
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            // 레이캐스트가 성공하면 충돌 지점을 lastPosition에 저장
            lastPosition = hit.point;
        }
        return lastPosition; // 마지막으로 선택된 위치 반환
    }
}