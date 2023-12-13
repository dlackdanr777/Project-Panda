using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPhoto : UIView
{
    [Tooltip("인화된 사진을 출력할 이미지 오브젝트")]
    [SerializeField] private Image _image;

    Material _tempMat;

    public override void Init(UINavigation uiNav)
    {
        base.Init(uiNav);
        _tempMat = _image.material;

    }

    public override void Show()
    {
        gameObject.SetActive(true);
        VisibleState = VisibleState.Appeared;

    }

    public override void Hide()
    {
        gameObject.SetActive(false);
        VisibleState = VisibleState.Disappeared;
    }


    public void Show(Texture2D texture)
    {
        _uiNav.Push("Photo");
        _tempMat.mainTexture = texture;
        _image.material = _tempMat;

    }


   /* private IEnumerator StartPrinting()
    {
        transform.position = _transform.position;
        Vector3 tempAngles = transform.eulerAngles;
        float timer = 0;

        while(timer < 0.05f)
        {
            timer += Time.deltaTime;
            transform.eulerAngles = Vector3.Lerp(tempAngles, new Vector3(tempAngles.x, tempAngles.y, _angle), timer / 0.1f);
            yield return null;
        }

        yield return new WaitForSeconds(_duration);

        transform.eulerAngles = new Vector3(0, 0, 0);
    }*/


}
