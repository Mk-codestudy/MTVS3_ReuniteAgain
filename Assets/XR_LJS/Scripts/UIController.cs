using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject Homebtn;
    public GameObject Edit;
    public GameObject PlaceMode;
    public GameObject ScrollView;
    PlacementSystem PlacementSystem;

    void Start()
    {
        Homebtn.gameObject.SetActive(true);
        Edit.gameObject.SetActive(true);
        PlaceMode.gameObject.SetActive(false);
        ScrollView.gameObject.SetActive(false);
    }

    void Update()
    {
       
    }

    public void EditMode()
    {
        PlaceMode.gameObject.SetActive(true);
    }

    public void PlaceModeActive()
    {
        ScrollView.gameObject.SetActive(true);
    }

    public void Home()
    {
        // 홈, 로비로 가는 함수 -- 아직 모름
    }


}
