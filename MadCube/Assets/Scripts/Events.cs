using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{

    [Header("Player Events")]

    public static Events instance;
    public Action OnPlayerFall; // Karakter d��t���nde.

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }
}
