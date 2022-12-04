using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    Rigidbody2D rigidbody;
    [Header("×Óµ¯ÊôÐÔ")]
    public float bulletDamage = 1f;
    public float speed = 2f;
    public int bulletPenHealth;
    public bool bulletKnockOff;
    public bool bulletBounce;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    public void SetSpeed(Vector2 dir) 
    {
        rigidbody.velocity = dir * speed;
    }
}
