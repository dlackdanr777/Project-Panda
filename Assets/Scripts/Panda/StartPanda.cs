using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Ǵ� ���� test - ���� �ʿ�
public class StartPanda : MonoBehaviour
{
    string mbti = "intp";
    public Sprite testImage;
    void Start()
    {
        SpawnPanda(mbti);
    }

    private void SpawnPanda(string mbti)
    {
        StarterPanda panda = new StarterPanda(mbti, testImage);
    }
}
