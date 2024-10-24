using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetPhotoManager : MonoBehaviour
{
    GetPhotoManager gtm;

    public Texture2D[] photos = new Texture2D[5]; //유저 임포트 사진 저장
    int photonum; //사진 번호
    
    public Button impbtn;

    public Texture2D catphoto; //전송하는 사진 정보
    public SkinnedMeshRenderer mango; //고양이 몸통 메쉬
    public Material catbody; //고양이 몸통 머티리얼

    [Header("다음 버튼")]
    public Button nextBtn;
    [Header("UI 미리보기 이미지")]
    public RawImage[] photosUI = new RawImage[5]; //UI표시

    private void Awake()
    {
        if (gtm == null) gtm = this;
        else Destroy(gameObject);
        //싱글턴

        DontDestroyOnLoad(gameObject);
        Debug.Log("DontDestroyOnLoad상에 GetPhotoManager 생성 :: 커스터마이징 로드 반려동물 사진 저장소");
    }

    private void Update()
    {
        if (photos[0] != null) nextBtn.interactable = true;//다음으로 넘어가는 버튼 활성화

        if (photonum > 4) impbtn.interactable = false; //사진함 꽉 차면 버튼 비활성화

    }

    //10/25(금) 오민석강사님 컨펌용 텍스쳐 불러오기
    public void OpenFileBrowser() //버튼 눌렀을 때 브라우저 여는 코루틴 호출
    {
        impbtn.interactable = false;
        string path = EditorUtility.OpenFilePanel("Select Image", "", "png,jpg,jpeg");
        if (!string.IsNullOrEmpty(path)) PlayAfterCondition(path).Forget();
    }

    async UniTaskVoid PlayAfterCondition(string path) //브라우저 여는 코루틴
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + path))
        {
            await www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                // 여기서 texture를 사용하여 이미지를 표시하거나 처리합니다.

                photos[photonum] = texture; //사진을 array에 저장한다.
                photosUI[photonum].texture = texture; //UI에도 둔다.
                photosUI[photonum].gameObject.SetActive(true); //켜!

                //if (photonum == 0) //첫 번째 사진으로 AI와 통신한다.
                //{
                //    catphoto = photos[0];

                //    //PostTextureResource(url).Forget(); //통 신 시 작

                //    //Material newbody = new Material(catbody);
                //    //newbody.mainTexture = catphoto;
                //    //mango.materials[0].mainTexture = catphoto;
                //    catbody.mainTexture = catphoto; //고양이 머티리얼에 입혀주는 작업
                //    Debug.Log("body 텍스쳐 적용완료");
                //}
                photonum++;
                impbtn.interactable = true;
            }
            else
            {
                Debug.Log("Error loading image: " + www.error);
                impbtn.interactable = true;
            }
        }
        Debug.Log("만족되면 여기도 이어서 실행됩니다.");
    }

    public void WrapBodyTexture()//고양이 머티리얼 입혀주기
    {
        catphoto = photos[0];
        catbody.mainTexture = catphoto;
        Debug.Log("body 텍스쳐 적용완료");
    }

}
