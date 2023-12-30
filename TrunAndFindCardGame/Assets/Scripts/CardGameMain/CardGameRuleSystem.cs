using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CardRecordSyStem))]
public class CardGameRuleSystem : MonoBehaviour
{
    [Header("Event")]
    [SerializeField]private SOClickEvent clickEvent;
    [SerializeField]private SOCardGameRuleWorkingEvent gameRuleWorkingEvent;

    [Header("Reference")][Space(10)]
    private CardRecordSyStem _cardRecordSyStem;
    [SerializeField] private Transform collectionPlace;
    public float waitDetectTime;

    [Header("GameRule")][Space(10)]
    public int collectionIndex;
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
    }

    private void RuleMistakeAction()
    {
        for (int cardIndex = 0; cardIndex < collectionIndex; cardIndex++)
        {
            _cardRecordSyStem.GetCard(cardIndex).CallCardFlip();
        }
        _cardRecordSyStem.ClearRecordList();
    }
    
    IEnumerator RuleWorkMoment(Card card)
    {
        gameRuleWorkingEvent.Trigger();
        yield return new WaitForSeconds(card.flipDuration);

        if (AllCardTypeDetect())
        {
            RuleCorrectAction();
        }
        else
        {
            RuleMistakeAction();
        }
        
        yield return new WaitForSeconds(waitDetectTime);
        gameRuleWorkingEvent.Trigger();
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
        return _cardRecordSyStem.GetRecordListCount() == collectionIndex;
    }
    //確認翻的所有卡片是否相同
    private bool AllCardTypeDetect()
    {
        int forLoopMaxIndex;
        if (collectionIndex <=2)
        {
            forLoopMaxIndex = 0;
        }
        else
        {
            forLoopMaxIndex = collectionIndex - 2;
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
