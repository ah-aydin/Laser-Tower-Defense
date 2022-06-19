using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjectPool : ObjectPool
{
    [SerializeField] GameObject[] enemies;

    private void Awake()
    {
        pool = new GameObject[enemies.Length];
        for (int i = 0; i < enemies.Length; ++i)
        {
            pool[i] = enemies[i];
        }
    }
}
