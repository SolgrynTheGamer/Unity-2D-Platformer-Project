using UnityEngine;

// 모든 아이템이 상속받을 기반 클래스
public abstract class Item : MonoBehaviour
{
    public string itemName = "Generic Item"; // 아이템 이름 (디버깅용)

    // 아이템이 플레이어와 상호작용할 때 호출될 가상 메서드 (자식 클래스에서 재정의)
    protected virtual void ApplyEffect(GameObject player)
    {
        Debug.Log($"{itemName} 효과 발동!");
    }

    // 플레이어가 아이템 영역에 들어왔을 때 (트리거)
    void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어 태그 확인
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Item Triggered: {itemName}"); // 디버그 로그 추가
            ApplyEffect(other.gameObject); // 효과 적용
            Destroy(gameObject); // 아이템 사용 후 자신을 파괴
        }
    }

    // 아이템이 맵에 배치될 때 충돌 감지를 위해 Collider2D와 Rigidbody2D 필요
    // Collider2D: Is Trigger를 체크하여 통과할 수 있게 함
    // Rigidbody2D: Is Kinematic을 체크하여 물리적 움직임 없게 함 (트리거 감지에 필요)
    void Reset() // 컴포넌트 추가 시 자동으로 설정되도록
    {
        if (GetComponent<Collider2D>() == null)
        {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>(); // BoxCollider2D
            collider.isTrigger = true; // 통과 가능하게 설정

            // CircleCollider2D
            // CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
            // collider.isTrigger = true;
            // collider.radius = 0.5f;
        }

        // Rigidbody2D 확인 및 추가
        if (GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic; // 물리적 움직임 없게 함 (Kinematic 설정)
            rb.gravityScale = 0; // 중력 영향 없게 함
        }
    }
}