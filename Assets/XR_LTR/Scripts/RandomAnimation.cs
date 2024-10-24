using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimation : MonoBehaviour
{
    // 랜덤으로 애니메이션들이 전환되어 재생된다.
    int num;
    float time;
    Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        WaitAnim(8f);
    }

    IEnumerator WaitAnim(float time) 
    {
        yield return new WaitForSeconds(time);
        num = Random.Range(0, 3);
        animator.SetInteger("Random", num);
    }
}
