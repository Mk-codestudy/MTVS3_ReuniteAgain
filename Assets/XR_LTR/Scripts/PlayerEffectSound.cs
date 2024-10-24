using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectSound : MonoBehaviour
{
    AudioSource sound;
    Rigidbody rb;

    public bool bitten = false;
    public bool water = false;

    void Start()
    {
        sound = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 물림 상태가 되면 rigidbody 비활성화, 사운드 재생
        if(bitten) 
        {
            sound.enabled = true;
            rb.isKinematic = true;
            if (Input.GetKeyDown(KeyCode.R)) 
            {
                transform.parent = null;
                rb.isKinematic = false;
                bitten = false;
            }
        }
    }
}
