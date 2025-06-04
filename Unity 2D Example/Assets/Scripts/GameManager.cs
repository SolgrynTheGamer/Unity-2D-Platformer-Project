using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // �̱��� �ν��Ͻ�

    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    // public int health; // HealthSystem���� �̵�
    public PlayerMove player;
    public GameObject[] Stage;
    // public Image[] UIHealth; // HealthSystem���� �̵�
    public TextMeshProUGUI UIPoint;
    public TextMeshProUGUI UIStage;
    // public GameObject UIRestartBtn; // HealthSystem�� gameOverPanel ���η� �̵�

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� �����ǰ� �Ϸ��� �ּ� ����
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (UIPoint != null) // null üũ �߰�
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

            if (UIStage != null) // null üũ �߰�
            {
                UIStage.text = "STAGE " + (stageIndex + 1);
            }
        }
        else
        {
            // ���� Ŭ���� ����
            Time.timeScale = 0; // ���� ����
            Debug.Log("Game Cleared");

            // Restart ��ư�� HealthSystem�� GameOverPanel�� �����Ƿ�,
            // ���⼭ ���� �����ϴ� ��� HealthSystem�� GameOverPanel�� Ȱ��ȭ�ϵ��� �� �� �ֽ��ϴ�.
            // ������ ���� Ŭ���� �޽����� ������ �����ϴ� ���� �����ϴ�.
            if (HealthSystem.Instance != null && HealthSystem.Instance.gameOverPanel != null)
            {
                // HealthSystem�� ���� ���� �г��� ��Ȱ���Ͽ� Ŭ���� �޽��� ǥ��
                HealthSystem.Instance.gameOverPanel.SetActive(true);
                Button retryBtn = HealthSystem.Instance.retryButton;
                if (retryBtn != null)
                {
                    TextMeshProUGUI btnText = retryBtn.GetComponentInChildren<TextMeshProUGUI>();
                    if (btnText != null)
                    {
                        btnText.text = "Game Clear!"; // ��ư �ؽ�Ʈ ����
                    }
                    retryBtn.onClick.RemoveAllListeners(); // ���� ������ ����
                    retryBtn.onClick.AddListener(() => SceneManager.LoadScene(HealthSystem.Instance.firstStageSceneName)); // ù ������ �̵�
                }
            }
        }

        totalPoint += stagePoint;
        stagePoint = 0;
    }

    // HealthDown ������ HealthSystem���� �̵�
    // public void HealthDown() { ... }

    // OnTriggerEnter2D ������ PlayerMove���� DeadZone �±׷� ���� ó��

    public void PlayerReposition()
    {
        if (player != null) // null üũ �߰�
        {
            player.transform.position = new Vector3(0, 0, -1); // �÷��̾� ���� ��ġ (���� �ʿ�)
            player.VelocityZero();
        }
    }

    // Restart ������ HealthSystem���� �̵�
    // public void Restart() { ... }

    // ������ ȹ�� �� ���� �߰� (PlayerMove���� ȣ��)
    public void AddStagePoint(int point)
    {
        stagePoint += point;
    }
}