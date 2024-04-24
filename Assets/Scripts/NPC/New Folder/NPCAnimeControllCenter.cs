using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>애니메이션이 랜덤으로 실행되는 NPC들을 관리하는 스크립트</summary>
public class NPCAnimeControllCenter : MonoBehaviour
{
    [Header("Description")]
    [SerializeField] private string _npcName;
    [SerializeField] private string _npcId;

    [Space]
    [Header("NpcControllers")]
    [SerializeField] private NPCAnimeContollor[] _mn01NPCs;
    [SerializeField] private NPCAnimeContollor[] _mn02NPCs;
    [SerializeField] private NPCAnimeContollor[] _mn03NPCs;
    [SerializeField] private NPCAnimeContollor[] _mn04NPCs;
    [SerializeField] private NPCAnimeContollor[] _mn05NPCs;
    [SerializeField] private NPCAnimeContollor[] _mn06NPCs;
    [SerializeField] private NPCAnimeContollor[] _mn07NPCs;
    [SerializeField] private NPCAnimeContollor[] _mn08NPCs;
    [SerializeField] private NPCAnimeContollor[] _mn09NPCs;
    [SerializeField] private NPCAnimeContollor[] _mn10NPCs;
    [SerializeField] private NPCAnimeContollor[] _mn11NPCs;
    [SerializeField] private NPCAnimeContollor[] _mn12NPCs;
    [SerializeField] private NPCAnimeContollor[] _mn13NPCs;
    [SerializeField] private NPCAnimeContollor[] _mn14NPCs;
    [SerializeField] private NPCAnimeContollor[] _mn15NPCs;
    


    private void Start()
    {
        Init();
        NpcInit();
        ChangeMapEvent();
    }


    private void Init()
    {
        FadeInOutManager.Instance.OnFadeOutHandler += ChangeMapEvent;
        LoadingSceneManager.OnLoadSceneHandler += ChangeSceneEvent;

    }


    /// <summary>씬이 변경되면 실행할 함수(등록했던 대리자를 전부 제거하는 부분)</summary>
    private void ChangeSceneEvent()
    {
        FadeInOutManager.Instance.OnFadeOutHandler -= ChangeMapEvent;
        LoadingSceneManager.OnLoadSceneHandler -= ChangeSceneEvent;
    }


    /// <summary>맵이 변경될때마다 실행해 NPC들을 하나만 활성화 시키는 함수</summary>
    private void ChangeMapEvent()
    {
        ChangeMapEvent_Mn01();
        ChangeMapEvent_Mn02();
        ChangeMapEvent_Mn03();
        ChangeMapEvent_Mn04();
        ChangeMapEvent_Mn05();
        ChangeMapEvent_Mn06();
        ChangeMapEvent_Mn07();
        ChangeMapEvent_Mn08();
        ChangeMapEvent_Mn09();
        ChangeMapEvent_Mn10();
        ChangeMapEvent_Mn11();
        ChangeMapEvent_Mn12();
        ChangeMapEvent_Mn13();
        ChangeMapEvent_Mn14();
        ChangeMapEvent_Mn15();
     

    }


    private void NpcInit()
    {
        for (int i = 0, count = _mn01NPCs.Length; i < count; i++)
            _mn01NPCs[i].Init();

        for (int i = 0, count = _mn02NPCs.Length; i < count; i++)
            _mn02NPCs[i].Init();

        for (int i = 0, count = _mn03NPCs.Length; i < count; i++)
            _mn03NPCs[i].Init();

        for (int i = 0, count = _mn04NPCs.Length; i < count; i++)
            _mn04NPCs[i].Init();

        for (int i = 0, count = _mn05NPCs.Length; i < count; i++)
            _mn05NPCs[i].Init();

        for (int i = 0, count = _mn06NPCs.Length; i < count; i++)
            _mn06NPCs[i].Init();

        for (int i = 0, count = _mn07NPCs.Length; i < count; i++)
            _mn07NPCs[i].Init();

        for (int i = 0, count = _mn08NPCs.Length; i < count; i++)
            _mn08NPCs[i].Init();

        for (int i = 0, count = _mn09NPCs.Length; i < count; i++)
            _mn09NPCs[i].Init();

        for (int i = 0, count = _mn10NPCs.Length; i < count; i++)
            _mn10NPCs[i].Init();

        for (int i = 0, count = _mn11NPCs.Length; i < count; i++)
            _mn11NPCs[i].Init();

        for (int i = 0, count = _mn12NPCs.Length; i < count; i++)
            _mn12NPCs[i].Init();

        for (int i = 0, count = _mn13NPCs.Length; i < count; i++)
            _mn13NPCs[i].Init();

        for (int i = 0, count = _mn14NPCs.Length; i < count; i++)
            _mn14NPCs[i].Init();
        for (int i = 0, count = _mn15NPCs.Length; i < count; i++)
            _mn15NPCs[i].Init();
        
    }


