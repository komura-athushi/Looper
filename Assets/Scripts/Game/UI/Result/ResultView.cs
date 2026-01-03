using UnityEngine;
using TMPro;

public class ResultView : MonoBehaviour
{
    [Header("UI要素")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TextMeshProUGUI resultTitleText;
    [SerializeField] private TextMeshProUGUI retryHintText;

    private void Start()
    {
        HideResult();
    }

    /// <summary>
    /// ゲームクリア画面を表示
    /// </summary>
    public void ShowGameClear()
    {
        resultPanel.SetActive(true);
        resultTitleText.text = "Game Clear!";
        retryHintText.text = "Press R to Retry";
    }

    /// <summary>
    /// リザルト画面を非表示
    /// </summary>
    public void HideResult()
    {
        resultPanel.SetActive(false);
    }
}
