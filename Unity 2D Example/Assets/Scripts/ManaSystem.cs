using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro�� ����ϱ� ���� �߰�

public class ManaSystem : MonoBehaviour
{
    public static ManaSystem Instance;

    // ���� �� UI (Image ������Ʈ)
    public Image currentManaBarFill; // ���� ä�� �κ� Image
    public TextMeshProUGUI manaText; // TextMeshPro Text UI ����

    [Header("Mana Settings")]
    public float manaPoint = 100f; // ���� ����
    public float maxManaPoint = 100f; // �ִ� ����

    [Header("Player Stats")] // �÷��̾� �ɷ�ġ ���� �߰�
    public float spellPower = 10f; // �ֹ��� (�⺻�� ����)

    // Regenerate Mana & Animation Speed
    public bool Regenerate = true;
    public float manaRegenRate = 0.1f;
    public float manaBarAnimationSpeed = 5f; // �ִϸ��̼� �ӵ� ���� (0.5���� ����)

    public bool GodMode;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        manaPoint = maxManaPoint;
        UpdateManaUI(true);
    }

    void Update()
    {
        if (Regenerate && manaPoint < maxManaPoint)
        {
            RegenMana();
        }

        if (GodMode)
        {
            RestoreMana(maxManaPoint);
        }

        UpdateManaUI(false);
    }

    private void RegenMana()
    {
        manaPoint += manaRegenRate * Time.deltaTime;
        manaPoint = Mathf.Min(manaPoint, maxManaPoint);
    }

    private void UpdateManaUI(bool instant = false)
    {
        if (currentManaBarFill != null)
        {
            float targetFillAmount = manaPoint / maxManaPoint;

            if (instant)
            {
                currentManaBarFill.fillAmount = targetFillAmount;
            }
            else
            {
                currentManaBarFill.fillAmount = Mathf.Lerp(currentManaBarFill.fillAmount, targetFillAmount, Time.deltaTime * manaBarAnimationSpeed);
            }
        }
        else
        {
            Debug.LogWarning("currentManaBarFill Image is not assigned in ManaSystem!");
        }

        if (manaText != null)
        {
            manaText.text = $"{Mathf.RoundToInt(manaPoint)} / {Mathf.RoundToInt(maxManaPoint)}";
        }
        else
        {
            Debug.LogWarning("manaText (TextMeshProUGUI) is not assigned in ManaSystem!");
        }
    }

    public bool UseMana(float Mana)
    {
        if (manaPoint >= Mana)
        {
            manaPoint -= Mana;
            if (manaPoint < 0) manaPoint = 0;
            UpdateManaUI(false);
            Debug.Log($"���� {Mana} ���. ���� ����: {manaPoint}");
            return true;
        }
        else
        {
            Debug.LogWarning($"���� ����! {Mana} �ʿ������� ���� {manaPoint} �ۿ� �����ϴ�.");
            return false;
        }
    }

    public void RestoreMana(float Mana)
    {
        manaPoint += Mana;
        if (manaPoint > maxManaPoint) manaPoint = maxManaPoint;
        UpdateManaUI(false);
        Debug.Log($"���� {Mana} ȸ��. ���� ����: {manaPoint}");
    }

    public void SetMaxMana(float max)
    {
        maxManaPoint += (int)(maxManaPoint * max / 100);
        UpdateManaUI(true);
    }

    // �ֹ��� ���� �Լ�
    public void IncreaseSpellPower(float amount)
    {
        spellPower += amount;
    }
}