using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField] private GameObject _startWallPaper;
    [SerializeField] private GameObject _wallPaper;
    [SerializeField] private GameObject _spotlight;
    [SerializeField] private Sprite _spotlightImage;

    void Start()
    {
        Invoke("StartWallPaper", 11f);
    }

    void StartWallPaper()
    {
        _wallPaper.GetComponent<Animator>().enabled = true;
        _startWallPaper.SetActive(false);
    }
}
