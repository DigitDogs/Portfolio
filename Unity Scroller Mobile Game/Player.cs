using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            GameObject gameManager = GameObject.Find("GameManager");
            if (gameManager != null)
            {
                gameManager.GetComponent<GameManager>().ShowEndScreen();
            }
                Destroy(this.gameObject);
        }
    }
}
