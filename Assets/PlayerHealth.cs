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

    private void Start()
    {
        statusModel = GetComponent<StatusModel>();
        stamina = maxStamina;
        health = ES3.Load("health", 0);
        if (health == 0) health = maxHealth;
    }
    private void Update()
    {
        if(statusModel.isGrounded) RefreshStamina();
        StaminaState();
        if (health <= 0) {
            ES3.Save("health", 0);
            ES3.Save("loadToSavePoint", true);
            SceneManager.LoadScene(ES3.Load("SaveScene", 0));
        }
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
        if (!statusModel.isInvincible) health -= reduction;
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
}
