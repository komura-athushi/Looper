using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーのライフ数に応じてマークを動的に生成・削除する
/// </summary>
public class LifeView : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Image lifeMarkPrefab;     // ライフマークのプレハブ（Image）

    private int currentLifeDisplay = 0;

    private void Start()
    {
        // PlayerControllerのイベントにサブスクライブ
        if (playerController != null)
        {
            playerController.OnHPChanged += UpdateHPDisplay;
            // 初期表示を設定
            UpdateHPDisplay(playerController.GetCurrentHP());
        }
    }

    private void OnDestroy()
    {
        // イベントのリスナーを削除（メモリリーク防止）
        if (playerController != null)
        {
            playerController.OnHPChanged -= UpdateHPDisplay;
        }
    }

    /// <summary>
    /// HP表示を更新する
    /// </summary>
    private void UpdateHPDisplay(int newHP)
    {
        // 表示するマークの数を増やす場合
        while (currentLifeDisplay < newHP)
        {
            AddLifeMark();
            currentLifeDisplay++;
        }

        // 表示するマークの数を減らす場合
        while (currentLifeDisplay > newHP)
        {
            RemoveLifeMark();
            currentLifeDisplay--;
        }
    }

    /// <summary>
    /// ライフマークを1つ追加
    /// </summary>
    private void AddLifeMark()
    {
        Image newMark = Instantiate(lifeMarkPrefab, transform);
        newMark.gameObject.SetActive(true);
    }

    /// <summary>
    /// ライフマークを1つ削除
    /// </summary>
    private void RemoveLifeMark()
    {
        if (transform.childCount > 0)
        {
            Transform lastMark = transform.GetChild(transform.childCount - 1);
            Destroy(lastMark.gameObject);
        }
    }
}
