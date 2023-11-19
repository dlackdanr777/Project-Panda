using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 판다 생성 test - 수정 필요
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
