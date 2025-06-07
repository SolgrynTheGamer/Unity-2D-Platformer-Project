using UnityEngine;

public class HealthPotion : Item // Item Ŭ���� ���
{
    // public int healAmount = 1; // ȸ���� ��Ʈ ĭ ��

    void Awake()
    {
        itemName = "��Ʈ ȸ�� ����"; // ������ �̸� ����
    }

    protected override void ApplyEffect(GameObject player)
    {
        base.ApplyEffect(player); // �θ� Ŭ������ Debug.Log ȣ��

        // HealthSystem.Instance�� �ִ��� Ȯ��
        if (HealthSystem.Instance != null)
        {
            // ���� ü���� �ִ� ü�º��� ���� ���� ȸ��
            if (HealthSystem.Instance.currentHealth < HealthSystem.Instance.maxHealth)
            {
                HealthSystem.Instance.HealHealth();
                Debug.Log($"��Ʈ 1 ĭ ȸ��!");
            }
            else
            {
                Debug.Log("ü���� �̹� ���� �� �ֽ��ϴ�.");
            }
        }
        else
        {
            Debug.LogError("HealthSystem.Instance�� ã�� �� �����ϴ�! �÷��̾ HealthSystem ��ũ��Ʈ�� �پ��ֳ���?");
        }
    }
}