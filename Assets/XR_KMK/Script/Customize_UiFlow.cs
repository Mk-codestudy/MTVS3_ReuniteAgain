using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [Header("반려동물 커스터마이징 영역")]
    public GameObject infoPannal;
    public TextMeshProUGUI userEmail; //구글 이메일
    public TextMeshProUGUI userInfo; //강아지, 고양이, 비반려인
    public TextMeshProUGUI userName; //유저 닉네임
    public TextMeshProUGUI petName; //반려동물 이름
    public bool isLost; //반려동물 이별 여부

    [Header("반려인/비반려인")]
    public GameObject guestOrNot;

    public GameObject choicepannal;//강아지, 고양이 패널

    public GameObject userQnA; //앙케이트
    public GameObject guestPannal; //게스트 선택창

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


        mainCanvas.SetActive(false); //메인화면 나가
                                     //loginCanvas.SetActive(false); //로그인 팝업 나가

        customizeCanvas.SetActive(true); //커스터마이징 캔버스 실행

        if (isProto) userEmail.text = "UserID: example@gmail.com"; //유저의 이메일 정보 적용

        infoPannal.SetActive(true);//유저 정보 패널 켜기
        guestOrNot.SetActive(true); //반려인 비반려인

    }

    //반려인/비반려인 여부 선택
    public void SelectOwner()
    {
        guestOrNot.SetActive(false);

        if (!isProto) choicepannal.SetActive(true); //프로토 아닐때는 강아지/고양이 선택
        else
        {
            userQnA.SetActive(true);
            //프로토일땐 무조건 고양이로 넘어가게 해놓기
        }

    }
    public void SelectGuest()
    {
        guestOrNot.SetActive(false);


    }

}
