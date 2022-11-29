using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrade/New Upgrade")]
public class PlayerUpgradeData : ScriptableObject
{
    public int upgradeIndex;  //总类目录
    public int upgradeType; //类型目录: 1-自身 2-子弹 3-手指 4-场地 。。。
    public string upgradeName;
    public string upgradeDescription;
}
