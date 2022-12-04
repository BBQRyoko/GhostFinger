using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.EventSystems;




public class GhostManager : MonoBehaviour
{
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

    //Enemy Ability


    void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerManager>();
        waveManager = GetComponentInParent<WaveManager>();
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        GhostInitialize();
    }
    void Update()
    {
        GhostStatus();
        GhostMovement();
        GhostDebuffManager();
        //¼¼ÄÜ¼ì²â
    }
    public void GhostInitialize() 
    {
        ghostHp = enemyInfo.enemyHealth;
        ghostSp = enemyInfo.enemySpeed;
        ghostDamage = enemyInfo.enemyDamage;
        //weight ...
        ghostDrop = enemyInfo.enemyDropIndex;
        for (int i = 0; i <= abilityList.Count - 1; i++) 
        {
            abilityList[i] = enemyInfo.enemyAbilities[i];
        }
        agent.speed *= ghostSp;
    }
    public void GhostSpeedSet(int wave) 
    {
        float random = Random.Range(-0.25f, 1.25f);
        ghostSp = (wave - 1) * 0.25f + 1.5f + random;
        
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
            sp.color = new Color(0.5f, 1, 1);
        }
        else 
        {
            sp.color = new Color(1, 1, 1);
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
        if (player.curHealth <= 0) GhostRemove();
        if (isDestroying)
        {
            gameObject.transform.DORotate(new Vector3(0, 0, 360), 0.75f, RotateMode.FastBeyond360).SetEase(Ease.OutQuad);
            gameObject.transform.DOScale(new Vector3(0.06f, 0.06f, 0.06f), 0.5f).SetEase(Ease.OutQuad).OnComplete(() => GhostRemove());
        }
    }
    void GhostMovement() 
    {
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
        if (isDestroying) 
        {
            GameObject exp = Instantiate(expDrop, transform);
            exp.transform.parent = null;
        }
        waveManager.curGhosts.Remove(this);
        Destroy(gameObject);
    }
    private void OnMouseOver()
    {
        //if (Input.GetMouseButtonDown(0)) 
        //{
        //    player.curEnergy += 10;
        //    isDestroying = true;
        //}
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //isDestroying = true;
            player.curHealth -= 10;
            GhostRemove();
        }
        else if (collision.gameObject.CompareTag("Ability"))
        {
            isDestroying = true;
        }
        else if (collision.gameObject.CompareTag("Bullet")) 
        {
            ghostHp -= collision.GetComponent<BulletManager>().bulletDamage;
            if (collision.GetComponent<BulletManager>().bulletPenHealth > 0) 
            {
                collision.GetComponent<BulletManager>().bulletPenHealth -= 1;
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
