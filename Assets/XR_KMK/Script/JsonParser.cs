using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //파일 입력/출력을 위해
using System.Text;
using System.IO; //한글 문자 처리를 할 수 있게 표쥰 규약인 UTF-8 어쩌구 구현


[System.Serializable] //직렬화
public struct UserData //struct? :: class의 구버전.<구조체> 데이터 처리는 얘가 더 빠르다.
{
    //public long userId;
    public string username;
    public int usertype; //1: 강아지, 2: 고양이, 3: 게스트
    public string petname;
    public int petage;
    public bool petgender;
    public bool islost;


    //아래와 같은 코드로 Vector(값, 값, 값)과 같이 괄호 안에 바로 변수 수치 부여가능
    public UserData(string username, int usertype, string petname, int petage, bool petgender, bool islost)
    {
        this.username = username;
        this.usertype = usertype;
        this.petname = petname;
        this.petage = petage;
        this.petgender = petgender;
        this.islost = islost;
    }
}

public class JsonParser : MonoBehaviour
{
    void Start()
    {
        #region Json데이터 만들고 저장하는 코드
        //구조체 인스턴스를 만든다
        UserData userdata1 = new UserData("레오언니", 1, "레오", 13, false, true);
        #region 원시적인 부여방법
        //UserData userdata1 = new UserData();
        //userdata1.username = "레오언니";
        //userdata1.usertype = 1; 
        //userdata1.petname = "레오";
        //userdata1.petage = 13;
        //userdata1.petgender = false;
        //userdata1.islost = true;
        //이런 식으로 부여해줄 수도 있지만 너무 힘드니 Vector3(값, 값, 값)<< 넣어주는 것처럼 함수화해야겟지???? 
        #endregion

        //구조체 데이터를 Json 형태로 변환한다. 
        string jsonUser1 = JsonUtility.ToJson(userdata1, true); //두번째 bool 변수는 이쁘게 배치하느냐 마느냐 여부 (사람 보기좋게)
                                                                //print(jsonUser1);
        #endregion

    }

    //Json을 파일 저장하기
    public void SaveJsonData(string json)
    {
        // 1. 파일 스트림을 쓰기 형태로 연다.
        //FileStream fs = new FileStream(); //1.파일 경로 설정


        // 2. 열린 스트림에 Json 데이터를 쓰기로 전달한다.

        // 3. 스트림을 닫아준다.

        // 
    }

}
