using UnityEngine;

public class SpellPowerScroll : Item // Item Ŭ���� ���
{
    public float spellPowerIncreaseAmount = 5f; // ������ų �ֹ��� ��

    void Awake()
    {
        itemName = "�ֹ��� ��ũ��"; // ������ �̸� ����
    }

    protected override void ApplyEffect(GameObject player)
    {
        base.ApplyEffect(player); // �θ� Ŭ������ Debug.Log ȣ��

        if (ManaSystem.Instance != null)
        {
            ManaSystem.Instance.IncreaseSpellPower(spellPowerIncreaseAmount);
            Debug.Log($"�ֹ��� {spellPowerIncreaseAmount} ����!");

            if (PlayerInfoManager.Instance != null && PlayerInfoManager.Instance.playerInfoPanel.activeSelf)
            {
                PlayerInfoManager.Instance.UpdatePlayerInfoUI();
            }
        }
        else
        {
            Debug.LogError("ManaSystem.Instance (or PlayerStats.Instance)�� ã�� �� �����ϴ�! �÷��̾ ManaSystem ��ũ��Ʈ�� �پ��ֳ���?");
        }
    }
}