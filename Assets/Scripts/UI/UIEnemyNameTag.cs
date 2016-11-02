using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIEnemyNameTag : MonoBehaviour
{
    private static Transform nameTagParent;

    [SerializeField]
    private Transform nameTagPrefab;
    private Transform nameTag;

    [SerializeField]
    private Vector3 positionOffset = Vector3.up; 

    private string enemyName;
	// Use this for initialization
	private void Start ()
    {
        if (nameTagPrefab != null)
        {
            if(nameTagParent == null)
            {
                nameTagParent = new GameObject("Enemy Name Tags").transform;
                nameTagParent.SetParent(GameObject.Find("Canvas").transform);
            }

            enemyName = Parser.GetName();
            nameTagPrefab.GetComponent<Text>().text = enemyName;

            nameTag = Instantiate(nameTagPrefab);
            nameTag.SetParent(nameTagParent);
        }
	}
	
	// Update is called once per frame
	private void Update ()
    {
        if (nameTagPrefab != null)
        {         
            if(GameManager.Instance.Castle.Health <= 0)
            {
                nameTag.gameObject.SetActive(false);
            }

            nameTag.position = Camera.main.WorldToScreenPoint(transform.position + positionOffset);
        }
    }

    private void OnDestroy()
    {
        if (nameTag != null)
        {
            Destroy(nameTag.gameObject);
        }
    }
}
