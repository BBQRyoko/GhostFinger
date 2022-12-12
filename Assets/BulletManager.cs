using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    Rigidbody2D rigidbody;
    [Header("×Óµ¯ÊôÐÔ")]
    public float bulletDamage = 1f;
    public float explosionDamamge = 5f;
    public float speed = 2f;
    public bool canExplode, autoTarget, penetrate;
    public int deflectNum;
    [SerializeField] GameObject explosion;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (penetrate)
        {
            this.GetComponent<Collider2D>().isTrigger = true;
        }
        else 
        {
            this.GetComponent<Collider2D>().isTrigger = false;
        }
    }
    public void SetSpeed(Vector2 dir)
    {
        rigidbody.velocity = dir * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BulletPool bulletPool = GetComponentInParent<BulletPool>();
        if (collision.gameObject.tag == "Bound")
        {
            bulletPool.bulletPrefabPool.Release(this.gameObject);
        }
        else if (collision.gameObject.tag == "Ghost") 
        {
            collision.gameObject.GetComponent<GhostManager>().DamageTaken(bulletDamage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BulletPool bulletPool = GetComponentInParent<BulletPool>();
        if (collision.gameObject.tag == "Ghost") 
        {
            if (!penetrate)
            {
                collision.gameObject.GetComponent<GhostManager>().DamageTaken(bulletDamage);
                if (deflectNum > 0)
                {
                    var direction = Vector3.Reflect(rigidbody.velocity.normalized, collision.contacts[0].normal);
                    rigidbody.velocity = direction * speed;
                    deflectNum -= 1;
                }
                else
                {
                    if (canExplode)
                    {
                        var explosionArea = Instantiate(explosion, transform);
                        explosionArea.transform.parent = null;
                        explosionArea.GetComponent<GeneralDamager>().damage = explosionDamamge;
                        Destroy(explosionArea, 0.75f);
                    }
                    bulletPool.bulletPrefabPool.Release(this.gameObject);
                }
            }
        }
    }
}
