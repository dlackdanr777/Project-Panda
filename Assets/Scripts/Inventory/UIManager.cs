using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum App
{
    None,
    Message,
    Inventory
}
abstract public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button _appButton;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private GameObject _detailView;

    private GameObject _spawnPrefab;
    
    public Player Player { get; private set; }
    public GameObject _prefab { get; private set; }
    public Transform SpawnPoint { get; private set; }

    private void Init()
    {
        Player = GameManager.Instance.Player;

        for (int i = 0; i < Player.Inventory.Items.Count; i++)
        {
            _spawnPrefab = Instantiate(_prefab, SpawnPoint);
        }

        _appButton.onClick.AddListener(OnAppButton);
        _closeButton.onClick.AddListener(OnClickCloseButton);
    }

    private void OnAppButton()
    {
        _appButton.transform.parent.gameObject.SetActive(false);
        _detailView.SetActive(true);
    }

    private void OnClickCloseButton()
    {
        _appButton.transform.parent.gameObject.SetActive(true);
        _detailView.SetActive(false);
    }
}
