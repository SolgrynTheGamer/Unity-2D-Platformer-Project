using UnityEngine;

public class ProjectileLaunch : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform launchPoint;

    public float shootTime;
    public float shootCounter;

    private PlayerMove player;
    private ManaSystem mana;

    void Start()
    {
        shootCounter = shootTime;
        player = GetComponent<PlayerMove>();
        mana = ManaSystem.Instance;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && shootCounter <= 0 && mana.manaPoint >= 30)
        {
            Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);
            mana.manaPoint -= 30;
            shootCounter = shootTime;
            player.PlaySound("ATTACK");
        }
        shootCounter -= Time.deltaTime;
    }
}
