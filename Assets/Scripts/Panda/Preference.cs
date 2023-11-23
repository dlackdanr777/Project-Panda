using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preference
{
    [SerializeField]
    public string _favoriteSnack;
    [SerializeField]
    public string _favoriteToy;

    public Preference(string favoriteSnack, string favoriteToy)
    {
        _favoriteSnack = favoriteSnack;
        _favoriteToy = favoriteToy;
    }
}
