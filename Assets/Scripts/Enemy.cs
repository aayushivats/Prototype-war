using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public float Health { get { return health; } }
    void Start()
    {
        target = "";
        health = 100;
    }
}
