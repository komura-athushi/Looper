using UnityEngine;
using TMPro;

// TextProMeshの親Panelにアタッチ、TextProMeshの数字を制御する
public class CountNumberController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countText;
    
    private void Awake()
    {
        // TextMeshProUGUIコンポーネントが設定されていない場合、自動で取得を試行
        if (countText == null)
        {
            countText = GetComponentInChildren<TextMeshProUGUI>();
            if (countText != null)
            {
                Debug.Log($"[CountNumberController] TextMeshProを自動取得しました: {countText.name}");
            }
            else
            {
                Debug.LogError("[CountNumberController] TextMeshProUGUIが見つかりません！");
            }
        }
        else
        {
            Debug.Log($"[CountNumberController] TextMeshProが設定済み: {countText.name}");
        }
        
        // 初期状態では空文字で表示状態を維持
        if (countText != null)
        {
            countText.text = "";
            Debug.Log($"[CountNumberController] 初期化完了 - GameObject: {countText.gameObject.name}, Active: {countText.gameObject.activeInHierarchy}");
        }
    }
    
    /// <summary>
    /// 指定した数字を画面に表示
    /// </summary>
    /// <param name="number">表示する数字</param>
    public void ShowNumber(int number)
    {
        if (countText != null)
        {
            countText.text = number.ToString();
            Debug.Log($"[CountNumberController] 数字を設定: {number}, Text: '{countText.text}', GameObject: {countText.gameObject.name}, Active: {countText.gameObject.activeInHierarchy}, Enabled: {countText.enabled}");
        }
        else
        {
            Debug.LogError("[CountNumberController] countTextがnullです！");
        }
    }
    
    /// <summary>
    /// 数字を非表示にする
    /// </summary>
    public void HideNumber()
    {
        if (countText != null)
        {
            countText.text = "";
        }
        Debug.Log("[CountNumberController] 数字を非表示");
    }
}
