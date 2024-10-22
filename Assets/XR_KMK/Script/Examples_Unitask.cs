using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Examples_Unitask : MonoBehaviour
{
    //유니태스크 명령어 정리 스크립트
    //실제 게임에 할 당 금 지


    void Start()
    {
        //유니태스크 실행하기
        HowtoStartUnitask().Forget(); //Q.Forget이 뭐예요? A.몰루? API가 걍 저렇게 쓰랬음
    }

    //유니태스크 실행하기
    async UniTaskVoid HowtoStartUnitask()
    {
        // 코루틴에 yield가 필요하듯이 유니태스크에는 await가 필요하다.
        await UniTask.Delay(TimeSpan.FromSeconds(10)); //10초를 기다린다.
    }

    //N초 뒤에 실행하기
    async UniTaskVoid PlayAftersec()
    {
        Debug.Log("이 시점에서 시간을 세기 시작하고요...");
        await UniTask.Delay(TimeSpan.FromSeconds(1/*N초*/));
        Debug.Log("사용자가 설정한 시간 이후에 이쪽 코드가 실행됩니다.");
    }

    // TimeScale이 0(시간이 흐르지 않을 때)이지만 시간을 재고 싶어!!!
    async UniTaskVoid IgnoreTimeScale()
    {
        Debug.Log("이 시점에서 시간을 세기 시작하고요...");
        await UniTask.Delay(TimeSpan.FromSeconds(1), DelayType.UnscaledDeltaTime);
        Debug.Log("사용자가 설정한 시간 이후에 이쪽 코드가 실행됩니다.");
    }

    //특정 조건을 만족했을 때 실행하기
    int a = 5;

    async UniTaskVoid PlayAfterCondition()
    {
        Debug.Log("조건이 만족되기 전에는 여기까지만 실행되고요...");
        await UniTask.WaitUntil(()=> a == 5); //조건이 만족된다면 (int a == 5 라면)
        Debug.Log("만족되면 여기도 이어서 실행됩니다.");
    }


    //웹에 있는 이미지 가져오기
    string imagepath; //URL 링크
    RawImage img; //유니티로 보여줄 이미지

    async UniTask<Texture2D> WaitGetTexture()
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(uri: imagepath);
        await request.SendWebRequest();

        if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
        {
            //실패일 때
            Debug.LogError(request.error);
        }
        else
        {
            //성공했을 때
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            return texture;
        }

        return null; //이거 적어줘야 에러 사라져용
    }

    async UniTaskVoid Getimage()
    {
        Texture2D texture = await WaitGetTexture();
        img.texture = texture;
    }

}
