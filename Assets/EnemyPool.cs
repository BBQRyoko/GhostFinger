using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] GhostManager ghostPrefab;
    public ObjectPool<GhostManager> ghostPrefabPool;
    [SerializeField] int defaultEnemySize = 50;
    [SerializeField] int maxPoolSize = 500;
    private void Awake()
    {
        ghostPrefabPool = new ObjectPool<GhostManager>(OnCreatePoolItem, OnGetPoolItem, OnReleasePoolItem, OnDestroyPoolItem, true, defaultEnemySize, maxPoolSize);
    }
    GhostManager OnCreatePoolItem()
    {
        var enemy = Instantiate(ghostPrefab, transform);
        enemy.gameObject.SetActive(false);
        return enemy;
    }
    void OnGetPoolItem(GhostManager enemy)
    {
        enemy.gameObject.SetActive(true);
    }
    void OnReleasePoolItem(GhostManager enemy)
    {
        enemy.gameObject.SetActive(false);
    }
    void OnDestroyPoolItem(GhostManager enemy)
    {
        Destroy(enemy.gameObject);
    }
}
