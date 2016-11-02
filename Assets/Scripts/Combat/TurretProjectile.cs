using UnityEngine;
using System.Collections;

public class TurretProjectile : MonoBehaviour
{
    [SerializeField]
    private float lifeTime = 3.0f;
    [SerializeField]
    private float damage;
    public float Damage
    {
        set { damage = value; }
    }

    // Use this for initialization
    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject, 0.1f);
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            collider.gameObject.GetComponent<EnemyController>().Health -= damage;
            Destroy(gameObject);
        }
    }
}
