using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
<<<<<<< Updated upstream
    public int health;
    public PlayerMove player;
    public GameObject[] Stage;
    public Image[] UIHealth;
    public TextMeshProUGUI UIPoint;
    public TextMeshProUGUI UIStage;
    public GameObject UIRestartBtn;
=======
    public PlayerMove player;
    public GameObject[] Stage;
    public TextMeshProUGUI UIPoint;
    public TextMeshProUGUI UIStage;

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
    }
>>>>>>> Stashed changes

    private void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
    }

    public void NextStage()
    {
        if (stageIndex < Stage.Length - 1)
        {
            Stage[stageIndex].SetActive(false);
            stageIndex++;
            Stage[stageIndex].SetActive(true);
            PlayerReposition();

            UIStage.text = "STAGE " + (stageIndex + 1);
        }
        else
        {
            Time.timeScale = 0;
            Debug.Log("Game Cleared");
<<<<<<< Updated upstream
            TextMeshProUGUI btnText = UIRestartBtn.GetComponentInChildren<TextMeshProUGUI>();
            btnText.text = "Game Clear!";
            UIRestartBtn.SetActive(true);
=======

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
>>>>>>> Stashed changes
        }

        totalPoint += stagePoint;
        stagePoint = 0;
    }

<<<<<<< Updated upstream
    public void HealthDown()
=======
    public void PlayerReposition()
>>>>>>> Stashed changes
    {
        if (health > 1)
        {
            health--;
            UIHealth[health].color = new Color(1, 0, 0, 0.4f);
        }
        else
        {
            UIHealth[0].color = new Color(1, 0, 0, 0.4f);
            player.OnDeath();
            Debug.Log("Dead");
            UIRestartBtn.SetActive(true);
        }
    }

<<<<<<< Updated upstream
    private void OnTriggerEnter2D(Collider2D collision)
=======
    // 아이템 획득 시 점수 추가 (PlayerMove에서 호출)
    public void AddStagePoint(int point)
>>>>>>> Stashed changes
    {
        if (collision.gameObject.tag == "Player")
        {
            {
                PlayerReposition();
            }
            HealthDown();
        }
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(0, 0, -1);
        player.VelocityZero();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
