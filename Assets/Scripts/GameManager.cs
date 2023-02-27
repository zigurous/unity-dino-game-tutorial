using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float gameSpeed { get; private set; }

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiscoreText;
    public TextMeshProUGUI gameOverText;
    public Button retryButton;
    private int level = 0;
    private Player player;
    private Spawner[] spawners;
    public AudioListener audioListener;
    public AudioClip duckClip;
    public AudioClip maazClip;
    public AudioClip nightClip;
    public AudioClip boomClip;
    private Vector3 startPosition = new Vector3(-6, 0, 0);

    private float score;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        spawners = FindObjectsOfType<Spawner>();
        maazClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sound/maaz.mp3");
        nightClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sound/night.mp3");
        boomClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sound/boom.mp3");
        audioListener = Camera.main.GetComponent<AudioListener>();

        NewGame();
    }

    public void NewGame()
    {
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();

        foreach (var obstacle in obstacles) {
            Destroy(obstacle.gameObject);
        }

        score = 0f;
        gameSpeed = initialGameSpeed;
        enabled = true;

        ConfigLevel0();

        player.gameObject.SetActive(true);
        foreach (Spawner spawner in spawners)
        {
            spawner.gameObject.SetActive(true);
        }
            
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        UpdateHiscore();
    }

    public void GameOver()
    {
        audioListener.GetComponent<AudioSource>().clip = boomClip;
        // Play the new audio clip
        audioListener.GetComponent<AudioSource>().Play();
        level = 0;
        gameSpeed = 0f;
        enabled = false;

        player.gameObject.SetActive(false);
        player.gameObject.transform.position = startPosition;
        foreach (Spawner spawner in spawners)
        {
            spawner.gameObject.SetActive(false);
        }
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
        
        UpdateHiscore();
    }

    private void Update()
    {
        

        gameSpeed += gameSpeedIncrease * Time.deltaTime;
        score += gameSpeed * Time.deltaTime * 2f;
        scoreText.text = Mathf.FloorToInt(score).ToString("D5");
        
        if (score >= 100)
        {
            if (level < 1)
            {
                // Set the new audio clip
                audioListener.GetComponent<AudioSource>().clip = maazClip;
                // Play the new audio clip
                audioListener.GetComponent<AudioSource>().Play();
                level = 1;
            }
            ConfigLevel1();
            ConfigLevel5();
        }
        if (score >= 200)
        {
            if (level < 2)
            {
                // Set the new audio clip
                audioListener.GetComponent<AudioSource>().clip = nightClip;
                // Play the new audio clip
                audioListener.GetComponent<AudioSource>().Play();
                level = 2;
            }
            ConfigLevel2();

        }


    }

    private void ConfigLevel1()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            for (int j = 0; j < spawners[i].objects.Length; j++)
            {
                if (spawners[i].objects[j].prefab.name == "Bird")
                {
                    spawners[i].objects[j].spawnChance = 0.2f;
                }
            }
        }
    }

    private void ConfigLevel2()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            for (int j = 0; j < spawners[i].objects.Length; j++)
            {
                if (spawners[i].objects[j].prefab.name == "Meteor")
                {
                    spawners[i].objects[j].spawnChance = 0.16f;
                }
                if (spawners[i].objects[j].prefab.name == "Meteorite_01")
                {
                    spawners[i].objects[j].spawnChance = 0.2f;
                }
            }
        }
    }

    private void ConfigLevel5()
    {
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        Camera.main.transform.position = new Vector3(0, 2, 10);
        var ground = GameObject.Find("Ground");
        ground.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
    }

    private void ConfigLevel0()
    {
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        Camera.main.transform.position = new Vector3(0, 2, -10);
        var ground = GameObject.Find("Ground");
        ground.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        for (int i = 0; i < spawners.Length; i++)
        {
            for (int j = 0; j < spawners[i].objects.Length; j++)
            {
                if (spawners[i].objects[j].prefab.name == "Bird" ||
                    spawners[i].objects[j].prefab.name == "Meteor" ||
                    spawners[i].objects[j].prefab.name == "Meteorite_01")
                {
                    spawners[i].objects[j].spawnChance = 0f;
                }

            }
        }
    }

    private void UpdateHiscore()
    {
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if (score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }

        hiscoreText.text = Mathf.FloorToInt(hiscore).ToString("D5");
    }

}
