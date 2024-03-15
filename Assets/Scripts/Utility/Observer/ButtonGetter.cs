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


        private bool _isAddListenerClear; //버튼에 함수가 등록됬나 안됬나 확인하는 함수
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
                Debug.LogError("넘겨받은 데이터가 존재하지 않습니다.");
                return;
            }

            _action = _data.Item;
            _button.onClick.AddListener(_action);
            _isAddListenerClear = true;

            Debug.Log(gameObject.name + "버튼 이벤트 등록 ");
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