using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameInfo : MonoBehaviour
{
    public static GameInfo S;

    public float score = 0;

    [SerializeField] private Text scoreText;

    private bool isGameOver = false;

    private void Awake()
    {
        if(S != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            S = this;
            DontDestroyOnLoad(this.gameObject);
        }

        isGameOver = false;
    }

    private void Update()
    {
        Restart();

        RenewalUI();
    }

    private void RenewalUI()
    {
        scoreText.text = String.Format("{0:#,###}", Convert.ToInt32(score)).ToString();
    }

    public void GameOver()
    {
        isGameOver = true;
    }

    private void Restart()
    {
        if(isGameOver == true && Input.GetMouseButtonDown(0))
        {
            isGameOver = false;
            SceneManager.LoadScene(SCENE.GAME_SCENE);
        }
    }
}
