using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    // 버튼을 눌러서 각도를 돌리기 vs 마우스를 드래그해서 회전하기
    // 우클릭하여 드래그해서 오브젝트를 중심으로 카메라를 회전한다
    public float mouseSecstv = 400f;
    public float maxYrot = 70f;
    public float minYrot = -70f;

    public float rotationSpeed = -0.5f;
    public Vector3 startRotation;

    float mouseY = 90;
    float mouseX = 30;

    private bool isRightClickHeld = false; // 우클릭이 처음 눌렸는지 확인하는 변수

    void Start()
    {
        Vector3 startRotation = transform.rotation.eulerAngles;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            mouseY += Input.GetAxis("Mouse X") * mouseSecstv * Time.deltaTime;

            Vector3 rotation = Vector3.zero;
            rotation.y = mouseY;
            transform.rotation = Quaternion.Euler(startRotation + rotation);
        }
    }
}
