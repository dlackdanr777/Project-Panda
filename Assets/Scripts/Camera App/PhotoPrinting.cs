using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoPrinting : MonoBehaviour
{
    [Tooltip("��ȭ�� ������ ����� �̹��� ������Ʈ")]
    [SerializeField] private Image _image;

    [Tooltip("��ȭ�� ������ ��Ÿ�� ��ġ")]
    [SerializeField] private Transform _transform;

    [Tooltip("ȸ���� ����")]
    [SerializeField] private float _angle;

    [Tooltip("�� �� ���� ���ӵǰ� ����� ���ΰ�?")]
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
