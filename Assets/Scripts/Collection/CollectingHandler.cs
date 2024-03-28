using UnityEngine;
using System;
using Newtonsoft.Json.Linq;

public class CollectingHandler : MonoBehaviour
{
    public Action<bool> OnExclamationMarkClicked;

    [SerializeField] private GameObject _checkResultButton;

    private void EndAnimation()
    {
        GetComponent<Animator>().enabled = false;

        // �ִϸ��̼� �Ϸ�Ǹ� ����ǥ ǥ��
        _checkResultButton.SetActive(true);

        gameObject.SetActive(false);
    }
}
