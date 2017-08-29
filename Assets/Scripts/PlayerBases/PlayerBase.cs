using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : Base
{
    // Events
    public delegate void PlayerBaseDestroyedHandler();
    public /*static*/ event PlayerBaseDestroyedHandler OnPlayerBaseDestroyed;

    private float healthBarLength;

    [SerializeField]
    private int baseCost;
    public int BaseCost
    {
        get { return baseCost; }
        set { baseCost = value; }
    }

    [SerializeField]
    private ParticleSystem explosionParticleSystem;

    // Use this for initialization
    override protected void Start()
    {
        base.Start();
        healthBarLength = Screen.width / 6;
    }

    private void Update()
    {
        AddjustCurrentHealth(0);
    }

    override public bool Damage(int damage)
    {
        base.Damage(damage);
        if (baseHealth <= 0 && BaseManager.instance.PlayerBases.Count > 0)
        {
            BaseManager.instance.PlayerBases.Remove(this);
            OnPlayerBaseDestroyed();
            // explosion particle
            if (explosionParticleSystem)
                Instantiate(explosionParticleSystem, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
        return baseHealth <= 0;
    }

    public void AddjustCurrentHealth(int adj)
    {
        baseHealth += adj;

        if (baseHealth < 0)
            baseHealth = 0;

        if (baseHealth > maxHealth)
            baseHealth = maxHealth;

        if (maxHealth < 1)
            maxHealth = 1;

        healthBarLength = (Screen.width / 6) * (baseHealth / (float)maxHealth);
    }


    void OnGUI()
    {

        Vector2 targetPos;
        targetPos = Camera.main.WorldToScreenPoint(transform.position);

        GUI.Box(new Rect(targetPos.x - 30, Screen.height - targetPos.y, 60, 20), baseHealth + "/" + maxHealth);

    }

}
