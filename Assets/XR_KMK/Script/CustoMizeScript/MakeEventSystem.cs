using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MakeEventSystem : MonoBehaviour
{
    //UI Prefab에 부여.
    //해당 Scene에 이벤트시스템이 없어서 UI가 작동되지 않는 오류를 예방한다.

    void Start()
    {
        GameObject checkEventSystem = GameObject.Find("EventSystem");

        if (checkEventSystem == null)
        {
            GameObject eventsystem = new GameObject("EventSystem");
            eventsystem.AddComponent<EventSystem>();
            eventsystem.AddComponent<StandaloneInputModule>();
            Debug.Log("EventSystem 추가");
        }
        else
        {
            Debug.Log("EventSystem 이미 있음!");
        }
    }

}
