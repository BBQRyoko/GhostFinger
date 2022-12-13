using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretManager : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] BulletPool bulletPool;
    float timerCounter = 0;
    float rotateRadius;
    public LayerMask targetLayer;
    public GhostManager curTarget;
    [SerializeField] Transform turretGO;

    [Header("TurretStats")]
    public TurretInfoData turretInfo;
    public int turretRank = 1;
    [SerializeField] float rotateSpeed = -0.75f;
    [SerializeField] float fovRadius;
    [Range(1, 360)][SerializeField] float fovAngle = 45f;
    [SerializeField] int bulletNum;
    [SerializeField] float defaultFireTimer, defaultShootingPause = 0.05f;
    int ammoNum;
    int defaultAmmo = 1;
    float fireTimer;
    float shootingTimer;

    [Header("BulletStats")]
    [SerializeField] GameObject bulletGO;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletDamage, bulletExplodeDamage;
    [Range(1, 2)][SerializeField] float explosionRadius;
    [SerializeField] bool bulletExplode, bulletAuto, bulletPentrate;
    [SerializeField] int bulletDeflectNum;
    [SerializeField] ElementType element;

    private void Awake()
    {
        bulletPool = GetComponentInParent<BulletPool>();
        if (turretInfo != null) TurretSetUp();
    }
    void Start()
    {
        rotateRadius = Mathf.Abs(Vector2.Distance(transform.position, playerManager.transform.position));
    }
    void Update()
    {
        if (turretInfo == null)
        {
            this.gameObject.SetActive(false);
        }
        else 
        {
            this.gameObject.SetActive(true);
        }

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
            curTarget = null;
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
    public void TurretSetUp() 
    {
        rotateSpeed = turretInfo.rotateSpeed;
        fovRadius = turretInfo.fovRadius;
        fovAngle = turretInfo.fovAngle;
        bulletNum = turretInfo.bulletNum;
        defaultFireTimer = turretInfo.defaultFireTimer;
        defaultShootingPause = turretInfo.defaultShootingPause;
        bulletGO = turretInfo.bulletPrefab;
        bulletSpeed = turretInfo.bulletSpeed;
        bulletDamage = turretInfo.attackDamage;
        bulletExplodeDamage = turretInfo.explodeDamage;
        explosionRadius = turretInfo.explosionRadius;
        bulletExplode = turretInfo.canExplode;
        bulletAuto = turretInfo.autoTarget;
        bulletPentrate = turretInfo.canPenetrate;
        bulletDeflectNum = turretInfo.deflectNum;
        element = turretInfo.bulletElement;
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
                BulletSetUp(bullet.GetComponent<BulletManager>());
                Vector2 dir = new Vector2(curTarget.transform.position.x - transform.position.x, curTarget.transform.position.y - transform.position.y);
                dir.Normalize();
                bullet.transform.right = dir;
                bullet.GetComponent<BulletManager>().SetSpeed(dir);
            }
            else
            {
                float bulletAngle = 15f;
                int median = bulletNum / 2;
                for (int i = 0; i < bulletNum; i++)
                {
                    GameObject bullet = bulletPool.bulletPrefabPool.Get();
                    BulletSetUp(bullet.GetComponent<BulletManager>());
                    Vector2 dir = new Vector2(curTarget.transform.position.x - transform.position.x, curTarget.transform.position.y - transform.position.y);
                    dir.Normalize();
                    bullet.transform.right = dir;
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
    public void TurretRankUp() 
    {
        turretRank += 1;
        if (turretInfo.upgradeList[turretRank - 2] == TurretUpgradeType.RotateSpeed)
        {
            rotateSpeed = turretInfo.upgradeEffectNum[turretRank - 2];
        }
        else if (turretInfo.upgradeList[turretRank - 2] == TurretUpgradeType.FovRadius)
        {
            fovRadius = turretInfo.upgradeEffectNum[turretRank - 2];
        }
        else if (turretInfo.upgradeList[turretRank - 2] == TurretUpgradeType.BulletNum)
        {
            bulletNum = (int)turretInfo.upgradeEffectNum[turretRank - 2];
        }
        else if (turretInfo.upgradeList[turretRank - 2] == TurretUpgradeType.FireTimer)
        {
            defaultFireTimer = turretInfo.upgradeEffectNum[turretRank - 2];
        }
        else if (turretInfo.upgradeList[turretRank - 2] == TurretUpgradeType.AttackDamage)
        {
            bulletDamage = turretInfo.upgradeEffectNum[turretRank - 2];
        }
        else if (turretInfo.upgradeList[turretRank - 2] == TurretUpgradeType.BulletSpeed) 
        {
            bulletSpeed = turretInfo.upgradeEffectNum[turretRank - 2];
        }
        else if (turretInfo.upgradeList[turretRank - 2] == TurretUpgradeType.ExplosionDamage)
        {
            bulletExplodeDamage = turretInfo.upgradeEffectNum[turretRank - 2];
        }
        else if (turretInfo.upgradeList[turretRank - 2] == TurretUpgradeType.ExplosionRadius)
        {
            explosionRadius = turretInfo.upgradeEffectNum[turretRank - 2];
        }
        else if (turretInfo.upgradeList[turretRank - 2] == TurretUpgradeType.ShootingPause)
        {
            defaultShootingPause = turretInfo.upgradeEffectNum[turretRank - 2];
        }
        else if (turretInfo.upgradeList[turretRank - 2] == TurretUpgradeType.DeflectNum)
        {
            bulletDeflectNum = (int)turretInfo.upgradeEffectNum[turretRank - 2];
        }
        else if (turretInfo.upgradeList[turretRank - 2] == TurretUpgradeType.Penetrate)
        {
            bulletPentrate = true;
        }
        else if (turretInfo.upgradeList[turretRank - 2] == TurretUpgradeType.TurretNum)
        {
            //Later
        }
    }
    public void BulletSetUp(BulletManager bullet) 
    {
        bullet.GetComponent<SpriteRenderer>().sprite = bulletGO.GetComponent<SpriteRenderer>().sprite;
        bullet.transform.localScale = bulletGO.transform.localScale;
        bullet.transform.rotation = bulletGO.transform.rotation;
        bullet.transform.position = turretGO.position;
        bullet.bulletDamage = bulletDamage; //之后还要加上玩家本身的数值计算
        bullet.speed = bulletSpeed;
        bullet.canExplode = bulletExplode;
        bullet.autoTarget = bulletAuto;
        bullet.penetrate = bulletPentrate;
        bullet.deflectNum = bulletDeflectNum;
        bullet.explosionRadius = explosionRadius;
        bullet.explosionDamamge = bulletExplodeDamage;
        bullet.bulletElement = element;
    }
}
