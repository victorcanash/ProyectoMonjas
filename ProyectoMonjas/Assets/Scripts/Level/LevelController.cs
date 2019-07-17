using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public Player player = null;
    public HUD_Level HUD_level = null;
    public GameCamera gameCamera = null;

    [SerializeField] private Enemy enemy = null;
    public List<Enemy> current_enemies;
    private GameObject enemy_parent = null, enemy_HUD_parent;

    [Header("INFO")]
    [SerializeField] private int current_round = 0;
    [SerializeField] private int kills = 0;

    private void Start()
    {
        GameManager.Instance.CurrentGameState = GameManager.GameState.STARTGAME;
        GameManager.Instance.LevelController = this;

        //Player 
        if (player == null)
        {
            GameObject playerObj = null;
            playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.GetComponent<Player>();
                player.Init();
            }
            else Debug.LogError("Can't find Player");
        }
        else
        {
            player.Init();
        }

        //GameCamera
        if (gameCamera == null)
        {
            GameObject gameCameraObj = null;
            gameCameraObj = GameObject.FindWithTag("MainCamera");
            if (gameCameraObj != null)
            {
                gameCamera = gameCameraObj.GetComponent<GameCamera>();
                gameCamera.InitLevel();
            }
            else Debug.LogError("Can't find GameCamera");
        }
        else
        {
            gameCamera.InitLevel();
        }

        //HUDLevel
        if (HUD_level == null)
        {
            GameObject HUD_levelObj = null;
            HUD_levelObj = GameObject.FindWithTag("HUD_Level");
            if (HUD_levelObj != null)
            {
                HUD_level = HUD_levelObj.GetComponent<HUD_Level>();
                HUD_level.Init();
            }
            else Debug.LogError("Can't find HUD_Level");
        }
        else
        {
            HUD_level.Init();
        }

        StartLevel();
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.STARTGAME)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameManager.Instance.CurrentGameState = GameManager.GameState.INGAME;
                Invoke("FirstRound", 1f);
            }
        }
    }

    private void StartLevel()
    {
        NewRound();
    }

    private void NewRound()
    {
        current_round++;
        HUD_level.UpdateTextRound(current_round, true);
    }

    private void FirstRound()
    {
        player.active = true;
        StartRound();
    }

    private void StartRound()
    {
        AddEnemy();
        HUD_level.DisappearTextRound();
    }

    private void AddEnemy()
    {
        if (enemy_parent == null)
        {
            enemy_parent = new GameObject("Enemies");
            enemy_HUD_parent = new GameObject("EnemiesCanvas");
            Canvas canvas_enemy_HUD_parent = enemy_HUD_parent.AddComponent<Canvas>();
            canvas_enemy_HUD_parent.renderMode = RenderMode.WorldSpace;
            enemy_HUD_parent.AddComponent<CanvasScaler>();
        }
        enemy = Instantiate(enemy.gameObject, new Vector3(3, 0, 0), new Quaternion(0, 0, 0, 0), enemy_parent.transform).GetComponent<Enemy>();
        enemy.enemy_HUD_parent = enemy_HUD_parent.transform;
        current_enemies.Add(enemy);
        enemy.Init();
        enemy.name += current_enemies.IndexOf(enemy).ToString();
    }

    public void DeleteEnemy(Enemy enemyDead)
    {
        current_enemies.Remove(enemyDead);
        kills++;
        HUD_level.UpdateTextKills(kills);
    }

    public void GameOver()
    {

    }
}
