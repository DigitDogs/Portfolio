using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText;
    public Text endScore;

    public GameObject endScreen;
    public GameObject enemy;

    public GameObject bronzeHat;
    public GameObject silverHat;
    public GameObject goldHat;

    public Transform player;

    #region playerPosistions
    public Transform playerPos1;
    public Transform playerPos2;
    public Transform playerPos3;
    #endregion

    #region enemySpawns
    public Vector3 EnemySpawn1;
    public Vector3 EnemySpawn2;
    public Vector3 EnemySpawn3;
    #endregion

    int randomNumber;

    public float enemySpawnTime;
    public int maxEnemies;
    public int totalEnemies;
    public int score;
    public int scene;

    public bool canSpawn;

    bool isLeft;
    bool isMiddle = true;
    bool isRight;

    private void Start()
    {
        canSpawn = true;
    }

    void Update()
    {
        // This checks if there is an enemy assigned and randomises the spawns.
        if (enemy != null)
        {
            if (totalEnemies < maxEnemies && canSpawn == true)
            {
                StartCoroutine(SpawnEnemy());
            }
        }

        // This checks if te requirment for the gold is is aquired.
        if(score >= 10)
        {
            if (bronzeHat != null)
            {
                bronzeHat.SetActive(true);
            }
        }

        if (score >= 20)
        {
            if (silverHat != null)
            {
                silverHat.SetActive(true);
                bronzeHat.SetActive(false);
            }
        }

        if (score >= 30)
        {
            if (goldHat != null)
            {
                goldHat.SetActive(true);
                silverHat.SetActive(false);
            }
        }
    }

    /// <summary>
    /// This function moves the player one step to the left.
    /// </summary>
    public void MoveToLeft()
    {
        if (player != null)
        {
            if (isRight == true)
            {
                isRight = false;
                isMiddle = true;
                player.position = playerPos2.position;
            }

            else if (isMiddle == true)
            {
                isMiddle = false;
                isLeft = true;
                player.position = playerPos1.position;
            }
        }
    }

    /// <summary>
    /// This function moves the player one step to the right.
    /// </summary>
    public void MoveToRight()
    {
        if (player != null)
        {
            if (isLeft == true)
            {
                isLeft = false;
                isMiddle = true;
                player.position = playerPos2.position;
            }

            else if (isMiddle == true)
            {
                isMiddle = false;
                isRight = true;
                player.position = playerPos3.position;
            }
        }
    }

    /// <summary>
    /// This function spawns enemies randomly at 3 fixed positions.
    /// </summary>
    IEnumerator SpawnEnemy()
    {
        randomNumber = Random.Range(1, 4);

        switch (randomNumber)
        {
            case 1:
                Instantiate(enemy, EnemySpawn1, Quaternion.identity);
                totalEnemies++;
                Debug.Log("case 1");
                canSpawn = false;
                break;

            case 2:
                Instantiate(enemy, EnemySpawn2, Quaternion.identity);
                totalEnemies++;
                Debug.Log("case 2");
                canSpawn = false;
                break;

            case 3:
                Instantiate(enemy, EnemySpawn3, Quaternion.identity);
                totalEnemies++;
                Debug.Log("case 3");
                canSpawn = false;
                break;

            default:
                Debug.Log("Error");
                break;
        }

        Debug.Log("Before Wait");
        yield return new WaitForSeconds(enemySpawnTime);
        Debug.Log("After Wait");

        canSpawn = true;
    }

    /// <summary>
    /// This function is used to start the game.
    /// </summary>
    public void GameStart()
    {
        SceneManager.LoadScene(scene);
    }

    /// <summary>
    /// Used to update the score of avoided enemies and lower spawntime;
    /// </summary>
    public void ScoreUp()
    {
        totalEnemies -= 1;
        score++;
        if (scoreText != null)
        {
            scoreText.text = ("Score: " + score);
        }

        if (enemySpawnTime > 0.2f)
        {
            enemySpawnTime -= 0.02f;
        }
    }

    /// <summary>
    /// Sets the End Screen active and shows the score.
    /// </summary>
    public void ShowEndScreen()
    {
        if (endScore != null)
        {
            endScore.text = ("Score: " + score);
        }

        if (endScreen != null)
        {
            endScreen.SetActive(true);
        }
    }
}
