using System.Collections;
using System.Net.Mime;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(CardRecordSyStem))]
public class CardGameRuleSystem : MonoBehaviour
{
    [Header("Event")]
    [SerializeField]private SOClickEvent clickEvent;
    [SerializeField]private SOSystemWorkingEvent SystemWorkingEvent;

    [Space(10)][Header("Reference")]
    private CardRecordSyStem _cardRecordSyStem;
    [SerializeField] private Transform collectionPlace;
    public float waitDetectTime;

    [Space(10)][Header("GameRule")]
    public int collectionCount;
    public int cardGameEndCollectionCount;
    public int cardGameEndPoint;
    public Text gameText;
    public Text gameEndText;

    public CardGameRuleSystem(int cardGameEndPointCount,int collectionIndex)
    {
        cardGameEndPoint = cardGameEndPointCount;
        collectionCount = collectionIndex;
    }
    private void Awake()
    {
        _cardRecordSyStem = GetComponent<CardRecordSyStem>();
    }

    private void RuleFoundation(Card card)
    {
        if (!CardDetect(card))return;
        
        if (!CheckCollectionCount())return;
        
        StartCoroutine(RuleWorkMoment(card));
    }
    

    private void RuleCorrectAction()
    {
        int countInRecordSystem = _cardRecordSyStem.GetRecordListCount();
        for (int cardIndex = 0; cardIndex < countInRecordSystem; cardIndex++)
        {
            _cardRecordSyStem.GetCard(cardIndex).CardCollecting(collectionPlace);
        }
        _cardRecordSyStem.ClearRecordList();
        
        
        cardGameEndCollectionCount++;
        gameText.text = "CollectionCount:" + cardGameEndCollectionCount;
    }

    private void RuleMistakeAction()
    {
        for (int cardIndex = 0; cardIndex < collectionCount; cardIndex++)
        {
            _cardRecordSyStem.GetCard(cardIndex).CallCardFlip();
        }
        _cardRecordSyStem.ClearRecordList();
    }
    
    IEnumerator RuleWorkMoment(Card card)
    {
        SystemWorkingEvent.Trigger();
        yield return new WaitForSeconds(card.flipDuration);

        if (AllCardTypeDetect())
        {
            RuleCorrectAction();
        }
        else
        {
            RuleMistakeAction();
        }

        if (cardGameEndPoint == cardGameEndCollectionCount)
        {
            gameEndText.enabled = true;
        }
        yield return new WaitForSeconds(waitDetectTime);
        SystemWorkingEvent.Trigger();
        yield return null;
    }

    //確認卡片是否重複
    private bool CardDetect(Card card)
    {
        if (!_cardRecordSyStem.RecordListDetect(card)) return false;

        if (card.isCollected) return false;
 
        _cardRecordSyStem.AddRecordList(card);
        card.CallCardFlip();
        return true;
    }
    //確認收集量是否達到
    private bool CheckCollectionCount()
    {
        return _cardRecordSyStem.GetRecordListCount() == collectionCount;
    }
    //確認翻的所有卡片是否相同
    private bool AllCardTypeDetect()
    {
        int forLoopMaxIndex;
        if (collectionCount <=2)
        {
            forLoopMaxIndex = 0;
        }
        else
        {
            forLoopMaxIndex = collectionCount - 2;
        }
        
        for (int cardIndex = 0; cardIndex <= forLoopMaxIndex; cardIndex++)
        {
            Card firstCard = _cardRecordSyStem.GetCard(cardIndex);
            Card nextCard = _cardRecordSyStem.GetCard(cardIndex + 1);

            if (firstCard.cardType != nextCard.cardType)
            {
                return false;
            }
        }
        return true;
    }
    private void OnEnable()
    {
        clickEvent.action += RuleFoundation;
    }
    
    private void OnDisable()
    {
        clickEvent.action -= RuleFoundation;
    }
}
