using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosCam : MonoBehaviour
{
   public Transform cameraTransform;
    public float moveSpeed = 500f;
    public Vector3 offset;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
        offset = cameraTransform.position - transform.position;
    }

    void Update()
    {

        // 카메라 이동
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0f, vertical) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // 카메라 위치 업데이트
        cameraTransform.position = transform.position + offset;
        cameraTransform.rotation = Quaternion.Euler(30f, 0f, 0f); // 원하는 고정 각도로 설정
        // 카메라가 항상 CameraFocus를 바라보도록 설정
        cameraTransform.LookAt(transform);
    }
}

