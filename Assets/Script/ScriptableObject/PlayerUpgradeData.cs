using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrade/New Upgrade")]
public class PlayerUpgradeData : ScriptableObject
{
    public int upgradeIndex;  //����Ŀ¼
    public int upgradeType; //����Ŀ¼: 1-���� 2-�ӵ� 3-��ָ 4-���� ������
    public string upgradeName;
    public string upgradeDescription;
}
