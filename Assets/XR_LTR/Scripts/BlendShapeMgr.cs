using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlendShapeMgr : MonoBehaviour
{
    // 메쉬의 BlendShape옵션을 슬라이더에 연결하여 슬라이더를 통해 BlendShape 키 값을 조정한다.
    /* 
     5 : shot 꼬리 길이
     4 : thin 꼬리 굵기
     3 : big 머리 크게
     2 : small 머리 작게
     1 : skinny 마르게
     0 : fat 뚱뚱하게
    */

    // 슬라이더 가져오기
    public Slider sliderBody; //몸통
    public Slider sliderHead; //머리
    public Slider sliderLength; //꼬리 길이
    public Slider sliderThick; //꼬리 굵기

    public float[] parameters = new float[6];
    public List<float> colorEye;
    public List<float> colorNose;

    public FlexibleColorPicker fcp_eye;
    public FlexibleColorPicker fcp_nose;
    
    // 커스텀할 오브젝트와 해당 오브젝트의 SkinnedMeshRenderer컴포넌트 불러오기
    [SerializeField]
    GameObject customObj;
    Mesh mesh;
    SkinnedMeshRenderer smr;
    Material eyeMat;
    Material noseMat;

    void Start()
    {
        customObj = GameObject.FindWithTag("Player"); // 나중에는 서버에서 명령을 받아서 해당 prefab을 불러오는 것으로 변경
        mesh = customObj.GetComponent<Mesh>();
        smr = customObj.GetComponent<SkinnedMeshRenderer>();
        eyeMat = smr.materials[3];
        noseMat = smr.materials[1];
    }

    void Update()
    {
        //SkinnedMeshRenderer.SetBlendShapeWeight(조절할 BlendShape의 인덱스 번호, 해당하는 슬라이더의 value);

        // 몸매 조절
        if (sliderBody.value < 0)
        {
            smr.SetBlendShapeWeight(1, sliderBody.value * -1); // 몸매 날씬함
        }
        else
        {
            smr.SetBlendShapeWeight(1, 0);
            smr.SetBlendShapeWeight(0, sliderBody.value); // 몸매 뚱뚱함
        }

        // 머리 크기 조절
        if (sliderHead.value < 0)
        {
            smr.SetBlendShapeWeight(3, sliderHead.value * -1); //머리 작음
        }
        else
        {
            smr.SetBlendShapeWeight(3, 0);
            smr.SetBlendShapeWeight(2, sliderHead.value); // 머리 큼
        }

        // 꼬리 조절
        smr.SetBlendShapeWeight(5, sliderLength.value);
        smr.SetBlendShapeWeight(4, sliderThick.value);

        // 특정 부위의 조절은 컬러피커로 한다
        eyeMat.color = fcp_eye.color;
        noseMat.color = fcp_nose.color;
    }

    public void clickSave() 
    {
        // 조절된 수치값을 배열에 담는다.
        for (int i = 0; i < 6; i++)
        {
            parameters[i] = smr.GetBlendShapeWeight(i);
            print(parameters[i]);
        }

        SaveRGB(colorEye, eyeMat);
        SaveRGB(colorNose, noseMat);
    }

    void SaveRGB(List<float> color, Material part) 
    {
        color.Add(part.color.r);
        color.Add(part.color.g);
        color.Add(part.color.b);
        print(color);
    }
}
