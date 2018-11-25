using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Events

    public delegate void SceneChange();
    public event SceneChange OnSceneChange;

    public delegate void BuildMode(bool state);
    public event BuildMode OnBuildModeStateChange;

    public delegate void MoneyChanged(int money);
    public event MoneyChanged OnMoneyChaned;

    #endregion
    #region Fields

    private GameObject player;
    private bool inBuildMode;
    private int money = 0;
    
    #endregion
    #region Properties

    public static GameManager Instance { get; private set; }
    public GameObject Player
    {
        get
        {
            if(player == null)
            {
                player = GameObject.Find("Player");
                if(player != null)
                {
                    player.GetComponent<Health>().OnDeath += HandlePlayerDeath;
                }
            }
            return player;
        }
    }
    public bool InBuildMode
    {
        get
        {
            return inBuildMode;
        }
        set
        {
            if (value != inBuildMode)
            {
                OnBuildModeStateChange?.Invoke(value);
                SetTimeScale( value ? 1 / 20f : 1);
            }

            inBuildMode = value;
        }
    }
    public int Money
    {
        get
        {
            return money;
        }
        set
        {
            money = value;
            OnMoneyChaned?.Invoke(value);
        }
    }

    #endregion
    #region UnityFunctions

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

    }
    private void Start()
    {

        SceneManager.sceneLoaded += StartLevel;

    }

    #endregion
    #region CusomFunctions

    public void ChangeScene(int index)
    {
        OnSceneChange?.Invoke();
        SceneManager.LoadScene(index);
    }
    public void ReloadScene()
    {
        ChangeScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartLevel(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.Find("Player");

        var portal = GameObject.Find("friendlyPortal");

        if (portal != null)
        {
            portal.GetComponent<Health>().OnDeath += HandlePlayerDeath;
        }

        if (player != null)
        {
            player.GetComponent<Health>().OnDeath += HandlePlayerDeath;
        }

        UserInterface.Instance.InitializeLevelUI();
        ObjectPool.Instance.InitializeLevel();

        Money = 50000;
    }

    public void SlowTime(float slowRate = 1 / 20f)
    {
        SetTimeScale(slowRate);
    }
    public void ContinueGame()
    {
        SetTimeScale(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    private void SetTimeScale(float timeScale = Constants.timeSlowScale)
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
    private void HandlePlayerDeath(GameObject sender)
    {
        ReloadScene();
    }

    #endregion
}
