using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] [Range(0, 50)] int poolSize = 5;
    [SerializeField] [Range(0.1f, 30f)] float spawnTime = 1f;

    GameObject[] pool;

    void Awake() {
        PopulatePool();
    }

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    void PopulatePool() {
        pool = new GameObject[poolSize];

        for(int i = 0; i < pool.Length; i++) {
            pool[i] = Instantiate(enemyPrefab, transform);
            pool[i].SetActive(false);
        }
    }

    IEnumerator SpawnEnemy() {
        while(true) {
            EnableObjectInPool();
            yield return new WaitForSeconds(spawnTime);
        }
    }

    void EnableObjectInPool() {
        foreach (GameObject go in pool) {
            if(!go.activeInHierarchy) {
                go.SetActive(true);
                return;
            }
        }
    }

    
}