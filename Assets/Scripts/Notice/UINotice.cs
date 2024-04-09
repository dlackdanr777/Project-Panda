using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UINotice : MonoBehaviour
{
    [Space]
    [Header("Components")]
    [SerializeField] private Transform _slotParent;
    [SerializeField] private UINoticeSlot _slotPrefab;
    [SerializeField] private UINoticeDetailView _detailView;


    private List<UINoticeSlot> _slotList = new List<UINoticeSlot>();

    public void Init()
    {
        _detailView.Init();

        List<Notice> noticeList = DatabaseManager.Instance.NoticeDatabase.GetNoticeList();

        for (int i = 0, count = noticeList.Count; i < count; i++)
        {
            UINoticeSlot slot = Instantiate(_slotPrefab);
            slot.transform.parent = _slotParent;

            int index = i;
            slot.Init(noticeList[i], () =>
            {
                SoundManager.Instance.PlayEffectAudio(SoundEffectType.ButtonClick);
                _detailView.Show(noticeList[index]);
            });
            _slotList.Add(slot);
        }

    }

    public void Show()
    {
        for(int i = 0, count =  _slotList.Count; i < count; i++)
        {
            _slotList[i].SetSprite();
        }
    }


    public void Hide()
    {
        _detailView.Hide();
    }

}
