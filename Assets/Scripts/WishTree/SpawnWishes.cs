using System;
using UnityEngine;

public class SpawnWishes : MonoBehaviour
{
    [SerializeField] private GameObject _wishesPf;
    // Start is called before the first frame update
    void Start()
    {
        //Test
        SpawnWish();
    }

    private void SpawnWish()
    {
  
        GameObject wishes = Instantiate(_wishesPf, GetRandomPosition(), Quaternion.identity);
        wishes.transform.SetParent(transform, false);
    }

    private Vector3 GetRandomPosition()
    {
        float width = GetComponent<RectTransform>().rect.width;
        float height = GetComponent<RectTransform>().rect.height;

        float randomX = UnityEngine.Random.Range(-width / 2, width / 2); ;
        float randomY = UnityEngine.Random.Range(-height / 2, height / 2); 

        Vector3 spawnPos = new Vector3(randomX, randomY, 0);

        return spawnPos;
    }
}
