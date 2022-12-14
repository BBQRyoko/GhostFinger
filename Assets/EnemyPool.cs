using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    public GhostManager ghostPrefab;
    public ObjectPool<GhostManager> ghostPrefabPool;
    [SerializeField] int defaultEnemySize = 50;
    [SerializeField] int maxPoolSize = 50;
    [SerializeField] int Active;
    [SerializeField] int InActive;
    [SerializeField] int All;


    private void Awake()
    {
        ghostPrefabPool = new ObjectPool<GhostManager>(OnCreatePoolItem, OnGetPoolItem, OnReleasePoolItem, OnDestroyPoolItem, true, defaultEnemySize, maxPoolSize);
        //for (int i = 0; i < defaultEnemySize; i++) 
        //{
        //    var enemy = ghostPrefabPool.Get();
        //    enemy.gameObject.SetActive(false);
        //}
    }

    private void Update()
    {
        Active = ghostPrefabPool.CountActive;
        InActive = ghostPrefabPool.CountInactive;
        All = ghostPrefabPool.CountAll;
    }
    GhostManager OnCreatePoolItem()
    {
        var enemy = Instantiate(ghostPrefab, transform);
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
