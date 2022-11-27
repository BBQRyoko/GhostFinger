using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class WaveManager : MonoBehaviour
{
    PlayerManager player;
    public int roundNum = 0;
    float roundCounter = 0;
    [SerializeField] float defaultRoundTime = 30f;
    float enemySpawnCounter = 0;
    [SerializeField] GhostManager ghost;
    public List<GhostManager> curGhosts = new List<GhostManager>();
    [SerializeField] float radius;
    [SerializeField] TextMeshProUGUI waveText;

    [Header("TempSpawnRule")]
    [SerializeField] TextAsset tempSpawnRuleData;
    public string[] data;
    [SerializeField] MapRoundData mapRounds;

    [Header("GameobjectHolders")]
    [SerializeField] Transform allParent;
    [SerializeField] Vector3 originalScale = new Vector3(1,1,1);
    [SerializeField] Vector3 finalScale = new Vector3(0.5f, 0.5f, 0.5f);
    public float gameTimer;

    private void Awake()
    {
        roundNum = 0;
        roundCounter = defaultRoundTime;
        enemySpawnCounter = 0;
        player = FindObjectOfType<PlayerManager>();
        allParent.localScale = originalScale;
        gameTimer = 0;
    }
    void Update()
    {
        RoundSpawnManager();
        GameScaleManager();
    }

    void GameScaleManager() 
    {
        gameTimer += Time.deltaTime;
        if (gameTimer < 60f)
        {
            allParent.localScale = originalScale;
        }
        else if (gameTimer > 360f)
        {
            allParent.localScale = finalScale;
        }
        else 
        {
            float tempScale = (1f - (0.35f / 300) * (gameTimer - 60));
            allParent.localScale = new Vector3(tempScale, tempScale, tempScale);
        }
    }

    void RoundSpawnManager() 
    {
        //RoundTimeManager
        if (roundCounter <= 0)
        {
            if (curGhosts.Count <= 0)
            {
                roundCounter = defaultRoundTime;
                roundNum += 1;
            }
            else 
            {
                roundCounter = 0;
            }
        }
        else 
        {
            roundCounter -= Time.deltaTime;
        }

        //EnemySpawnTimer
        if (enemySpawnCounter <= 0)
        {
            if (roundCounter > 0f)
            {
                enemySpawnCounter = 3f;
                curGhosts = GameObject.FindObjectsOfType<GhostManager>().ToList();
                if (curGhosts.Count >= mapRounds.roundsInfo[roundNum].minEnemyNum && curGhosts.Count <= mapRounds.roundsInfo[roundNum].maxEnemyNum)
                {
                    GhostSpawn(mapRounds.roundsInfo[roundNum].autoSpawn);
                }
                else if (curGhosts.Count < mapRounds.roundsInfo[roundNum].minEnemyNum)
                {
                    GhostSpawn(mapRounds.roundsInfo[roundNum].autoSpawn + (mapRounds.roundsInfo[roundNum].minEnemyNum - curGhosts.Count));
                }
            }
            else 
            {
                enemySpawnCounter = 0;
            }
        }
        else 
        {
            enemySpawnCounter -= Time.deltaTime;
        }

        waveText.text = (roundNum + 1).ToString();
    }
    void GhostSpawn(int wave)
    {
        List<GhostManager> tempGhosts = new List<GhostManager>();
        //改怪物生成数量的地方
        for (int i = 0; i < wave; i++)
        {
            GhostManager spawnedGhost = Instantiate(ghost, transform);
            tempGhosts.Add(spawnedGhost);
        }

        float radiansOfSeparation = (Mathf.PI * 2) / tempGhosts.Count;
        for (int i = 0; i < tempGhosts.Count; i++)
        {
            float randomRadian = Random.Range(-1, 1) * (Mathf.PI / 12);

            float x = Mathf.Sin(radiansOfSeparation * i + randomRadian) * (radius + Random.Range(-0.75f, 0.75f));
            if (x == 0) x = 0.01f;
            float y = Mathf.Cos(radiansOfSeparation * i + randomRadian) * (radius + Random.Range(-0.75f, 0.75f));

            tempGhosts[i].GetComponent<Transform>().position = new Vector3(x, y, 0);
        }
    }
}
