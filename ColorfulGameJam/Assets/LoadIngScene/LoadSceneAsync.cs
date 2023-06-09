using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// Loading Scene class that utilizes a loading scene with a UI loading bar to show loading progress to the player
/// </summary>
public class LoadSceneAsync : MonoBehaviour
{
    private static Image loadBar;
    public static LoadSceneAsync instance; // singelton shared across the entire game
    [SerializeField]
    private int loadingSceneBuildIndex = 1; // scene index set in the build settings of the game
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // preventing the object from being destroyed between scenes
            loadBar = GetComponentInChildren<Image>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void LoadScene(int i)
    {
        SceneManager.LoadSceneAsync(loadingSceneBuildIndex);
        StartCoroutine(LoadingBar(i));
    }

    IEnumerator LoadingBar(int i)
    {
        loadBar.enabled = true;

        AsyncOperation operation = SceneManager.LoadSceneAsync(i);
        while(!operation.isDone)
        {
            Debug.Log(operation.progress);
            loadBar.fillAmount = operation.progress / 0.9f;
            yield return null;
        }

        loadBar.enabled = false;
    }

    /// <summary>
    /// If you want to have the scene loading finished by pressing a button
    /// </summary>
    bool SceneLoaded;
    int sceneToLoad;

    public void ClickableLoadingScene(int i)
    {
        StartCoroutine(ClickableLoadScene(i));
    }

    public void FinishLoad()
    {
        if(SceneLoaded == true)
        {

        }
    }

    IEnumerator ClickableLoadScene(int i)
    {
        // the loading scene exits at the same time as the current scene
        SceneManager.LoadScene(loadingSceneBuildIndex, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()); // current scene is active one

        // ensure the loading scene is the active scene, depending on the complexity of your scene, you may not need to do this
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(loadingSceneBuildIndex));

        sceneToLoad = i; // cache scene index
        AsyncOperation operation = SceneManager.LoadSceneAsync(i);

        // or whatever you want to happen in the laoding scene
        while(!operation.isDone)
        {
            // if this exits only in the loading scene, make the loading bar a static instance
            loadBar.fillAmount = operation.progress / 0.9f;
            yield return null;
            //yield return new WaitForSecondsRealtime(2000f);
        }
        SceneLoaded = true;
    }
}
