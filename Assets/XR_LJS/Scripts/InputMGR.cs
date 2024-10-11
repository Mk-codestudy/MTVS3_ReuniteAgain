using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputMGR : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private LayerMask placementLayermask;
    [SerializeField] private float raycastDistance = 1000f;

    private int uiLayer;

    public event Action OnClicked;
    public event Action OnExit;

    private void Start()
    {
        uiLayer = LayerMask.NameToLayer("UI");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClicked?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }
    }

    public Vector3Int GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;

        int layerMask = placementLayermask & ~(1 << uiLayer);

        if (Physics.Raycast(ray, out hit, raycastDistance, layerMask))
        {
            Vector3Int gridPosition = Vector3Int.RoundToInt(hit.point);
            Debug.Log($"선택된 맵 위치 (그리드): {gridPosition}");
            return gridPosition;
        }
        else
        {
            Vector3 defaultPosition = ray.GetPoint(10f);
            Vector3Int gridPosition = Vector3Int.RoundToInt(defaultPosition);
            Debug.Log($"레이캐스트 실패. 기본 위치 사용 (그리드): {gridPosition}");
            return gridPosition;
        }
    }

    public bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    private void OnDrawGizmos()
    {
        if (sceneCamera != null)
        {
            Ray ray = sceneCamera.ScreenPointToRay(Input.mousePosition);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(ray.origin, ray.direction * raycastDistance);
        }
    }
}