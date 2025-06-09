using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D projectileRb;
    public float speed;

    public float projectileLife;
    public float projectileCount;

    public PlayerMove playerMove;
    public bool facingRight;

    void Start()
    {
        projectileCount = projectileLife;
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        facingRight = playerMove.facingRight;
        if (!facingRight)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void Update()
    {
        projectileCount -= Time.deltaTime;
        if (projectileCount <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (facingRight)
        {
            projectileRb.linearVelocity = new Vector2(speed, projectileRb.linearVelocity.y);
        }
        else
        {
            projectileRb.linearVelocity = new Vector2(-speed, projectileRb.linearVelocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyMove enemy = collision.gameObject.GetComponent<EnemyMove>();
            if (enemy != null)
            {
                enemy.OnDamaged();
            }
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}
