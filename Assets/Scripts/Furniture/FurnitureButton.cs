using UnityEngine;
using UnityEngine.SceneManagement;
using Muks.DataBind;
using BT;

public class FurnitureButton : MonoBehaviour
{
    void Start()
    {
        //gameObject.GetComponent<Button>().onClick.AddListener(OnCostumeButtonClicked);
        DataBind.SetUnityActionValue("FurnitureButton", OnCostumeButtonClicked);
    }

    private void OnCostumeButtonClicked()
    {
        //if (StarterPanda.Instance.SetFalseUI())
        //{
        if(FadeInOutManager.Instance != null)
        {
            LoadingSceneManager.LoadScene("FurnitureTest");
        }
        else
        {
            SceneManager.LoadScene("FurnitureTest");
        }

        //}
    }
}
