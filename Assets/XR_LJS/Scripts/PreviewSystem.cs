using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    float previewYoffset = 0.06f; // 프리뷰 객체의 Y축 오프셋

    [SerializeField]
    GameObject cellIndicator; // 셀 표시기 게임 오브젝트
    GameObject previewObject; // 프리뷰 객체

    [SerializeField]
    Material previewMaterialsPrefab; // 프리뷰 재질 프리팹
    Material previewMasterialInstance; // 프리뷰 재질 인스턴스

    Renderer cellIndicatorRenderer; // 셀 표시기의 렌더러

    private void Start()
    {
        previewMasterialInstance = new Material(previewMaterialsPrefab); // 프리뷰 재질 인스턴스 생성
        cellIndicator.SetActive(false); // 초기에 셀 표시기 비활성화
    }

    // 배치 프리뷰 표시 시작
    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int Size)
    {
        previewObject = Instantiate(prefab); // 프리팹으로부터 프리뷰 객체 생성
        preparePreview(previewObject); // 프리뷰 객체 준비
        prepareCursor(Size); // 커서 준비
        cellIndicator.SetActive(true); // 셀 표시기 활성화
    }

    // 커서 준비
    private void prepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y); // 셀 표시기 크기 조정
            cellIndicatorRenderer.GetComponentInChildren<Renderer>().material.mainTextureScale = size; // 텍스처 스케일 조정
        }
    }

    // 프리뷰 객체 준비
    private void preparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>(); // 모든 렌더러 컴포넌트 가져오기
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMasterialInstance; // 모든 재질을 프리뷰 재질로 교체
            }
            renderer.materials = materials;
        }
    }

    // 프리뷰 표시 중지
    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false); // 셀 표시기 비활성화
        Destroy(previewObject); // 프리뷰 객체 제거
    }

    // 위치 업데이트
    public void updatePostion(Vector3 position, bool validity)
    {
        MovePreview(position); // 프리뷰 이동
        MoveCursor(position); // 커서 이동
        ApplyFeedback(validity); // 유효성 피드백 적용
    }

    // 유효성 피드백 적용
    private void ApplyFeedback(bool validity)
    {
        Color c = validity ? Color.white : Color.red; // 유효하면 흰색, 아니면 빨간색
        c.a = 0.5f; // 알파값 설정
        cellIndicatorRenderer.material.color = c; // 셀 표시기 색상 변경
        previewMasterialInstance.color = c; // 프리뷰 재질 색상 변경
    }

    // 커서 이동
    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    // 프리뷰 이동
    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(
            position.x,
            position.y + previewYoffset, // Y축 오프셋 적용
            position.z);
    }
}