using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalIn : MonoBehaviour
{
    public GameObject goalEffect;
    public Transform goalPos;

    AudioSource goalSound;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ball") 
        {
            Instantiate(goalEffect, goalPos);
        }
        else if (this.gameObject.layer == 4 && other.gameObject.name.Contains("Toy_duck")) // 해당 바닥이 물이고 오브젝트가 오리일 경우
        {
            Instantiate(goalEffect, goalPos);
        }
    }
}
