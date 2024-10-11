using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputMGR : MonoBehaviour
{
    [SerializeField]
    Camera sceneCamera; // �� ī�޶� ����

    Vector3 lastPosition; // ���������� ���õ� ��ġ�� �����ϴ� ����

    [SerializeField]
    LayerMask placementLayermask; // ��ġ ������ ���̾� ����ũ

    public event Action OnClicked, OnExit; // Ŭ�� �� ���� �̺�Ʈ

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //���ű��� ����ǥ�� �����ϰ� ȣ���ϴ� ����� 
            // �۾� �̺�Ʈ�� �߻��ϰ� ���𰡰� ���ŵǰ� �ִ� ���
            // �Ϻ� �޼��尡 �Ҵ�Ǿ� �ش��ư�� Ŭ���Ǿ��� � ���� �߻��ؾ� �Ѵٴ� ���� �� �޼��忡 �˸�
            // �� ���Ǻ� ������(?.)�� ����Ͽ� OnClicked �̺�Ʈ�� null�� �ƴ� ���� ȣ��
            OnClicked?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Escape Ű�� ������ �� OnExit �̺�Ʈ ȣ��
            OnExit?.Invoke();
        }
    }

    public bool isPointerOverUI()
        // ���ٽ� �̺�Ʈ �ý��� c#���� �ҷ��ͼ� true / false ��ȯ 
        // UI ��� ���� �����Ͱ� �ִ��� Ȯ���ϴ� �޼���
        => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition; // ���콺 ��ġ ��������
        mousePos.z = sceneCamera.nearClipPlane; // ī�޶��� ����� �Ÿ��� z�� ����
        Ray ray = sceneCamera.ScreenPointToRay(mousePos); // ���콺 ��ġ���� ���� ����
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            // ����ĳ��Ʈ�� �����ϸ� �浹 ������ lastPosition�� ����
            lastPosition = hit.point;
        }
        return lastPosition; // ���������� ���õ� ��ġ ��ȯ
    }
}