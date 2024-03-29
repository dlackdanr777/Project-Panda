using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NPCButton : MonoBehaviour
{
    private Button _button;

    private Transform _targetTransform;


    public void Init(Transform target, Vector2 size, Sprite sprite, UnityAction onClicked)
    {
        name = "[" + target.name + "] NPC Button";
        _button = GetComponent<Button>();
        _button.GetComponent<RectTransform>().sizeDelta = size;
        _button.onClick.AddListener(onClicked);
        _targetTransform = target;
    }

    private void Update()
    {
/*        if (!_targetTransform.gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
*/
        transform.position = Camera.main.WorldToScreenPoint(_targetTransform.position);
    }

}