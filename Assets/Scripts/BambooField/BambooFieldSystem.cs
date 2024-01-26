using Muks.Tween;
using UnityEngine;

public class BambooFieldSystem : MonoBehaviour
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
    #endregion

    #region UI
    public GameObject _UIBambooField;
    [SerializeField] private GameObject _UIBamboo;
    #endregion

    public HarvestButton HarvestButton;
    private Vector3 _targetPos;

    [SerializeField] private GameObject _harvestBamboos;
    [SerializeField] private GameObject _harvestBamboo;
    private GameObject[] _bambooPrefabs = new GameObject[300];
    private int _bambooPrefabCount;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        Invoke("FirstCheckGrowth", 0.1f);
    }

    private void Init()
    {
        _fieldSlots = _fieldSlotParent.GetComponentsInChildren<FieldSlot>();
        _fieldIndex = _fieldSlots.Length - 1;

        for (int i = 0; i <= _fieldIndex; i++)
        {
            _fieldSlots[i].Init(this, "BM001");// 0: �켱 �׼�ID�� ����
        }
        for(int i = 0; i < _bambooPrefabs.Length; i++)
        {
            _bambooPrefabs[i] = Instantiate(_harvestBamboo, _harvestBamboos.transform);
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
            HarvestBamboo(0, _fieldSlots[i].Yield, _fieldSlots[i].transform);
            _fieldSlots[i].Yield = 0;

            // ���� �ܰ� �ʱ�ȭ
            _fieldSlots[i].GrowthStage = 0;
            _fieldSlots[i].ChangeGrowthStageImage(_fieldSlots[i].GrowthStage);
        }
        Tween.SpriteRendererAlpha(HarvestButton.gameObject, 0, 0.5f, TweenMode.Quadratic, () => { HarvestButton.IsSet = false; });
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
            if (currentCount != totalCount)
            { 
                HarvestBamboo(currentCount + 1, totalCount, fieldSlotTransform);
            }
        });

        Tween.TransformMove(_bambooPrefabs[count], _targetPos, 2, TweenMode.Quadratic, () =>
        {
            _player.GainBamboo(1);
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
            if ((_database.UserInfo.TODAY - _database.UserInfo.LastAccessDay).Minutes >= _fieldSlots[i].GrowthTime * 2)
            {
                _fieldSlots[i].GrowingCrops(2);
            }
            else if ((_database.UserInfo.TODAY - _database.UserInfo.LastAccessDay).Minutes >= _fieldSlots[i].GrowthTime)
            {
                _fieldSlots[i].GrowingCrops(1);
            }
        }
    }

}
