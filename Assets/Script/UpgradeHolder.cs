using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeHolder : MonoBehaviour
{
    PlayerManager playerManager;
    public PlayerUpgradeData curUpgrade;
    [SerializeField] Image upgradeImage;
    [SerializeField] TextMeshProUGUI upgradeName;
    [SerializeField] TextMeshProUGUI upgradeDes;
    public void UpgradeUpdate() 
    {
        playerManager = FindObjectOfType<PlayerManager>();
        //upgradeImage = curUpgrade.upgradeIcon;
        if (playerManager.turretsUpgradeList.Contains(curUpgrade))
        {
            if (playerManager.turretsUpgradeList.Count == 1)
            {
                if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[0].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.RotateSpeed)
                {
                    upgradeDes.text = "Rotate Faster";
                }
                else if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[0].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.AttackDamage) 
                {
                    upgradeDes.text = "Higher Damage";
                }
                else if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[0].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.BulletNum)
                {
                    upgradeDes.text = "More Bullet";
                }
                else if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[0].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.BulletSpeed)
                {
                    upgradeDes.text = "Bullet Faster";
                }
                else if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[0].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.DeflectNum)
                {
                    upgradeDes.text = "Deflect More";
                }
                else if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[0].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.ExplosionDamage)
                {
                    upgradeDes.text = "Higher Explosion Damage";
                }
                else if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[0].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.ExplosionRadius)
                {
                    upgradeDes.text = "Larger Explosion Radius";
                }
                else if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[0].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.FireTimer)
                {
                    upgradeDes.text = "Fire Fast";
                }
                else if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[0].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.FovRadius)
                {
                    upgradeDes.text = "Increase Range";
                }
                else if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[0].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.Penetrate)
                {
                    upgradeDes.text = "Can Penetrate";
                }
                else if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[0].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.ShootingPause)
                {
                    upgradeDes.text = "Shoter Pausing";
                }
            }
            else if (playerManager.turretsUpgradeList.Count >= 2) 
            {
                for (int i = 0; i <= playerManager.turretsUpgradeList.Count - 1; i++) 
                {
                    if (playerManager.turretsUpgradeList[i] == curUpgrade)
                    {
                        if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[i].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.RotateSpeed)
                        {
                            upgradeDes.text = "Rotate Faster";
                        }
                        else if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[i].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.AttackDamage)
                        {
                            upgradeDes.text = "Higher Damage";
                        }
                        else if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[i].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.BulletNum)
                        {
                            upgradeDes.text = "More Bullet";
                        }
                        else if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[i].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.BulletSpeed)
                        {
                            upgradeDes.text = "Bullet Faster";
                        }
                        else if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[i].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.DeflectNum)
                        {
                            upgradeDes.text = "Deflect More";
                        }
                        else if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[i].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.ExplosionDamage)
                        {
                            upgradeDes.text = "Higher Explosion Damage";
                        }
                        else if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[i].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.ExplosionRadius)
                        {
                            upgradeDes.text = "Larger Explosion Radius";
                        }
                        else if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[i].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.FireTimer)
                        {
                            upgradeDes.text = "Fire Fast";
                        }
                        else if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[i].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.FovRadius)
                        {
                            upgradeDes.text = "Increase Range";
                        }
                        else if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[i].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.Penetrate)
                        {
                            upgradeDes.text = "Can Penetrate";
                        }
                        else if (curUpgrade.curTurretData.upgradeList[playerManager.tempTurretsSlots[i].GetComponent<TurretManager>().turretRank - 1] == TurretUpgradeType.ShootingPause)
                        {
                            upgradeDes.text = "Shoter Pausing";
                        }
                    }
                }
            }
        }
        else 
        {
            upgradeDes.text = "New";
        }
        upgradeName.text = curUpgrade.upgradeName;
    } 
}
