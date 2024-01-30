
using UnityEngine;
using Muks.DataBind;

public class ShopKeeper : MonoBehaviour
{


    void Start()
    {
        DataBind.SetButtonValue("ShopButton", OnShopButtonClicked);
    }
    

    private void OnShopButtonClicked()
    {
        LoadingSceneManager.LoadScene("ShopScene");
    }


}
