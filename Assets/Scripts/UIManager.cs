using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    public RectTransform healthBar;

    // Use this for initialization
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
    }

    public void EnableHealthBarUI(float currentHealth, float maxHealth)
    {
        float percentage = (currentHealth / maxHealth) * 100;
        healthBar.sizeDelta = new Vector2(percentage, healthBar.sizeDelta.y);

        healthBar.gameObject.SetActive(true);
        healthBar.parent.gameObject.SetActive(true);
    }

    public void DisableHealthBarUI()
    {
        healthBar.gameObject.SetActive(false);
        healthBar.parent.gameObject.SetActive(false);
    }
}
