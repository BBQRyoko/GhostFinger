using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using DG.Tweening;
using TMPro;


public class PlayerManager : MonoBehaviour
{
    GameManager gameManager;
    NavMeshObstacle finger;
    WaveManager waveManager;
    [SerializeField] float maxHealth;
    public float curHealth;
    public float curEnergy;
    [SerializeField] Image healthBar;
    [SerializeField] Image energyBar;
    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject abilityButton;
    [SerializeField] GameObject abilityPrefab;
    public bool abilityUsing;

    [Header("PlayerStats")]
    [SerializeField] float attackDamage = 1;
    [SerializeField] int bulletNum = 1;
    [SerializeField] float attackSpeed = 1;
    [SerializeField] int bulletPenNum = 0;

    [Header("EXP")]
    [SerializeField] int curLevel;
    [SerializeField] float curExp;
    [SerializeField] float requiredExp;
    [SerializeField] GameObject skillSelectScreen;

    [Header("Finger")]
    [SerializeField] Vector2 mousePos;
    [SerializeField] GameObject fingerObject;
    [SerializeField] Vector2 fingerDirection;

    [Header("Upgrades")]
    [SerializeField] PlayerUpgradeListData listData;
    public List<PlayerUpgradeData> passiveUpgradeList = new List<PlayerUpgradeData>();
    [SerializeField] List<int> passiveUpgradeProgress = new List<int>();
    [SerializeField] GameObject[] playerPassiveShows = new GameObject[4];

