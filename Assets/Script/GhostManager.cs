using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.EventSystems;




public class GhostManager : MonoBehaviour
{
    ExpPool expPool;
    EnemyPool enemyPool;
    SpriteRenderer sp;
    [SerializeField] PlayerManager player;
    NavMeshAgent agent;
    WaveManager waveManager;
    public bool isDestroying;
    public int ghostDamage = 1;
    public float ghostHp = 30;
    public float ghostSp;
    public float ghostSize;
    public int ghostDrop;
    [SerializeField] GameObject expDrop;
    public List<bool> abilityList = new List<bool>();

    [Header("EnemyStats")]
    public EnemyInfoData enemyInfo;

    [Header("EnemyDebuff")]
    public bool isElected;
    public bool isFired;
    public bool isFreezed;
    float debuffTimer;

    //Enemy Ability


    void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerManager>();
        waveManager = GetComponentInParent<WaveManager>();
        expPool = GetComponentInParent<ExpPool>();
        enemyPool = GetComponentInParent<EnemyPool>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
    }
    void Update()
    {
        GhostStatus();
        GhostMovement();
        GhostDebuffManager();
        //¼¼ÄÜ¼ì²â
    }
    void GhostDebuffManager() 
    {
        if (isElected)
        {
            sp.color = new Color(1, 1, 0.5F);
        }
        else if (isFired)
        {
            sp.color = new Color(1, 0.5f, 0.5f);
        }
        else if (isFreezed)
        {
            agent.speed = (ghostSp * 0.4f) / 2;
            sp.color = new Color(0.5f, 1, 1);
        }
        else 
        {
            sp.color = new Color(1, 1, 1);
            agent.speed = (ghostSp * 0.4f);
        }
    }
    void GhostStatus() 
    {
        if (gameObject.transform.position.x < 0)
        {
            sp.flipX = false;
        }
        else
        {
            sp.flipX = true;
        }
        if (ghostHp <= 0)
        {
            isDestroying = true;
        }
        if (player.curHealth <= 0) 
        {
            GhostRemove();
        } 
        if (isDestroying)
        {
            gameObject.transform.DORotate(new Vector3(0, 0, 360), 0.75f, RotateMode.FastBeyond360).SetEase(Ease.OutQuad);
            gameObject.transform.DOScale(new Vector3(0.06f, 0.06f, 0.06f), 0.5f).SetEase(Ease.OutQuad).OnComplete(() => GhostRemove());
        }
        if (debuffTimer > 0)
        {
            debuffTimer -= Time.deltaTime;
        }
        else 
        {
            isFreezed = false;
            isFired = false;
            isElected = false;
        }
    }
    void GhostMovement() 
    {
        if (isDestroying) return;
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance > 0) 
        {
            //MoveTowards
            agent.SetDestination(player.transform.position); 
            //gameObject.transform.position = Vector2.MoveTowards(transform.position, player.transform.position, ghostSp*Time.deltaTime*0.25f);
        }
    }
    public void GhostRemove() 
    {
        waveManager.curGhosts.Remove(this);
        //enemyPool.ghostPrefabPool.Release(this);
        Destroy(this.gameObject);
        ghostHp = enemyInfo.enemyHealth;
        gameObject.transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
        isFreezed = false;
        debuffTimer = 0;
        isDestroying = false;
    }
    public void EnemyInfoImport(EnemyInfoData enemyData) 
    {
        enemyInfo = enemyData;
        sp.sprite = enemyData.sprite;
        ghostHp = enemyData.enemyHealth;
        ghostSp = enemyData.enemySpeed;
        ghostDamage = enemyData.enemyDamage;
        ghostDrop = enemyData.enemyDropIndex;
        for (int i = 0; i <= abilityList.Count - 1; i++)
        {
            abilityList[i] = enemyData.enemyAbilities[i];
        }
        gameObject.transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
        agent.speed = ghostSp * 0.4f;
    }
    public void DamageTaken(float damage, ElementType type) 
    {
        ghostHp -= damage;
        if (type == ElementType.Ice) 
        {
            debuffTimer = 3f;
            isFreezed = true;
        }
        if (ghostHp <= 0)
        {
            var exp = expPool.expPrefabPool.Get();
            exp.transform.position = transform.position;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.curHealth -= ghostDamage;
            GhostRemove();
        }
    }
}
