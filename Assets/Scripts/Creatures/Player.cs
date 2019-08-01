using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{
    [SerializeField] private Transform PlayerBody;

    public int KeyCount { get; private set; }

    private Rigidbody2D m_rgb2d;

    private float m_deltaX;

    private float m_deltaY;


    protected override void Awake()
    {
        base.Awake();

        m_rgb2d = GetComponent<Rigidbody2D>();

        Pistol weapon = new Pistol(this, "bulletPistol", BulletStartPosition);
        SetWeapon(weapon);        
    }

    protected override void Update()
    {
        base.Update();

        if (!m_gameManager.IsGameStarted) return;

        m_deltaX = Input.GetAxis("Horizontal");
        m_deltaY = Input.GetAxis("Vertical");

        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        PlayerBody.rotation = rot;
    }

    private void FixedUpdate()
    {
        if (!m_gameManager.IsGameStarted) return;

        m_rgb2d.velocity = new Vector2(m_deltaX, m_deltaY) * m_speedMoving * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckCollision(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckCollision(collision.gameObject);
    }

    public override void SetupOnStart(GameManager mngr)
    {
        base.SetupOnStart(mngr);

        m_speedMoving = 150f;
    }

    private void CheckCollision(GameObject collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            switch (item.ItemType)
            {
                case Item.eItemType.DOOR_LOCKABLE:
                    if(KeyCount > 0)
                    {
                        KeyCount--;
                        item.Hide();
                    }
                    break;
                case Item.eItemType.HEALTH:
                    if (Health < HealthMax)
                    {
                        Health += 10f;
                        item.Hide();
                    }
                    break;
                case Item.eItemType.KEY:
                    KeyCount++;
                    item.Hide();
                    break;
                case Item.eItemType.EXIT:
                    m_gameManager.GameOver();
                    break;
            }
        }

    }

    protected override void CheckShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DoShoot();
        }
    }

    protected override void DoKill()
    {
        m_gameManager.GameOver();
    }

}
