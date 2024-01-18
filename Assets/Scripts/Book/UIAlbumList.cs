using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAlbumList : MonoBehaviour
{
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    
    private List<Album> _albumDatabase;
    private int _current;

    private void Awake()
    {
        _albumDatabase = DatabaseManager.Instance.GetAlbumList();

        _leftButton.onClick.AddListener(() => OnClickPageButton(0));
        _rightButton.onClick.AddListener(() => OnClickPageButton(1));
    }

    private void OnEnable()
    {
        _current = 0;
        UpdateContents();
    }

    private void UpdateContents()
    {
        for(int i=0;i<transform.childCount;i++)
        {
            Transform child = transform.GetChild(i);
            if((_current * 2) + i >= _albumDatabase.Count)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
                
                if (_albumDatabase[(_current * 2) + i].IsReceived)
                {
                    child.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = _albumDatabase[(_current * 2) + i].Description; //설명
                    child.GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = _albumDatabase[(_current * 2) + i].Name; //이름
                    child.GetChild(1).GetChild(1).GetComponent<Image>().sprite = _albumDatabase[(_current * 2) + i].Image; //이미지
                }
                else
                {
                    child.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = ""; //설명

                    Debug.Log(child.GetChild(1).GetChild(0).GetChild(1));
                    child.GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = ""; //이름
                    child.GetChild(1).GetChild(1).GetComponent<Image>().sprite = null; //이미지
                }
            }
        }

    }

    private void OnClickPageButton(int arrow) //arrow: 0 이면 왼쪽, 1이면 오른쪽
    {
        if (arrow == 0)
        {
            _current--;
            if (_current < 0)
            {
                _current = 0;
            }
        }
        else if (arrow == 1)
        {
            _current++;
            if (_current > _albumDatabase.Count / 2)
            {
                    _current = _albumDatabase.Count / 2;
            }
            else if(_current == _albumDatabase.Count / 2)
            {
                if (_albumDatabase.Count % 2 == 0)
                {
                    _current = _albumDatabase.Count / 2 - 1;
                }
            }
        }
        UpdateContents();
    }
}
