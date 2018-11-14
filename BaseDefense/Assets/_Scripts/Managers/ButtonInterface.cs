using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ButtonInterface", fileName = "ButtonInterface")]
public class ButtonInterface : ScriptableObject
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
    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }

    public void SellTrap()
    {

        GameManager.Instance.SellTrap();

    }
    public void BuyTrap(TrapData newData)
    { 

        GameManager.Instance.BuyTrap(newData);

    }
}
