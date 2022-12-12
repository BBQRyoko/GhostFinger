using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrade/New Upgrade")]
public class PlayerUpgradeData : ScriptableObject
{
    public int upgradeIndex;  //总类目录
    public UpgradeType curType; //类型目录: 1-自身 2-炮台 3-手指 4-场地
    public Image upgradeIcon;
    public string upgradeName;
    public string upgradeDescription;
    public ElementType curElement = ElementType.Normal;

    [Header("被动相关")]
    public int requiredFireTimes = 0;
    public int effectRadius = 0;
    public float fireRateChange = 0f;
    public float[] effectNumberByLevel = new float[6];

    [Header("炮台相关")]
    public TurretInfoData curTurretData;
}

public enum UpgradeType {Passive,Turret};
public enum ElementType {Normal,Fire,Ice,Elect};
