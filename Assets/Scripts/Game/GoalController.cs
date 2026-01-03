using UnityEngine;

public class GoalController : MonoBehaviour
{
    private GameController gameController;
    private bool hasReached = false;

    public void Initialize(GameController controller)
    {
        gameController = controller;
    }

    private void Update()
    {
        if(hasReached) return;
        if(gameController==null) return;

        Move();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasReached) return;
        
        if (other.GetComponent<PlayerController>() != null)
        {
            hasReached = true;
            OnGoalReached();
        }
    }

    private void Move()
    {
        // GameControllerから現在のプレイヤー速度を取得して左に移動
        transform.position += Vector3.left * gameController.CurrentPlayerSpeed * Time.deltaTime;    
    }

    private void OnGoalReached()
    {
        if(gameController == null) return;
        float elapsedTime = gameController.ElapsedTime;
        Debug.Log($"ゴール到達！経過時間: {elapsedTime:F2}秒");
        // ゲームクリアを通知
        gameController.SetGameClear();
    }
}
