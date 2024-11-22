using UnityEngine;

public class Coin : Collectable
{
    [SerializeField] private GameObject coinToTween;
    private int coinValue = 1, coinStack = 5;
    private float minTweenDuration = 0.5f, maxTweenDuration = 1f, spreadRadius = 0.15f;

    public override void Collect(Collider collector)
    {
        if (collector.gameObject.CompareTag("Player"))
        {
            for(int i = 0; i < coinStack; i++)
            {
                Vector3 spread = new Vector3(Random.Range(-spreadRadius, spreadRadius), Random.Range(0, spreadRadius), Random.Range(-spreadRadius, spreadRadius));

                GameObject newCoin = Instantiate(coinToTween, transform.position + spread, Quaternion.identity);

                newCoin.AddComponent<RectTransform>();

                RectTransform tempRect = newCoin.GetComponent<RectTransform>();
                tempRect.anchorMin = Vector2.one;
                tempRect.anchorMin = Vector2.one;

                float tweenDuration = Random.Range(minTweenDuration, maxTweenDuration);

                GameInfoUIManager.Instance.MoveCoinTowardsUI(newCoin, tweenDuration, coinValue);
            }
        }
    }


}
