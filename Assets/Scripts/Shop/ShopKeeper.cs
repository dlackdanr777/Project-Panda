
using UnityEngine;
using Muks.DataBind;

public class ShopKeeper : MonoBehaviour
{


    void Start()
    {
        DataBind.SetUnityActionValue("ShopButton", OnShopButtonClicked);
    }
    

    private void OnShopButtonClicked()
    {
        LoadingSceneManager.LoadScene("ShopScene");
    }


}
