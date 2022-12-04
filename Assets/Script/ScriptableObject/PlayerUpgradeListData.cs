using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deck", menuName = "Upgrade/New Deck")]
public class PlayerUpgradeListData : ScriptableObject
{
    public List<PlayerUpgradeData> upgradeList = new List<PlayerUpgradeData>();
}
