using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerUpgradeListData curUpgradeList;
    [SerializeField] UpgradeHolder[] upgradeSlots;

    public void RandomUpgradePopUp() 
    {
        List<PlayerUpgradeData> tempUprades = new List<PlayerUpgradeData>();
        foreach (PlayerUpgradeData upgrade in curUpgradeList.upgradeList) 
        {
            tempUprades.Add(upgrade);
        }
        for (int i = 0; i <= upgradeSlots.Length-1; i++) 
        {
            int random = Random.Range(0, tempUprades.Count);
            upgradeSlots[i].curUpgrade = tempUprades[random];
            upgradeSlots[i].UpgradeUpdate();
            tempUprades.Remove(tempUprades[random]);
        }
    }
}
