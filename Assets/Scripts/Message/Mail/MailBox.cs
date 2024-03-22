using Muks.DataBind;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MailBox : MonoBehaviour
{
    [SerializeField] private UIMailList _uiMailList;
    [SerializeField] private GameObject _mailNotice;
    private Player player;
    

    private void Awake()
    {
        player = GameManager.Instance.Player;
        player.GetMailList(Player.MailType.Mail).NoticeHandler += SetNotice;
        _uiMailList.NoticeHandler += SetNotice;
    }


    private void SetNotice() //�˸� ����, �˸� ���� ����
    {
        int count = 0;
        for (int i = 0; i < player.GetMailList(Player.MailType.Mail).MessagesCount; i++)
        {
            count += player.GetMailList(Player.MailType.Mail).CurrentNotCheckedMessage;
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
