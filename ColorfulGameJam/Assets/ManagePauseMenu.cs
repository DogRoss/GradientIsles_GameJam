using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagePauseMenu : MonoBehaviour
{
    public GameObject canvas;
    public PlayerUIController UIControl;
    public void LoadScene(int levelIndex)
    {
        LoadSceneAsync.instance.LoadScene(levelIndex);
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DisableMenu()
    {
        UIControl.OnEscape();
        canvas.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

}