    private void ChangeMapEvent_Mn01()
    {
        if (_mn01NPCs.Length <= 0)
            return;

        //맵이 바뀔 경우 NPC들을 전부 끈 후 맵마다 랜덤으로 하나씩 활성화 시킨다.
        for (int i = 0, count = _mn01NPCs.Length; i < count; i++)
        {
            _mn01NPCs[i].HideNpc();
        }

        int randInt = UnityEngine.Random.Range(0, _mn01NPCs.Length);
        _mn01NPCs[randInt].ShowNpc();
    }


    private void ChangeMapEvent_Mn02()
    {
        if (_mn02NPCs.Length <= 0)
            return;

        //맵이 바뀔 경우 NPC들을 전부 끈 후 맵마다 랜덤으로 하나씩 활성화 시킨다.
        for (int i = 0, count = _mn02NPCs.Length; i < count; i++)
        {
            _mn02NPCs[i].HideNpc();
        }

        int randInt = UnityEngine.Random.Range(0, _mn02NPCs.Length);
        _mn02NPCs[randInt].ShowNpc();
    }


    private void ChangeMapEvent_Mn03()
    {
        if (_mn03NPCs.Length <= 0)
            return;

        //맵이 바뀔 경우 NPC들을 전부 끈 후 맵마다 랜덤으로 하나씩 활성화 시킨다.
        for (int i = 0, count = _mn03NPCs.Length; i < count; i++)
        {
            _mn03NPCs[i].HideNpc();
        }

        int randInt = UnityEngine.Random.Range(0, _mn03NPCs.Length);
        _mn03NPCs[randInt].ShowNpc();
    }


    private void ChangeMapEvent_Mn04()
    {
        if (_mn04NPCs.Length <= 0)
            return;

        //맵이 바뀔 경우 NPC들을 전부 끈 후 맵마다 랜덤으로 하나씩 활성화 시킨다.
        for (int i = 0, count = _mn04NPCs.Length; i < count; i++)
        {
            _mn04NPCs[i].HideNpc();
        }

        int randInt = UnityEngine.Random.Range(0, _mn04NPCs.Length);
        _mn04NPCs[randInt].ShowNpc();
    }


    private void ChangeMapEvent_Mn05()
    {
        if (_mn05NPCs.Length <= 0)
            return;

        //맵이 바뀔 경우 NPC들을 전부 끈 후 맵마다 랜덤으로 하나씩 활성화 시킨다.
        for (int i = 0, count = _mn05NPCs.Length; i < count; i++)
        {
            _mn05NPCs[i].HideNpc();
        }

        int randInt = UnityEngine.Random.Range(0, _mn05NPCs.Length);
        _mn05NPCs[randInt].ShowNpc();
    }


    private void ChangeMapEvent_Mn06()
    {
        if (_mn06NPCs.Length <= 0)
            return;

        //맵이 바뀔 경우 NPC들을 전부 끈 후 맵마다 랜덤으로 하나씩 활성화 시킨다.
        for (int i = 0, count = _mn06NPCs.Length; i < count; i++)
        {
            _mn06NPCs[i].HideNpc();
        }

        int randInt = UnityEngine.Random.Range(0, _mn06NPCs.Length);
        _mn06NPCs[randInt].ShowNpc();
    }


    private void ChangeMapEvent_Mn07()
    {
        if (_mn07NPCs.Length <= 0)
            return;

        //맵이 바뀔 경우 NPC들을 전부 끈 후 맵마다 랜덤으로 하나씩 활성화 시킨다.
        for (int i = 0, count = _mn07NPCs.Length; i < count; i++)
        {
            _mn07NPCs[i].HideNpc();
        }

        int randInt = UnityEngine.Random.Range(0, _mn07NPCs.Length);
        _mn07NPCs[randInt].ShowNpc();
    }


