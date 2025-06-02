using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoManager : MonoBehaviour
{
    public GameObject playerInfoPanel; // �÷��̾� ����â Panel
    public TextMeshProUGUI maxManaText; // �ִ� ���� ǥ�ÿ� TextMeshProUGUI
    public TextMeshProUGUI spellPowerText; // �ֹ��� ǥ�ÿ� TextMeshProUGUI
    // ���� �߰��� �������� ���� TextMeshProUGUI ������:
    // public TextMeshProUGUI levelText;
    // public TextMeshProUGUI strengthText;

    private bool isPanelOpen = false;

    void Start()
    {
        // ���� �� ����â�� ��Ȱ��ȭ
        if (playerInfoPanel != null)
        {
            playerInfoPanel.SetActive(false);
        }
    }

    /// "�÷��̾�" ��ư Ŭ�� �� ȣ��˴ϴ�. ����â�� ����մϴ�.
    public void TogglePlayerInfoPanel()
    {
        isPanelOpen = !isPanelOpen;
        playerInfoPanel.SetActive(isPanelOpen);

        if (isPanelOpen)
        {
            UpdatePlayerInfoUI(); // â�� ���� �� ������ �ֽ�ȭ
        }
    }

    /// �÷��̾� ������ UI�� ������Ʈ�մϴ�.
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
            // ���� �߰��� �������� ������Ʈ�ϴ� ����:
            // if (levelText != null) levelText.text = $"����: {PlayerStats.Instance.Level}";
            // if (strengthText != null) strengthText.text = $"��: {PlayerStats.Instance.Strength}";
        }
        else
        {
            Debug.LogWarning("ManaSystem.Instance (or PlayerStats.Instance) not found in the scene.");
        }
    }
}