using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MenuManager : MonoBehaviour
{
    [Header("Player UI")]
    [SerializeField] private GameObject playerUI;
    [Header("PauseMenu")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject areYouSureMenu;
    // Start is called before the first frame update
    private void Start()
    {
        playerUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        areYouSureMenu.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetButtonUp("Cancel") && playerUI.activeInHierarchy)
        {
            PauseClicked();
        } else if (Input.GetButtonUp("Cancel") && !playerUI.activeInHierarchy && pauseMenuUI.activeInHierarchy)
        {
            UnPauseClicked();
        }
    }

    #region PauseMenu

    public void PauseClicked()
    {
        playerUI.SetActive(false);
        Time.timeScale = 0;
        pauseMenuUI.SetActive(true); 
    }
    
    public void UnPauseClicked()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        playerUI.SetActive(true);
    }
    
    public void RestartClicked()
    {
        playerUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 0;
        //ToDo implement restart Level
    }
    
    public void ExitClicked()
    {
        areYouSureMenu.SetActive(true);
    }

    #region AreYouSure

    public void ExitYesClicked()
    {
        playerUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        areYouSureMenu.SetActive(false);
        Time.timeScale = 0;
        //ToDo Main menu exit
    }

    public void ExitNoClicked()
    {
        areYouSureMenu.SetActive(false);
    }


    #endregion

    #endregion
}
