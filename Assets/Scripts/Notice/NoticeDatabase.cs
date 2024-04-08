using BackEnd;
using LitJson;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class NoticeDatabase
{
    private const int _maxRepeatCount = 10;

    private List<Notice> _noticeList;
    private int _repeatCount;
    

    public NoticeDatabase()
    {
        _noticeList = new List<Notice>();
        _repeatCount = _maxRepeatCount;
    }


    public List<Notice> GetNoticeList()
    {
        return _noticeList;
    }


    public void LoadData()
    {
        if (_repeatCount <= 0)
            return;

        Backend.Notice.NoticeList((callback) =>
        {
            if (!callback.IsSuccess())
            {
                _repeatCount--;
                LoadData();
                return;
            }

            _noticeList.Clear();
            JsonData jsonList = callback.FlattenRows();

            for(int i = 0, count = jsonList.Count; i < count; i++)
            {
 
                string title = jsonList[i]["title"].ToString();
                string contents = jsonList[i]["content"].ToString();
                DateTime postingDate = DateTime.Parse(jsonList[i]["postingDate"].ToString());
                string inDate = jsonList[i]["inDate"].ToString();
                string uuid = jsonList[i]["uuid"].ToString();
                bool isPublic = jsonList[i]["isPublic"].ToString() == "y" ? true : false;
                string author = jsonList[i]["author"].ToString();

                string imageKey = string.Empty;
                string linkUrl = string.Empty;
                string lunkButtonName = string.Empty;

                if (jsonList[i].ContainsKey("imageKey"))
                {
                    imageKey = "http://upload-console.thebackend.io" + jsonList[i]["imageKey"].ToString();
                }
                if (jsonList[i].ContainsKey("linkUrl"))
                {
                    linkUrl = jsonList[i]["linkUrl"].ToString();
                }
                if (jsonList[i].ContainsKey("linkButtonName"))
                {
                    lunkButtonName = jsonList[i]["linkButtonName"].ToString();
                }

                Notice notice = new Notice(title, contents, postingDate, imageKey, inDate, uuid, linkUrl, isPublic, lunkButtonName, author);

                _noticeList.Add(notice);
            }

            _repeatCount = _maxRepeatCount;
            UnityEngine.Debug.Log("공지사항 불러오기 성공");
        });


    }

}
