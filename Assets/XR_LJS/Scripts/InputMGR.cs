using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera; // �� ī�޶� ����

    private Vector3 lastPosition; // ���������� ������ ��ġ

    [SerializeField]
    private LayerMask placementLayermask; // ��ġ ������ ���̾� ����ũ

    public event Action OnClicked, OnExit; // Ŭ�� �� ���� �̺�Ʈ

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnClicked?.Invoke(); // ���콺 ���� ��ư Ŭ�� �� OnClicked �̺�Ʈ �߻�

        if (Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke(); // ESC Ű �Է� �� OnExit �̺�Ʈ �߻�
    }

    // UI ��� ���� ���콺�� �ִ��� Ȯ���ϴ� �޼��� (���� �ּ� ó����)
    //public bool IsPointerOverUI()
    //    => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition; // ���� ���콺 ��ġ ��������
        mousePos.z = sceneCamera.nearClipPlane; // ī�޶��� �����(near plane)���� Z ��ǥ ����

        Ray ray = sceneCamera.ScreenPointToRay(mousePos); // ���콺 ��ġ���� ���� ����
        RaycastHit hit;

        // ����ĳ��Ʈ�� ���� ��ġ ������ ǥ�� ����
        if (Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            lastPosition = hit.point; // ����ĳ��Ʈ ��Ʈ ������ ������ ��ġ�� ����
        }

        return lastPosition; // ���������� ������ ��ġ ��ȯ
    }
}