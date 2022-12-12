using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrade/New Upgrade")]
public class PlayerUpgradeData : ScriptableObject
{
    public int upgradeIndex;  //����Ŀ¼
    public UpgradeType curType; //����Ŀ¼: 1-���� 2-��̨ 3-��ָ 4-����
    public Image upgradeIcon;
    public string upgradeName;
    public string upgradeDescription;
    public ElementType curElement = ElementType.Normal;

    [Header("�������")]
    public int requiredFireTimes = 0;
    public int effectRadius = 0;
    public float fireRateChange = 0f;
    public float[] effectNumberByLevel = new float[6];

    [Header("��̨���")]
    public TurretInfoData curTurretData;
}

public enum UpgradeType {Passive,Turret};
public enum ElementType {Normal,Fire,Ice,Elect};
