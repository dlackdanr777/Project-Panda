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
    public StateData stateData;
    [SerializeField]
    private UIPanda _uiPanda;
    private bool _isUISetActive;

    // 수정 필요
    public StarterPanda(string mbti, Sprite image)
    {
        Nature = mbti;
        State = "행복";
        Intimacy = 0; //친밀도 0으로 시작
        Image = image;
    }

    // 수정 필요
    private void Awake()
    {
        //test
        Nature = "intp";
        State = "행복";
        Intimacy = 0; //친밀도 0으로 시작

        //판다 상태 초기 설정
        stateData = new StateData();
        stateData.InitStateData();
    }

    private void Update()
    {
        PandaMouseClick();
        stateData.ChangeState();
    }

    //판다 클릭하면 버튼 표시
    private void PandaMouseClick()
    {
        //(수정)판다 클릭하는 것으로 변경
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleUIPanda();
        }
    }
    private void ToggleUIPanda()
    {
        _uiPanda.gameObject.SetActive(_isUISetActive);
        _isUISetActive = !_isUISetActive;
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
