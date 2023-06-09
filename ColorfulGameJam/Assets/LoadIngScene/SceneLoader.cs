using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Will - Scene loader that has public methods for loading the next scene in the build index, loading the first scene in the build index, quiting the application, and reloading the current active scene in the build index
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public void LoadNextScene()
    {
        try
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        catch(Exception e)
        {
            Debug.LogError("WARNING " + e.ToString() + " Check build settings no scene to load found");
        }
        
    }
    public void LoadSceneByName(string name) // call this method  from scripting when you want to get a scene by its name
    {
        try
        {
            SceneManager.LoadScene(name);
        }
        catch(Exception e)
        {
            Debug.LogError("WARNING " + e.ToString() + " Check that the scene you are trying to load is named correctly");
        }
        
    }
    public void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReloadGameScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
