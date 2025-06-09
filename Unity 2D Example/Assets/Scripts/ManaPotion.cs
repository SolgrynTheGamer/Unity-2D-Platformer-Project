using UnityEngine;

public class ManaPotion : Item // Item Ŭ���� ���
{
    public float manaRestoreAmount = 30f; // ȸ���� ���� ��

    void Awake()
    {
        itemName = "���� ����"; // ������ �̸� ����
    }

    protected override void ApplyEffect(GameObject player)
    {
        base.ApplyEffect(player); // �θ� Ŭ������ Debug.Log ȣ��

        // ManaSystem.Instance�� �ִ��� Ȯ��
        if (ManaSystem.Instance != null)
        {
            // ���� ������ �ִ� �������� ���� ���� ȸ��
            if (ManaSystem.Instance.manaPoint < ManaSystem.Instance.maxManaPoint)
            {
                ManaSystem.Instance.RestoreMana(manaRestoreAmount);
                Debug.Log($"���� {manaRestoreAmount} ȸ��!");
            }
            else
            {
                Debug.Log("������ �̹� ���� �� �ֽ��ϴ�.");
            }
        }
        else
        {
            Debug.LogError("ManaSystem.Instance�� ã�� �� �����ϴ�! �÷��̾ ManaSystem ��ũ��Ʈ�� �پ��ֳ���?");
        }
    }
}