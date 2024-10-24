using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBall : MonoBehaviour
{
    public float pushPower = 1.0f;  // 밀치는 힘의 크기

    // 공에 닿으면 공이 밀려난다
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // 충돌한 오브젝트가 Rigidbody를 가지고 있고, kinematic이 아니어야 함
        if (body != null && !body.isKinematic)
        {
            if (hit.collider.gameObject.tag == "Ball")
            {
                // 축구공에 힘을 가할 방향 계산 (수평 방향만 사용)
                Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

                // 힘을 가함
                body.AddForce(pushDir * pushPower, ForceMode.Impulse);
            }
        }
    }
    
}
