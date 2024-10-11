using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    float previewYoffset = 0.06f; // ������ ��ü�� Y�� ������

    [SerializeField]
    GameObject cellIndicator; // �� ǥ�ñ� ���� ������Ʈ
    GameObject previewObject; // ������ ��ü

    [SerializeField]
    Material previewMaterialsPrefab; // ������ ���� ������
    Material previewMasterialInstance; // ������ ���� �ν��Ͻ�

    Renderer cellIndicatorRenderer; // �� ǥ�ñ��� ������

    private void Start()
    {
        previewMasterialInstance = new Material(previewMaterialsPrefab); // ������ ���� �ν��Ͻ� ����
        cellIndicator.SetActive(false); // �ʱ⿡ �� ǥ�ñ� ��Ȱ��ȭ
    }

    // ��ġ ������ ǥ�� ����
    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int Size)
    {
        previewObject = Instantiate(prefab); // ���������κ��� ������ ��ü ����
        preparePreview(previewObject); // ������ ��ü �غ�
        prepareCursor(Size); // Ŀ�� �غ�
        cellIndicator.SetActive(true); // �� ǥ�ñ� Ȱ��ȭ
    }

    // Ŀ�� �غ�
    private void prepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y); // �� ǥ�ñ� ũ�� ����
            cellIndicatorRenderer.GetComponentInChildren<Renderer>().material.mainTextureScale = size; // �ؽ�ó ������ ����
        }
    }

    // ������ ��ü �غ�
    private void preparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>(); // ��� ������ ������Ʈ ��������
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMasterialInstance; // ��� ������ ������ ������ ��ü
            }
            renderer.materials = materials;
        }
    }

    // ������ ǥ�� ����
    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false); // �� ǥ�ñ� ��Ȱ��ȭ
        Destroy(previewObject); // ������ ��ü ����
    }

    // ��ġ ������Ʈ
    public void updatePostion(Vector3 position, bool validity)
    {
        MovePreview(position); // ������ �̵�
        MoveCursor(position); // Ŀ�� �̵�
        ApplyFeedback(validity); // ��ȿ�� �ǵ�� ����
    }

    // ��ȿ�� �ǵ�� ����
    private void ApplyFeedback(bool validity)
    {
        Color c = validity ? Color.white : Color.red; // ��ȿ�ϸ� ���, �ƴϸ� ������
        c.a = 0.5f; // ���İ� ����
        cellIndicatorRenderer.material.color = c; // �� ǥ�ñ� ���� ����
        previewMasterialInstance.color = c; // ������ ���� ���� ����
    }

    // Ŀ�� �̵�
    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    // ������ �̵�
    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(
            position.x,
            position.y + previewYoffset, // Y�� ������ ����
            position.z);
    }
}