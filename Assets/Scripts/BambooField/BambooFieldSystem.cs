using Muks.DataBind;
using Muks.Tween;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BambooFieldSystem : SingletonHandler<BambooFieldSystem>
{
    #region �ܺ� ����
    //�÷��̾�� ������ ���������� �����Ƿ� �����´�.
    private Player _player => GameManager.Instance.Player;

    //ä�� �������� ������ �������� ������ �Ѵ�.
    //ä�� ������ ���� DatabaseManager�� �߰�
    private DatabaseManager _database => DatabaseManager.Instance;
    #endregion

    #region FieldSlot ����
    [SerializeField] private GameObject _fieldSlotParent;
    //�۹��� ������ ���Ե�
    private FieldSlot[] _fieldSlots;
    private int _fieldIndex;
    private string _growingCropID;
    #endregion

    #region UI
    public GameObject _UIBambooField;
    private GameObject _UIBamboo;
    #endregion

    public HarvestButton HarvestButton;
    private Vector3 _targetPos;

    [SerializeField] private GameObject _harvestBamboos;
    [SerializeField] private GameObject _harvestBamboo;
    private GameObject[] _bambooPrefabs = new GameObject[300];
    private int _bambooPrefabCount;

    // ������ ��
    private List<int> _yield;
    private List<float> _time;
    private List<float> _second; // ���� x
    private List<TimeSpan> _timeDifference;



    public override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += LoadedSceneEvent;
        Init();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= LoadedSceneEvent;
    }

    private void Start()
    {
        for (int i = 0; i <= _fieldIndex; i++)
        {
            // if ������ ����� ���� ���ٸ�
            _yield.Add(0);
            _time.Add(0);
            _second.Add(0);
            _timeDifference.Add(_fieldSlots[i].SetTimeDifference());

            // else 0 ��� ����� �� �������� �ҷ�����

        }
        Invoke("FirstCheckGrowth", 0.1f);
    }

    private void Update()
    {
        for(int i=0; i < _fieldSlots.Length; i++)
        {
            _time[i] += Time.deltaTime;
            _second[i] += Time.deltaTime;
            if (_timeDifference[i] != TimeSpan.Zero)
            {
                _timeDifference[i] = _timeDifference[i] - TimeSpan.FromSeconds(Mathf.FloorToInt(_second[i]));
                _second[i] -= Mathf.FloorToInt(_second[i]);
                _fieldSlots[i].SetRemainingTime(_timeDifference[i]);
            }
            else
            {
                _timeDifference[i] = TimeSpan.Zero;
                _fieldSlots[i].SetRemainingTimeZero();
            }

            if (_fieldSlots[i].HarvestItem != null)
            {
                _time[i] = _fieldSlots[i].IsIncreaseYields(_time[i]);
                _yield[i] = _fieldSlots[i].Yield;
            }
            else
            {
                Debug.Log("HarvestItem null");
                _fieldSlots[i].HarvestItem = DatabaseManager.Instance.GetHarvestItemdata(_growingCropID);
            }

        }
    }

    private void Init()
    {
        _fieldSlots = _fieldSlotParent.GetComponentsInChildren<FieldSlot>();
        _fieldIndex = _fieldSlots.Length - 1;

        _yield = new List<int>();
        _time = new List<float>();
        _second = new List<float>();
        _timeDifference = new List<TimeSpan>();

        _growingCropID = "BM001";

        for (int i = 0; i <= _fieldIndex; i++)
        {
            _fieldSlots[i].Init(this, _growingCropID); // 0: �켱 �׼�ID�� ����
        }
        for(int i = 0; i < _bambooPrefabs.Length; i++)
        {
            _bambooPrefabs[i] = Instantiate(_harvestBamboo, _harvestBamboos.transform);
        }
    }

    private void LoadedSceneEvent(Scene scene, LoadSceneMode mode)
    {
        GameObject uiBambooFieldParent = GameObject.Find("UIBambooFieldParent");
        _UIBamboo = GameObject.Find("UIBambooImage");
        if(uiBambooFieldParent != null)
        {
            _UIBambooField = uiBambooFieldParent.transform.GetChild(0).gameObject;
            for (int i = 0; i <= _fieldIndex; i++)
            {
                foreach (Transform child in _fieldSlots[i].gameObject.transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            Debug.Log("���� �������� �ʴ� ���Դϴ�.");
            for (int i = 0; i <= _fieldIndex; i++)
            {
                foreach (Transform child in _fieldSlots[i].gameObject.transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }

    }


    public void AddFieldSlot()
    {
        _fieldSlots[++_fieldIndex].gameObject.SetActive(true);
    }

    /// <summary>
    /// �۹� ��Ȯ ��ư Ŭ�� </summary>
    public void ClickHavestButton()
    {
        // ��Ȯ
        _targetPos = Camera.main.ScreenToWorldPoint(_UIBamboo.transform.position);
        for (int i = 0; i <= _fieldIndex; i++)
        {
            HarvestBamboo(1, _fieldSlots[i].Yield, _fieldSlots[i].transform);
            _fieldSlots[i].Yield = 0;

            // ���� �ܰ� �ʱ�ȭ
            _fieldSlots[i].GrowthStage = 0;
            _fieldSlots[i].ChangeGrowthStageImage(_fieldSlots[i].GrowthStage);
            _timeDifference[i] = _fieldSlots[i].SetTimeDifference();
            _time[i] = 0;
            _second[i] = 0;
        }
        //Tween.SpriteRendererAlpha(HarvestButton.gameObject, 0, 0.5f, TweenMode.Quadratic, () => { HarvestButton.IsSet = false; });
        HarvestButton.IsSet = false;
    }

    /// <summary>
    /// �볪�� ��Ȯ </summary>
    public void HarvestBamboo(int currentCount, int totalCount, Transform fieldSlotTransform)
    {
            int count = _bambooPrefabCount;
            _bambooPrefabCount = (_bambooPrefabCount + 1) % _bambooPrefabs.Length;
            _bambooPrefabs[count].transform.position = fieldSlotTransform.position;
            _bambooPrefabs[count].SetActive(true);
            Vector3 addPosition = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0);
            _bambooPrefabs[count].GetComponent<Animator>().enabled = true;


            Tween.TransformMove(_bambooPrefabs[count], fieldSlotTransform.position + addPosition, 0.05f, TweenMode.Quadratic, () =>
            {
                if (totalCount - currentCount > 10)
                {
                    HarvestBamboo(currentCount + 10, totalCount, fieldSlotTransform);
                }
            });

            Tween.TransformMove(_bambooPrefabs[count], _targetPos, 1.5f, TweenMode.Quadratic, () =>
            {
                if(totalCount - currentCount <= 10) 
                {
                    for (int i = currentCount; i <= totalCount; i++)
                    {
                        _player.GainBamboo(1);
                    }
                }
                else
                {
                    for (int i = 0; i < 10; i++)
                    {
                        _player.GainBamboo(1);
                    }
                }
                _bambooPrefabs[count].SetActive(false);
            });

    }



    /// <summary>
    /// ���� ������ ���� ���ӽð��� �ð� ���̸� Ȯ�� �� �۹� ���� </summary>
    private void FirstCheckGrowth()
    {
        for(int i = 0; i <= _fieldIndex; i++)
        {
            //���� ���� �ð��� ������ ���� �ð��� ��
            if ((_database.UserInfo.TODAY - _database.UserInfo.LastAccessDay).Minutes + _time[i] >= _fieldSlots[i].GrowthTime * 2 * _fieldSlots[i].HarvestItem.HarvestTime)
            {
                //_fieldSlots[i].GrowingCrops(2);

                // �۹� ���� �ð� �ִ�
                _time[i] = _fieldSlots[i].GrowthTime * 2 * _fieldSlots[i].HarvestItem.HarvestTime;
            }
            else
            {
                // _fieldSlots[i].GrowingCrops(1);

                // �ۿ� �ִ� �ð� _time�� �߰�
                _time[i] += (_database.UserInfo.TODAY - _database.UserInfo.LastAccessDay).Minutes;
            }
        }
    }

}
