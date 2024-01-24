using UnityEngine;
using UnityEngine.SceneManagement;
using Muks.DataBind;
using BT;

public class FurnitureButton : MonoBehaviour
{
    void Start()
    {
        //gameObject.GetComponent<Button>().onClick.AddListener(OnCostumeButtonClicked);
        DataBind.SetButtonValue("FurnitureButton", OnCostumeButtonClicked);
    }

    private void OnCostumeButtonClicked()
    {
        if (StarterPanda.Instance.SetFalseUI())
        {
            SceneManager.LoadScene("FurnitureTest");
        }
    }
}
