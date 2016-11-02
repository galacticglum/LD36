using UnityEngine;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; protected set; }

    public uint CastleRepairCost = 200;
    public uint BlitzTurretCost = 200;
    public uint PowerTurretCost = 300;
    public uint BlastTurretCost = 500;
    [SerializeField]
    private uint enemySlowdownCost = 450;

    private int blitzCount = 0;
    private int powerCount = 0;

    private void Start()
    {
        Instance = this;
    }

    public void CastleRepair()
    {
        if(GameManager.Instance.Doubloons < CastleRepairCost)
        {
            GameStateManager.Instance.ShopNotEnough();
            return;
        }
        GameManager.Instance.Doubloons -= CastleRepairCost;
        GameManager.Instance.Castle.Heal();
    }

    public void BlitzTurret(int turretID)
    {
        if (blitzCount <= turretID)
        {
            if (GameManager.Instance.Doubloons < BlitzTurretCost)
            {
                GameStateManager.Instance.ShopNotEnough();
                return;
            }

            GameManager.Instance.Doubloons -= BlitzTurretCost;
            GameManager.Instance.Turrets[turretID].FireRate = 0.35f;

            blitzCount++;
        }
        else
        {
            GameStateManager.Instance.ShopAlreadyPurchased("You already purchased Blitz Turret for turret " + (turretID + 1) + ", why buy it again?");
        }
    }

    public void PowerTurret(int turretID)
    {
        if (powerCount <= turretID)
        {
            if (GameManager.Instance.Doubloons < PowerTurretCost)
            {
                GameStateManager.Instance.ShopNotEnough();
                return;
            }
            GameManager.Instance.Doubloons -= PowerTurretCost;
            GameManager.Instance.Turrets[turretID].MinDamage = 5f;
            GameManager.Instance.Turrets[turretID].MaxDamage = 15f;

            powerCount++;
        }
        else
        {
            GameStateManager.Instance.ShopAlreadyPurchased("You already purchased Power Turret for turret " + (turretID + 1) + ", why buy it again?");
        }
    }

    public void BlastTurret()
    {
        if (GameManager.Instance.Doubloons < BlastTurretCost)
        {
            GameStateManager.Instance.ShopNotEnough();
            return;
        }
        GameManager.Instance.Doubloons -= BlastTurretCost;

        for (int i = 0; i < GameManager.Instance.Turrets.Length; i++)
        {
            GameManager.Instance.Turrets[i].Blast = true;
        }
    }

    public void EnemySlowdown()
    {
        if (GameManager.Instance.Doubloons < enemySlowdownCost)
        {
            GameStateManager.Instance.ShopNotEnough();
            return;
        }
        GameManager.Instance.Doubloons -= enemySlowdownCost;
        EnemySpawner.Instance.EnemySlowdown = true;
    }
}
