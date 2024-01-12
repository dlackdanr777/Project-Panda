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

        private void Awake()
        {
            _button = GetComponent<Button>();

            if (string.IsNullOrEmpty(_dataID))
            {
                Debug.LogWarningFormat("Invalid text data ID. {0}", gameObject.name);
                _dataID = gameObject.name;
            }
        }


        private void OnEnable()
        {
            Invoke("Enabled", 0.02f);
        }


        private void OnDisable()
        {
            Invoke("Disabled", 0.02f);
        }


        private void UpdateButton(UnityAction action)
        {
            _action = action;

            if (IsListenerRegistered(_action))
                _button.onClick?.RemoveListener(_action);

            _button.onClick.AddListener(_action);
        }


        private void Enabled()
        {
            _data = DataBind.GetButtonValue(_dataID);
            _data.CallBack += UpdateButton;
            _button.onClick?.AddListener(_data.Item);
        }


        private void Disabled()
        {
            _data.CallBack -= UpdateButton;
        }


        private void OnDestroy()
        {
            if (_data == null)
                return;

            _data.CallBack -= UpdateButton;
        }


        /// <summary> 버튼 이벤트로 매개 변수로 넘긴 함수가 있는지 확인해 참, 거짓을 반환 하는 함수 </summary>
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