using Cooking;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

/// <summary>�ִϸ��̼��� �����ӿ� ���� ���带 �����Ű�� ��ũ��Ʈ</summary>
public class AnimationPlaySound : StateMachineBehaviour
{

    [Tooltip("��� ����� Ŭ��")][SerializeField] private AudioClip[] _audioClips;
    [Tooltip("Ȱ��ȭ ������")] [SerializeField] private int _startFrame;
    [Tooltip("�� Ȱ��ȭ ������")] [SerializeField] private int _finishedFrame;
    [SerializeField] private bool _isPlayOneShot;
    [SerializeField] private bool _isFinishSoundStop;

    private AnimationClip _clip;
    private bool _isStarted;
    private bool _isFinished;
    private AudioSource _audioSource;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //AudioSource�� ���� ��� ���� ������ �����Ѵ�.
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
        //�ִϸ��̼� Ŭ���� �������� ����� �ش� �������� ����ġ�� ���带 ����ϵ��� �Ѵ�.
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
