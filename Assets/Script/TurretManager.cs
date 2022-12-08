using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] BulletPool bulletPool;
    float timerCounter = 0;
    float rotateRadius;
    [SerializeField] GameObject turretRef;
    public LayerMask targetLayer;
    public GhostManager curTarget;
    [SerializeField] Transform turretGO;

    [Header("TurretStats")]
    [SerializeField] float rotateSpeed = -0.75f;
    [SerializeField] float fovRadius;
    [Range(1, 360)][SerializeField] float fovAngle = 45f;
    [SerializeField] int bulletNum;
    [SerializeField] float defaultFireTimer, defaultShootingPause = 0.05f, attackDamage;
    int ammoNum;
    int defaultAmmo = 1;
    float fireTimer;
    float shootingTimer;

    private void Awake()
    {
        bulletPool = GetComponentInParent<BulletPool>();
    }
    void Start()
    {
        rotateRadius = Mathf.Abs(Vector2.Distance(transform.position, playerManager.transform.position));
    }

    void Update()
    {
        TurretMoving();
        StartCoroutine(FOVCheck());
        TurretShooting();

        if (ammoNum <= 0)
        {
            if (shootingTimer > 0)
            {
                shootingTimer -= Time.deltaTime;
            }
            else
            {
                shootingTimer = 0;
            }
        }

        if (fireTimer > 0)
        {
            if (shootingTimer <= 0)
            {
                fireTimer -= Time.deltaTime;
            }
        }
        else
        {
            fireTimer = 0;
            ammoNum = defaultAmmo;
        }

        if (curTarget) 
        {
            if (curTarget.isDestroying || !curTarget.gameObject.active) 
            {
                curTarget = null;
            }
        }
    }

    private void TurretMoving()
    {
        if (curTarget && ammoNum > 0) return;
        if (shootingTimer > 0) return;
        timerCounter += Time.deltaTime * (-rotateSpeed);

        float x = Mathf.Cos(timerCounter) * rotateRadius;
        float y = Mathf.Sin(timerCounter) * rotateRadius;

        transform.position = new Vector2(x, y);

        Vector2 turretDirection = (transform.position - playerManager.transform.position).normalized;
        transform.right = turretDirection;

    }

    void TurretShooting()
    {
        if (!curTarget || ammoNum <= 0) return;
        if (curTarget.isDestroying) curTarget = null;

        if (curTarget)
        {
            if (bulletNum <= 1)
            {
                bulletNum = 1;
                GameObject bullet = bulletPool.bulletPrefabPool.Get();
                bullet.transform.position = turretGO.position;
                bullet.GetComponent<BulletManager>().bulletDamage *= attackDamage;
                Vector2 dir = new Vector2(curTarget.transform.position.x - transform.position.x, curTarget.transform.position.y - transform.position.y);
                dir.Normalize();
                bullet.GetComponent<Rigidbody2D>().AddForce(dir * 200f);
            }
            else
            {
                float bulletAngle = 15f;
                int median = bulletNum / 2;
                for (int i = 0; i < bulletNum; i++)
                {
                    GameObject bullet = bulletPool.bulletPrefabPool.Get();
                    bullet.transform.position = turretGO.position;
                    bullet.GetComponent<BulletManager>().bulletDamage *= attackDamage;
                    Vector2 dir = new Vector2(curTarget.transform.position.x - transform.position.x, curTarget.transform.position.y - transform.position.y);

                    if (bulletNum % 2 == 1)
                    {
                        bullet.GetComponent<BulletManager>().SetSpeed(Quaternion.AngleAxis(bulletAngle * (i - median), Vector3.forward) * dir);
                    }
                    else
                    {
                        bullet.GetComponent<BulletManager>().SetSpeed(Quaternion.AngleAxis(bulletAngle * (i - median) + bulletAngle / 2, Vector3.forward) * dir);
                    }
                }
            }
            ammoNum -= 1;
            if (ammoNum <= 0)
            {
                shootingTimer = defaultShootingPause;
                fireTimer = defaultFireTimer;
            }
        }
    }
    private IEnumerator FOVCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        while (true)
        {
            yield return wait;
            FOV();
        }
    }

    private void FOV()
    {
        if (fireTimer > 0 ) return;
        if (curTarget) return;
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, fovRadius, targetLayer);
        float shortestDistance = 10;
        if (rangeCheck != null)
        {
            foreach (Collider2D enemy in rangeCheck) 
            {
                Transform target = enemy.transform;
                Vector2 directionToTarget = (target.position - transform.position).normalized;

                if (Vector2.Angle(turretGO.position - transform.position, directionToTarget) < fovAngle / 2)
                {
                    float distanceToTarget = Vector2.Distance(transform.position, target.position);
                    if (!target.GetComponent<GhostManager>().isDestroying && distanceToTarget < shortestDistance) 
                    {
                        shortestDistance = distanceToTarget;
                        curTarget = target.GetComponent<GhostManager>();
                    }
                }
            }
        }
    }
}
