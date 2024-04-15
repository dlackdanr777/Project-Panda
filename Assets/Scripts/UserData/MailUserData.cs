using BackEnd;
using LitJson;
using Muks.BackEnd;
using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary> ������ ���� ������ �����ϴ� Ŭ���� </summary>
public class MailUserData
{
    private MessageList[] _mailLists = new MessageList[Enum.GetValues(typeof(MessageField)).Length - 1];
    private List<string> _mailRecivedList = new List<string>();
    private List<ServerMessageData> MessageDataArray = new List<ServerMessageData>(); //������ �޽��� ������


    public MailUserData()
    {
        for (int i = 0; i < _mailLists.Length - 1; i++)
        {
            _mailLists[i] = new MessageList();
        }
    }

    /// <summary> ���� ����Ʈ ��ȯ </summary>
    public MessageList GetMailList(MailType mailType)
    {
        return _mailLists[(int)mailType];
    }


    /// <summary> ���� ����Ʈ���� ������ ã�� �Լ� </summary>
    public bool FindMailReceivedById(string id)
    {
        for (int i = 0, count = _mailRecivedList.Count; i < count; i++)
        {
            if (_mailRecivedList[i] == id)
                return true;
        }

        return false;
    }


    /// <summary> ���� ���� ����Ʈ�� ������ �߰��ϴ� �Լ� </summary>
    public void AddMailReceived(string id)
    {
        if (_mailRecivedList.Find((x) => x == id) != null)
        {
            Debug.Log("�̹� �����ϴ� ���� �Դϴ�.");
            return;
        }

        _mailRecivedList.Add(id);
    }


    #region Save&Load Mail

    /// <summary> ���������� ���� ���� ���� ������ �ҷ��� </summary>
    public void LoadMailData(BackendReturnObject callback)
    {
        JsonData json = callback.FlattenRows();

        if (json.Count <= 0)
        {
            Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
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


            Debug.Log("Mail Load����");
        }
    }

    /// <summary> ���������� ���� ���� ���� ���� ���� </summary>
    public void SaveMailData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Mail";

        if (!Backend.IsLogin)
        {
            Debug.LogError("�ڳ��� �α��� �Ǿ����� �ʽ��ϴ�.");
            return;
        }

        if (maxRepeatCount <= 0)
        {
            Debug.LogErrorFormat("{0} ��Ʈ�� ������ �޾ƿ��� ���߽��ϴ�.", selectedProbabilityFileId);
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

                Debug.LogFormat("{0} ������ �����߽��ϴ�..", selectedProbabilityFileId);
                break;
        }
    }


    /// <summary> �񵿱������� ���� ���� ���� ���� ���� </summary>
    public void AsyncSaveMailData(int maxRepeatCount)
    {
        string selectedProbabilityFileId = "Mail";

        if (!Backend.IsLogin)
        {
            Debug.LogError("�ڳ��� �α��� �Ǿ����� �ʽ��ϴ�.");
            return;
        }

        if (maxRepeatCount <= 0)
        {
            Debug.LogErrorFormat("{0} ��Ʈ�� ������ �޾ƿ��� ���߽��ϴ�.", selectedProbabilityFileId);
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

                    Debug.LogFormat("{0} ������ �����߽��ϴ�..", selectedProbabilityFileId);
                    break;
            }
        });
    }


    /// <summary> ���������� ���� ���� ���� ���� ���� </summary>
    private void InsertMailData(string selectedProbabilityFileId)
    {
        SaveUserMailData();
        SaveMailReceived();
        Param param = GetMailParam();
        Debug.LogFormat("{0} ������ ������ ��û�մϴ�.", selectedProbabilityFileId);
        BackendManager.Instance.GameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> ���������� ���� ���� ���� ���� ���� </summary>
    private void UpdateMailData(string selectedProbabilityFileId, string inDate)
    {
        SaveUserMailData();
        SaveMailReceived();

        Param param = GetMailParam();
        Debug.LogFormat("{0} ������ ������ ��û�մϴ�.", selectedProbabilityFileId);
        BackendManager.Instance.GameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }


    /// <summary> �񵿱������� ���� ���� ���� ���� ���� </summary>
    private void AsyncInsertMailData(string selectedProbabilityFileId)
    {
        SaveUserMailData();
        SaveMailReceived();
        Param param = GetMailParam();
        Debug.LogFormat("{0} ������ ������ ��û�մϴ�.", selectedProbabilityFileId);
        BackendManager.Instance.AsyncGameDataInsert(selectedProbabilityFileId, 10, param);
    }


    /// <summary> �񵿱������� ���� ���� ���� ���� ���� </summary>
    private void AsyncUpdateMailData(string selectedProbabilityFileId, string inDate)
    {
        SaveUserMailData();
        SaveMailReceived();

        Param param = GetMailParam();
        Debug.LogFormat("{0} ������ ������ ��û�մϴ�.", selectedProbabilityFileId);
        BackendManager.Instance.AsyncGameDataUpdate(selectedProbabilityFileId, inDate, 10, param);
    }



    /// <summary> ������ ������ ���� �����͸� ��� ��ȯ�ϴ� Ŭ���� </summary>
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
