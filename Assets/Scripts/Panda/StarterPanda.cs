using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 모든 컴포넌트를 관리하는 컨테이너 클래스
/// 이곳에서 모든 컴포넌트의 함수를 실행 및 관리한다.
/// 이유: 판다에 속한 컴포넌트들이 서로 참조를 하지 않도록 하여 확장성과 유연성을 올릴 수 있다.
/// </summary>
public class StarterPanda : Panda
{
    [SerializeField]
    private UIPanda _uiPanda;
    private bool _isUISetActive;
    public PreferenceData preferenceData;
    public MBTIData mbtiData;

    // 수정 필요 - NPC와의 공통 부분 Panda로 옮기기
    private void Awake()
    {
        //test - 판다 상태 초기 설정
        Nature = MBTIData.MBTI.intp;
        Intimacy = 0; //친밀도 0으로 시작
        //(수정)mbti 취향 설정
        //mbtiData = new MBTIData();
        //mbtiData.SetMBTI();
        //preferenceData = mbtiData._preferenceDatas[(int)Nature];

        stateData = new StateData();
        stateData.InitStateData();
        _uiPanda.gameObject.SetActive(true);
        _isUISetActive = false;
    }

    private void Update()
    {
        PandaMouseClick();
        stateData.CheckState();
    }

    //판다 클릭하면 버튼 표시
    private void PandaMouseClick()
    {
        //(수정)판다 클릭하는 것으로 변경
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleUIPandaButton();
        }
    }
    public void ToggleUIPandaButton()
    {
        _isUISetActive = !_isUISetActive;
        _uiPanda.transform.GetChild(0).gameObject.SetActive(_isUISetActive);
    }

    private void TakeGift()
    {
        // 장난감인지 간식인지 확인,.
        // 선물 받으면 취향 클래스에서 취향인지 확인
        // 취향에 따라 상태 클래스를 이용해 상태 변경

    }
    public override void AddIntimacy()
    {
        throw new System.NotImplementedException();
    }
    public override void SubIntimacy()
    {
        throw new System.NotImplementedException();
    }
}
