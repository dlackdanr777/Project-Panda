using System;
using UnityEngine;

namespace Cooking
{
    public enum CookValue
    {
        LargeValue,
        NomalValue,
        SmallValue,
        Count
    }

    [Serializable]
    public class CookUserData
    {
        [SerializeField] private int _maxFireValue;
        public int MaxFireValue => _maxFireValue;

        [SerializeField] private int _maxStamina;
        public int MaxStamina => _maxStamina;

        [Space]
        [SerializeField] private int[] _largeAddValue;
        [SerializeField] private int _largeAddValueStamina;

        [Space]
        [SerializeField] private int[] _addValue;
        [SerializeField] private int _addValueStamina;

        [Space]
        [SerializeField] private int[] _smallAddValue;
        [SerializeField] private int _smallAddValueStamina;

        public int[] LargeAddValue => _largeAddValue;
        public int LargeAddValueStamina => _largeAddValueStamina;
        public int[] AddValue => _addValue;
        public int AddValueStamina => _addValueStamina;
        public int[] SmallAddValue => _smallAddValue;
        public int SmallAddValueStamina => _smallAddValueStamina;
    }
}
