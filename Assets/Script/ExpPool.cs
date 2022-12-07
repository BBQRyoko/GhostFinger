using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ExpPool : MonoBehaviour
{
    [SerializeField] GameObject expPrefab;
    public ObjectPool<GameObject> expPrefabPool;
    [SerializeField] int defaultExpDropSize = 200;
    [SerializeField] int maxPoolSize = 500;
    [SerializeField] Transform poolParent;
    private void Awake()
    {
        expPrefabPool = new ObjectPool<GameObject>(OnCreatePoolItem,OnGetPoolItem, OnReleasePoolItem, OnDestroyPoolItem, true, defaultExpDropSize, maxPoolSize);
    }
    private void Start()
    {
        //for (int i = 0; i <= 200; i++)
        //{
        //    var exp = expPrefabPool.Get();
        //    exp.gameObject.SetActive(false);
        //}
    }
    GameObject OnCreatePoolItem() 
    {
        var exp = Instantiate(expPrefab, poolParent);
        return exp;
    }
    void OnGetPoolItem(GameObject exp) 
    {
        exp.gameObject.SetActive(true);
    }
    void OnReleasePoolItem(GameObject exp) 
    {
        exp.gameObject.SetActive(false);
    }
    void OnDestroyPoolItem(GameObject exp) 
    {
        Destroy(exp);
    }
}
