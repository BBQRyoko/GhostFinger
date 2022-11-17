using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    Rigidbody2D rigidbody;
    public float bulletDamage = 10f;
    public float speed = 2f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    public void SetSpeed(Vector2 dir) 
    {
        rigidbody.velocity = dir * speed;
    }
}
