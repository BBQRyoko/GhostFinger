using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Turret", menuName = "Turret/New Turret")]
public class TurretInfoData : ScriptableObject
{
    [Header("Default")]
    public float rotateSpeed = -0.75f, fovRadius = 3;
    [Range(1, 360)] public float fovAngle = 45f;
    public int bulletNum;
    public float defaultFireTimer, defaultShootingPause = 0.05f;

    [Header("BulletRelated")]
    public BulletManager bulletPrefab;
    public bool canExplode, autoTarget, canPenetrate;
    [Range(0, 2)]public float defaultLifeTime;
    public float attackDamage;
    public float explodeDamage;

    [Header("Upgrade")]
    public TurretUpgradeType[] upgradeList;
    public float[] upgradeEffectNum;
}

public enum TurretUpgradeType {RotateSpeed, FovRadius, BulletNum, FireTimer, AttackDamage, TurretNum};
