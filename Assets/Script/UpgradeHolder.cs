using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeHolder : MonoBehaviour
{
    public PlayerUpgradeData curUpgrade;
    [SerializeField] Image upgradeImage;
    [SerializeField] TextMeshProUGUI upgradeName;
    [SerializeField] TextMeshProUGUI upgradeDes;

    public void UpgradeUpdate() 
    {
        //upgradeImage = curUpgrade.upgradeIcon;
        upgradeName.text = curUpgrade.upgradeName;
        //upgradeDes.text = curUpgrade.upgradeDescription;
    } 
}
