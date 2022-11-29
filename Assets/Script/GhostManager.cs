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

    //Enemy Ability


    void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerManager>();
        waveManager = GetComponentInParent<WaveManager>();
        GhostInitialize();
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
    }
    void Update()
    {
        GhostStatus();
        GhostMovement();
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
    }
    public void GhostSpeedSet(int wave) 
    {
        float random = Random.Range(-0.25f, 1.25f);
        ghostSp = (wave - 1) * 0.25f + 1.5f + random;
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
        if (ghostHp <= 0) isDestroying = true;
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
        waveManager.curGhosts.Remove(this);
        GameObject exp = Instantiate(expDrop, transform);
        exp.transform.parent = null;
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
