using Muks.DataBind;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CostumeView : UIList<CostumeData, EBodyParts>
{
    private CostumeViewModel _costumeViewModel;


    private void Start()
    {
        _costumeViewModel = new CostumeViewModel();
    }


    private void Awake()
    {
        for (int i = 0; i < CostumeManager.Instance.CostumeDic.Count; i++)
        {
            
        }
        Init();
    }



    protected override void GetContent(int index)
    {
        DataBind.SetTextValue("CostumeDetailName", _lists[(int)_currentField][index].CostumeName);
        //DataBind.SetTextValue("CostumeDetailDescription", _lists[(int)_currentField][index].Description);
        DataBind.SetSpriteValue("CostumeDetailImage", _lists[(int)_currentField][index].Image);

    }

    protected override void UpdateListSlots()
    {

    }
}
