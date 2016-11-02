using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; protected set; }

    [SerializeField]
    private GameObject gameStateObject;
    [SerializeField]
    private GameObject pauseStateObject;
    [SerializeField]
    private GameObject shopStateObject;

    private bool paused;
    public bool Paused
    {
        get { return paused; }
        set
        {
            if (value)
            {
                gameStateObject.SetActive(false);
                if (!shop)
                {
                    pauseStateObject.SetActive(true);
                }
            }
            else if(!value)
            {
                shopStateObject.SetActive(false);
                gameStateObject.SetActive(true);
                pauseStateObject.SetActive(false);   
            }
            paused = value;
        }
    }

    private bool shop;

    private void OnEnable()
    {
        if (Instance != null)
        {
            Debug.LogError("There should never be two game state managers!");
        }
        Instance = this;

        gameStateObject.SetActive(true);
        pauseStateObject.SetActive(false);
        shopStateObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Shop();
        }
    }

    public void Shop()
    {
        if(shop)
        {
            shop = false;
            paused = false;

            ShopReset(false);
        }
        else
        {
            shop = true;
            paused = true;

            ShopReset(true);
        }
    }

    public void ShopReset(bool enable)
    {
        shopStateObject.transform.FindChild("Menu").gameObject.SetActive(true);
        shopStateObject.transform.FindChild("Not Enough").gameObject.SetActive(false);
        shopStateObject.transform.FindChild("Already Purchased").gameObject.SetActive(false);
        shopStateObject.transform.FindChild("Turret Selection - Blitz").gameObject.SetActive(false);
        shopStateObject.transform.FindChild("Turret Selection - Power").gameObject.SetActive(false);

        shopStateObject.SetActive(enable);
    }

    public void ShopNotEnough()
    {
        shopStateObject.transform.FindChild("Menu").gameObject.SetActive(false);
        shopStateObject.transform.FindChild("Not Enough").gameObject.SetActive(true);
    }

    public void ShopAlreadyPurchased(string message)
    {
        shopStateObject.transform.FindChild("Menu").gameObject.SetActive(false);
        shopStateObject.transform.FindChild("Already Purchased").gameObject.SetActive(true);
        shopStateObject.transform.FindChild("Already Purchased").FindChild("Description").GetComponent<Text>().text = message;
    }

    public void OpenBlitzSelection()
    {
        if(shop)
        {
            if(GameManager.Instance.Doubloons < ShopManager.Instance.BlitzTurretCost)
            {
                ShopNotEnough();
                return;
            }

            shopStateObject.transform.FindChild("Turret Selection - Blitz").gameObject.SetActive(true);
            shopStateObject.transform.FindChild("Menu").gameObject.SetActive(false);
        }
        else if(!shop)
        {
            Shop();

            if (GameManager.Instance.Doubloons < ShopManager.Instance.BlitzTurretCost)
            {
                ShopNotEnough();
                return;
            }

            shopStateObject.transform.FindChild("Turret Selection - Blitz").gameObject.SetActive(true);
            shopStateObject.transform.FindChild("Menu").gameObject.SetActive(false);
        }
    }

    public void CloseBlitzSelection()
    {
        shopStateObject.transform.FindChild("Turret Selection - Blitz").gameObject.SetActive(false);      
        shopStateObject.transform.FindChild("Menu").gameObject.SetActive(true);
    }

    public void OpenPowerSelection()
    {
        if (shop)
        {
            if (GameManager.Instance.Doubloons < ShopManager.Instance.PowerTurretCost)
            {
                ShopNotEnough();
                return;
            }

            shopStateObject.transform.FindChild("Turret Selection - Power").gameObject.SetActive(true);
            shopStateObject.transform.FindChild("Menu").gameObject.SetActive(false);
        }
        else if (!shop)
        {
            Shop();

            if (GameManager.Instance.Doubloons < ShopManager.Instance.PowerTurretCost)
            {
                ShopNotEnough();
                return;
            }

            shopStateObject.transform.FindChild("Turret Selection - Power").gameObject.SetActive(true);
            shopStateObject.transform.FindChild("Menu").gameObject.SetActive(false);
        }
    }

    public void ClosePowerSelection()
    {
        shopStateObject.transform.FindChild("Turret Selection - Power").gameObject.SetActive(false);
        shopStateObject.transform.FindChild("Menu").gameObject.SetActive(true);
    }
}
