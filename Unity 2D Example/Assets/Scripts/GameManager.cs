using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // 싱글톤 인스턴스

    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    // public int health; // HealthSystem으로 이동
    public PlayerMove player;
    public GameObject[] Stage;
    // public Image[] UIHealth; // HealthSystem으로 이동
    public TextMeshProUGUI UIPoint;
    public TextMeshProUGUI UIStage;
    // public GameObject UIRestartBtn; // HealthSystem의 gameOverPanel 내부로 이동

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지되게 하려면 주석 해제
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (UIPoint != null) // null 체크 추가
        {
            UIPoint.text = (totalPoint + stagePoint).ToString();
        }
    }

    public void NextStage()
    {
        if (stageIndex < Stage.Length - 1)
        {
            Stage[stageIndex].SetActive(false);
            stageIndex++;
            Stage[stageIndex].SetActive(true);
            PlayerReposition();

            if (UIStage != null) // null 체크 추가
            {
                UIStage.text = "STAGE " + (stageIndex + 1);
            }
        }
        else
        {
            // 게임 클리어 로직
            Time.timeScale = 0; // 게임 정지
            Debug.Log("Game Cleared");

            // Restart 버튼이 HealthSystem의 GameOverPanel에 있으므로,
            // 여기서 직접 제어하는 대신 HealthSystem의 GameOverPanel을 활성화하도록 할 수 있습니다.
            // 하지만 게임 클리어 메시지는 별도로 관리하는 것이 좋습니다.
            if (HealthSystem.Instance != null && HealthSystem.Instance.gameOverPanel != null)
            {
                // HealthSystem의 게임 오버 패널을 재활용하여 클리어 메시지 표시
                HealthSystem.Instance.gameOverPanel.SetActive(true);
                Button retryBtn = HealthSystem.Instance.retryButton;
                if (retryBtn != null)
                {
                    TextMeshProUGUI btnText = retryBtn.GetComponentInChildren<TextMeshProUGUI>();
                    if (btnText != null)
                    {
                        btnText.text = "Game Clear!"; // 버튼 텍스트 변경
                    }
                    retryBtn.onClick.RemoveAllListeners(); // 기존 리스너 제거
                    retryBtn.onClick.AddListener(() => SceneManager.LoadScene(HealthSystem.Instance.firstStageSceneName)); // 첫 씬으로 이동
                }
            }
        }

        totalPoint += stagePoint;
        stagePoint = 0;
    }

    // HealthDown 로직은 HealthSystem으로 이동
    // public void HealthDown() { ... }

    // OnTriggerEnter2D 로직은 PlayerMove에서 DeadZone 태그로 직접 처리

    public void PlayerReposition()
    {
        if (player != null) // null 체크 추가
        {
            player.transform.position = new Vector3(0, 0, -1); // 플레이어 시작 위치 (조절 필요)
            player.VelocityZero();
        }
    }

    // Restart 로직은 HealthSystem으로 이동
    // public void Restart() { ... }

    // 아이템 획득 시 점수 추가 (PlayerMove에서 호출)
    public void AddStagePoint(int point)
    {
        stagePoint += point;
    }
}