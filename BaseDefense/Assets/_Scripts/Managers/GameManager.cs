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
    #endregion
    #region Fields
    private GameObject player;
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
            }
            return player;
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
    #endregion
}
