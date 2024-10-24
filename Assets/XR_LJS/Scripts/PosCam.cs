using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosCam : MonoBehaviour
{
    // 버튼을 눌러서 각도를 돌리기 vs 마우스를 드래그해서 회전하기
    // 우클릭하여 드래그해서 오브젝트를 중심으로 카메라를 회전한다
    public float mouseSecstv = 400f;
    public float maxYrot = 30f;
    public float minYrot = -30f;

    public float rotationSpeed = -0.5f;
    public Vector3 startRotation;

    float mouseY = 0;

    public GameObject cam;


    void Start()
    {
        Vector3 startRotation = transform.rotation.eulerAngles;
    }

    void Update()
    {
        // 마우스 오른쪽 버튼을 누른 상태에서 드래그하면 펫의 각도를 바꿀 수 있음
        if (Input.GetMouseButton(1))
        {
            // 전체 카메라일 경우
            if (cam.activeInHierarchy)
            {
                mouseY += Input.GetAxis("Mouse X") * mouseSecstv * Time.deltaTime;

                Vector3 rotation = Vector3.zero;
                rotation.y = mouseY;
                cam.transform.rotation = Quaternion.Euler(startRotation + rotation);
            }

        }

    }
}