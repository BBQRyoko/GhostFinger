using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrade/New Upgrade")]
public class PlayerUpgradeData : ScriptableObject
{
    public int upgradeIndex;  //总类目录
    public UpgradeType curType; //类型目录: 1-自身 2-子弹 3-手指 4-场地
    public Image upgradeIcon;
    public string upgradeName;
    public string upgradeDescription;
    public ElementType curElement = ElementType.Normal;

    [Header("自身相关")]
    public int requiredFireTimes = 0;
    public int effectRadius = 0;
    public float fireRateChange = 0f;
    public int targetNum;

    [Header("子弹相关")]
    public int bulletNumChange = 0;
    public float bulletDamageChange = 0f;
    public bool canChaseTarget = false;

    [Header("场地相关")]
    public bool isPet;
    public float petDamageChange = 0f;

    public float[] effectNumberByLevel = new float[6];
}

public enum UpgradeType {Self,Bullet,Finger};
public enum ElementType {Normal,Fire,Ice,Elect};
