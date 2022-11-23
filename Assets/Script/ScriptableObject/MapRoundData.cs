using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map", menuName = "Map/New Map")]
public class MapRoundData : ScriptableObject
{
    public int mapIndex = 0;

    public RoundData[] roundsInfo;

}
