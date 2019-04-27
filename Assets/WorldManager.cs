using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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



        levelParent = GetComponentInChildren<Level>().transform.parent;
        for (int i = 1; i < levelParent.childCount; i++)
        {
            levelParent.GetChild(i).gameObject.SetActive(false);
        }

        playerUnit = GameObject.FindObjectOfType<PlayerUnit>();
    }

    public GameObject ScrapPrefab;
    public Sprite[] ScrapeSprites;
    float baseScrapChance = 0.25f;
    public float BonusScrapChance = 0;

    Transform levelParent;
    int currentLevel = 0;

    public Bounds ScreenBounds;

    static public WorldManager Instance;

    public bool IsPaused = false;

    PlayerUnit playerUnit;

    float bulletTimeLeft = 0;

    // Update is called once per frame
    void Update()
    {
        bulletTimeLeft -= Time.deltaTime;
    }

    public void StartBulletTime(float time)
    {
        bulletTimeLeft = time;
    }

    public bool IsBulletTime()
    {
        return bulletTimeLeft > 0;
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

    public void EndLevel()
    {
        // Cleanup the level, show shop!
        levelParent.GetChild(currentLevel).gameObject.SetActive(false);
        IsPaused = true;
    }

    public void StartLevel()
    {
        currentLevel++;

        if(currentLevel >= levelParent.childCount)
        {
            // TODO: Show victory screen? Restart NG+?
            Debug.Log("Game is over.");
            return;
        }

        levelParent.GetChild(currentLevel).gameObject.SetActive(true);
    }
}
