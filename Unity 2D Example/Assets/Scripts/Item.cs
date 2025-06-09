using UnityEngine;

// ��� �������� ��ӹ��� ��� Ŭ����
public abstract class Item : MonoBehaviour
{
    public string itemName = "Generic Item"; // ������ �̸� (������)

    protected virtual void ApplyEffect(GameObject player)
    {
        Debug.Log($"{itemName} ȿ�� �ߵ�!");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾� �±� Ȯ��
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Item Triggered: {itemName}"); // ����� �α� �߰�
            ApplyEffect(other.gameObject); // ȿ�� ����
            Destroy(gameObject); // ������ ��� �� �ڽ��� �ı�
        }
    }

    // �������� �ʿ� ��ġ�� �� �浹 ������ ���� Collider2D�� Rigidbody2D �ʿ�
    // Collider2D: Is Trigger�� üũ�Ͽ� ����� �� �ְ� ��
    // Rigidbody2D: Is Kinematic�� üũ�Ͽ� ������ ������ ���� �� (Ʈ���� ������ �ʿ�)
    void Reset()
    {
        if (GetComponent<Collider2D>() == null)
        {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true; // ��� �����ϰ� ����
            // collider.size = new Vector2(0.5f, 0.5f); // ������ �̹��� ũ�⿡ ���� ����
            // collider.offset = new Vector2(0, 0); // ��ġ ����
        }

        // Rigidbody2D Ȯ�� �� �߰�
        if (GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic; // ������ ������ ���� �� (Kinematic ����)
            rb.gravityScale = 0; // �߷� ���� ���� ��
        }
    }
}