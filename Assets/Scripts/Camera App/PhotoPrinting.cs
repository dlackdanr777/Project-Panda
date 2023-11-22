using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoPrinting : MonoBehaviour
{
    [Tooltip("인화된 사진을 출력할 이미지 오브젝트")]
    [SerializeField] private Image _image;

    [Tooltip("인화된 사진이 나타날 위치")]
    [SerializeField] private Transform _transform;

    [Tooltip("회전할 각도")]
    [SerializeField] private float _angle;

    [Tooltip("몇 초 동안 지속되고 사라질 것인가?")]
    [SerializeField] private float _duration;

    Material _tempMat;

    private void Awake()
    {
        _tempMat = _image.material;
        gameObject.SetActive(false);
    }

    public void Show(Texture2D texture)
    {
        gameObject.SetActive(true);
         
        _image.material.mainTexture = texture;
        _image.material = null;
        _image.material = _tempMat;

        if(gameObject.activeSelf)
            StartCoroutine(StartPrinting());
    }

    private IEnumerator StartPrinting()
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
        gameObject.SetActive(false);
    }
    
}
