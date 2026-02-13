using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [Header("Settings")]
    public float floatSpeed = 2f;
    public float fadeSpeed = 1.5f;
    public float lifetime = 0.8f;
    public float scaleUpAmount = 1.3f;

    TextMeshPro textMesh;
    float timer;
    Vector3 moveDir;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void Setup(float damage, Vector3 position)
    {
        transform.position = position + new Vector3(Random.Range(-0.3f, 0.3f), 0.5f, 0f);
        textMesh.text = Mathf.RoundToInt(damage).ToString();

        if (damage >= 15f)
        {
            textMesh.color = new Color(1f, 0.2f, 0f);
            textMesh.fontSize = 8f;
            transform.localScale = Vector3.one * scaleUpAmount;
        }
        else
        {
            textMesh.color = Color.white;
            textMesh.fontSize = 6f;
            transform.localScale = Vector3.one;
        }

        moveDir = new Vector3(Random.Range(-0.5f, 0.5f), 1f, 0f).normalized;
        timer = lifetime;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        transform.position += moveDir * floatSpeed * Time.deltaTime;

        if (textMesh != null)
        {
            Color c = textMesh.color;
            c.a = Mathf.Lerp(0f, 1f, timer / lifetime);
            textMesh.color = c;
        }

        float scaleT = timer / lifetime;
        transform.localScale = Vector3.one * Mathf.Lerp(0.5f, 1f, scaleT);

        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
