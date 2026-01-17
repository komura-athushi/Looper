using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public static FadeController Instance { get; private set; }
    
    [SerializeField] private Image blackBackground;
    [SerializeField] private float fadeDuration = 2f;
    
    private bool isFading = false;
    private bool isFadingOut = false;
    private bool isFadingIn = false;
    private float fadeTimer = 0f;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetAlpha(0f);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Update()
    {
        if (!isFading) return;
        
        fadeTimer += Time.unscaledDeltaTime;
        float t = Mathf.Clamp01(fadeTimer / fadeDuration);
        
        if (isFadingOut)
        {
            float alpha = Mathf.Lerp(0f, 1f, t);
            SetAlpha(alpha);
            
            if (t >= 1f)
            {
                isFading = false;
            }
        }
        else if (isFadingIn)
        {
            float alpha = Mathf.Lerp(1f, 0f, t);
            SetAlpha(alpha);
            
            if (t >= 1f)
            {
                isFading = false;
            }
        }
    }
    
    public void StartFadeOut()
    {
        isFading = true;
        isFadingOut = true;
        isFadingIn = false;
        fadeTimer = 0f;
        SetAlpha(0f);
    }
    
    public bool IsFadeOut()
    {
        return isFadingOut && !isFading;
    }
    
    public void StartFadeIn()
    {
        isFading = true;
        isFadingIn = true;
        isFadingOut = false;
        fadeTimer = 0f;
        SetAlpha(1f);
    }
    
    public bool IsFadeIn()
    {
        return isFadingIn && !isFading;
    }
    
    private void SetAlpha(float alpha)
    {
        Color color = blackBackground.color;
        color.a = alpha;
        blackBackground.color = color;
    }
}
