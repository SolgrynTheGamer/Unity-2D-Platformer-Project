using UnityEngine;
<<<<<<< Updated upstream
=======
using UnityEngine.SceneManagement;
>>>>>>> Stashed changes

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    public float jumpCount;
<<<<<<< Updated upstream
    public GameManager manager;
=======

>>>>>>> Stashed changes
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider2D;
    Animator anim;
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDeath;
    public AudioClip audioFinish;
    AudioSource audioSource;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>(); // 2D �ݶ��̴�
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void PlaySound(string action)
    {
        switch (action)
        {
            case "JUMP":
                audioSource.clip = audioJump;
                audioSource.Play();
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                audioSource.Play();
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                audioSource.Play();
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                audioSource.Play();
                break;
            case "DEATH":
                audioSource.clip = audioDeath;
                audioSource.Play();
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                audioSource.Play();
                break;

        }
    }

    private void Update()
    {
<<<<<<< Updated upstream
=======
        if (Time.timeScale == 0) return; // ������ �������� ���� (Time.timeScale = 0) �÷��̾� ������ �����ϴ�.

>>>>>>> Stashed changes
        if (Input.GetButtonDown("Jump") && jumpCount < 2)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
            PlaySound("JUMP");
            jumpCount++;
        }

        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.normalized.x * 0.0000001f, rigid.linearVelocityY);
        }

        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }
        
        if (rigid.linearVelocity.normalized.x == 0)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
    }
    void FixedUpdate()
    {
<<<<<<< Updated upstream
=======
        if (Time.timeScale == 0)
        {
            rigid.linearVelocity = Vector2.zero; // ����
            return;
        }

>>>>>>> Stashed changes
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.linearVelocityX > maxSpeed)
            rigid.linearVelocity = new Vector2(maxSpeed, rigid.linearVelocityY);
        else if (rigid.linearVelocityX < -maxSpeed)
            rigid.linearVelocity = new Vector2(-maxSpeed, rigid.linearVelocityY);

        if (rigid.linearVelocityY < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, Color.yellow);
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                {
                    anim.SetBool("isJumping", false);
                    jumpCount = 0;
                }
<<<<<<< Updated upstream
                    
=======

>>>>>>> Stashed changes
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
<<<<<<< Updated upstream
        if (collision.gameObject.tag == "Enemy")
=======
        if (collision.gameObject.CompareTag("Enemy"))
>>>>>>> Stashed changes
        {
            if (rigid.linearVelocityY < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            else
            {
                OnDamaged(collision.transform.position);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
<<<<<<< Updated upstream
        if (collision.gameObject.tag == "Item")
=======
        if (collision.gameObject.CompareTag("Item"))
>>>>>>> Stashed changes
        {
            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");

            if (isBronze) manager.stagePoint += 50;
            else if (isSilver) manager.stagePoint += 100;
            else if (isGold) manager.stagePoint += 200;

            collision.gameObject.SetActive(false);
            PlaySound("ITEM");
        }
        else if (collision.gameObject.tag == "Finish")
        {
<<<<<<< Updated upstream
            manager.NextStage();
            PlaySound("FINISH");
        }
=======
            if (GameManager.Instance != null)
            {
                GameManager.Instance.NextStage();
            }
            PlaySound("FINISH");
        }
        else if (collision.gameObject.CompareTag("DeadZone"))
        {
            if (HealthSystem.Instance != null)
            {
                HealthSystem.Instance.TakeDamage(); // DeadZone�� ������ ü�� 1 ����
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.PlayerReposition(); // �÷��̾� ������
                }
            }
        }
>>>>>>> Stashed changes
    }

    void OnAttack(Transform enemy)
    {
        PlaySound("ATTACK");
<<<<<<< Updated upstream
        manager.stagePoint += 100;
=======
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddStagePoint(100);
        }

>>>>>>> Stashed changes
        rigid.AddForce(Vector2.up * 8, ForceMode2D.Impulse);
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
    }

    void OnDamaged(Vector2 targetPos)
    {
<<<<<<< Updated upstream
        PlaySound("DAMAGED");
        manager.HealthDown();
        gameObject.layer = 8;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
=======
        if (gameObject.layer == LayerMask.NameToLayer("PlayerDamaged")) return;

        PlaySound("DAMAGED");

        if (HealthSystem.Instance != null)
        {
            HealthSystem.Instance.TakeDamage();
        }

        gameObject.layer = LayerMask.NameToLayer("PlayerDamaged"); // ���� ���̾�� ���� (Layer 8)
        spriteRenderer.color = new Color(1, 1, 1, 0.4f); // �����ϰ�
>>>>>>> Stashed changes

        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);

        anim.SetTrigger("isDamaged");

<<<<<<< Updated upstream
        Invoke("OffDamaged", 3);
=======
        Invoke("OffDamaged", 1); // 1�� �� ���� ���� (���� 3�ʿ��� ����)
>>>>>>> Stashed changes
    }

    void OffDamaged()
    {
        gameObject.layer = 7;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDeath()
    {
        PlaySound("DEATH");
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        spriteRenderer.flipY = true;
        capsuleCollider2D.enabled = false; // �÷��̾��� �ݶ��̴� ��Ȱ��ȭ
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
<<<<<<< Updated upstream
=======
        // �÷��̾� ������ ����
>>>>>>> Stashed changes
    }

    public void VelocityZero()
    {
        rigid.linearVelocity = Vector2.zero;
    }
}
