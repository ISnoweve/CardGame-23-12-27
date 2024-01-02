using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class CardGameInitialSystem : MonoBehaviour
{

    [Header("reference variable")]
    [SerializeField] private int rows;
    [SerializeField] private int cols;
    [FormerlySerializedAs("collectionIndex")] [SerializeField] private int collectIndex;
    //下面為生成所需
    [SerializeField]private List<int> controlTypeList;
    [SerializeField]private List<int> controlCardList;
    [SerializeField]private List<ECardType> cardtype;
    
    [Space(10)] 
    [SerializeField] private GameObject prefabsObject;
    [SerializeField] private List<Card> allCards;
    public Vector3 positionOffset;

    private void Start()
    {
        InstantiateCard();
        InitialCardType();
    }

    private void InitialCardType()
    {
        int cardTeamCount = allCards.Count / collectIndex;
        int cardTypeCount = Enum.GetNames(typeof(ECardType)).Length;
        
        for (int round = 0; round < cardTeamCount; round++)
        {
            int enumindex = GetRandomNumber(cardTypeCount);
            int enumd = ControlRandomForType(enumindex);
            for (int j = 0; j < collectIndex; j++)
            {
                int cardIndex = GetRandomNumber(allCards.Count);
                int card = ControlRandomForCard(cardIndex);
                
                allCards[card].cardType = cardtype[enumd];
            }
        }
    }

    private void InstantiateCard()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                Vector3 cardSpawnPosition = GetCardPosition(x,y);
                
                GameObject emptyCard = Instantiate(prefabsObject, cardSpawnPosition, Quaternion.identity);
                Card card = emptyCard.GetComponent<Card>();
                allCards.Add(card);
            }
        }
    }

    private Vector3 GetCardPosition(int x,int y)
    {
        
        float spawnX = x*2 - rows / 2;
        float spawnY = y*3 - cols / 2;

        Vector3 originalSpawnPoint = new Vector3(spawnX, spawnY);

        Vector3 modifySpawnPoint = originalSpawnPoint - positionOffset*2;

        return modifySpawnPoint;
    }
    private int ControlRandomForType(int index)
    {
        if(controlTypeList.Count == 10)controlTypeList.Clear();

        if(index > 9) index = 0;

        if(controlTypeList.Contains(index)) return ControlRandomForType(index + 1);
        
        controlTypeList.Add(index);
        return index;
    }
    
    private int ControlRandomForCard(int index)
    {
        if(controlCardList.Count == allCards.Count)controlCardList.Clear();

        if(index > allCards.Count-1) index = 0;

        if(controlCardList.Contains(index)) return ControlRandomForCard(index + 1);
        
        controlCardList.Add(index);
        return index;
    }

    private int GetRandomNumber(int index)
    {
        Random random = new Random();
        
        int randomNumber = random.Next(0, index);
        
        return randomNumber;
    }
}
