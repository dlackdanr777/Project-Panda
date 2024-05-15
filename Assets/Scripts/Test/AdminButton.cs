using Muks.BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminButton : MonoBehaviour
{
    [SerializeField] private Button _button;


    // Start is called before the first frame update
    void Start()
    {
        _button.onClick.AddListener(() => BackendManager.Instance.UpdateNickName("Admin" + Random.Range(1000000, 2000000)));
    }

}
