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
            _data.Item = action;
        }

        private void Enabled()
        {
            _data = DataBind.GetButtonValue(_dataID);
            _data.CallBack += UpdateButton;
            _button.onClick?.AddListener(_data.Item);
        }


        private void Disabled()
        {
            if (_data == null)
                return;

            _data.CallBack -= UpdateButton;
            _button.onClick?.RemoveListener(_data.Item);
        }
    }
}
