using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Events
    public delegate void SceneChange();
    /// <summary>
    /// Triggered just before the scene change.
    /// </summary>
    public event SceneChange OnSceneChange;

    public delegate void BuildMode(bool state);
    public event BuildMode OnBuildModeStateChange;
    #endregion
    #region Fields
    private GameObject player;
    private bool inBuildMode;
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
            }
            inBuildMode = value;
        }
    }
    public int Money { get; set; }
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
        Money = 1000;
        player = GameObject.Find("Player");
        var portal = GameObject.Find("friendlyPortal");
        if(portal != null)
        {
            portal.GetComponent<Health>().OnDeath += HandlePlayerDeath;
        }
        if (player != null)
        {
            player.GetComponent<Health>().OnDeath += HandlePlayerDeath;
        }
        UserInterface.Instance.InitializeLevelUI();
        ObjectPool_Manager.Instance.InitializeLevel();
    }

    public void PauseGame()
    {
        SetTimeScale(0);
    }
    public void ContinueGame()
    {
        SetTimeScale(1);
    }
    private void SetTimeScale(float timeScale = Constants.timeSlowScale)
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }


    #region Shop Functions
    public void BuyTrap(TrapData newData)
    {
        Money += UserInterface.Instance.CurrentTrap.Data.price;
        Money -= newData.price;
        UserInterface.Instance.CurrentTrap.Data = newData;
        UserInterface.Instance.InitializeTrapMenu();
    }
    public void SellTrap()
    {
        Money += UserInterface.Instance.CurrentTrap.Data.price;
        UserInterface.Instance.CurrentTrap.Data = null;
        UserInterface.Instance.InitializeTrapMenu();
    }

    public void BuyAmmo(WeaponData wep,int cost)
    {
        if (Money - cost >= 0)
        {
            if (UserInterface.Instance.IncreaseExtraAmmo(wep))
            {
                Money -= cost;
            }
        }
    }

    public int BuyWeapon(WeaponData wep,int cost)
    {
        if (Money - cost >= 0)
        {
            Money -= cost;
            return UserInterface.Instance.BuyWeapon(wep);
        }
        return -1;
    }
    
    #endregion
    private void HandlePlayerDeath(GameObject sender)
    {
        ReloadScene();
    }
    #endregion
}
