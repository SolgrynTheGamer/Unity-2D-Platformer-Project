using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public PlayerMove player;

    public void NextStage()
    {
        stageIndex++;
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
        }
        else
        {
            player.OnDeath();
            Debug.Log("Dead");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            HealthDown();

            collision.attachedRigidbody.linearVelocity = Vector2.zero;
            collision.transform.position = new Vector3(0, 0, -1);
        }
    }
}
