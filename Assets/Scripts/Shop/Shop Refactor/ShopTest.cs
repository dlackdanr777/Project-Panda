using Muks.DataBind;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BT;

public class ShopTest : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnShopButtonClicked);
    }

    private void OnShopButtonClicked()
    {
        StarterPanda.Instance.SetFalseUI();
        SceneManager.LoadScene("ShopScene");
    }
}
