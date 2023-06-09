using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageScene : MonoBehaviour
{
    public void LoadScene(int levelIndex)
    {
        LoadSceneAsync.instance.LoadScene(levelIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
