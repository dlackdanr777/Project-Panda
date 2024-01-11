using UnityEngine;
using UnityEngine.SceneManagement;
using Muks.DataBind;

public class FurnitureButton : MonoBehaviour
{
    void Start()
    {
        //gameObject.GetComponent<Button>().onClick.AddListener(OnCostumeButtonClicked);
        DataBind.SetButtonValue("FurnitureButton", OnCostumeButtonClicked);
    }

    private void OnCostumeButtonClicked()
    {
        DatabaseManager.Instance.StartPandaInfo.StarterPanda.SetFalseUI();
        SceneManager.LoadScene("FurnitureTest");
    }
}
