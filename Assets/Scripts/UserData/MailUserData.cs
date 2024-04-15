using BackEnd;
using LitJson;
using Muks.BackEnd;
using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary> 유저의 메일 정보를 보관하는 클래스 </summary>
public class MailUserData
{
    private MessageList[] _mailLists = new MessageList[Enum.GetValues(typeof(MessageField)).Length - 1];
    private List<string> _mailRecivedList = new List<string>();
    private List<ServerMessageData> MessageDataArray = new List<ServerMessageData>(); //저장할 메시지 데이터


    public MailUserData()
    {
        for (int i = 0; i < _mailLists.Length - 1; i++)
        {
            _mailLists[i] = new MessageList();
        }
    }

    /// <summary> 메일 리스트 반환 </summary>
    public MessageList GetMailList(MailType mailType)
    {
        return _mailLists[(int)mailType];
    }


    /// <summary> 메일 리스트에서 메일을 찾는 함수 </summary>
    public bool FindMailReceivedById(string id)
    {
        for (int i = 0, count = _mailRecivedList.Count; i < count; i++)
        {
            if (_mailRecivedList[i] == id)
                return true;
        }

        return false;
    }


    /// <summary> 받은 메일 리스트에 메일을 추가하는 함수 </summary>
    public void AddMailReceived(string id)
    {
        if (_mailRecivedList.Find((x) => x == id) != null)
        {
            Debug.Log("이미 존재하는 메일 입니다.");
            return;
        }

        _mailRecivedList.Add(id);
    }


    #region Save&Load Mail

