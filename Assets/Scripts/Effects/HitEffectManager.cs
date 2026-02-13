using UnityEngine;
using TMPro;

public class HitEffectManager : MonoBehaviour
{
    public static HitEffectManager Instance { get; private set; }

    [Header("Prefab (auto-created if null)")]
    public GameObject damagePopupPrefab;

    [Header("Screen Shake")]
    public float screenShakeAmount = 0.15f;
    public float screenShakeDuration = 0.1f;

    Camera mainCam;
    Vector3 originalCamPos;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        mainCam = Camera.main;
        if (mainCam != null)
        {
            originalCamPos = mainCam.transform.position;
        }

        if (damagePopupPrefab == null)
        {
            damagePopupPrefab = CreateDamagePopupPrefab();
        }
    }

    GameObject CreateDamagePopupPrefab()
    {
        GameObject go = new GameObject("DamagePopup_Template");
        go.SetActive(false);

        TextMeshPro tmp = go.AddComponent<TextMeshPro>();
        tmp.fontSize = 6;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.sortingOrder = 100;

        go.AddComponent<DamagePopup>();

        go.transform.SetParent(transform);
        return go;
    }

    public void SpawnDamagePopup(float damage, Vector3 position)
    {
        if (damagePopupPrefab == null) return;

        GameObject popup = Instantiate(damagePopupPrefab, position, Quaternion.identity);
        popup.SetActive(true);

        DamagePopup dp = popup.GetComponent<DamagePopup>();
        if (dp != null)
        {
            dp.Setup(damage, position);
        }

        StartCoroutine(ScreenShake());
    }

    System.Collections.IEnumerator ScreenShake()
    {
        if (mainCam == null) yield break;

        float elapsed = 0f;

        while (elapsed < screenShakeDuration)
        {
            float x = originalCamPos.x + Random.Range(-screenShakeAmount, screenShakeAmount);
            float y = originalCamPos.y + Random.Range(-screenShakeAmount, screenShakeAmount);
            mainCam.transform.position = new Vector3(x, y, originalCamPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCam.transform.position = originalCamPos;
    }
}
