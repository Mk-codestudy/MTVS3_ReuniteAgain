using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlendShapeMgr : MonoBehaviour
{
    // 메쉬의 BlendShape옵션을 슬라이더에 연결하여 슬라이더를 통해 BlendShape 키 갑을 조정한다.

    // 슬라이더 가져오기
    public Slider slider1; //몸통
    public Slider slider2; //머리
    public Slider slider3; //꼬리1
    public Slider slider4; //꼬리2

    public FlexibleColorPicker fcp;
    
    // 커스텀할 오브젝트와 해당 오브젝트의 SkinnedMeshRenderer컴포넌트 불러오기
    [SerializeField]
    GameObject customObj;
    Mesh mesh;
    SkinnedMeshRenderer smr;
    Material material;

    void Start()
    {
        customObj = GameObject.FindWithTag("Player"); // 나중에는 서버에서 명령을 받아서 해당 prefab을 불러오는 것으로 변경
        mesh = customObj.GetComponent<Mesh>();
        smr = customObj.GetComponent<SkinnedMeshRenderer>();
        material = smr.materials[2];
    }

    void Update()
    {
        //SkinnedMeshRenderer.SetBlendShapeWeight(조절할 BlendShape의 인덱스 번호, 해당하는 슬라이더의 value);

        // 만약 0을 기준으로 
        //value값이 음수에 가까워진다면 'skinny'수치가 증가, 
        if (slider1.value < 0)
        {
            smr.SetBlendShapeWeight(4, slider1.value * -1); //몸매 날씬함
            smr.SetBlendShapeWeight(3, slider2.value * -1); //얼굴 홀쭉함
        }
        //양수에 가까워진다면 'fat'값이 증가
        else
        {
            smr.SetBlendShapeWeight(5, slider1.value); // 몸매 뚱뚱함
            smr.SetBlendShapeWeight(2, slider2.value); // 얼굴 넙적함
        }
        smr.SetBlendShapeWeight(0, slider3.value);
        smr.SetBlendShapeWeight(1, slider4.value);
        material.color = fcp.color;
    }
}