    private void ChangeMapEvent_Mn08()
    {
        if (_mn08NPCs.Length <= 0)
            return;

        //맵이 바뀔 경우 NPC들을 전부 끈 후 맵마다 랜덤으로 하나씩 활성화 시킨다.
        for (int i = 0, count = _mn08NPCs.Length; i < count; i++)
        {
            _mn08NPCs[i].HideNpc();
        }

        int randInt = UnityEngine.Random.Range(0, _mn08NPCs.Length);
        _mn08NPCs[randInt].ShowNpc();
    }


    private void ChangeMapEvent_Mn09()
    {
        if (_mn09NPCs.Length <= 0)
            return;

        //맵이 바뀔 경우 NPC들을 전부 끈 후 맵마다 랜덤으로 하나씩 활성화 시킨다.
        for (int i = 0, count = _mn09NPCs.Length; i < count; i++)
        {
            _mn09NPCs[i].HideNpc();
        }

        int randInt = UnityEngine.Random.Range(0, _mn09NPCs.Length);
        _mn09NPCs[randInt].ShowNpc();
    }


    private void ChangeMapEvent_Mn10()
    {
        if (_mn10NPCs.Length <= 0)
            return;

        //맵이 바뀔 경우 NPC들을 전부 끈 후 맵마다 랜덤으로 하나씩 활성화 시킨다.
        for (int i = 0, count = _mn10NPCs.Length; i < count; i++)
        {
            _mn10NPCs[i].HideNpc();
        }

        int randInt = UnityEngine.Random.Range(0, _mn10NPCs.Length);
        _mn10NPCs[randInt].ShowNpc();
    }


    private void ChangeMapEvent_Mn11()
    {
        if (_mn11NPCs.Length <= 0)
            return;

        //맵이 바뀔 경우 NPC들을 전부 끈 후 맵마다 랜덤으로 하나씩 활성화 시킨다.
        for (int i = 0, count = _mn11NPCs.Length; i < count; i++)
        {
            _mn11NPCs[i].HideNpc();
        }

        int randInt = UnityEngine.Random.Range(0, _mn11NPCs.Length);
        _mn11NPCs[randInt].ShowNpc();
    }


    private void ChangeMapEvent_Mn12()
    {
        if (_mn12NPCs.Length <= 0)
            return;

        //맵이 바뀔 경우 NPC들을 전부 끈 후 맵마다 랜덤으로 하나씩 활성화 시킨다.
        for (int i = 0, count = _mn12NPCs.Length; i < count; i++)
        {
            _mn12NPCs[i].HideNpc();
        }

        int randInt = UnityEngine.Random.Range(0, _mn12NPCs.Length);
        _mn12NPCs[randInt].ShowNpc();
    }


    private void ChangeMapEvent_Mn13()
    {
        if (_mn13NPCs.Length <= 0)
            return;

        //맵이 바뀔 경우 NPC들을 전부 끈 후 맵마다 랜덤으로 하나씩 활성화 시킨다.
        for (int i = 0, count = _mn13NPCs.Length; i < count; i++)
        {
            _mn13NPCs[i].HideNpc();
        }

        int randInt = UnityEngine.Random.Range(0, _mn13NPCs.Length);
        _mn13NPCs[randInt].ShowNpc();
    }


    private void ChangeMapEvent_Mn14()
    {
        if (_mn14NPCs.Length <= 0)
            return;

        //맵이 바뀔 경우 NPC들을 전부 끈 후 맵마다 랜덤으로 하나씩 활성화 시킨다.
        for (int i = 0, count = _mn14NPCs.Length; i < count; i++)
        {
            _mn14NPCs[i].HideNpc();
        }

        int randInt = UnityEngine.Random.Range(0, _mn14NPCs.Length);
        _mn14NPCs[randInt].ShowNpc();
    }

    private void ChangeMapEvent_Mn15()
    {
        if (_mn15NPCs.Length <= 0)
            return;

        //맵이 바뀔 경우 NPC들을 전부 끈 후 맵마다 랜덤으로 하나씩 활성화 시킨다.
        for (int i = 0, count = _mn15NPCs.Length; i < count; i++)
        {
            _mn15NPCs[i].HideNpc();
        }

        int randInt = UnityEngine.Random.Range(0, _mn15NPCs.Length);
        _mn15NPCs[randInt].ShowNpc();
    }

}
