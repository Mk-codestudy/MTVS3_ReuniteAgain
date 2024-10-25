using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Proto_ControllScene : MonoBehaviour
{
    Proto_ControllScene pcm;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Debug.Log("DontDestroyOnLoad상에 Proto_ControllScene 생성 :: 씬 조절");
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene("MainFlowScene");
        }
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            SceneManager.LoadScene("MainFlatformScene");
        }
        else if (Input.GetKeyDown(KeyCode.F7))
        {
            SceneManager.LoadScene("LJS_Scene");
        }
        else if (Input.GetKeyDown(KeyCode.F8))
        {
            SceneManager.LoadScene("PlayerTestScene");
        }
    }

    public void ScenePagging(int num)
    {
        // 현재 씬 인덱스(순서)를 확인한 뒤
        int currentSceneindex = SceneManager.GetActiveScene().buildIndex;

        // F4 ~ F6을 통해 해당 씬 불러오기
        // F4(이전), F5(현재), F6(다음)
        SceneManager.LoadScene(currentSceneindex + num);

    }

    public void GotoLJS()
    {
        SceneManager.LoadScene("LJS_Scene");
    }

    public void GotoCommunity()
    {
        SceneManager.LoadScene("PlayerTestScene");
    }
}
