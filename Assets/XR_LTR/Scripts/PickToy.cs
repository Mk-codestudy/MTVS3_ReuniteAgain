using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickToy : MonoBehaviour
{
    GameObject toy;

    // 입 위치
    public Transform biteToyPos;
    public Vector3 pos;

    public bool checkBite;

    // 오브젝트에 다가가 E키를 누르면 입에 오리를 문다.
    // 다시 E를 누르면 문 것을 놓는다.
    void Start()
    {
        checkBite = false;
    }

    void Update()
    {
        // 마우스 포인터 위치에 레이를 쏜다.
        Ray ray = new Ray(transform.position + pos, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawRay(transform.position + pos, transform.forward * 100, Color.magenta);
            GameObject obj = hit.collider.gameObject;

            // 장난감 오브젝트일 경우
            if (obj.tag == "Toy")
            {
                toy = obj;

                // R키를 누르면 장난감을 입에 문다
                if (Input.GetKeyDown(KeyCode.R)) 
                {
                    // 장난감을 복제하고 원본은 비활성화
                    GameObject instantiatedToy = Instantiate(toy, biteToyPos.transform.position, biteToyPos.transform.rotation);
                    instantiatedToy.transform.SetParent(biteToyPos);

                    PlayerEffectSound effectSound = instantiatedToy.GetComponent<PlayerEffectSound>();
                    if (effectSound != null) 
                    {
                        effectSound.bitten = true;
                        obj.SetActive(false);
                    }
                    this.enabled = false;
                }
            }
        }
    }    
}
