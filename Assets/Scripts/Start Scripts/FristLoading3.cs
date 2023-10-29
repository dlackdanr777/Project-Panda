using UnityEngine;

public class FristLoading3 : UIStartList
{
    [Tooltip("캔버스에 있는 UI")]
    [SerializeField] private GameObject _uiFirstLoading3;


    private UIStart _uiStart;

    private bool _isStart;

    public override void Init(UIStart uiStart)
    {
        _uiStart = uiStart;
        _uiFirstLoading3.SetActive(false);
    }

    public override void UIStart()
    {
        if (!_isStart)
        {
            _isStart = true;
            _uiFirstLoading3.SetActive(true);
        }
        
    }
    public override void UIUpdate()
    {
    }

    public override void UIEnd()
    {
    }
}
