using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    public static HealthSystem Instance; // �̱��� �ν��Ͻ�

    // ü�� �� UI (Image ������Ʈ)
    public Image[] healthHearts; // ��Ʈ �̹��� �迭 (UI�� ��Ʈ ������ŭ ����)

    [Header("Health Settings")]
    public int currentHealth = 3; // ���� ü�� (��Ʈ 3�� ����)
    public int maxHealth = 3; // �ִ� ü�� (��Ʈ �ִ� 3��)

    [Header("Game Over UI")]
    public GameObject gameOverPanel; // ���� ���� �� Ȱ��ȭ�� Panel
    public Button retryButton; // ����� ��ư
    public string firstStageSceneName = "Game"; // ù �������� �� �̸� (���� �ʿ�!)

    // �÷��̾� ���� (��� �� �÷��̾� ���� ���� ���� ����)
    public PlayerMove playerMove;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // �ʿ��ϴٸ� �ּ� ���� (�÷��̾ �� �̵� �� �ı����� �ʰ�)
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // ���� ���� �г� �ʱ� ��Ȱ��ȭ
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Start()
    {
        // currentHealth = maxHealth; // Start���� �ʱ�ȭ�ϸ� ������ �Ծ ���µǹǷ�, Awake���� �ʱ�ȭ�ϰų� �� �ε� �ÿ��� �ʱ�ȭ
        UpdateHealthUI();

        if (retryButton != null)
        {
            retryButton.onClick.AddListener(RestartGame);
        }

        // PlayerMove ������Ʈ ���� (���� ������Ʈ�� �ִٸ�)
        if (playerMove == null)
        {
            playerMove = GetComponent<PlayerMove>();
            if (playerMove == null)
            {
                Debug.LogError("PlayerMove component not found on this GameObject! Assign it manually or ensure it's on the same object.");
            }
        }
    }

    //==============================================================
    // ü�� UI ������Ʈ (��Ʈ �̹���)
    //==============================================================
    public void UpdateHealthUI()
    {
        if (healthHearts == null || healthHearts.Length == 0)
        {
            Debug.LogWarning("Health Hearts Images are not assigned in HealthSystem or array is empty!");
            return;
        }

        for (int i = 0; i < healthHearts.Length; i++)
        {
            if (healthHearts[i] != null)
            {
                // ���� ü�º��� ��Ʈ �ε����� ������ Ȱ��ȭ (�� �� ��Ʈ), �ƴϸ� ��Ȱ��ȭ (�� ��Ʈ)
                // �Ǵ� ���� ����: healthHearts[i].color = (i < currentHealth) ? Color.white : new Color(1, 1, 1, 0.3f);
                healthHearts[i].color = (i < currentHealth) ? Color.white : new Color(1, 1, 1, 0.3f);
                // healthHearts[i].enabled = (i < currentHealth); �ܼ��� �Ѱ� ���� ���
            }
        }
    }

    /// <summary>
    /// �÷��̾ �������� �Խ��ϴ� (��Ʈ �� ĭ).
    /// </summary>
    public void TakeDamage()
    {
        // �̹� �׾����� �� �̻� ������ ���� ����
        if (currentHealth <= 0) return;

        currentHealth--; // 1ĭ ����
        if (currentHealth < 0) currentHealth = 0; // 0 �̸����� �������� �ʰ�

        UpdateHealthUI();
        Debug.Log($"������ 1 ����. ���� ü��: {currentHealth}");

        if (currentHealth <= 0)
        {
            PlayerDied();
        }
    }

    /// <summary>
    /// �÷��̾ ü���� ȸ���մϴ� (��Ʈ �� ĭ).
    /// </summary>
    public void HealHealth()
    {
        // �̹� �ִ� ü���̸� ȸ������ ����
        if (currentHealth >= maxHealth) return;

        currentHealth++; // 1ĭ ȸ��
        if (currentHealth > maxHealth) currentHealth = maxHealth; // �ִ� ü�� �ʰ����� �ʰ�

        UpdateHealthUI();
        Debug.Log($"ü�� 1 ȸ��. ���� ü��: {currentHealth}");
    }

    /// <summary>
    /// �÷��̾ ������� �� ȣ��˴ϴ�.
    /// </summary>
    private void PlayerDied()
    {
        Debug.Log("�÷��̾� ���!");
        Time.timeScale = 0; // ���� �Ͻ� ����

        // �÷��̾� ĳ���� ���� �Ұ����ϰ�
        if (playerMove != null)
        {
            playerMove.OnDeath(); // PlayerMove�� ��� ���� ���� ȣ��
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // ���� ���� �г� Ȱ��ȭ
        }
    }

    /// <summary>
    /// ������ ������մϴ� (ù ���������� �̵�).
    /// </summary>
    public void RestartGame()
    {
        Debug.Log("���� �����!");
        Time.timeScale = 1; // �ð� �ٽ� �帣��

        // ���� ���� �г� ��Ȱ��ȭ
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // �÷��̾� ���� �ʱ�ȭ (�� �ε� ����)
        currentHealth = maxHealth; // ü�� �ʱ�ȭ
        // ManaSystem.Instance.manaPoint = ManaSystem.Instance.maxManaPoint; // ���� �ý����� �ִٸ� �ʱ�ȭ
        // PlayerInfoManager.Instance.UpdatePlayerInfoUI(); // ���� UI ������Ʈ (�ʿ��ϴٸ�)

        // ù �������� �� �ε�
        SceneManager.LoadScene(firstStageSceneName);
    }
}