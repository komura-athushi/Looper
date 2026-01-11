using UnityEngine;
using TMPro;

public class DistanceView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI distanceText;
    private GameController gameController;

    private void Start()
    {
        gameController = FindFirstObjectByType<GameController>();
    }

    private void Update()
    {
        if(gameController == null) return;

        int value = (int)gameController.RemainingDistance;
        distanceText.text = $"Goal in {value}";
    }
}
