using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/New Enemy")]
public class EnemyInfoData : ScriptableObject
{
    public int enemyDamage;
    public float enemyHealth;
    public float enemySpeed;
    public float enemyWeight;
    public int enemyDropIndex;
    public List<bool> enemyAbilities = new List<bool>();
}
