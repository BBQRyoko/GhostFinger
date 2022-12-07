using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class BulletPool : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    public ObjectPool<GameObject> bulletPrefabPool;
    [SerializeField] int defaultExpDropSize = 200;
    [SerializeField] int maxPoolSize = 500;
    [SerializeField] Transform poolParent;

    private void Awake()
    {
        bulletPrefabPool = new ObjectPool<GameObject>(OnCreatePoolItem, OnGetPoolItem, OnReleasePoolItem, OnDestroyPoolItem, true, defaultExpDropSize, maxPoolSize);
    }
    GameObject OnCreatePoolItem()
    {
        var bullet = Instantiate(bulletPrefab, poolParent);
        return bullet;
    }
    void OnGetPoolItem(GameObject bullet)
    {
        bullet.gameObject.SetActive(true);
    }
    void OnReleasePoolItem(GameObject bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    void OnDestroyPoolItem(GameObject bullet)
    {
        Destroy(bullet);
    }
}
