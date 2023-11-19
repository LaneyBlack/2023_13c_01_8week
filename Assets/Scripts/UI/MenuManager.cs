using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject pauseMenuUI;
    // Start is called before the first frame update
    private void Start()
    {
        playerUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetButtonUp("Cancel") && playerUI.activeInHierarchy)
        {
            PauseGameClicked();
        } else if (Input.GetButtonUp("Cancel") && !playerUI.activeInHierarchy && pauseMenuUI.activeInHierarchy)
        {
            UnPauseGameClicked();
        }
    }

    #region PauseMenu

    public void PauseGameClicked()
    {
        playerUI.SetActive(false);
        Time.timeScale = 0;
        pauseMenuUI.SetActive(true); 
    }
    
    public void UnPauseGameClicked()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        playerUI.SetActive(true);
    }
    
    public void RestartClicked()
    {
        playerUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        //ToDo implement restart Level
    }
    
    public void ExitClicked()
    {
        playerUI.SetActive(false);
        pauseMenuUI.SetActive(false);
    }
    #endregion
}
