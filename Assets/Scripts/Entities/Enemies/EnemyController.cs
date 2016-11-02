using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(EnemyMotor))]
public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100;
    private float health;
    public float Health
    {
        get { return health; }
        set { health = value; }
    }

    [SerializeField]
    private float maxDamage = 5.0f;
    [SerializeField]
    private float attackRate = 0.5f;
    public float AttackRate
    {
        get { return attackRate; }
        set { attackRate = value; }
    }

    private float timeSinceLastAttack;

    private Transform target;
    private EnemyMotor m_Character;

    private void Awake()
    {
        GetComponent<EnemyMotor>().OnObstacleCollide += ObstacleCollideCallback;
        health = maxHealth;

        m_Character = GetComponent<EnemyMotor>();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
        target = GameManager.Instance.EnemyTarget;
    }

    private void Update()
    {
        if (!GameStateManager.Instance.Paused)
        {
            if (health <= 0)
            {
                GameManager.Instance.Doubloons += (uint)UnityEngine.Random.Range(GameManager.Instance.DoubloonMin, GameManager.Instance.DoubloonMax);
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!GameStateManager.Instance.Paused)
        {
            Vector2 direction = (target.position - transform.position) / (target.position - transform.position).magnitude;

            if (m_Character.Grounded)
            {
                if (direction.x < 0)
                {
                    m_Character.Move(-0.3f, false, false);
                }
                else if (direction.x > 0)
                {
                    m_Character.Move(-0.3f, false, false);
                }
            }
        }
    }

    private void ObstacleCollideCallback(Collider2D collider)
    {
        // they are the same object!
        if(GameManager.Instance.Castle.gameObject == collider.gameObject)
        {
            if (Time.time > attackRate + timeSinceLastAttack)
            {
                timeSinceLastAttack = Time.time;
                GameManager.Instance.Castle.Hurt(UnityEngine.Random.Range(0, maxDamage));

            }
        }
    }
}

