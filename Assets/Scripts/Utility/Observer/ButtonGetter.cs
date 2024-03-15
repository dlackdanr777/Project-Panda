using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Muks.DataBind
{
    [RequireComponent(typeof(Button))]
    public class ButtonGetter : MonoBehaviour
    {
        [SerializeField] private string _dataID;
        private Button _button;
        private BindData<UnityAction> _data;
        private UnityAction _action;


        private bool _isAddListenerClear; //��ư�� �Լ��� ��ω糪 �ȉ糪 Ȯ���ϴ� �Լ�
        private void Awake()
        {
            _button = GetComponent<Button>();

            if (string.IsNullOrEmpty(_dataID))
            {
                Debug.LogWarningFormat("Invalid text data ID. {0}", gameObject.name);
                _dataID = gameObject.name;
            }
        }

        private void Start()
        {
            Invoke("Enabled", 0.05f);
        }


        private void OnEnable()
        {
            Invoke("Enabled", 0.05f);
        }


        private void Enabled()
        {
            if (_isAddListenerClear)
                return;

            _data = DataBind.GetButtonValue(_dataID);

            if(_data.Item == null)
            {
                Debug.LogError("�Ѱܹ��� �����Ͱ� �������� �ʽ��ϴ�.");
                return;
            }

            _action = _data.Item;
            _button.onClick.AddListener(_action);
            _isAddListenerClear = true;

            Debug.Log(gameObject.name + "��ư �̺�Ʈ ��� ");
        }


        /// <summary> ��ư �̺�Ʈ�� �Ű� ������ �ѱ� �Լ��� �ִ��� Ȯ���� ��, ������ ��ȯ �ϴ� �Լ� </summary>
        private bool IsListenerRegistered(UnityAction method)
        {
            var listeners = _button.onClick.GetPersistentEventCount();

            for (int i = 0; i < listeners; i++)
            {
                if (_button.onClick.GetPersistentMethodName(i) == method.Method.Name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}