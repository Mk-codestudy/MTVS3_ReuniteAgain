using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; //Http 네트워크 통신을 위한 네임스페이스 추가
using System.Text;
using UnityEngine.UI;
using System; //Json, csv의 문서 형태의 인코딩(UTF-8) 사용을 위한 네임스페이스 추가

public class HttpManager : MonoBehaviour
{
    public string url; //Public 변수로 주소를 편하게 집어넣을 수 있게 한다.

    [Header("버튼 변수")]
    public Button btn_idle;
    public Button btn_image;
    public Button btn_PostJson;


    [Header("GET 결과값 관련 변수")]
    public RawImage img_response; //이미지를 가져왔을 때 출력
    public Text text_response; //결과값 텍스트

    void Start()
    {

    }

    public void GetIdle() //Get통신 버튼 실행 함수
    {
        btn_idle.interactable = false; //버튼 비활성화
        StartCoroutine(GetIdleRequest(url));
    }

    //Get 통신 기본형 코루틴
    IEnumerator GetIdleRequest(string url)
    {
        //http Get 통신 준비를 한다.
        UnityWebRequest request = UnityWebRequest.Get(url);

        //서버에 Get 요청을 하고, 서버로부터 응답이 올 때까지 대기한다.
        yield return request.SendWebRequest(); //이게 보낸거

        //만일 서버로부터 온 응답이 성공이라면... (성공 코드가 200)
        //응답받은 데이터를 출력한다
        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            print(response);
            text_response.text = response;
        }
        //그렇지 않다면...(400, 404, etc...)
        else
        {
            //에러 내용을 출력한다
            print(request.error);
            text_response.text = request.error;
        }

        btn_idle.interactable = true; //버튼 다시 활성화
    }


    public void GetImeage()
    {
        btn_image.interactable = false;
        StartCoroutine(GetImeageRequest(url));
    }
    //Get 통신 이미지 받아오기
    IEnumerator GetImeageRequest(string url)
    {
        // get(Texture) 통신을 준비한다.
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        // 서버에 요청을 하고, 응답이 있을 때까지 기다린다.
        yield return request.SendWebRequest();

        // 만일, 응답이 성공이라면...
        if (request.result == UnityWebRequest.Result.Success)
        {
            // 받은 텍스쳐 데이터를 Texture2D 변수에 받아놓는다.
            Texture2D response = DownloadHandlerTexture.GetContent(request);

            // Texture2D 데이터를 img_response의 texture 값으로 넣어눈다.
            img_response.texture = response;

            // text_response에 성공 코드 번호를 출력한다.
            text_response.text = "성공 - " + request.responseCode.ToString();
        }
        // 그렇지 않다면...
        else
        {
            // 에러 내용을 text_response에 출력한다.
            print(request.error);
            text_response.text = request.error;
        }

        btn_image.interactable = true;
    }

    [System.Serializable]
    public struct RequestImage
    {
        public string img;
    }


    //Post 통신
    //Json 데이터를 Post하는 함수

    public void PostJson()
    {
        btn_PostJson.interactable = false;
        StartCoroutine(PostJsonRequest(url));
    }

    IEnumerator PostJsonRequest(string url)
    {
        JoinUserData userData = new JoinUserData(1, "asdf", "레오언니"); //테스트할때는 이 값을 수정하면 됨!!!!!!!!
        string userjsondata = JsonUtility.ToJson(userData, true); //Json으로 변환!
        byte[] jsonBins = Encoding.UTF8.GetBytes(userjsondata); //바이트 형태로 바꿔야 전송이 되니까 제이슨을 바이트로 변환!

        UnityWebRequest request = new UnityWebRequest(url, "POST"); //포스트!

        request.SetRequestHeader("Content-Type", "application/json"); //서버에게 어떤 정보를 보냈는지 헤더를 통해 알려주는 과정. 서버가 이 정보를 토대로 받을 준비를 함.
        request.uploadHandler = new UploadHandlerRaw(jsonBins); //바이트로 변환된 Json파일 전송!
        request.downloadHandler = new DownloadHandlerBuffer(); //바이트 형태로 받을 거라서 Buffer라는 것 사용함

        //서버에 Post를 전송하고 응답이 올 때까지 기다린다.
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success) //통신성공
        {
            //다운로드핸들러에서 텍스트 값을 받은 뒤 UI에 출력하기
            string response = request.downloadHandler.text;
            text_response.text = response;
            Debug.Log(response);
        }
        else //실패
        {
            text_response.text = request.error;
            Debug.LogError(request.error);
        }
        btn_PostJson.interactable = true;
    }


}

    [System.Serializable]
    public struct JoinUserData
    {
        public int id;
        public string password;
        public string nickname;

        public JoinUserData(int id, string password, string nickname)
        {
            this.id = id;
            this.password = password;
            this.nickname = nickname;
        }
    }
