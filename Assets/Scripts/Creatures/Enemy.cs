using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
    [SerializeField] private int ViewDistance = 15;

    [SerializeField] private int ShootDistance = 7;

    [SerializeField] private Transform EnemyBody;

    private Rigidbody2D m_rgb2d;

    private Transform m_playerTransform;

    private bool m_isTargetOnDistance;

    private Vector2 m_targetDirection;

    private float m_targetDistance;


    public override void SetupOnStart(GameManager mngr)
    {
        base.SetupOnStart(mngr);

        m_playerTransform = mngr.Player.transform;

        m_speedMoving = 50f;

        m_speedRotating = 5f;
    }

    protected override void Awake()
    {
        base.Awake();

        m_rgb2d = GetComponent<Rigidbody2D>();

        MachineGun weapon = new MachineGun(this, "bulletMachineGun", BulletStartPosition);
        SetWeapon(weapon);
    }

    protected override void Update()
    {
        base.Update();

        if (!m_gameManager.IsGameStarted) return;

        m_isTargetOnDistance = false;
        m_targetDistance = Vector2.Distance(m_playerTransform.position, this.transform.position);
        if (m_targetDistance < ViewDistance)
        {
            LayerMask mask = LayerMask.GetMask("Default");
            RaycastHit2D hit = Physics2D.Linecast(m_playerTransform.position, this.transform.position, mask);
            if (hit.collider == null)
            {
                m_isTargetOnDistance = true;
            }
        }

        if (m_isTargetOnDistance)
        {
            m_targetDirection = m_playerTransform.position - transform.position;
            float angle = Mathf.Atan2(m_targetDirection.y, m_targetDirection.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            EnemyBody.rotation = Quaternion.Slerp(EnemyBody.rotation, targetRotation, Time.deltaTime * m_speedRotating);
        }
    }

    private void FixedUpdate()
    {
        if (!m_gameManager.IsGameStarted) return;

        if (m_isTargetOnDistance && m_targetDistance > 1f)
        {
            m_rgb2d.velocity = EnemyBody.right * m_speedMoving * Time.deltaTime;
        }
        else
        {
            m_rgb2d.velocity = Vector2.zero;
        }
    }

    protected override void CheckShoot()
    {
        if (m_isTargetOnDistance && m_targetDistance < ShootDistance)
        {
            DoShoot();
        }
    }

    protected override void DoKill()
    {
        GameObject asset = Resources.Load<GameObject>("EnemyExplosionDeath");
        GameObject go = Instantiate(asset);

        go.transform.position = this.transform.position;

        this.gameObject.SetActive(false);
    }
}
