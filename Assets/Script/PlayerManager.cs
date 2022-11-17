using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using DG.Tweening;

public class PlayerManager : MonoBehaviour
{
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

    [Header("Finger")]
    [SerializeField] Vector2 mousePos;
    [SerializeField] GameObject fingerObject;
    [SerializeField] Vector2 fingerDirection;

    [Header("Turret")]
    [SerializeField] float detectRange;
    [SerializeField] GhostManager curTarget;
    [SerializeField] LayerMask ghostLayer;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float fireRate;
    float fireCountdown;

    [Header("EXP")]
    [SerializeField] int curLevel;
    [SerializeField] float curExp;
    [SerializeField] float requiredExp;
    [SerializeField] GameObject skillSelectScreen;

    private void Awake()
    {
        curHealth = maxHealth;
        curEnergy = 0;
        curLevel = 1;
        requiredExp = 5 + curLevel;
        waveManager = FindObjectOfType<WaveManager>();
        finger = fingerObject.GetComponent<NavMeshObstacle>();
        Time.timeScale = 0;
        startScreen.SetActive(true);
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
        TurretManagment();
        FingerManagment();
        LevelManager();
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
        requiredExp = 5 + curLevel;
        if (curExp >= requiredExp) 
        {
            float overExp = curExp - requiredExp;
            curExp = overExp;
            curLevel += 1;
            skillSelectScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void UpgradeSelection(int index) 
    {
        if (index == 0)
        {
            bulletNum += 1;
        }
        else if (index == 1)
        {
            attackDamage *= 1.5f;
        }
        else if (index == 2) 
        {
            attackSpeed *= 1.5f;
        }
        skillSelectScreen.SetActive(false);
        Time.timeScale = 1;
    }
    public void TurretManagment() 
    {
        if (!curTarget)
        {
            GhostDetect();
        }
        else 
        {
            if (curTarget.isDestroying) curTarget = null;
            if (fireCountdown <= 0)
            {
                TurretShoot();
                fireCountdown = 1 / (fireRate * attackSpeed);
            }
            fireCountdown -= Time.deltaTime;
        }
    }
    public void FingerManagment() 
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        fingerObject.transform.position = mousePos;
        fingerDirection = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        fingerObject.transform.right = fingerDirection;

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
        }
    }
    public void GhostDetect() 
    {
        //var ghosts = Physics2D.OverlapCircleAll(transform.position, detectRange, ghostLayer);
        //foreach (var ghost in ghosts) 
        //{
        //    ghostInRange.Add(ghost.gameObject);
        //}
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        float shortestDistance = Mathf.Infinity;
        foreach (GameObject ghost in ghosts) 
        {
            float distance = Vector2.Distance(transform.position, ghost.transform.position);
            if (distance < shortestDistance) 
            {
                shortestDistance = distance;
                curTarget = ghost.GetComponent<GhostManager>();
            }
        }

    }
    public void TurretShoot() 
    {
        if (curTarget) 
        {
            if (bulletNum <= 1)
            {
                bulletNum = 1;
                GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                bullet.GetComponent<BulletManager>().bulletDamage *= attackDamage;
                Vector2 dir = new Vector2(curTarget.transform.position.x - transform.position.x, curTarget.transform.position.y - transform.position.y);
                dir.Normalize();
                bullet.GetComponent<Rigidbody2D>().AddForce(dir * 200f);
                Destroy(bullet, 2f);
            }
            else 
            {
                float bulletAngle = 15f;
                int median = bulletNum / 2;
                for (int i = 0; i < bulletNum; i++) 
                {
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                    bullet.GetComponent<BulletManager>().bulletDamage *= attackDamage;
                    Vector2 dir = new Vector2(curTarget.transform.position.x - transform.position.x, curTarget.transform.position.y - transform.position.y);

                    if (bulletNum % 2 == 1)
                    {
                        bullet.GetComponent<BulletManager>().SetSpeed(Quaternion.AngleAxis(bulletAngle*(i- median),Vector3.forward) * dir);
                    }
                    else 
                    {
                        bullet.GetComponent<BulletManager>().SetSpeed(Quaternion.AngleAxis(bulletAngle * (i - median) + bulletAngle/2, Vector3.forward) * dir);
                    }
                    Destroy(bullet, 2f);
                }
            }
        }
    }
    public void TempAbilityButton() 
    {
        curEnergy = 0;
        GameObject abilityRing = Instantiate(abilityPrefab, null);
        abilityUsing = true;
        abilityRing.transform.DOScale(new Vector3(1.4f, 1.4f, 1.4f), 1.5f).SetEase(Ease.OutQuad).OnComplete(() => TempDestroy(abilityRing));
    }
    void TempDestroy(GameObject gameObject) 
    {
        abilityUsing = false;
        Destroy(gameObject);
    }
    public void GameRestart() 
    {
        Time.timeScale = 1;
        waveManager.waveNum = 1;
        curHealth = maxHealth;
        curEnergy = 0;
        startScreen.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Exp")
        {
            Destroy(collision.gameObject);
            curExp += 1;
        }
    }
}
