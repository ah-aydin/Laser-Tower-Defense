using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] [Range(0, 50)] int poolSize = 5;
    [SerializeField] [Range(0.1f, 30f)] float spawnTimer = 1f;
    
    private Transform startPosition;

    protected GameObject[] pool;

    private void Start()
    {
        startPosition = FindObjectOfType<EnemySpawnLocation>().transform;
        PopulatePool();
        StartCoroutine(SpawnEnemy());
    }

    void PopulatePool()
    {
        pool = new GameObject[poolSize];

        for (int i = 0; i < pool.Length; ++i)
        {
            GameObject go = Instantiate(enemyPrefab, transform);
            if (go == null) Debug.Log("I dont have enemy");
            pool[i] = go;
            pool[i].SetActive(false);
            pool[i].transform.position = startPosition.position;
        }
    }

    void EnableObjectInPool()
    {
        for (int i = 0; i < pool.Length; ++i)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                return;
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            EnableObjectInPool();
            yield return new WaitForSeconds(spawnTimer);
        }
    }

    public GameObject[] GetActiveEnemies()
    {
        return pool.Where(go => go.activeInHierarchy).ToArray();
    }
}
