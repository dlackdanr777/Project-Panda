using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStart : MonoBehaviour
{
    [SerializeField] private Text _startTitle;

    [SerializeField] private Button _startBackgroundButton;

    private bool _isStart;
    private void Awake()
    {
        _startBackgroundButton.onClick.AddListener(OnBackgroundButtonClickd);
    }

    private void OnBackgroundButtonClickd()
    {
        if (!_isStart)
        {
            _isStart = true;

            Vector3 moveUIPos = new Vector3(0, 1200, 0);
            Vector3 moveCameraPos = new Vector3(0, -5, 3);

            StartCoroutine(MoveObject(_startTitle.gameObject, moveUIPos, 5));
            StartCoroutine(MoveObject(Camera.main.gameObject, moveCameraPos, 5));

            _startBackgroundButton.onClick.RemoveListener(OnBackgroundButtonClickd);
        }
    }

    private IEnumerator MoveObject(GameObject targetUI, Vector3 movePos, float time)
    {
        Vector3 targetPos = targetUI.transform.position;
        Vector3 destinationPos = targetPos + movePos;
        float timer = 0;

        Debug.Log("Ω√¿€");
        while (timer < time)
        {
            targetUI.transform.position = Vector3.Lerp(targetPos, destinationPos, timer / time);
            timer += Time.deltaTime;
            Debug.Log(timer);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
