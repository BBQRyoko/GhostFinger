using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    PlayerManager player;
    public int waveNum;
    public int roundNum = 0;
    float roundGap = 0;
    [SerializeField] GhostManager ghost;
    public List<GhostManager> curGhosts = new List<GhostManager>();
    [SerializeField] float radius;
    [SerializeField] TextMeshProUGUI waveText;
    private void Awake()
    {
        waveNum = 1;
        player = FindObjectOfType<PlayerManager>();
    }

    void Update()
    {
        GhostManagment();
    }
    void GhostManagment() 
    {
        if (roundGap > 0) 
        {
            roundGap -= Time.deltaTime;
        }
        if (roundNum >= waveNum + 2)
        {
            if (curGhosts.Count <= 0) 
            {
                if (roundGap > 1.5f) 
                {
                    roundGap = 1.5f;
                }
                roundNum = 0;
                waveNum += 1;
                curGhosts.Clear();
            }
        }
        else 
        {
            if (roundGap <= 0)
            {
                if (player.curHealth > 0 && !player.abilityUsing)
                {
                    roundNum++;
                    GhostSpawn(waveNum, roundNum);
                    roundGap = waveNum + 5f;
                }
            }
            else 
            {
                if (curGhosts.Count <= 0) 
                {
                    if (roundGap > 1.5f)
                    {
                        roundGap = 1.5f;
                    }
                }
            }
        }
        waveText.text = waveNum.ToString();
    }
    void GhostSpawn(int wave, int round) 
    {
        for (int i = 0; i < 2 + (wave - 1) + round; i++) 
        {
            GhostManager spawnedGhost = Instantiate(ghost, transform);
            spawnedGhost.GhostSpeedSet(wave);
            curGhosts.Add(spawnedGhost);
        }
        float radiansOfSeparation = (Mathf.PI * 2) / curGhosts.Count;
        for (int i = 0; i < curGhosts.Count; i++) 
        {
            float randomRadian = Random.Range(-1, 1) * (Mathf.PI / 12);

            float x = Mathf.Sin(radiansOfSeparation * i + randomRadian) * (radius + Random.Range(-0.75f, 0.75f));
            if (x == 0) x = 0.01f;
            float y = Mathf.Cos(radiansOfSeparation * i + randomRadian) * (radius + Random.Range(-0.75f, 0.75f));

            curGhosts[i].GetComponent<Transform>().position = new Vector3(x, y, 0);
        }
    }
}
