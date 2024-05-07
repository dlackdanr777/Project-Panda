using Cooking;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

/// <summary>애니메이션의 프레임에 맞춰 사운드를 재생시키는 스크립트</summary>
public class AnimationPlaySound : StateMachineBehaviour
{

    [Tooltip("재생 오디오 클립")][SerializeField] private AudioClip[] _audioClips;
    [Tooltip("활성화 프레임")] [SerializeField] private int _startFrame;
    [Tooltip("비 활성화 프레임")] [SerializeField] private int _finishedFrame;
    [SerializeField] private bool _isPlayOneShot;
    [SerializeField] private bool _isFinishSoundStop;

    private AnimationClip _clip;
    private bool _isStarted;
    private bool _isFinished;
    private AudioSource _audioSource;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //AudioSource가 없을 경우 새로 생성해 세팅한다.
        if(animator.GetComponent<AudioSource>() == null)
        {
            AudioSource audio = animator.AddComponent<AudioSource>();
            audio.playOnAwake = false;
            audio.loop = false;
            audio.outputAudioMixerGroup = Resources.Load<AudioMixer>("AudioMixer").FindMatchingGroups("Master")[2];
            audio.volume = 0.5f;
        }

        _isStarted = false;
        _isFinished = false;
    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //애니메이션 클립의 프레임을 계산해 해당 프레임을 지나치면 사운드를 재생하도록 한다.
        _clip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
        float currentTime = _clip.length * stateInfo.normalizedTime;
        int _currentFrame = Mathf.RoundToInt(_clip.frameRate * currentTime);

        if (_startFrame <= _currentFrame && !_isStarted)
        {
            if (_audioSource == null)
                _audioSource = animator.GetComponent<AudioSource>();

            int rendInt = Random.Range(0, _audioClips.Length);

            if (_isPlayOneShot)
            {
                _audioSource.PlayOneShot(_audioClips[rendInt]);
            }
            else
            {
                _audioSource.clip = _audioClips[rendInt];
                _audioSource.Play();
            }

            _isStarted = true;
        }

        else if (_finishedFrame <= _currentFrame && !_isFinished)
        {
            if (_audioSource == null)
                _audioSource = animator.GetComponent<AudioSource>();

            if(_isFinishSoundStop)
                _audioSource.Stop();
            _isFinished = true;
        }
    }


   public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_isFinished)
            return;

        if(_audioSource == null)
            _audioSource = animator.GetComponent<AudioSource>();

        if (_isFinishSoundStop)
            _audioSource.Stop();

    }
}
