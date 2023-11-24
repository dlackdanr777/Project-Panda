using System;
using System.Collections;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SpawnWishes : MonoBehaviour
{
    [SerializeField] private GameObject _wishesPf;
    public Action NoticeHandler; 

    // Start is called before the first frame update
    void Start()
    {
        Predicate<int> _condition1Handler = (int amount) => (amount > 10);

        //Test
        StartCoroutine(SpawnWish(_condition1Handler, GameManager.Instance.MessageDatabase.Messages[0]));
    }


    private IEnumerator SpawnWish(Predicate<int> condition, Message message)
    {
        while(!message.IsSend)
        {
            if (condition(GameManager.Instance.Player.Familiarity))
            {
                GameObject wishes = Instantiate(_wishesPf, GetRandomPosition(), Quaternion.identity);
                wishes.transform.SetParent(transform, false);
                wishes.GetComponent<Button>().onClick.AddListener(()=>NoticeHandler?.Invoke());

                //Test
                wishes.GetComponent<Wish>().Message = message;
                message.IsSend = true;
                GameManager.Instance.Player.Messages[1].Add(message);
                NoticeHandler?.Invoke();

                yield break;
            }
            yield return null;

        }
        
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
