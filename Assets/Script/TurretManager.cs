using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] float rotateSpeed = -0.75f;
    float timerCounter = 0;
    float rotateRadius;

    [Header("FOV")]
    [SerializeField] GameObject turretRef;
    [SerializeField] float fovRadius;
    [Range(1, 360)][SerializeField] float fovAngle = 45f;
    public LayerMask targetLayer;

    public GhostManager curTarget;

    [Header("TurretStats")]
    [SerializeField] Transform turretGO;
    bool isShooting;
    int ammoNum;
    float fireTimer;
    float reloadTimer;
    float shootingTimer;
    [SerializeField] int bulletNum, defaultAmmo;
    [SerializeField] float defaultFireRate, reloadSpeed, attackDamage;
    [SerializeField] GameObject bulletPrefab;


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

        if (reloadTimer > 0)
        {
            if (shootingTimer <= 0) 
            {
                reloadTimer -= Time.deltaTime;
            }
        }
        else
        {
            reloadTimer = 0;
            ammoNum = defaultAmmo;
        }
    }

    private void TurretMoving()
    {
        if (curTarget && ammoNum > 0) return;
        if (shootingTimer > 0) return;
        timerCounter += Time.deltaTime * (rotateSpeed);

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

        if (fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
        }
        else 
        {
            if (curTarget)
            {
                if (bulletNum <= 1)
                {
                    bulletNum = 1;
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                    bullet.GetComponent<BulletManager>().bulletDamage *= attackDamage;
                    Vector2 dir = new Vector2(curTarget.transform.position.x - transform.position.x, curTarget.transform.position.y - transform.position.y);
                    dir.Normalize();
                    bullet.GetComponent<Rigidbody2D>().AddForce(dir * 200f);
                    Destroy(bullet, 2f);
                }
                else
                {
                    float bulletAngle = 15f;
                    int median = bulletNum / 2;
                    for (int i = 0; i < bulletNum; i++)
                    {
                        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
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
                        Destroy(bullet, 1f);
                    }
                }
                ammoNum -= 1;
                if (ammoNum <= 0)
                {
                    shootingTimer = 0.05f;
                    reloadTimer = reloadSpeed;
                }
                else 
                {
                    fireTimer = defaultFireRate;
                }
            }
        }
    }
    private IEnumerator FOVCheck() 
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true) 
        {
            yield return wait;
            FOV();
        } 
    }

    private void FOV() 
    {
        if (ammoNum<=0 || reloadTimer>0) return;
        Collider2D rangeCheck = Physics2D.OverlapCircle(transform.position, fovRadius, targetLayer);

        if (rangeCheck != null)
        {
            Transform target = rangeCheck.transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;
            if (Vector2.Angle(turretGO.position - transform.position, directionToTarget) < fovAngle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);
                if(!target.GetComponent<GhostManager>().isDestroying) curTarget = target.GetComponent<GhostManager>();
            }
            else 
            {
                curTarget = null;
            }
        }
        else 
        {
            curTarget = null;
        }
    }

    private void OnDrawGizmos()
    {

    }
}
