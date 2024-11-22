using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected CharAttrSO charAttributes;
    private TextMeshPro nameText;
    private int rank;
    protected float runSpeed;
    private string charName;
    protected Vector3 finishPos;
    protected Vector3 startPos;
    protected Animator animator;
    protected Rigidbody rb;
    protected float distance;
    public static List<Character> runners = new List<Character>();

    protected virtual void Start()
    {
        startPos = transform.position;
        finishPos = GameObject.FindGameObjectWithTag("FinishLine").transform.position;
        nameText = GetComponent<TextMeshPro>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        runners.Add(this);

        UnpackCharAttributes();
        ApplyUnpackedData();
    }

    protected virtual void Update()
    {
        distance = Vector3.Distance(transform.position, finishPos);
        CalculateRank();
    }

    private void CalculateRank()
    {
        if (runners.Count == 0)
            return;

        runners = runners.OrderBy(x => x.distance).ToList();
        runners[0].rank = 1;
        runners[1].rank = 2;
        runners[2].rank = 3;
        runners[3].rank = 4;
        runners[4].rank = 5;
        runners[5].rank = 6;
        runners[6].rank = 7;
        runners[7].rank = 8;
        runners[8].rank = 9;
        runners[9].rank = 10;
        runners[10].rank = 11;

        List<TextMeshProUGUI> rankingTexts = GameInfoUIManager.Instance.GetRankingTextList();
        
        for(int i=0; i<runners.Count; i++)
        {
            rankingTexts[i].text = runners[i].rank.ToString() + " " + runners[i].charName;
        }
    }

    protected virtual void UnpackCharAttributes()
    {
        charName = charAttributes.charName;
        runSpeed = charAttributes.runSpeed;
    }

    protected virtual void ApplyUnpackedData()
    {
        nameText.text = charName;
    }

    public CharAttrSO GetCharAttributes()
    {
        return charAttributes;
    }

}
