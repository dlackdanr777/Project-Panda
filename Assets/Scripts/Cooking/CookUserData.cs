using System;
using UnityEngine;

namespace Cooking
{
    [Serializable]
    public class CookUserData
    {
        [SerializeField] private int _maxFireValue;
        public int MaxFireValue => _maxFireValue;

        [SerializeField] private int _maxStamina;
        public int MaxStamina => _maxStamina;

        [Space]
        [SerializeField] private int[] _moreAddValue;
        [SerializeField] private int _moreAddValueStamina;

        [Space]
        [SerializeField] private int[] _addValue;
        [SerializeField] private int _addValueStamina;

        [Space]
        [SerializeField] private int[] _smallAddValue;
        [SerializeField] private int _smallAddValueStamina;

        public int[] MoreAddValue => _moreAddValue;
        public int MoreAddValueStamina => _moreAddValueStamina;
        public int[] AddValue => _addValue;
        public int AddValueStamina => _addValueStamina;
        public int[] SmallAddValue => _smallAddValue;
        public int SmallAddValueStamina => _smallAddValueStamina;
    }
}
