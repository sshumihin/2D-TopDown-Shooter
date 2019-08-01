using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour, IDamagable
{
    public Transform BulletStartPosition;

    public float HealthMax = 100f;

    protected float m_speedMoving = 150f;

    protected float m_speedRotating = 5f;

    private float m_health;
    internal float Health
    {
        get { return m_health; }
        set
        {
            m_health = value;

            if (m_health > HealthMax) m_health = HealthMax;

            m_healthBar.SetHealthBar(m_health / HealthMax);

            if (m_health <= 0f)
            {
                DoKill();
            }
        }
    }

    internal bool IsAlive
    {
        get
        {
            return Health > 0f;
        }
    }

    private WeaponBase m_weapon;

    protected GameManager m_gameManager;

    protected HealthBar m_healthBar;

    protected virtual void Awake()
    {
        m_health = HealthMax;

        m_healthBar = GetComponentInChildren<HealthBar>();
        m_healthBar.SetHealthBar(1f);
    }

    protected virtual void Update()
    {
        if (!m_gameManager.IsGameStarted) return;


        CheckShoot();
    }

    public virtual void SetupOnStart(GameManager mngr)
    {
        m_gameManager = mngr;
    }

    public void DoDamage(float dmg)
    {
        Health -= dmg;
    }

    protected virtual void DoKill() { }

    protected virtual void SetWeapon(WeaponBase weapon)
    {
        m_weapon = weapon;
    }

    protected virtual void CheckShoot() { }

    protected virtual void DoShoot()
    {
        if (m_weapon != null) m_weapon.Shoot();
    }
}
