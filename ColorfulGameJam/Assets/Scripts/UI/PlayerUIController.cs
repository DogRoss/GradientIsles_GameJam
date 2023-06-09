using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] GameObject menuUI;
    [SerializeField] MouseLook mouse;

    private void Start()
    {
        menuUI.SetActive(false);    
    }

    //This was private.
    public void OnEscape()
    {
        if (menuUI.activeSelf)
        {
            menuUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            mouse.mouseLock = false;
            Time.timeScale = 1;

        }
        else
        {
            menuUI.SetActive(true);
            
            Cursor.lockState = CursorLockMode.None;
            mouse.mouseLock = true;
            Time.timeScale = 0;
        }

    }

    public void LoadScene(string name)
    {
        if (FindObjectOfType<LoadSceneAsync>(false))
        {
            LoadSceneAsync aSyncLoader = FindObjectOfType<LoadSceneAsync>(false);
            if (aSyncLoader != null)
            {
                Debug.Log("Successful grab");
            }
        }
        else
        {
            Debug.Log("Fail");
        }
    }


}
