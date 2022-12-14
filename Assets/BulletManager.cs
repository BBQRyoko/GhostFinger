using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    Rigidbody2D rigidbody;
    [Header("×Óµ¯ÊôÐÔ")]
    public float bulletDamage = 1f;
    public float explosionDamamge = 5f;
    [Range(1,2)] public float explosionRadius = 1f;
    [SerializeField] Vector3 shootDir;
    [SerializeField] Vector3 ve;
    public float speed = 2f;
    public bool canExplode, autoTarget, penetrate;
    public int deflectNum;
    public ElementType bulletElement = ElementType.Normal;
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
        ve = rigidbody.velocity;
        if (Mathf.Abs(rigidbody.velocity.x) <= 0.005f || Mathf.Abs(rigidbody.velocity.y) <= 0.005f ) 
        {
            rigidbody.velocity = shootDir * speed;
        }
    }
    public void SetSpeed(Vector2 dir)
    {
        shootDir = dir;
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
            collision.gameObject.GetComponent<GhostManager>().DamageTaken(bulletDamage, bulletElement);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BulletPool bulletPool = GetComponentInParent<BulletPool>();
        if (collision.gameObject.tag == "Ghost") 
        {
            if (!penetrate)
            {
                collision.gameObject.GetComponent<GhostManager>().DamageTaken(bulletDamage, bulletElement);
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
                        var radius = explosionArea.transform.localScale.x;
                        explosionArea.transform.localScale = new Vector3(radius, radius, radius) * explosionRadius;
                        explosionArea.GetComponent<GeneralDamager>().damage = explosionDamamge;
                        Destroy(explosionArea, 0.75f);
                    }
                    bulletPool.bulletPrefabPool.Release(this.gameObject);
                }
            }
        }
    }
}
