using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralDamager : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ghost") 
        {
            collision.gameObject.GetComponent<GhostManager>().DamageTaken(damage);
        }
    }
}
