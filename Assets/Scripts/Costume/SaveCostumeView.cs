using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Muks.Tween;

public class SaveCostumeView : MonoBehaviour
{
    [SerializeField] private GameObject _saveCostume;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;
    [SerializeField] private Button _notExitButton;

    private CostumeViewModel _costumeViewModel;

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
            Debug.Log("저장코스튬 바인드");
            _costumeViewModel.CostumeSceneChanged += SaveCostume;
        }

        Debug.Log("저장코스튬 바인드: "+ _costumeViewModel);
    }

    private void SaveCostume(bool IsExit)
    {
        _saveCostume.gameObject.SetActive(true);

        Tween.IamgeAlpha(_saveCostume, 1, 0.5f, TweenMode.Quadratic);
        _yesButton.gameObject.SetActive(true);
        _noButton.gameObject.SetActive(true);
        //Tween.IamgeAlpha(_yesButton.gameObject, 1, 0.5f, TweenMode.Quadratic);
        //Tween.IamgeAlpha(_noButton.gameObject, 1, 0.5f, TweenMode.Quadratic);
    }

    private void OnSaveYesButtonClicked()
    {
        _costumeViewModel.SaveCostume();
        SceneManager.LoadScene("CostumeTestMainScene"); // 나중에 메인 씬으로 변경
    }
    private void OnSaveNoButtonClicked()
    {
        SceneManager.LoadScene("CostumeTestMainScene"); // 나중에 메인 씬으로 변경
    }

    private void NotExitCostumeButtonClicked()
    {
        _costumeViewModel.IsExitCostume = false;
        Tween.IamgeAlpha(_saveCostume, 0, 0.5f, TweenMode.Quadratic, () =>
        {
            _saveCostume.gameObject.SetActive(false);
        });
        _yesButton.gameObject.SetActive(false);
        _noButton.gameObject.SetActive(false);
    }

}
