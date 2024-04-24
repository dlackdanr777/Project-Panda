using Muks.Tween;
using System;
using System.Collections;
using UnityEngine;

public class Event5_4 : InteractionStoryEvent
{
    public override void EventStart(Action onComplate)
    {
        _nextActionHandler = onComplate;
        // 상점 기능 구현되면 상점에서 잠자리채 사는 것으로 변경
        //잠자리채 구매 시 이벤트 종료
        ShowFollowButton();
    }

    public override void EventCancel(Action onComplate = null)
    {
        HideFollowButton();

    }

    protected override void OnFollowButtonClicked()
    {
        HideFollowButton();

        // 잠자리채 인벤토리로 이동

        _nextActionHandler?.Invoke();

    }
}
