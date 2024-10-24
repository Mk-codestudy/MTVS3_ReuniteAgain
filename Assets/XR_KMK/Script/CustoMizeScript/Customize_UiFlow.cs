using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Customize_UiFlow : MonoBehaviour
{
    [Header("프로토 알파 베타 체크")]
    public bool isProto;
    public bool isAlpha;
    public bool isBeta;

    [Header("메인 화면 및 로그인화면 오브제")]
    public GameObject mainCanvas; //메인화면 UI 캔버스
    public GameObject loginCanvas; //로그인 화면 UI캔버스

    [Header("커스터마이징 캔버스")]
    public GameObject customizeCanvas;
    [Header("세부 커스터마이징")]
    public GameObject customDetail;
    public Camera defultCam;

    [Header("반려동물 커스터마이징 영역")]
    public GameObject infoPannal;
    public TextMeshProUGUI userEmail; //구글 이메일
    public TextMeshProUGUI userInfo; //강아지, 고양이, 비반려인
    public TextMeshProUGUI userName; //유저 닉네임
    public TextMeshProUGUI petName; //반려동물 이름
    public bool isLost; //반려동물 이별 여부
    public bool ItsDog; //강아지일 때 체크. 안내자 UI를 바꿔준다.

    [Header("선택 패널")]
    public GameObject guestOrNot; //반려인, 게스트 패널
    public GameObject choicepannal;//강아지, 고양이 패널
    public GameObject grettingPlayer;//안녕? 네 반려동물의 행복한 기억으로 이 세상을 모험할...
    public GameObject getPhoto; //먼저 사진을 보여줘
    public GameObject userQnA; //앙케이트
    public GameObject mbti;//반려동물 수치

    public GameObject guestPannal; //게스트 선택창

    [Header("인풋 필드")]
    public TMP_InputField nicknamefield;
    public TMP_InputField petnamefield;
    public Toggle toggleisLost;
    public Toggle toggleLostapply;
    Color32 inputcompletecolor = new Color32(79, 79, 79, 255);

    private void Start()
    {
        nicknamefield.onEndEdit.AddListener(TakeNickName);
        petnamefield.onEndEdit.AddListener(TakePetName);
    }


    public void TurnLoginPannal() //구글 연동 로그인 패널을 연다.
    {
        print("로그인 캔버스 실행!");
        loginCanvas.SetActive(true);
    }

    public void TurnAboutUs() //어바웃어스 패널을 클릭한다
    {
        //휠이 쭈르륵 내려갔으면 좋겠써.
        //쭈르륵 내려가면서 프로젝트의 의의와 팀원 소개가 들어갔으면 좋겠씀.
        //휠 내려갈때마다 배경사진이 바뀌었으면 좋겠씀.
        //input중에 휠값도 있지 않을까? 해보자고
    }

    public void LetsLogin() //Google 로그인 버튼을 누른다.
    {
        Debug.Log("커스터마이징 C1 시작!");


        customizeCanvas.SetActive(true); //커스터마이징 캔버스 실행
        mainCanvas.SetActive(false); //메인화면 나가
                                     //loginCanvas.SetActive(false); //로그인 팝업 나가


        if (isProto) userEmail.text = "UserID: example@gmail.com"; //유저의 이메일 정보 적용

        infoPannal.SetActive(true);//유저 정보 패널 켜기
        guestOrNot.SetActive(true); //반려인 비반려인

    }

    //반려인/비반려인 여부 선택
    public void SelectOwner() 
    {
        guestOrNot.SetActive(false);

        choicepannal.SetActive(true); //강아지/고양이 선택
    }
    public void SelectGuest()
    {
        guestOrNot.SetActive(false);

        //다음 항목 아직 안만들엇음!!!!!!!!!!!!!
    }

    public void ItisDog()
    {
        ItsDog = true;
    }

    public void GrettingPlayer() //강아지, 고양이 여부를 선택하면 나오는 함수
    {
        //강아지 주인이면 강아지 이미지가, 고양이 주인이면 고양이 이미지가 뜨게 한다.
        //Proto 시점 고양이만 있다.
        choicepannal.SetActive(false);

        if (ItsDog) //Dog일때 이미지 바꿔주는 변수
        {
            userInfo.text = "강아지";
        }
        else
        {
            userInfo.text = "고양이";
        }

        grettingPlayer.SetActive(true);
    }

    public void GetPhoto() //그리팅을 클릭하면 나오는 함수
    {
        grettingPlayer.SetActive(false);

        getPhoto.SetActive(true);
    }

    public void UserQnA() //사진을 넣고 다음을 누르면 나오는 함수
    {
        getPhoto.SetActive(false);

        userQnA.SetActive(true);
    }

    public void TakeNickName(string input) //인풋필드 함수
    {
        userName.text = input;
        userName.color = inputcompletecolor;
        Debug.Log("유저 닉네임 : " + input);
        // 유저 정보를 저장할 클래스에 유저 이름 추가
    }

    public void TakePetName(string input) //인풋필드 함수
    {
        petName.text = input;
        petName.color = inputcompletecolor;
        Debug.Log("반려동물 이름 : " + input);
    }

    public void PetMBTI() //이름 입력 후 다음 버튼을 눌렀을 때 실행되는 함수
    {
        isLost = toggleisLost.isOn; //토글 버튼에 따라 정보 입력
        toggleLostapply.isOn = isLost;
        userQnA.SetActive(false);

        mbti.SetActive(true); //MBTI 패널 실행
    }

    public void CustomizeDetail() //MBTI까지 입력하고 다음 버튼을 눌렀다. 세부 커스터마이징 단계.
    {
        mbti.SetActive(false);
        customizeCanvas.SetActive(false); //세부 커스텀을 아예 끈다.
        defultCam.gameObject.SetActive(false); //카메라 꺼봐

        customDetail.SetActive(true);
    }

    public void CustomizeExit() //커스터마이징을 끝냈다.
    {
        customDetail.SetActive(false);
        // 씬 이동
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
