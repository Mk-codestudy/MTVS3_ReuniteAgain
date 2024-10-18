using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float previewYOffset = 0.06f; // ������ ������Ʈ�� Y�� ������

    [SerializeField]
    private GameObject cellIndicator; // �� ǥ�ñ� ���� ������Ʈ
    private GameObject previewObject; // ������ ������Ʈ

    [SerializeField]
    private Material previewMaterialPrefab; // ������ ��Ƽ���� ������
    private Material previewMaterialInstance; // ������ ��Ƽ���� �ν��Ͻ�

    private Renderer cellIndicatorRenderer; // �� ǥ�ñ��� ������

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab); // ������ ��Ƽ���� �ν��Ͻ� ����
        cellIndicator.SetActive(false); // �ʱ⿡ �� ǥ�ñ� ��Ȱ��ȭ
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>(); // �� ǥ�ñ��� ������ ������Ʈ ��������
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab); // �������� �ν��Ͻ�ȭ�Ͽ� ������ ������Ʈ ����
        PreparePreview(previewObject); // ������ ������Ʈ �غ�
        PrepareCursor(size); // Ŀ��(�� ǥ�ñ�) �غ�
        cellIndicator.SetActive(true); // �� ǥ�ñ� Ȱ��ȭ
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y); // Ŀ�� ũ�� ����
            cellIndicatorRenderer.material.mainTextureScale = size; // Ŀ�� �ؽ�ó ������ ����
        }
    }

    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>(); // ��� �ڽ� ������ ��������
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance; // ��� ��Ƽ������ ������ ��Ƽ����� ��ü
            }
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false); // �� ǥ�ñ� ��Ȱ��ȭ
        if (previewObject != null)
            Destroy(previewObject); // ������ ������Ʈ ����
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        if (previewObject != null)
        {
            MovePreview(position); // ������ ������Ʈ �̵�
            ApplyFeedbackToPreview(validity); // ������ ������Ʈ�� ��ȿ�� �ǵ�� ����
        }

        MoveCursor(position); // Ŀ�� �̵�
        ApplyFeedbackToCursor(validity); // Ŀ���� ��ȿ�� �ǵ�� ����
    }

    private void ApplyFeedbackToPreview(bool validity)
    {
        Color c = validity ? Color.white : Color.red; // ��ȿ���� ���� ���� ����
        c.a = 0.5f; // ���İ� ����
        previewMaterialInstance.color = c; // ������ ��Ƽ���� ���� ����
    }

    private void ApplyFeedbackToCursor(bool validity)
    {
        Color c = validity ? Color.white : Color.red; // ��ȿ���� ���� ���� ����
        c.a = 0.5f; // ���İ� ����
        cellIndicatorRenderer.material.color = c; // Ŀ�� ��Ƽ���� ���� ����
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position; // Ŀ�� ��ġ �̵�
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(
            position.x,
            position.y + previewYOffset,
            position.z); // ������ ������Ʈ ��ġ �̵� (Y�� ������ ����)
    }

    internal void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true); // �� ǥ�ñ� Ȱ��ȭ
        PrepareCursor(Vector2Int.one); // 1x1 ũ���� Ŀ�� �غ�
        ApplyFeedbackToCursor(false); // Ŀ���� '����' ���� �ǵ�� ����
    }
}