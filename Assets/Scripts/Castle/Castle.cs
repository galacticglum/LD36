using UnityEngine;
using System.Collections;

public class Castle : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100.0f;
    public float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    private float health;
    public float Health
    {
        get { return health; }
    }

	public Castle()
    {
        health = maxHealth;
	}

    public void Heal()
    {
        health = maxHealth;
    }

    public void Heal(float health)
    {
        this.health += health;
    }

    public void Hurt(float amount)
    {
        health -= amount;
    }
}
