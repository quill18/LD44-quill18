using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldManager : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnEnable()
    {
        Instance = this;
        ScreenBounds = new Bounds(Vector3.zero, new Vector3(
              Camera.main.orthographicSize * Screen.width / Screen.height * 2,
              Camera.main.orthographicSize * 2,
              0
            ));
    }

    void Start()
    {
        playerUnit = GameObject.FindObjectOfType<PlayerUnit>();

        GameObject.FindObjectOfType<LevelSetup>().InitLevels();

        levelParent = this.transform.Find("Levels");

        // Activate the first level
        //levelParent.GetChild(0).gameObject.SetActive(true);
        StartLevel();

        /*for (int i = 1; i < levelParent.childCount; i++)
        {
            levelParent.GetChild(i).gameObject.SetActive(false);
        }*/

    }

    public GameObject ScrapPrefab;
    public Sprite[] ScrapeSprites;
    float baseScrapChance = 0.10f;
    public float BonusScrapChance = 0;

    public GameObject[] ExplosionsSmall;
    public GameObject[] Explosions;
    public GameObject[] ExplosionsBig;

    Transform levelParent;
    int currentLevel = 0;

    public Bounds ScreenBounds;

    static public WorldManager Instance;

    public bool IsPaused = false;

    PlayerUnit playerUnit;
    public GameObject TeleportFX;

    float bulletTimeLeft = 0;

    public GameObject UpgradeWindow;

    public GameObject TutorialWindow_Scrap;
    public GameObject TutorialWindow_Overheat;
    public GameObject TutorialWindow_Bombs;

    public GameObject EndScreen;
    public GameObject EndScreen_Levels;
    public GameObject EndScreen_Enemies;
    public GameObject EndScreen_Scrap;

    public int EnemiesSpawned = 0;
    public int EnemiesKilled = 0;
    public int ScrapCollected = 0;

    public GameObject MenuWindow;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OpenMenu();
        }

        if (IsPaused)
            return;

        if (bulletTimeLeft > 0)
        {
            bulletTimeLeft -= Time.deltaTime;
            if(bulletTimeLeft <= 0)
            {
                Camera.main.GetComponent<BulletTimeFX>().enabled = false;
            }
        }

        if(playerUnit == null)
        {
            // player is DED
            GameOver();
        }
    }

    public void StartBulletTime(float time)
    {
        bulletTimeLeft = time;

        Camera.main.GetComponent<BulletTimeFX>().enabled = true;


    }

    public bool IsBulletTime()
    {
        return bulletTimeLeft > 0;
    }

    public void FX_MegaBomb()
    {
        StartCoroutine(FX_MegaBomb_Co());
    }

    IEnumerator FX_MegaBomb_Co()
    {
        for (int i = 0; i < 10; i++)
        {
            Camera.main.transform.position = new Vector3
                (
                    Random.Range(-0.1f, 0.1f),
                    Random.Range(-0.1f, 0.1f),
                    -10
                );

            for (int j = 0; j < 10; j++)
            {
                Vector3 pos = new Vector3(
                        Random.Range(ScreenBounds.min.x, ScreenBounds.max.x),
                        Random.Range(ScreenBounds.min.y, ScreenBounds.max.y),
                        0
                    );

                Explosion(pos);
            }
            yield return new WaitForSeconds(.05f);
        }
        Camera.main.transform.position = new Vector3(0, 0, -10);
    }

    public float BulletDeltaTime()
    {
        return IsBulletTime() ? Time.deltaTime / 3f : Time.deltaTime;
    }

    public void RollScrap( Vector3 position )
    {
        float chance = baseScrapChance + BonusScrapChance;
        while (chance > 0)
        {
            if(Random.Range(0f, 1f) < chance)
            {
                GameObject go = SimplePool.Spawn(ScrapPrefab, position + (Vector3)Random.insideUnitCircle / 2f, Quaternion.identity);
                go.GetComponentInChildren<SpriteRenderer>().sprite = ScrapeSprites[Random.Range(0, ScrapeSprites.Length)];
            }
            chance -= 1;
        }
    }

    public void ExplosionSmall(Vector3 position)
    {
        //Instantiate(ExplosionsSmall[Random.Range(0, ExplosionsSmall.Length)], position, Quaternion.identity);
    }

    public void Explosion(Vector3 position)
    {
        Instantiate(Explosions[Random.Range(0, Explosions.Length)], position, Quaternion.identity);
    }

    public void ExplosionBig(Vector3 position)
    {
        Instantiate(ExplosionsBig[Random.Range(0, ExplosionsBig.Length)], position, Quaternion.identity);
    }



    public void EndLevel()
    {
        // Cleanup the level, show shop!
        levelParent.GetChild(currentLevel).gameObject.SetActive(false);

        Pause();

        if (currentLevel >= levelParent.childCount-1)
        {
            // TODO: Show victory screen? Restart NG+?
            Debug.Log("Game is over.");
            GameOver();
            return;
        }

        currentLevel++;

        UpgradeWindow.SetActive(true);
        SoundManager.Instance.PlayOpenShop();
    }

    public void StartLevel()
    {
        UpgradeWindow.SetActive(false);

        UnPause();

        playerUnit.gameObject.SetActive(false);

        StartCoroutine(StartLevelCO());
    }

    bool didBombTutorial = false;

    IEnumerator StartLevelCO()
    {

        GameObject teleGO = Instantiate(TeleportFX);
        playerUnit.transform.position = teleGO.transform.position;

        yield return new WaitForSeconds(3.0f);

        playerUnit.ResetForNewLevel();
        playerUnit.gameObject.SetActive(true);

        levelParent.GetChild(currentLevel).gameObject.SetActive(true);

        if(didBombTutorial == false && (playerUnit.MaxBullettime > 0 || playerUnit.MaxMegabombs > 0))
        {
            didBombTutorial = true;
            Tutorial_Bombs();
        }
    }

    public void Tutorial_Scrap()
    {
        Pause();
        TutorialWindow_Scrap.transform.parent.gameObject.SetActive(true);
        TutorialWindow_Scrap.SetActive(true);
    }

    public void Tutorial_Overheat()
    {
        Pause();
        TutorialWindow_Overheat.transform.parent.gameObject.SetActive(true);
        TutorialWindow_Overheat.SetActive(true);
    }

    public void Tutorial_Bombs()
    {
        Pause();
        TutorialWindow_Bombs.transform.parent.gameObject.SetActive(true);
        TutorialWindow_Bombs.SetActive(true);
    }

    public void Pause()
    {
        IsPaused = true;
    }

    public void UnPause()
    {
        IsPaused = false;
    }

    public void GameOver()
    {
        Pause();
        EndScreen.SetActive(true);

        string s = "game over";

        if (playerUnit != null && playerUnit.GetComponent<Unit>().Health >= 0)
        {
            s = "victory!";
            SoundManager.Instance.PlayVictory();
        }
        else
        {
            SoundManager.Instance.PlayGameOver();
        }

        EndScreen.transform.Find("title").GetComponent<TextMeshProUGUI>().text = s;

        EndScreen_Levels.GetComponent<TextMeshProUGUI>().text  = "you reached level "+(currentLevel+1)+" of " + levelParent.childCount;
        EndScreen_Enemies.GetComponent<TextMeshProUGUI>().text = "you killed "+ EnemiesKilled + " enemies ("+((EnemiesKilled*100)/EnemiesSpawned) +"%)";
        EndScreen_Scrap.GetComponent<TextMeshProUGUI>().text   = "you collected "+(ScrapCollected)+" scrap";
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void OpenMenu()
    {
        Pause();
        MenuWindow.SetActive(true);
    }

    public void CloseMenu()
    {
        UnPause();
        MenuWindow.SetActive(false);

    }
}
