using System;
using System.Collections.Generic;
using UnityEngine;

public class CardRecordSyStem : MonoBehaviour
{
    public List<Card> cardCountInGame; 
    [SerializeField]private List<Card> cardRecordList;

    public void ClearCardCountList()
    {
        
    }
    
    public void AddCardCountList()
    {
        
    }
    
    public void ClearRecordList()
    {
        cardRecordList.Clear();
    }

    public void AddRecordList(Card cardVariable)
    {
        cardRecordList.Add(cardVariable);
    }
    
    public bool RecordListDetect(Card card)
    {
        bool isInside = cardRecordList.Contains(card);
        
        return !isInside;
    }

    public Card GetCard(int index)
    {
        return cardRecordList[index];
    }

    public int GetRecordListCount()
    {
        return cardRecordList.Count;
    }
}
