using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject login;
    [SerializeField] private GameObject auth;

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartLogin()
    {      
        login.SetActive(true);

        auth.SetActive(true);
    }
}
