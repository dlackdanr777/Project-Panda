using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MailBox : MonoBehaviour
{
    [SerializeField] private UIMailList _uiMailList;
    [SerializeField] private GameObject _mailNotice;
    

    private void Awake()
    {
        MailUserData data = DatabaseManager.Instance.UserInfo.MailUserData;
        data.GetMailList(MailType.Mail).NoticeHandler += SetNotice;
        _uiMailList.NoticeHandler += SetNotice;
    }


    private void SetNotice() //�˸� ����, �˸� ���� ����
    {
        MailUserData data = DatabaseManager.Instance.UserInfo.MailUserData;
        int count = 0;
        for (int i = 0; i < data.GetMailList(MailType.Mail).MessagesCount; i++)
        {
            count += data.GetMailList(MailType.Mail).CurrentNotCheckedMessage;
        }
        if (count == 0)
        {
            _mailNotice.SetActive(false);
        }
        else
        {
            //�˸� ���̱�
            _mailNotice.SetActive(true);

            //_notice.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = count.ToString();// �˸� ���� ����
        }
    }
}
