using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cooking
{
    [Serializable]
    public class CookwareAnimatorData
    {
        [SerializeField] private Animator _animator;
        public Animator Animator => _animator;

        [SerializeField] private bool _startSetActive;
        public bool StartSetActive => _startSetActive;

        public GameObject gameObject => Animator.gameObject;
    }
}


