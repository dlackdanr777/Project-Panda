using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


namespace Muks.DataBind
{
    public class GameObjectActiveGetter : MonoBehaviour
    {
        [SerializeField] private string _dataID;

        private BindData<bool> _data;

        private void Awake()
        {
            if (string.IsNullOrEmpty(_dataID))
            {
                Debug.LogErrorFormat("Invalid text data ID. {0}", gameObject.name);
                enabled = false;
            }

            _data = DataBind.GetBoolBindData(_dataID);
            _data.CallBack += UpdateSetActive;  
        }


        private void OnDestroy()
        {
            _data.CallBack -= UpdateSetActive;
            _data = null;
        }


        private void UpdateSetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}

