using UnityEngine;
using System;

public class StartPlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 100f;
    
    private RectTransform rectTransform;
    private bool reachedEdge = false;
    private float screenEdgeX;
    
    public event Action OnReachedEdge;
    
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            screenEdgeX = canvasRect.rect.width;
        }
    }
    
    void Update()
    {   
        rectTransform.anchoredPosition += Vector2.right * moveSpeed * Time.deltaTime;
        
        if (!reachedEdge && rectTransform.anchoredPosition.x >= screenEdgeX)
        {
            reachedEdge = true;
            OnReachedEdge?.Invoke();
        }
    }
}
