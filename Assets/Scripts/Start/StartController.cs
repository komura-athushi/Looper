using UnityEngine;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    [SerializeField] private StartPlayerController playerController;
    [SerializeField] private float delayBeforeFade = 1f;
    [SerializeField] private string gameSceneName = "GameScene";
    
    private float waitTimer = 0f;
    private bool isWaiting = false;
    private bool isFading = false;
    
    void Start()
    {
        playerController.OnReachedEdge += OnPlayerReachedEdge;

        AudioManager.Instance.PlaySE("GameStart");
    }
    
    void Update()
    {
        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= delayBeforeFade)
            {
                isWaiting = false;
                isFading = true;
                FadeController.Instance.StartFadeOut();
            }
        }
        
        if (isFading && FadeController.Instance.IsFadeOut())
        {
            SceneManager.LoadScene(gameSceneName);
        }
    }
    
    private void OnPlayerReachedEdge()
    {
        isWaiting = true;
        waitTimer = 0f;
    }
    
    void OnDestroy()
    {
        if (playerController != null)
        {
            playerController.OnReachedEdge -= OnPlayerReachedEdge;
        }
    }
}
