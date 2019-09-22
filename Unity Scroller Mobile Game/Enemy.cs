using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 rbForce;
    public Vector3 rbAditionalForce;



    void Start()
    {
        // Gives the owner of this script a start force to start moving.
        rb.AddForce(rbForce);
    }

    private void Update()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        for (int i = 0; i < gameManager.GetComponent<GameManager>().score; i++)
        {
            rb.AddForce(rbAditionalForce);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject gameManager = GameObject.Find("GameManager");
        // This checks if the gameObject has one of the 2 assigned nametags to destroy the gameObject.
        if (other.tag == "Player" || other.tag == "DeathBarrier")
        {
            if (gameManager != null)
            {
                gameManager.GetComponent<GameManager>().ScoreUp();
            }
            Destroy(this.gameObject);
        }
    }
}
