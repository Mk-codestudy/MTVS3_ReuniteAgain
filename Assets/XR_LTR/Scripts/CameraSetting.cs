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

    public float moveSpeed = 0.5f;
    public float rotationSpeed = -0.5f;

    float mouseY = 90;
    float mouseX = 30;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButton(1)) MouseRotate();
    }

    void MouseRotate() 
    {
        mouseY += Input.GetAxisRaw("Mouse X") * mouseSecstv * Time.deltaTime;
        mouseX -= Input.GetAxisRaw("Mouse Y") * mouseSecstv * Time.deltaTime;

        // 상하 회전 각도를 제한하여 너무 뒤로 회전하지 않도록 합니다.
        mouseX = Mathf.Clamp(mouseX, minYrot, maxYrot);

        Vector3 rotation = Vector3.zero;
        rotation.y = mouseY;
        rotation.x = mouseX;
        transform.rotation = Quaternion.Euler(rotation);
    }
}
