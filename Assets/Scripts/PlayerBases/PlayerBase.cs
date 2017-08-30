using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : Base
{
    // Events
    public delegate void PlayerBaseDestroyedHandler();
    public event PlayerBaseDestroyedHandler OnPlayerBaseDestroyed;

    [SerializeField]
    private int baseCost;
    public int BaseCost
    {
        get { return baseCost; }
        set { baseCost = value; }
    }
    public RectTransform healthBar;

    [SerializeField]
    private ParticleSystem explosionParticleSystem;

    // Use this for initialization
    override protected void Start()
    {
        base.Start();
    }

    private void Update()
    {
        AddjustCurrentHealth(0);
    }

    override public bool Damage(float damage)
    {
        base.Damage(damage);
        //Debug.Log("PlayerBase Damage() " + baseHealth);
        if (baseHealth <= 0.0f && BaseManager.instance.PlayerBases.Count > 0.0f)
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

        healthBar.sizeDelta = new Vector2(baseHealth, healthBar.sizeDelta.y);
    }

}
