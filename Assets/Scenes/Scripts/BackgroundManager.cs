using System.Collections;
using UnityEngine;

/// <summary>
/// Quản lý chuyển đổi background theo level.
/// Gắn script này vào bất kỳ GameObject nào trong scene (ví dụ: GameManager hoặc một empty GameObject tên "BackgroundManager").
///
/// Cách dùng:
///   - Kéo 3 GameObject background (Quad) vào 3 field tương ứng trong Inspector.
///   - Mỗi khi level thay đổi, script tự bật đúng background và tắt 2 cái còn lại.
///
/// Mapping mặc định:
///   Level 1–3  → bgFar   (bg_far  — không gian sâu, yên tĩnh)
///   Level 4–6  → bgMid   (bg_mid  — tinh vân tím/xanh)
///   Level 7+   → bgNear  (bg_near — vùng nguy hiểm, đầy asteroid)
/// </summary>
public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance { get; private set; }

    [Header("Background GameObjects (kéo 3 Quad vào đây)")]
    [Tooltip("bg_far  — Level 1–3: Không gian sâu thẳm, yên tĩnh")]
    public GameObject bgFar;

    [Tooltip("bg_mid  — Level 4–6: Tinh vân tím/xanh, hành tinh")]
    public GameObject bgMid;

    [Tooltip("bg_near — Level 7+ : Vùng nguy hiểm, đầy asteroid")]
    public GameObject bgNear;

    [Header("Ngưỡng level để chuyển background")]
    [Tooltip("Level bắt đầu dùng bg_mid (mặc định: 4)")]
    public int levelForMid = 4;

    [Tooltip("Level bắt đầu dùng bg_near (mặc định: 7)")]
    public int levelForNear = 7;

    [Header("Hiệu ứng chuyển cảnh")]
    [Tooltip("Thời gian fade khi chuyển background (giây). Đặt 0 để tắt fade.")]
    public float fadeDuration = 1.2f;

    // --- Private ---
    private int lastLevel = -1;
    private GameObject currentBg;
    private Coroutine fadeCoroutine;

    // -------------------------------------------------------
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Tắt hết 3 background trước
        SetActiveImmediate(bgFar,  false);
        SetActiveImmediate(bgMid,  false);
        SetActiveImmediate(bgNear, false);

        // Bật background tương ứng level ban đầu (không fade)
        GameObject startBg = GetBgForLevel(1);
        SetActiveImmediate(startBg, true);
        currentBg = startBg;
        lastLevel = 1;
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;

        int level = GameManager.Instance.Level;
        if (level == lastLevel) return;

        lastLevel = level;
        SwitchBackground(GetBgForLevel(level));
    }

    // -------------------------------------------------------
    /// <summary>Trả về GameObject background phù hợp với level.</summary>
    private GameObject GetBgForLevel(int level)
    {
        if (level >= levelForNear && bgNear != null) return bgNear;
        if (level >= levelForMid  && bgMid  != null) return bgMid;
        return bgFar;
    }

    /// <summary>Chuyển sang background mới. Nếu đã là cái đang hiện thì bỏ qua.</summary>
    private void SwitchBackground(GameObject nextBg)
    {
        if (nextBg == null || nextBg == currentBg) return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        if (fadeDuration > 0f)
            fadeCoroutine = StartCoroutine(FadeRoutine(currentBg, nextBg));
        else
            SwapImmediate(currentBg, nextBg);
    }

    // -------------------------------------------------------
    /// <summary>Đổi ngay không có fade.</summary>
    private void SwapImmediate(GameObject hide, GameObject show)
    {
        SetActiveImmediate(hide, false);
        SetActiveImmediate(show, true);
        currentBg = show;
    }

    /// <summary>Fade-out background cũ → bật background mới → fade-in.</summary>
    private IEnumerator FadeRoutine(GameObject oldBg, GameObject newBg)
    {
        float half = fadeDuration * 0.5f;

        // --- Fade OUT background cũ ---
        Renderer oldRenderer = oldBg != null ? oldBg.GetComponent<Renderer>() : null;
        if (oldRenderer != null)
        {
            Color c = oldRenderer.material.color;
            float t = 0f;
            while (t < half)
            {
                t += Time.deltaTime;
                float a = Mathf.Lerp(1f, 0f, t / half);
                oldRenderer.material.color = new Color(c.r, c.g, c.b, a);
                yield return null;
            }
            oldRenderer.material.color = new Color(c.r, c.g, c.b, 0f);
        }

        // --- Đổi active ---
        SetActiveImmediate(oldBg, false);

        // Reset alpha về 1 để lần sau hiện đúng
        if (oldRenderer != null)
        {
            Color c = oldRenderer.material.color;
            oldRenderer.material.color = new Color(c.r, c.g, c.b, 1f);
        }

        SetActiveImmediate(newBg, true);
        currentBg = newBg;

        // --- Fade IN background mới ---
        Renderer newRenderer = newBg != null ? newBg.GetComponent<Renderer>() : null;
        if (newRenderer != null)
        {
            Color c = newRenderer.material.color;
            float t = 0f;
            newRenderer.material.color = new Color(c.r, c.g, c.b, 0f);
            while (t < half)
            {
                t += Time.deltaTime;
                float a = Mathf.Lerp(0f, 1f, t / half);
                newRenderer.material.color = new Color(c.r, c.g, c.b, a);
                yield return null;
            }
            newRenderer.material.color = new Color(c.r, c.g, c.b, 1f);
        }

        fadeCoroutine = null;
    }

    // -------------------------------------------------------
    private static void SetActiveImmediate(GameObject go, bool active)
    {
        if (go != null) go.SetActive(active);
    }

    private void OnDisable()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
    }
}
