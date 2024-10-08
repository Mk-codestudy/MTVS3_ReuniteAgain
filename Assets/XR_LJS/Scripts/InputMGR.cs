using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputMGR : MonoBehaviour
{
    [SerializeField]
    Camera sceneCamera;

    Vector3 lastPosition;

    [SerializeField]
    LayerMask placementLayermask;

    public event Action OnClicked, OnExit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { //개신기함 물음표가 안전하게 호출하는 방법임 
            // 작업 이벤트가 발생하고 무언가가 수신되고 있는 경우
            // 일부 메서드가 할당되어 해당버튼이 클릭되었고 어떤 일이 발생해야 한다는 것을 이 메서드에 알림
            OnClicked?.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }
    }

    public bool isPointerOverUI()
        // 람다식 이벤트 시스템 c#통해 불러와서 true / false 반환 
        => EventSystem.current.IsPointerOverGameObject();


    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit,100, placementLayermask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }
}
