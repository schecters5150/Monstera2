using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public float maxStamina;
    public float stamina;
    public float staminaRefreshRate;
    public int maxHeals;
    public int heals;

    private StatusModel statusModel;
    private InputManager inputManager;
    private SoundController soundController;

    private void Start()
    {
        statusModel = GetComponent<StatusModel>();
        inputManager = GetComponent<InputManager>();
        soundController = GetComponent<SoundController>();

        stamina = maxStamina;
        heals = ES3.Load("heals", maxHeals);
        health = ES3.Load("health", 0);
        if (health == 0) health = maxHealth;
    }
    private void Update()
    {
        if (statusModel.isGrounded) RefreshStamina();
        StaminaState();
        CheckHealing();
        if (health <= 0)
        {
            ES3.Save("health", 0);
            ES3.Save("loadToSavePoint", true);
            SceneManager.LoadScene(ES3.Load("SaveScene", 0));
        }
    }

    private void CheckHealing()
    {
        if (inputManager.HealTriggered() && heals > 0)
        {
            heals--;
            health += maxHealth / 2;
        }

        if (health > maxHealth) health = maxHealth;
    }


    public int GetHealth()
    {
        return health;
    }
    public void SetHealth(int health)
    {
        this.health = health;
    }
    public void ReduceHealth(int reduction)
    {
        if (!statusModel.isInvincible)
        {
            health -= reduction;
            soundController.PlayShatter();
        }
    }
    public void ReduceStamina(float reduction)
    {
        stamina -= reduction;
    }
    public void RefreshStamina()
    {
        if (stamina < maxStamina) stamina += staminaRefreshRate * Time.deltaTime;
        else stamina = maxStamina;
    }
    public void RefreshAllStamina()
    {
        stamina = maxStamina;
    }
    public void AddStamina(float stamina)
    {
        this.stamina += stamina;
        if (this.stamina > maxStamina) this.stamina = maxStamina;
    }
    public void StaminaState()
    {
        if (stamina > 0) statusModel.staminaDepleted = false;
        else statusModel.staminaDepleted = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "StaminaRegen")
        {
            stamina = maxStamina;
            //Destroy(collider.transform.gameObject);
        }
    }
}
