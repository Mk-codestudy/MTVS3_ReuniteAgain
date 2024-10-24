using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    [Header("망고")]
    public GameObject mango;

    [Header("플레이어 속도")]
    [Range(1.0f, 30f)]
    public float movespeed = 10;

    [Header("점프 파워")]
    public float jumpPower = 3;

    float yVelocity = 0; //떨어지는 속력
    [Header("중력값")]
    public float gravity = -9.8f; //중력

    Animator animator;
    CharacterController cc;
    Collider col;

    public bool check = false;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dirH = mango.transform.right * h;
        Vector3 dirV = mango.transform.forward * v;

        Vector3 dir = dirH + dirV;

        animator.SetFloat("DirLength", dir.magnitude);
        dir = transform.TransformDirection(dir);
        dir.Normalize();


        //바닥에 닿아 있을 때 받는 중력값 리셋
        if (cc.isGrounded) yVelocity = 0;
        
        //점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Jump");

            yVelocity = jumpPower;
        }
        
        yVelocity += gravity * Time.deltaTime; //중력가속도

        dir.y = yVelocity; //중력값 적용
        cc.Move(dir * movespeed * Time.deltaTime); //이동값 최종적용

        #region 애니메이션
        // 걷기
        animator.SetFloat("DirH", h);
        animator.SetFloat("DirV", v);

        // E 키를 눌렀을 때 체크 상태를 토글
        if (Input.GetKeyDown(KeyCode.E))
        {
            check = !check;
            animator.SetBool("Down", check);
        }

        animator.SetBool("Run", Input.GetKey(KeyCode.LeftShift));
        #endregion
    }
}
