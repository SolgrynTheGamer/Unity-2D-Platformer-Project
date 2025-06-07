using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoManager : MonoBehaviour
{
    public static PlayerInfoManager Instance; // �̱��� �ν��Ͻ� �߰�

    public GameObject playerInfoPanel;
    public TextMeshProUGUI maxManaText;
    public TextMeshProUGUI spellPowerText;

    private bool isPanelOpen = false;

    // Awake���� �̱��� �ʱ�ȭ
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // �ʿ��ϴٸ� �� ��ȯ �� �����ǵ���
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (playerInfoPanel != null)
        {
            playerInfoPanel.SetActive(false);
        }
    }

    public void TogglePlayerInfoPanel()
    {
        isPanelOpen = !isPanelOpen;
        playerInfoPanel.SetActive(isPanelOpen);

        if (isPanelOpen)
        {
            UpdatePlayerInfoUI();
        }
    }

    public void UpdatePlayerInfoUI()
    {
        if (ManaSystem.Instance != null)
        {
            if (maxManaText != null)
            {
                maxManaText.text = $"{ManaSystem.Instance.maxManaPoint}";
            }
            if (spellPowerText != null)
            {
                spellPowerText.text = $"{ManaSystem.Instance.spellPower}";
            }
        }
        else
        {
            Debug.LogWarning("ManaSystem.Instance (or PlayerStats.Instance) not found in the scene.");
        }
    }
}