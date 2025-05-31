using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoManager : MonoBehaviour
{
    public GameObject playerInfoPanel; // 플레이어 정보창 Panel
    public TextMeshProUGUI maxManaText; // 최대 마나 표시용 TextMeshProUGUI
    public TextMeshProUGUI spellPowerText; // 주문력 표시용 TextMeshProUGUI
    // 추후 추가될 정보들을 위한 TextMeshProUGUI 변수들:
    // public TextMeshProUGUI levelText;
    // public TextMeshProUGUI strengthText;

    private bool isPanelOpen = false;

    void Start()
    {
        // 시작 시 정보창은 비활성화
        if (playerInfoPanel != null)
        {
            playerInfoPanel.SetActive(false);
        }
    }

    /// "플레이어" 버튼 클릭 시 호출됩니다. 정보창을 토글합니다.
    public void TogglePlayerInfoPanel()
    {
        isPanelOpen = !isPanelOpen;
        playerInfoPanel.SetActive(isPanelOpen);

        if (isPanelOpen)
        {
            UpdatePlayerInfoUI(); // 창이 열릴 때 정보를 최신화
        }
    }

    /// 플레이어 정보를 UI에 업데이트합니다.
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
            // 추후 추가될 정보들을 업데이트하는 로직:
            // if (levelText != null) levelText.text = $"레벨: {PlayerStats.Instance.Level}";
            // if (strengthText != null) strengthText.text = $"힘: {PlayerStats.Instance.Strength}";
        }
        else
        {
            Debug.LogWarning("ManaSystem.Instance (or PlayerStats.Instance) not found in the scene.");
        }
    }
}