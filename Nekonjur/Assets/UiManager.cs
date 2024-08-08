using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI tankHealth;
    public TextMeshProUGUI dpsHealth;
    public TextMeshProUGUI supportHealth;
    public TextMeshProUGUI enemyHealth;

    public GameObject tankEliminated;
    public GameObject dpsEliminated;
    public GameObject supportEliminated;

    public GameObject VictoryScreen;
    public GameObject GameOverScreen;

    private void Start()
    {
        tankEliminated.SetActive(false);
        dpsEliminated.SetActive(false);
        supportEliminated.SetActive(false);

        VictoryScreen.SetActive(false);
        GameOverScreen.SetActive(false);
    }

    public void UpdateTankHealth(int currHealth, int maxHealth)
    {
        tankHealth.text = "HP: " + currHealth.ToString() + " / " + maxHealth.ToString();
    }

    public void UpdateDpsHealth(int currHealth, int maxHealth)
    {
        dpsHealth.text = "HP: " + currHealth.ToString() + " / " + maxHealth.ToString();
    }

    public void UpdateSupportHealth(int currHealth, int maxHealth)
    {
        supportHealth.text = "HP: " + currHealth.ToString() + " / " + maxHealth.ToString();
    }

    public void UpdateEnemyHealth(int currHealth, int maxHealth)
    {
        enemyHealth.text = "HP " + currHealth.ToString() + " / " + maxHealth.ToString();
    }

    public void PrintVictory()
    {
        VictoryScreen.SetActive(true);
    }

    public void PrintGameOver()
    {
        GameOverScreen.SetActive(true);
    }

    public void LoadOpenWorldScene()
    {
        SceneManager.LoadSceneAsync("OpenWorldScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
