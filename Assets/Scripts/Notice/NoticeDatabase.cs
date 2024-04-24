using BackEnd;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;

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

                LoadImage(notice);
                _noticeList.Add(notice);

            }

            _repeatCount = _maxRepeatCount;
            UnityEngine.Debug.Log("공지사항 불러오기 성공");
        });
    }

    private void LoadImage(Notice notice)
    {
        if (notice.Sprite != null || string.IsNullOrEmpty(notice.ImageKey))
            return;

        UnityWebRequest webRequest = new UnityWebRequest(notice.ImageKey);
        webRequest.useHttpContinue = false;
        DownloadHandlerTexture dlTex = new DownloadHandlerTexture(true);
        webRequest.downloadHandler = dlTex;

        webRequest.SendWebRequest().completed += (asyncOperation) =>
        {
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                UnityEngine.Debug.LogError("Failed to download image: " + webRequest.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                notice.Sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            }

        };
    }

}
