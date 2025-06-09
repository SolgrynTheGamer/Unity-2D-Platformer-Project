using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    public static HealthSystem Instance;

    // 체력 바 UI (Image 컴포넌트)
    public Image[] healthHearts; // 하트 이미지 배열 (UI에 하트 개수만큼 연결)

    [Header("Health Settings")]
    public int currentHealth = 3; // 현재 체력 (하트 3개 기준)
    public int maxHealth = 3; // 최대 체력 (하트 최대 3개)

    [Header("Game Over UI")]
    public GameObject gameOverPanel; // 게임 오버 시 활성화될 Panel
    public Button retryButton; // 재시작 버튼
    public string firstStageSceneName = "Game";

    // 플레이어 참조 (사망 시 플레이어 조작 중지 등을 위해)
    public PlayerMove playerMove;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // 게임 오버 패널 초기 비활성화
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Start()
    {
        UpdateHealthUI();

        if (retryButton != null)
        {
            retryButton.onClick.AddListener(RestartGame);
        }

        if (playerMove == null)
        {
            playerMove = GetComponent<PlayerMove>();
            if (playerMove == null)
            {
                Debug.LogError("PlayerMove component not found on this GameObject! Assign it manually or ensure it's on the same object.");
            }
        }
    }

    // 체력 UI 업데이트 (하트 이미지)
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
                healthHearts[i].color = (i < currentHealth) ? Color.white : new Color(1, 1, 1, 0.3f);
                // healthHearts[i].enabled = (i < currentHealth); 단순히 켜고 끄는 방식
            }
        }
    }

    /// 플레이어가 데미지를 입습니다 (하트 한 칸).
    public void TakeDamage()
    {
        // 이미 죽었으면 더 이상 데미지 입지 않음
        if (currentHealth <= 0) return;

        currentHealth--; // 1칸 감소
        if (currentHealth < 0) currentHealth = 0; // 0 미만으로 내려가지 않게

        UpdateHealthUI();
        Debug.Log($"데미지 1 입음. 남은 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            PlayerDied();
        }
    }

    /// 플레이어가 체력을 회복합니다 (하트 한 칸).
    public void HealHealth()
    {
        // 이미 최대 체력이면 회복하지 않음
        if (currentHealth >= maxHealth) return;

        currentHealth++; // 1칸 회복
        if (currentHealth > maxHealth) currentHealth = maxHealth; // 최대 체력 초과하지 않게

        UpdateHealthUI();
        Debug.Log($"체력 1 회복. 현재 체력: {currentHealth}");
    }

    //플레이어가 사망했을 때 호출됩니다.
    private void PlayerDied()
    {
        Debug.Log("플레이어 사망!");
        Time.timeScale = 0; // 게임 일시 정지

        // 플레이어 캐릭터 조작 불가능하게
        if (playerMove != null)
        {
            playerMove.OnDeath(); // PlayerMove의 사망 관련 로직 호출
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // 게임 오버 패널 활성화
        }
    }

    // 게임을 재시작합니다 (첫 스테이지로 이동).
    public void RestartGame()
    {
        Debug.Log("게임 재시작!");
        Time.timeScale = 1; // 시간 다시 흐르게

        // 게임 오버 패널 비활성화
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // 플레이어 스탯 초기화 (씬 로드 전에)
        currentHealth = maxHealth; // 체력 초기화
        // ManaSystem.Instance.manaPoint = ManaSystem.Instance.maxManaPoint; // 마나 시스템 초기화

        // 첫 스테이지 씬 로드
        SceneManager.LoadScene(firstStageSceneName);
    }
}