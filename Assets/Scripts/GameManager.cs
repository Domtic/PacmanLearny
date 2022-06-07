using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Koko.Utilities;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    [SerializeField]
    private PlayerStats playerStats;
    [SerializeField]
    private GameState currentGameSate;
    [Header("Game configuration")]
    [SerializeField]
    private int dotsToEat;
    [SerializeField]
    private int maxEnemiesOnScreen;
    [SerializeField]
    private int maxPowerUps;
    [SerializeField]
    private bool randomizeEnemies;
    [SerializeField]
    private int minimalDistanceToSpawn;
    [SerializeField]
    private int maxDistanceToSpawn;
    [Header("GamePrefabs")]
    [SerializeField]
    private GameObject dotPrefab;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject enemiesPrefab;
    [SerializeField]
    private List<GameObject> powerUpsPrefab;
    
    [Header("PoolObjects")]
    [SerializeField]
    private Transform dotsPoolParent;
    [SerializeField]
    private Transform enemiesPoolParent;
    [SerializeField]
    private Transform powerUpsPoolParent;
    [SerializeField]
    private Transform popUpPoolParent;

    [Header("UI Components")]
    [SerializeField]
    private GameObject eatScoreTextPrefab;
    [SerializeField]
    private GameObject MenuUI, PauseUI, InGameUI, GameOverUI,LoadingUI;
    [SerializeField]
    private Button StartGameButton, ReplayButton;

    [SerializeField]
    private TMP_Text currentScoreTxt;
    [SerializeField]
    private TMP_Text dotsLeftTxt;

    [SerializeField]
    private TMP_Text mainMenuHighScoreTxt, totalEnemiesEatedTXT,totalPowerUpsEatedTXT;
    [SerializeField]
    private TMP_Text gameOverHighScoreTxt;
    [SerializeField]
    private TMP_Text gameOverScore;
    [SerializeField]
    private TMP_Text gameOverTitle;

    private uint gameSessionScore = 0;
    private uint dotsEaten = 0;
    private uint enemiesEaten = 0;
    private uint powerUpEaten = 0;

    private List<Transform> dotsPool = new List<Transform>();
    private List<Transform> enemiesPool = new List<Transform>();
    private List<Transform> powerUpsPool = new List<Transform>();
    private List<RectTransform> popUpTextPool = new List<RectTransform>();

    public event EventHandler OnStartGame;
    public event EventHandler OnFinishGame;
    public event EventHandler OnPause;
    public event EventHandler OnUnPause;
    public event EventHandler OnMenu;

    [SerializeField]
    private GameObject playerReference;
    #region getters/setters

    public GameState CurrentGameState { get => currentGameSate; set => currentGameSate = value; }
    public uint DotsEaten { get => dotsEaten; set => dotsEaten += value; }

    public uint GameSessionScore { get => gameSessionScore; }
    public uint EnemiesEaten { get => enemiesEaten; }
    public uint PowerUpEaten { get => powerUpEaten; }

    public bool RandomizeEnemies { get => randomizeEnemies; }
    #endregion

    #region GameEvents

 
    public void GameFinish()
    {
        InGameUI.SetActive(false);
        GameOverUI.SetActive(true);
        if(dotsEaten == dotsToEat)
            gameOverTitle.text = "YOU WIN";
        else
            gameOverTitle.text = "YOU LOSE";

        gameOverHighScoreTxt.text = "Highscore: " +playerStats.HighScore.ToString();
        gameOverScore.text = "Score: " + gameSessionScore.ToString();
        Camera.main.transform.parent = null;
        currentGameSate = GameState.GameOver;
        OnFinishGame?.Invoke(this, EventArgs.Empty);
        IncreasePlayerStats();
    }
    public void GamePaused()
    {
        if(currentGameSate == GameState.Paused)
        {
            PauseUI.SetActive(false);
            InGameUI.SetActive(true);
            currentGameSate = GameState.Game;
            OnUnPause?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            PauseUI.SetActive(true);
            InGameUI.SetActive(false);
            currentGameSate = GameState.Paused;
            OnPause?.Invoke(this, EventArgs.Empty);
        }
        
    }

    public void GameStart()
    {
        Camera.main.transform.position = new Vector3(0,0,-15);
        InGameUI.SetActive(true);
        LoadingUI.SetActive(false);
        currentGameSate = GameState.Game;
        currentScoreTxt.text = "Score: 0";
        dotsLeftTxt.text = "Dots left: "+dotsToEat.ToString();

        OnStartGame?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region GAME FUNCTIONS
    public void IncreasePlayerStats()
    {   
       
        playerStats.EnemiesEaten += enemiesEaten;
        playerStats.DotsEaten += dotsEaten;
        playerStats.PowerUpsEaten += powerUpEaten;
        if(playerStats.HighScore < gameSessionScore)
        {
            playerStats.HighScore = gameSessionScore;
        }
    }
    //getting an object for text
    public RectTransform ReturnText()
    {
        RectTransform objectToReturn = null;
        for(int x=0;x< popUpTextPool.Count;x++)
        {
            
            if(!popUpTextPool[x].gameObject.activeSelf)
            {
                popUpTextPool[x].gameObject.SetActive(true);
                objectToReturn = popUpTextPool[x];
                break;
            }
        }

        return objectToReturn;
    }


    private void StartGame()
    {
        
        MenuUI.SetActive(false);
        GameOverUI.SetActive(false);
        InGameUI.SetActive(false);
        LoadingUI.SetActive(true);
        dotsEaten = 0;
        gameSessionScore = 0;
        //creating pool for dots
        if (dotsPool.Count == 0)
            KokoUtilities.CreatePoolForObject(dotPrefab, dotsPoolParent, dotsToEat, dotsPool);
        else if (dotsPool.Count < dotsToEat)
            KokoUtilities.CreatePoolForObject(dotPrefab, dotsPoolParent, dotsToEat - dotsPool.Count, dotsPool);

        //Create pool for UIText
        KokoUtilities.CreatePoolForObject(eatScoreTextPrefab, popUpPoolParent, 10, popUpTextPool);
        //creating pool for powerups
        if (powerUpsPool.Count == 0)
        {
            for (int x = 0; x < powerUpsPrefab.Count; x++)
            {
              
                KokoUtilities.CreatePoolForObject(powerUpsPrefab[x], powerUpsPoolParent, maxPowerUps, powerUpsPool);
            }

        }
        else if (powerUpsPool.Count < maxPowerUps)
        {
            for (int x = 0; x < powerUpsPrefab.Count; x++)
            {
                KokoUtilities.CreatePoolForObject(powerUpsPrefab[x], powerUpsPoolParent, maxPowerUps - powerUpsPool.Count, powerUpsPool);
            }
        }

        //create pool for enemies
        if (enemiesPool.Count == 0)
        {
            KokoUtilities.CreatePoolForObject(enemiesPrefab, enemiesPoolParent, maxEnemiesOnScreen, enemiesPool);
        }
        else if (enemiesPool.Count < maxEnemiesOnScreen)
        {
            KokoUtilities.CreatePoolForObject(enemiesPrefab, enemiesPoolParent, maxEnemiesOnScreen - enemiesPool.Count, enemiesPool);     
        }

        SpawnEntity(dotsToEat, dotsPool,false);
        SpawnEntity(maxEnemiesOnScreen, enemiesPool,false);
        SpawnEntity(maxPowerUps, powerUpsPool,true);

        SpawnPlayer();

        GameStart();
    }

    void SpawnPlayer()
    {
        playerReference = GameObject.FindWithTag("Player");
        if (playerReference != null)
        {
            playerReference.SetActive(true);
        }
        else
        {
            playerReference = GameObject.Instantiate(playerPrefab, null, true);
            Camera.main.transform.parent = playerReference.transform;
        }
    }

    //Check entities TO spawn
    void SpawnEntity(int amount, List<Transform> pool, bool spawnsDiferentObjects)
    {
        if (spawnsDiferentObjects)
            amount += amount;
        for (int x = 0; x < amount; x++)
        {
            if (!pool[x].gameObject.activeSelf)
            {
                pool[x].gameObject.SetActive(true);
                int posX = UnityEngine.Random.Range(minimalDistanceToSpawn, maxDistanceToSpawn);
                int posY = UnityEngine.Random.Range(minimalDistanceToSpawn, maxDistanceToSpawn);

                if(UnityEngine.Random.Range(0,100) > 50)
                {
                    posX = -posX;
                }
                if (UnityEngine.Random.Range(0, 100) > 50)
                {
                    posY = -posY;
                }

                pool[x].position = new Vector2(posX,posY);
              
            }
          
        }
    }


    public void GainPoints(uint amount, int type)
    {
        gameSessionScore += amount;
        currentScoreTxt.text = "Score: " + gameSessionScore;

        switch(type)
        {
            case 0:
                dotsEaten++;
                dotsLeftTxt.text = "Dots left: " + (dotsToEat - dotsEaten);
                if (dotsEaten == dotsToEat)
                    GameFinish();
                break;
            case 1:
                enemiesEaten++;
                break;
            case 2:
                powerUpEaten++;
                break;
        }
    }
    #endregion

    #region INIT Methods
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;        
            DontDestroyOnLoad(this.gameObject);
            InitGame();
        }

    }
    private void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.P))
            GamePaused();
    }
    //sets the entire game to its initial state
    void InitGame()
    {
       
        mainMenuHighScoreTxt.text = playerStats.HighScore.ToString();
        totalEnemiesEatedTXT.text = playerStats.EnemiesEaten.ToString();
        totalPowerUpsEatedTXT.text = playerStats.PowerUpsEaten.ToString();
        ReplayButton.onClick.AddListener(() => StartGame());
        StartGameButton.onClick.AddListener(() => StartGame());
       
        currentGameSate = GameState.Menu;
        OnMenu?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    #region POWERUPS

    //search and killa  certain amount of enemies
    public void KillEnemies(int amount)
    {
        int counter = 0;
        for(int x=0;x<enemiesPoolParent.childCount;x++)
        {
            if(enemiesPool[x].gameObject.activeSelf)
            {
                enemiesPool[x].gameObject.SetActive(false);
                counter++;
                if (counter == amount)
                    break;
            }
        }
    }

    //Kill all enemies
    public void KillAllEnemies()
    {
        for (int x = 0; x < enemiesPoolParent.childCount; x++)
        {
            if (enemiesPool[x].gameObject.activeSelf)
            {
                enemiesPool[x].gameObject.SetActive(false);
            }
        }
    }


    public void EnableEatEnemiesMode(float timeEnabled)
    {
        playerReference.GetComponent<Player>().CanEatEnemies = true;
        for (int x = 0; x < enemiesPoolParent.childCount; x++)
        {
            if (enemiesPool[x].gameObject.activeSelf)
            {
                enemiesPool[x].gameObject.GetComponent<Enemy>().CanMove = false;

            }
        }
        StartCoroutine(EatEnemiesTimer(timeEnabled));
    }

    IEnumerator EatEnemiesTimer(float time)
    {
        yield return new WaitForSeconds(time);
        for (int x = 0; x < enemiesPoolParent.childCount; x++)
        {
            if (enemiesPool[x].gameObject.activeSelf)
            {
                enemiesPool[x].gameObject.GetComponent<Enemy>().CanMove = true;

            }
        }
        playerReference.GetComponent<Player>().CanEatEnemies = false;
    }
    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(minimalDistanceToSpawn, minimalDistanceToSpawn, minimalDistanceToSpawn));
    }
}

public enum GameState
{
    Menu,
    Paused,
    Game,
    GameOver
}
