using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float previewYOffset = 0.06f; // 프리뷰 오브젝트의 Y축 오프셋

    [SerializeField]
    private GameObject cellIndicator; // 셀 표시기 게임 오브젝트
    private GameObject previewObject; // 프리뷰 오브젝트

    [SerializeField]
    private Material previewMaterialPrefab; // 프리뷰 머티리얼 프리팹
    private Material previewMaterialInstance; // 프리뷰 머티리얼 인스턴스

    private Renderer cellIndicatorRenderer; // 셀 표시기의 렌더러

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab); // 프리뷰 머티리얼 인스턴스 생성
        cellIndicator.SetActive(false); // 초기에 셀 표시기 비활성화
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>(); // 셀 표시기의 렌더러 컴포넌트 가져오기
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab); // 프리팹을 인스턴스화하여 프리뷰 오브젝트 생성
        PreparePreview(previewObject); // 프리뷰 오브젝트 준비
        PrepareCursor(size); // 커서(셀 표시기) 준비
        cellIndicator.SetActive(true); // 셀 표시기 활성화
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y); // 커서 크기 조정
            cellIndicatorRenderer.material.mainTextureScale = size; // 커서 텍스처 스케일 조정
        }
    }

    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>(); // 모든 자식 렌더러 가져오기
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance; // 모든 머티리얼을 프리뷰 머티리얼로 교체
            }
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false); // 셀 표시기 비활성화
        if (previewObject != null)
            Destroy(previewObject); // 프리뷰 오브젝트 제거
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        if (previewObject != null)
        {
            MovePreview(position); // 프리뷰 오브젝트 이동
            ApplyFeedbackToPreview(validity); // 프리뷰 오브젝트에 유효성 피드백 적용
        }

        MoveCursor(position); // 커서 이동
        ApplyFeedbackToCursor(validity); // 커서에 유효성 피드백 적용
    }

    private void ApplyFeedbackToPreview(bool validity)
    {
        Color c = validity ? Color.white : Color.red; // 유효성에 따라 색상 결정
        c.a = 0.5f; // 알파값 설정
        previewMaterialInstance.color = c; // 프리뷰 머티리얼 색상 변경
    }

    private void ApplyFeedbackToCursor(bool validity)
    {
        Color c = validity ? Color.white : Color.red; // 유효성에 따라 색상 결정
        c.a = 0.5f; // 알파값 설정
        cellIndicatorRenderer.material.color = c; // 커서 머티리얼 색상 변경
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position; // 커서 위치 이동
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(
            position.x,
            position.y + previewYOffset,
            position.z); // 프리뷰 오브젝트 위치 이동 (Y축 오프셋 적용)
    }
    public void UpdateRotation(int rotation)
    {
        if (previewObject != null)
        {
            previewObject.transform.rotation = Quaternion.Euler(0,rotation,0);  // 프리뷰 오브젝트 회전 업데이트
        }
    }

    internal void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true); // 셀 표시기 활성화
        PrepareCursor(Vector2Int.one); // 1x1 크기의 커서 준비
        ApplyFeedbackToCursor(false); // 커서에 '제거' 상태 피드백 적용
    }
}