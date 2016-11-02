using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; protected set; }

    [SerializeField]
    private Turret[] turrets;
    public Turret[] Turrets
    {
        get { return turrets; }
    }

    private int activeTurretIndex;

    [SerializeField]
    private Transform enemyTarget;
    public Transform EnemyTarget
    {
        get { return enemyTarget; }
    }

    [SerializeField]
    private Text doubloonText;

    [SerializeField]
    private GameObject gameOver;
    [SerializeField]
    private GameObject healthBar;

    private float timeScale;

    [SerializeField]
    private uint doubloons;
    public uint Doubloons
    {
        get { return doubloons; }
        set
        {
            doubloons = value;
            doubloonText.text = "Doubloons: " + doubloons;
        }
    }

    [SerializeField]
    private uint doubloonMin = 1;
    public uint DoubloonMin
    {
        get { return doubloonMin; }
    }

    [SerializeField]
    private uint doubloonMax = 6;
    public uint DoubloonMax
    {
        get { return doubloonMax; }
    }

    [SerializeField]
    private GameObject castle;
    public Castle Castle
    {
        get { return castle.GetComponent<Castle>(); }
    }

    private void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("There should never be two game managers!");
        }
        Instance = this;

        for (int i = 0; i < turrets.Length; i++)
        {
            turrets[i].Active = false;          
        }
        turrets[0].Active = true;
        activeTurretIndex = 0;

        gameOver.SetActive(false);

        Time.timeScale = 1;

        doubloonText.text = "Doubloons: " + doubloons;
    }

    private void Update()
    {
        healthBar.GetComponent<Scrollbar>().size = Castle.Health / Castle.MaxHealth;
        healthBar.transform.GetChild(1).GetComponent<Text>().text = Mathf.RoundToInt((Castle.Health / Castle.MaxHealth) * 100) + "%";

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameStateManager.Instance.Paused)
            {
                GameStateManager.Instance.Paused = false;
            }
            else if (!GameStateManager.Instance.Paused)
            {               
                GameStateManager.Instance.Paused = true;
            }
        }

        if(GameStateManager.Instance.Paused)
        {
            if (Time.timeScale != 0)
            {
                timeScale = Time.timeScale;
                Time.timeScale = 0;
            }
        }
        else if (!GameStateManager.Instance.Paused)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                turrets[activeTurretIndex].Active = false;

                activeTurretIndex++;
                activeTurretIndex = activeTurretIndex % turrets.Length;

                turrets[activeTurretIndex].Active = true;
            }

            if (Time.timeScale == 0)
            {
                Time.timeScale = timeScale;
            }
        }

        if(Castle.Health <= 0)
        {
            gameOver.SetActive(true);
        }
    }

    public void SpitName()
    {
        GameObject.Find("Name Spit").GetComponent<Button>().transform.GetChild(0).GetComponent<Text>().text = Parser.GetName();
    }

    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Unpause()
    {
        GameStateManager.Instance.Paused = false;
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
