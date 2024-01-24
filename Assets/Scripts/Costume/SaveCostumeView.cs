using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Muks.Tween;
using TMPro;
using BT;

public class SaveCostumeView : MonoBehaviour
{
    [SerializeField] private GameObject _saveCostume;
    [SerializeField] private TextMeshProUGUI _saveText;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;
    [SerializeField] private Button _notExitButton;

    private CostumeViewModel _costumeViewModel;

    private float _loadTime = 0.05f; // 창 띄우고 끄는데 걸리는 시간

    // CostumeView 이후에 실행돼야 함
    void Start()
    {
        Invoke("Init", 1f);
    }

    private void OnDestroy()
    {
        _costumeViewModel.CostumeSceneChanged -= SaveCostume;
    }

    private void Init()
    {
        _yesButton.onClick.AddListener(OnSaveYesButtonClicked);
        _noButton.onClick.AddListener(OnSaveNoButtonClicked);
        _notExitButton.onClick.AddListener(NotExitCostumeButtonClicked);

        Bind();
    }

    private void Bind()
    {
        _costumeViewModel = DatabaseManager.Instance.StartPandaInfo.CostumeViewModel;
        if (_costumeViewModel != null)
        {
            _costumeViewModel.CostumeSceneChanged += SaveCostume;
        }

    }

    private void SaveCostume(bool IsExit)
    {
        _saveCostume.gameObject.SetActive(true);

        Tween.IamgeAlpha(_saveCostume, 1, _loadTime, TweenMode.Quadratic);
        Tween.IamgeAlpha(_yesButton.gameObject, 1, _loadTime, TweenMode.Quadratic);
        Tween.IamgeAlpha(_noButton.gameObject, 1, _loadTime, TweenMode.Quadratic);
        //Tween.TextAlpha(_saveText.gameObject, 1, _loadTime, TweenMode.Quadratic);
    }

    private void OnSaveYesButtonClicked()
    {
        StarterPanda.Instance.gameObject.SetActive(true);
        StarterPanda.Instance.IsSwitchingScene = true;
        _costumeViewModel.SaveCostume();
        SceneManager.LoadScene("CostumeTestMainScene"); // 나중에 메인 씬으로 변경
    }
    private void OnSaveNoButtonClicked()
    {
        StarterPanda.Instance.gameObject.SetActive(true);
        StarterPanda.Instance.IsSwitchingScene = true;
        SceneManager.LoadScene("CostumeTestMainScene"); // 나중에 메인 씬으로 변경
    }

    private void NotExitCostumeButtonClicked()
    {
        _costumeViewModel.IsExitCostume = false;

        Tween.IamgeAlpha(_saveCostume, 0, _loadTime, TweenMode.Quadratic, () =>
        {
            _saveCostume.gameObject.SetActive(false);
        });
        Tween.IamgeAlpha(_yesButton.gameObject, 0, _loadTime, TweenMode.Quadratic);
        Tween.IamgeAlpha(_noButton.gameObject, 0, _loadTime, TweenMode.Quadratic);
        //Tween.TextAlpha(_saveText.gameObject, 0, _loadTime, TweenMode.Quadratic);

    }

}