    [Header("Turret")]
    [SerializeField] PlayerUpgradeData starterTurret;
    public GameObject[] tempTurretsSlots;
    public List<PlayerUpgradeData> turretsUpgradeList = new List<PlayerUpgradeData>();
    [SerializeField] GameObject[] turretsShows = new GameObject[4];
    private void Awake()
    {
        curHealth = maxHealth;
        curEnergy = 0;
        curLevel = 1;
        waveManager = FindObjectOfType<WaveManager>();
        finger = fingerObject.GetComponent<NavMeshObstacle>();
        gameManager = GetComponentInParent<GameManager>();
        Time.timeScale = 0;
        startScreen.SetActive(true);
        passiveUpgradeList.Clear();
        passiveUpgradeProgress.Clear();
        turretsUpgradeList.Clear();
        turretsUpgradeList.Add(starterTurret);
    }
    void Update()
    {
        healthBar.fillAmount = curHealth / maxHealth;
        energyBar.fillAmount = curEnergy / 100;
        if (curHealth <= 0)
        {
            Time.timeScale = 0;
            restartButton.SetActive(true);
        }
        else
        {
            restartButton.SetActive(false);
        }
        if (curEnergy < 100)
        {
            abilityButton.SetActive(false);
        }
        else 
        {
            curEnergy = 100;
            abilityButton.SetActive(true);
        }
        TempGameManager();
        FingerManagment();
        LevelManager();
        PlayerUpgradeStatus();
    }
    public void TempGameManager() 
    {
        if (Input.GetKeyDown("escape"))
        {
            if (Time.timeScale != 0)
            {
                Time.timeScale = 0;
                pauseScreen.SetActive(true);
            }
            else 
            {
                Time.timeScale = 1;
                pauseScreen.SetActive(false);
            }
        }
    }
    public void LevelManager() 
    {
        //改经验值的地方
        requiredExp = (curLevel + 1) * (curLevel + 1);
        if (curExp >= requiredExp) 
        {
            float overExp = curExp - requiredExp;
            curExp = overExp;
            curLevel += 1;
            gameManager.RandomUpgradePopUp();
            skillSelectScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void UpgradeSelection(UpgradeHolder holder) 
    {
        if (holder.curUpgrade.curType == UpgradeType.Passive)
        {
            if (!passiveUpgradeList.Contains(holder.curUpgrade))
            {
                passiveUpgradeList.Add(holder.curUpgrade);
                passiveUpgradeProgress.Add(1);
            }
            else
            {
                if (passiveUpgradeList.Count == 1)
                {
                    passiveUpgradeProgress[0] += 1;
                }
                else if (passiveUpgradeList.Count >= 2)
                {
                    for (int i = 0; i <= passiveUpgradeList.Count - 1; i++)
                    {
                        if (passiveUpgradeList[i] == holder.curUpgrade)
                        {
                            passiveUpgradeProgress[i] += 1;
                        }
                    }
                }
            }
        }
        else if (holder.curUpgrade.curType == UpgradeType.Turret) 
        {
            if (!turretsUpgradeList.Contains(holder.curUpgrade))
            {
                turretsUpgradeList.Add(holder.curUpgrade);
                tempTurretsSlots[turretsUpgradeList.Count - 1].SetActive(true);
                tempTurretsSlots[turretsUpgradeList.Count-1].GetComponent<TurretManager>().turretInfo = holder.curUpgrade.curTurretData;
                tempTurretsSlots[turretsUpgradeList.Count-1].GetComponent<TurretManager>().turretRank = 1;
                tempTurretsSlots[turretsUpgradeList.Count - 1].GetComponent<TurretManager>().TurretSetUp();
            }
            else
            {
                if (turretsUpgradeList.Count == 1)
                {
                    tempTurretsSlots[0].GetComponent<TurretManager>().TurretRankUp();
                }
                else if (turretsUpgradeList.Count >= 2)
                {
                    for (int i = 0; i <= turretsUpgradeList.Count - 1; i++)
                    {
                        if (turretsUpgradeList[i] == holder.curUpgrade)
                        {
                            tempTurretsSlots[i].GetComponent<TurretManager>().TurretRankUp();
                        }
                    }
                }
            }
        }
        skillSelectScreen.SetActive(false);
        Time.timeScale = 1;
    }
    public void PlayerUpgradeStatus() 
    {
        if (passiveUpgradeList.Count == 1)
        {
            playerPassiveShows[0].SetActive(true);
            playerPassiveShows[0].GetComponentInChildren<TextMeshProUGUI>().text = passiveUpgradeProgress[0].ToString();
        }
        else if (passiveUpgradeList.Count >= 2)
        {
            for (int i = 0; i <= passiveUpgradeList.Count - 1; i++)
            {
                playerPassiveShows[i].SetActive(true);
                playerPassiveShows[i].GetComponentInChildren<TextMeshProUGUI>().text = passiveUpgradeProgress[i].ToString();
            }
        }
        else
        {
            foreach (GameObject upgradeIcon in playerPassiveShows)
            {
                upgradeIcon.SetActive(false);
            }
        }

        if (turretsUpgradeList.Count == 1)
        {
            turretsShows[0].SetActive(true);
            turretsShows[0].GetComponentInChildren<TextMeshProUGUI>().text = tempTurretsSlots[0].GetComponent<TurretManager>().turretRank.ToString();
        }
        else if (turretsUpgradeList.Count >= 2)
        {
            for (int i = 0; i <= turretsUpgradeList.Count - 1; i++)
            {
                turretsShows[i].SetActive(true);
                turretsShows[i].GetComponentInChildren<TextMeshProUGUI>().text = tempTurretsSlots[i].GetComponent<TurretManager>().turretRank.ToString();
            }
        }
        else
        {
            foreach (GameObject upgradeIcon in turretsShows)
            {
                upgradeIcon.SetActive(false);
            }
        }
    }
    public void FingerManagment() 
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        fingerObject.transform.position = mousePos;

        if (mousePos.x > 0 && mousePos.y > 0) //1 
        {
            if (Input.GetAxis("Mouse X") <= 0 && Input.GetAxis("Mouse Y") <= 0)
            {
                fingerObject.layer = 7;
            }
            else
            {
                fingerObject.layer = 8;
            }
        }
        else if (mousePos.x > 0 && mousePos.y < 0)
        {
            if (Input.GetAxis("Mouse X") <= 0 && Input.GetAxis("Mouse Y") >= 0)
            {
                fingerObject.layer = 7;
            }
            else
            {
                fingerObject.layer = 8;
            }
        }
        else if (mousePos.x < 0 && mousePos.y > 0)
        {
            if (Input.GetAxis("Mouse X") >= 0 && Input.GetAxis("Mouse Y") <= 0)
            {
                fingerObject.layer = 7;
            }
            else
            {
                fingerObject.layer = 8;
            }
        }
        else if (mousePos.x < 0 && mousePos.y < 0)
        {
            if (Input.GetAxis("Mouse X") >= 0 && Input.GetAxis("Mouse Y") >= 0)
            {
                fingerObject.layer = 7;
            }
            else
            {
                fingerObject.layer = 8;
            }
        }
        else if (mousePos.x == 0 && mousePos.y > 0) 
        {
            if (Input.GetAxis("Mouse Y") <= 0)
            {
                fingerObject.layer = 7;
            }
            else
            {
                fingerObject.layer = 8;
            }
        }
        else if (mousePos.x == 0 && mousePos.y < 0)
        {
            if (Input.GetAxis("Mouse Y") >= 0)
            {
                fingerObject.layer = 7;
            }
            else
            {
                fingerObject.layer = 8;
            }
        }
        else if (mousePos.x > 0 && mousePos.y == 0)
        {
            if (Input.GetAxis("Mouse X") <= 0)
            {
                fingerObject.layer = 7;
            }
            else
            {
                fingerObject.layer = 8;
            }
        }
        else if (mousePos.x < 0 && mousePos.y == 0)
        {
            if (Input.GetAxis("Mouse X") >= 0)
            {
                fingerObject.layer = 7;
            }
            else
            {
                fingerObject.layer = 8;
            }
        }

        if (Input.GetMouseButton(0))
        {
            finger.enabled = true;
            fingerObject.GetComponent<Collider2D>().enabled = true;
        }
        else 
        {
            finger.enabled = false;
            fingerObject.GetComponent<Collider2D>().enabled = false;
            fingerDirection = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
            fingerObject.transform.right = fingerDirection;
        }
    }
    public void RecycleExp() 
    {
        var exps = GameObject.FindGameObjectsWithTag("Exp");
        foreach (GameObject exp in exps) 
        {
            gameManager.GetComponent<ExpPool>().expPrefabPool.Release(exp);
        }
    }
    public void GameRestart() 
    {
        Time.timeScale = 1;
        waveManager.roundNum = 0;
        waveManager.gameTimer = 0;
        curHealth = maxHealth;
        curEnergy = 0;
        startScreen.SetActive(false);
        passiveUpgradeList.Clear();
        passiveUpgradeProgress.Clear();
        turretsUpgradeList.Clear();
        turretsUpgradeList.Add(starterTurret);
        tempTurretsSlots[1].SetActive(false);
        tempTurretsSlots[2].SetActive(false);
        tempTurretsSlots[3].SetActive(false);
        RecycleExp();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Exp")
        {
            gameManager.GetComponent<ExpPool>().expPrefabPool.Release(collision.gameObject);
            curExp += 1;
        }
    }
}
