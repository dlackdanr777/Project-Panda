using System;
using UnityEditor.Events;
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
            Invoke("Enabled", 0.02f);
        }


        private void OnEnable()
        {
            Invoke("Enabled", 0.02f);
        }


        private void Enabled()
        {
            _data = DataBind.GetButtonValue(_dataID);

            if(_data.Item == null)
            {
                Debug.LogError("�Ѱܹ��� �����Ͱ� �������� �ʽ��ϴ�.");
                return;
            }

            if (IsListenerRegistered(_data.Item))
                return;

            _action = _data.Item;
            UnityEventTools.AddPersistentListener(_button.onClick, _action);
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


        private void RemoveListener(UnityAction method)
        {
            var listeners = _button.onClick.GetPersistentEventCount();

            for (int i = 0; i < listeners; i++)
            {
                if (_button.onClick.GetPersistentMethodName(i) == method.Method.Name)
                {
                    UnityEventTools.RemovePersistentListener(_button.onClick, i);
                }
            }
        }

    }
}