using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveZone : MonoBehaviour
{
    private GameObject player;
    public GameObject promptText;
    public InputManager inputManager;
    public PlayerHealth playerHealth;
    public IntersceneStatusModel intersceneStatusModel;

    private bool allowSave;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inputManager = player.GetComponent<InputManager>();
        playerHealth = player.GetComponent<PlayerHealth>();
        intersceneStatusModel = player.GetComponent<IntersceneStatusModel>();
        promptText.SetActive(false);
    }

    private void Update()
    {
        /*if (inputManager.JumpTriggered() && allowSave)
        {
            intersceneStatusModel.SetRestId(SceneManager.GetActiveScene().buildIndex);
            playerHealth.SetHealth(playerHealth.maxHealth);
            playerHealth.heals = playerHealth.maxHeals;
            intersceneStatusModel.SaveFile();
        }*/
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            promptText.SetActive(true);
            allowSave = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            promptText.SetActive(false);
            allowSave = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (inputManager.JumpIsPressed())
            {
                ES3.Save("SaveScene", SceneManager.GetActiveScene().buildIndex);
                ES3.Save("SavePosX", transform.position.x);
                ES3.Save("SavePosY", transform.position.y);

                playerHealth.SetHealth(playerHealth.maxHealth);
                playerHealth.heals = playerHealth.maxHeals;
            }
        }
    }
}
