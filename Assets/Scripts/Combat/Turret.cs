using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret : MonoBehaviour
{
    [SerializeField]
    private bool active = true;
    public bool Active
    {
        get { return active; }
        set { active = value; }
    }

    [SerializeField]
    private Transform turretBarrel;
    [SerializeField]
    private Transform nozzle;
    [SerializeField]
    private GameObject turretBulletPrefab;
    [SerializeField]
    private float turretBulletVelocity = 10.0f;
    [SerializeField]
    private float maxDamage = 5.0f;
    public float MaxDamage
    {
        get { return maxDamage; }
        set { maxDamage = value; }
    }

    [SerializeField]
    private float minDamage = 1.0f;
    public float MinDamage
    {
        get { return minDamage; }
        set { minDamage = value; }
    }

    [SerializeField]
    private bool blast;
    public bool Blast
    {
        get { return blast; }
        set { blast = value; }
    }
    [SerializeField]
    private uint blastDuration = 30;
    private float blastTimer;

    [SerializeField]
    private float fireRate = 0.5f;
    public float FireRate
    {
        get { return fireRate; }
        set { fireRate = value; }
    }

    private float timeSinceLastFire;

    [SerializeField]
    private float angleMin = 0.0f;
    [SerializeField]
    private float angleMax = 0.0f;

    // trajectory variables
    [SerializeField]
    private bool drawTrajectory = true;
    [SerializeField]
    private GameObject trajectoryPointPrefab;
    private int trajectoryVertices = 30;
    private List<GameObject> trajectoryPoints;

    private GameObject turretBulletParent;

    private void Start()
    {
        blastTimer = blastDuration;

        turretBulletParent = new GameObject("Turret Bullets");
        turretBulletParent.transform.SetParent(transform);

        if (drawTrajectory)
        {
            GameObject trajectoryPointParent = new GameObject("Trajectory Points");
            trajectoryPointParent.transform.SetParent(transform);

            trajectoryPoints = new List<GameObject>();
            for (int i = 0; i < trajectoryVertices; i++)
            {
                GameObject point = Instantiate(trajectoryPointPrefab);
                point.GetComponent<Renderer>().enabled = false;
                trajectoryPoints.Insert(i, point);

                point.transform.SetParent(trajectoryPointParent.transform);
            }
        }
    }

    private void Update()
    {
        if (!GameStateManager.Instance.Paused)
        {
            if (Blast)
            {
                blastTimer -= Time.deltaTime;
            }

            if (active)
            {
                Vector3 mousePosition = (new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5.23f)) - Camera.main.WorldToScreenPoint(transform.position);
                float angle = ClampAngle((Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg + 90), angleMin, angleMax);

                Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                turretBarrel.rotation = rotation;

                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    Fire();
                }
                CalculateTrajectoryPoints(nozzle.transform.position, (-nozzle.up * turretBulletVelocity) / turretBulletPrefab.GetComponent<Rigidbody2D>().mass);
            }
            else if (!active && drawTrajectory)
            {
                for (int i = 0; i < trajectoryPoints.Count; i++)
                {
                    trajectoryPoints[i].GetComponent<Renderer>().enabled = false;
                }
            }
        }
    }

    private void Fire()
    {
        if (!Blast)
        {
            if (Time.time > fireRate + timeSinceLastFire)
            {
                timeSinceLastFire = Time.time;

                GameObject bullet = (GameObject)Instantiate(turretBulletPrefab, nozzle.position, Quaternion.identity);
                bullet.transform.SetParent(turretBulletParent.transform);
                bullet.GetComponent<TurretProjectile>().Damage = Random.Range(minDamage, maxDamage);
                Vector2 velocity = -nozzle.up * turretBulletVelocity;

                bullet.GetComponent<Rigidbody2D>().velocity = velocity;
            }
        }
        else if(Blast)
        {
            if (blastTimer > 0)
            {
                if (Time.time > 0.2f + timeSinceLastFire)
                {
                    timeSinceLastFire = Time.time;

                    GameObject bullet = (GameObject)Instantiate(turretBulletPrefab, nozzle.position, Quaternion.identity);
                    bullet.transform.SetParent(turretBulletParent.transform);
                    bullet.GetComponent<TurretProjectile>().Damage = maxDamage;
                    Vector2 velocity = -nozzle.up * turretBulletVelocity;

                    bullet.GetComponent<Rigidbody2D>().velocity = velocity;
                }
            }
            else
            {
                blastTimer = blastDuration;
                Blast = false;
            }
        }
    }

    private void CalculateTrajectoryPoints(Vector3 startPosition, Vector3 velocity)
    {
        if (drawTrajectory)
        {
            float speed = Mathf.Sqrt((velocity.x * velocity.x) + (velocity.y * velocity.y));
            float angle = Mathf.Rad2Deg * (Mathf.Atan2(velocity.y, velocity.x));
            float time = 0.1f;

            for (int i = 0; i < trajectoryVertices; i++)
            {
                float x = speed * time * Mathf.Cos(angle * Mathf.Deg2Rad);
                float y = speed * time * Mathf.Sin(angle * Mathf.Deg2Rad) - (Physics.gravity.magnitude * time * time / 2.0f);
                Vector3 position = new Vector3(startPosition.x + x, startPosition.y + y, 1);

                trajectoryPoints[i].transform.position = position;
                trajectoryPoints[i].GetComponent<Renderer>().enabled = true;
                trajectoryPoints[i].transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(velocity.y - Physics.gravity.magnitude * time, velocity.x) * Mathf.Rad2Deg);

                time += 0.1f;
            }
        }
    }

    public Vector3 PlotTrajectoryAtTime(Vector3 start, Vector3 startVelocity, float time)
    {
        return start + startVelocity * time + Physics.gravity * time * time * 0.5f;
    }

    private float ClampAngle(float angle, float angleMin, float angleMax)
    {
        float angleCap = angleMin - (angleMin - (angleMax)) / 2;
        if (angle <= angleMin && angle >= angleCap)
        {
            angle = angleMin;
        }
        if (angle >= angleMax && angle <= angleCap)
        {
            angle = angleMax;
        }
        return angle;
    }
}