    /// <summary> 동기적으로 서버 유저 메일 정보를 불러옴 </summary>
    public void LoadMailData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("데이터가 존재하지 않습니다.");
            return;
        }

        else
        {
            for (int i = 0, count = json[0]["MessageDataArray"].Count; i < count; i++)
            {
                ServerMessageData item = JsonUtility.FromJson<ServerMessageData>(json[0]["MessageDataArray"][i].ToJson());
                MessageDataArray.Add(item);
            }
            LoadUserMailData();

            for (int i = 0, count = json[0]["MailRecivedList"].Count; i < count; i++)
            {
                string id = json[0]["MailRecivedList"].ToString();
                _mailRecivedList.Add(id);
            }


            Debug.Log("Mail Load성공");
        }
    }

    /// <summary> 동기적으로 서버 유저 도감 정보 저장 </summary>
    public void SaveMailData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Mail";

        if (!Backend.IsLogin)
        {
            Debug.LogError("뒤끝에 로그인 되어있지 않습니다.");
            return;
        }

        if (maxRepeatCount <= 0)
        {
            Debug.LogErrorFormat("{0} 차트의 정보를 받아오지 못했습니다.", selectedProbabilityFileId);
            return;
        }

        BackendReturnObject bro = Backend.GameData.Get(selectedProbabilityFileId, new Where());

        switch (BackendManager.Instance.ErrorCheck(bro))
        {
            case BackendState.Failure:
                break;

            case BackendState.Maintainance:
                break;

            case BackendState.Retry:
                SaveMailData(maxRepeatCount - 1);
                break;

            case BackendState.Success:

                if (bro.GetReturnValuetoJSON() != null)
                {
                    if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                    {
                        InsertMailData(selectedProbabilityFileId);
                    }
                    else
                    {
                        UpdateMailData(selectedProbabilityFileId, bro.GetInDate());
                    }
                }
                else
                {
                    InsertMailData(selectedProbabilityFileId);
                }

                Debug.LogFormat("{0} 정보를 저장했습니다..", selectedProbabilityFileId);
                break;
        }
    }


    /// <summary> 비동기적으로 서버 유저 도감 정보 저장 </summary>
    public void AsyncSaveMailData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Mail";

        if (!Backend.IsLogin)
        {
            Debug.LogError("뒤끝에 로그인 되어있지 않습니다.");
            return;
        }

        if (maxRepeatCount <= 0)
        {
            Debug.LogErrorFormat("{0} 차트의 정보를 받아오지 못했습니다.", selectedProbabilityFileId);
            return;
        }

        Backend.GameData.Get(selectedProbabilityFileId, new Where(), bro =>
        {
            switch (BackendManager.Instance.ErrorCheck(bro))
            {
                case BackendState.Failure:
                    break;

                case BackendState.Maintainance:
                    break;

                case BackendState.Retry:
                    AsyncSaveMailData(maxRepeatCount - 1);
                    break;

                case BackendState.Success:

                    if (bro.GetReturnValuetoJSON() != null)
                    {
                        if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                        {
                            AsyncInsertMailData(selectedProbabilityFileId);
                        }
                        else
                        {
                            AsyncUpdateMailData(selectedProbabilityFileId, bro.GetInDate());
                        }
                    }
                    else
                    {
                        AsyncInsertMailData(selectedProbabilityFileId);
                    }

                    Debug.LogFormat("{0} 정보를 저장했습니다..", selectedProbabilityFileId);
                    break;
            }
        });
    }


    /// <summary> 동기적으로 서버 유저 도감 정보 삽입 </summary>
    private void InsertMailData(string selectedProbabilityFileId)
    {
        SaveUserMailData();
        SaveMailReceived();
        Param param = GetMailParam();
        Debug.LogFormat("{0} 데이터 삽입을 요청합니다.", selectedProbabilityFileId);
        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> 동기적으로 서버 유저 도감 정보 수정 </summary>
    private void UpdateMailData(string selectedProbabilityFileId, string inDate)
    {
        SaveUserMailData();
        SaveMailReceived();

        Param param = GetMailParam();
        Debug.LogFormat("{0} 데이터 수정을 요청합니다.", selectedProbabilityFileId);
        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> 비동기적으로 서버 유저 도감 정보 삽입 </summary>
    private void AsyncInsertMailData(string selectedProbabilityFileId)
    {
        SaveUserMailData();
        SaveMailReceived();
        Param param = GetMailParam();
        Debug.LogFormat("{0} 데이터 삽입을 요청합니다.", selectedProbabilityFileId);
        BackendManager.Instance.AsyncGameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> 비동기적으로 서버 유저 도감 정보 수정 </summary>
    private void AsyncUpdateMailData(string selectedProbabilityFileId, string inDate)
    {
        SaveUserMailData();
        SaveMailReceived();

        Param param = GetMailParam();
        Debug.LogFormat("{0} 데이터 수정을 요청합니다.", selectedProbabilityFileId);
        BackendManager.Instance.AsyncGameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }



    /// <summary> 서버에 저장할 메일 데이터를 모아 반환하는 클래스 </summary>
    private Param GetMailParam()
    {
        Param param = new Param();
        param.Add("MessageDataArray", MessageDataArray);
        param.Add("MailRecivedList", _mailRecivedList);
        return param;
    }


    private void SaveUserMailData()
    {
        MessageDataArray.Clear();

        for (int i = 0; i < _mailLists.Length; i++)
        {
            if (_mailLists[i] == null)
                continue;

            List<Message> mailList = _mailLists[i].GetMessageList();
            for (int j = 0; j < _mailLists[i].MessagesCount; j++)
            {
                string id = mailList[j].Id;
                bool isCheck = mailList[j].IsCheck;
                bool isReceived = mailList[j].IsReceived;

                MessageDataArray.Add(new ServerMessageData(id, isCheck, isReceived));
            }

        }
    }


    private void LoadUserMailData()
    {
        for (int i = 0; i < MessageDataArray.Count; i++)
        {
            if (!MessageDataArray[i].Id.StartsWith("ML"))
                continue;

            _mailLists[0].AddById(MessageDataArray[i].Id, MessageField.Mail, false);
            _mailLists[0].GetMessageList()[i].IsCheck = MessageDataArray[i].IsCheck;
            _mailLists[0].GetMessageList()[i].IsReceived = MessageDataArray[i].IsReceived;
        }
    }


    private void SaveMailReceived()
    {
        List<Message> mailList = new List<Message>();
        mailList.AddRange(_mailLists[(int)MailType.Mail].GetMessageList());
        for (int i = 0, count = mailList.Count; i < count; i++)
        {
            if (_mailRecivedList.Find((x) => x == mailList[i].Id) != null)
                continue;

            _mailRecivedList.Add(mailList[i].Id);
        }
    }

    #endregion
}
