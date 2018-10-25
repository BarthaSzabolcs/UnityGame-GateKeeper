using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameManagerInterface", fileName = "GameManagerInterface")]
public class GameManagerInterface : ScriptableObject
{
    public void ChangeScene(int index)
    {
        GameManager.Instance.ChangeScene(index);
    }
    public void ReloadScene()
    {
        GameManager.Instance.ReloadScene();
    }

    public void PauseGame()
    {
        GameManager.Instance.PauseGame();
    }
    public void ContinueGame()
    {
        GameManager.Instance.ContinueGame();
    }
}
