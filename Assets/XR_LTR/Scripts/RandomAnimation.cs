using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomAnimation : MonoBehaviour
{
    // 랜덤으로 애니메이션들이 전환되어 재생된다.
    int num;
    
    public float time = 8f;

    Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(PlayRandomAnimation(time));
    }

    IEnumerator PlayRandomAnimation(float sec)
    {
        while (true) // 무한 반복
        {
            // 랜덤한 애니메이션 선택
            num = Random.Range(1, 4);
            animator.SetInteger("Random", num);

            // 현재 애니메이션이 끝날 때까지 대기
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            // 애니메이션이 끝나면 기본 상태로 되돌림
            animator.SetInteger("Random", 0);

            yield return new WaitForSeconds(sec);

            // 디버그 메시지 출력 (선택 사항)
            Debug.Log("랜덤 애니메이션 번호: " + num);
        }
    }
}
