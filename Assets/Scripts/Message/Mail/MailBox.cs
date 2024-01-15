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
        player.Messages[0].NoticeHandler += SetNotice;
        _uiMailList.NoticeHandler += SetNotice;
    }

    private void SetNotice() //�˸� ����, �˸� ���� ����
    {
        int count = 0;
        for (int i = 0; i < player.Messages[0].MessagesCount; i++)
        {
            count += player.Messages[0].CurrentNotCheckedMessage;
        }
        Debug.Log("count : " + count);
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
