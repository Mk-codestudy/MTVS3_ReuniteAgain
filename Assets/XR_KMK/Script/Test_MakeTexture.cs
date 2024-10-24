using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEditor;
using Cysharp.Threading.Tasks;
using System.Text;
using System.Collections.Generic;

public class Test_MakeTexture : MonoBehaviour
{
    public string url; //Public 변수로 주소를 편하게 집어넣을 수 있게 한다.

    public Button btn_MakeTexture;

    public Button openbrowser;
    public Material catbody;
    public Texture2D catphoto; //전송하는 사진 정보

    public void OpenFileBrowser()
    {
        btn_MakeTexture.enabled = false;
        string path = EditorUtility.OpenFilePanel("Select Image", "", "png,jpg,jpeg");
        if (!string.IsNullOrEmpty(path))
        {
            PlayAfterCondition(path).Forget();
            //StartCoroutine(LoadImage(path));
        }
    }

    //구버전
    //private IEnumerator LoadImage(string path)
    //{
    //    using (UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + path))
    //    {
    //        yield return www.SendWebRequest();

    //        if (www.result == UnityWebRequest.Result.Success)
    //        {
    //            Texture2D texture = DownloadHandlerTexture.GetContent(www);
    //            // 여기서 texture를 사용하여 이미지를 표시하거나 처리합니다.

    //            catphoto = texture;
    //            //PostTextureResource().Forget();
    //            catbody.mainTexture = texture; //고양이 머티리얼에 입혀주는 작업
    //        }
    //        else
    //        {
    //            Debug.Log("Error loading image: " + www.error);
    //        }
    //    }
    
    async UniTaskVoid PlayAfterCondition(string path)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + path))
        {
            await www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                // 여기서 texture를 사용하여 이미지를 표시하거나 처리합니다.

                catphoto = texture;
                PostTextureResource(url).Forget(); //통 신 시 작
                //catbody.mainTexture = texture; //고양이 머티리얼에 입혀주는 작업
            }
            else
            {
                Debug.Log("Error loading image: " + www.error);
            }
        }
        Debug.Log("만족되면 여기도 이어서 실행됩니다.");
    }

    public struct TestTexture
    {
        public int model;
        public int userid;
        public Texture photo;

        public TestTexture(int model, int userid, Texture photo)
        {
            this.model = model;
            this.userid = userid;
            this.photo = photo;
        }
    }

    async UniTask<Texture> PostTextureResource(string url)
    {
        TestTexture testTexture = new TestTexture(1, 1, catphoto);

        string texturedata = JsonUtility.ToJson(testTexture, true);
        byte[] jsonbins = Encoding.UTF8.GetBytes(texturedata);
        
        //List<IMultipartFormSection> mySections = new List<IMultipartFormSection>();
        //mySections.Add(new MultipartFormDataSection("Test.png", jsonbins, "multipart/form-data"));


        UnityWebRequest request = new UnityWebRequest(url, "POST");
        //UnityWebRequest request = UnityWebRequest.Post(url, mySections);
        //request.SetRequestHeader("Content-Type", "image/PNG"); //서버에게 어떤 정보를 보냈는지 헤더를 통해 알려주는 과정. 서버가 이 정보를 토대로 받을 준비를 함.
        //request.SetRequestHeader("Content-Type", "multipart/form-data"); //서버에게 어떤 정보를 보냈는지 헤더를 통해 알려주는 과정. 서버가 이 정보를 토대로 받을 준비를 함.

        request.uploadHandler = new UploadHandlerRaw(jsonbins);
        

        //request.downloadHandler = new DownloadHandlerBuffer(); //바이트 형태 Buffer
        request.downloadHandler = new DownloadHandlerTexture(); //이미지 URL이므로 텍스쳐로 받아봅시다

        await request.SendWebRequest();

        if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("통신 실패!!!!" + request.error.ToString());
            btn_MakeTexture.enabled = true;
            return null;
        }
        else
        {
            Debug.Log("통신 성공! : " + request.result.ToString());
            Texture texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            catbody.mainTexture = texture;
            btn_MakeTexture.enabled = true;
            return texture;
        }
    }

}