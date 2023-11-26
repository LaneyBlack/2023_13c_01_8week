using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pressButtonText;
    [SerializeField] private GameObject bigGameLogo;
    [SerializeField] private GameObject menu;
    [SerializeField] private float pressTimeToAppear = 0.7f;
    [SerializeField] public string sceneName;
    private float pressTextTimer;
    private bool wasButtonPressed;

    private void Awake()
    {
        menu.SetActive(false);
        pressTextTimer = pressTimeToAppear;
        wasButtonPressed = false;
    }

    // Update is called once per frame
    private void Update()
    {
        // Wait for any button input to go further
        if (Input.anyKey && !wasButtonPressed)
        {
            wasButtonPressed = true;
            bigGameLogo.SetActive(false);
            pressButtonText.SetActive(false);
            menu.SetActive(true);
        }
        // If there was button press there is no need for press button text animation
        if (wasButtonPressed) return;
        if (pressTextTimer < 0)
        {
            pressButtonText.SetActive(!pressButtonText.activeInHierarchy); // switch
            pressTextTimer += pressTimeToAppear;
        }
        pressTextTimer -= Time.deltaTime;
    }

    #region MenuButtons

    public void ExitGameClick()
    {
        Application.Quit();
    }
    
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    #endregion
}
