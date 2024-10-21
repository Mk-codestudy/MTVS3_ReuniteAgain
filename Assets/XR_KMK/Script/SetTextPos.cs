using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetTextPos : MonoBehaviour
{
    [SerializeField] float textBoxWidth = 100f; //이름 텍스트박스
    RectTransform rectTransform; //이름 텍스트박스의 위치
    TextMeshProUGUI tmpText; //이름 텍스트

    [Header("닉네임 옆에 따라오는 문자 할당")]
    public RectTransform grettingPos;
    private Vector2 initialGrettingPos;

    float plusXpos; //더해야 하는 X값

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        tmpText = GetComponent<TextMeshProUGUI>();
        //컴포넌트 캐싱
    }

    private void Start()
    {
        initialGrettingPos = grettingPos.anchoredPosition;
        Canvas.ForceUpdateCanvases();
        ResizeTMP();
    }

    void Update()
    {

    }

    void ResizeTMP() //텍스트 박스 크기 조절
    {
        // 1. 캔버스 강제 업데이트
        Canvas.ForceUpdateCanvases();

        // 2. 레이아웃 리빌더 사용
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

        //Tmp텍스트 강지로 업데이트
        tmpText.ForceMeshUpdate();
        //실제 렌더링된 텍스트의 크기 가져오기
        Vector2 textsize = tmpText.GetRenderedValues(false);

        //새 너비를 계산한다.
        float newWidth = Mathf.Min(textsize.x, textBoxWidth);

        //너비 업데이트
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);

        float offset = newWidth - textBoxWidth;
        grettingPos.anchoredPosition = new Vector2(initialGrettingPos.x + offset, initialGrettingPos.y);

        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }


}
