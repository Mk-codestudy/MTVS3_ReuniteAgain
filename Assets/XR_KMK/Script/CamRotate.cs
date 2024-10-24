using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    // 회전 속력
    public float rotSpeed = 200;

    // 회전 값
    float rotX;
    float rotY;

    // 회전 가능 여부
    public bool useRotX = true;
    public bool useRotY = true;

    void Update()
    {
        float mX = Input.GetAxis("Mouse X");
        float mY = Input.GetAxis("Mouse Y");

        if (useRotX) rotX += mY * rotSpeed * Time.deltaTime;
        if (useRotY) rotY += mX * rotSpeed * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -40, 40); //세로 움직임 클램프

        // 구해진 회전 값을 나의 회전 값으로 셋팅
        transform.localEulerAngles = new Vector3(-rotX, rotY, 0);

    }
}
