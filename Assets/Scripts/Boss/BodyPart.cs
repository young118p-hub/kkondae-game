using UnityEngine;
using UnityEngine.Events;

public enum BodyPartType
{
    Head,
    Cheek,
    Hand,
    Calf,
    Torso,
    Butt
}

public class BodyPart : MonoBehaviour
{
    [Header("Settings")]
    public BodyPartType partType;
    public float baseDamage = 10f;
    public float damageVariance = 3f;

    [Header("Hit Feel")]
    public float hitShakeAmount = 0.1f;
    public float hitShakeDuration = 0.1f;
    public Color hitFlashColor = Color.red;

    [Header("Events")]
    public UnityEvent<float, Vector3> onHit;

    BossCharacter boss;
    SpriteRenderer spriteRenderer;
    Color originalColor;

    void Awake()
    {
        boss = GetComponentInParent<BossCharacter>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void OnMouseDown()
    {
        Debug.Log($"[BodyPart] {partType} clicked!");

        if (GameManager.Instance != null && !GameManager.Instance.IsPlaying) return;

        Hit();
    }

    public void Hit()
    {
        float damage = baseDamage + Random.Range(-damageVariance, damageVariance);
        damage = Mathf.Max(1f, damage);

        Vector3 hitPos = transform.position;

        if (boss != null)
        {
            boss.TakeDamage(damage, partType.ToString());
        }

        onHit?.Invoke(damage, hitPos);

        StartCoroutine(HitFlash());
        StartCoroutine(HitShake());
    }

    System.Collections.IEnumerator HitFlash()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = hitFlashColor;
            yield return new WaitForSeconds(0.08f);
            spriteRenderer.color = originalColor;
        }
    }

    System.Collections.IEnumerator HitShake()
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < hitShakeDuration)
        {
            float x = originalPos.x + Random.Range(-hitShakeAmount, hitShakeAmount);
            float y = originalPos.y + Random.Range(-hitShakeAmount, hitShakeAmount);
            transform.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
