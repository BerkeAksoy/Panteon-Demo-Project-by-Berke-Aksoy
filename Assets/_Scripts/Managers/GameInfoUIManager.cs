using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class GameInfoUIManager : MonoBehaviour
{
    private static GameInfoUIManager _instance;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI tryText;
    [SerializeField] private RectTransform coinImageRect;
    [SerializeField] private List<TextMeshProUGUI> rankingTexts = new List<TextMeshProUGUI>();

    List<GameObject> tweeners = new List<GameObject>();
    public static GameInfoUIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameInfoUI manager is null.");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance == this) { return; }
            Destroy(gameObject);
        }
    }

    public void UpdateCoinText(int value)
    {
        coinText.text = "Coins: " + value.ToString();
    }

    public void UpdateTryText(int value)
    {
        tryText.text = "Count: " + value.ToString();
    }

    public void MoveCoinTowardsUI(GameObject coin, float duration, int coinValue)
    {
        tweeners.Add(coin);
        coin.transform.SetParent(coinImageRect.transform);
        coin.transform.DOMove(coinImageRect.position, duration).SetEase(Ease.InOutQuad).OnComplete(() => { GameManager.Instance.AddCoin(coinValue); tweeners.Remove(coin); Destroy(coin.gameObject);} );
    }

    public void AbortTween()
    {
        foreach(GameObject obj in tweeners)
        {
            DOTween.Kill(obj.transform);
            Destroy(obj);
        }

        tweeners.Clear();
    }

    public List<TextMeshProUGUI> GetRankingTextList()
    {
        return rankingTexts;
    }
}
