using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
using DG.Tweening;

public class CardGameInitialSystem : MonoBehaviour
{
    [Header("Event")] 
    [SerializeField] private SOSystemWorkingEvent systemWorkingEvent;
    [Header("Reference variable")]
    [SerializeField] private Transform cardSpawner;
    [SerializeField] private GameObject pointObject;
    [SerializeField] private SOCard soCard;
    [SerializeField] private Vector3 instantiateOffset;
    [SerializeField] private int rows;
    [SerializeField] private int cols;
    [SerializeField] private int collectIndex;
    [SerializeField] private List<ECardType> cardtype;
    private List<int> controlTypeList = new List<int>();

    [Header("Object")][Space(5)] 
    [SerializeField] private List<GameObject> allPoints;
    [SerializeField] private List<Card> allCards;

    private void Start()
    {
        InstantiatePoint();
        InstantiateCard();
        StartCoroutine(InitialPlaying());
    }

    private IEnumerator InitialCardPosition(List<GameObject> points,List<Card> cards)
    {
        int allCount = rows * cols;
        for (int i = 0; i < allCount; i++)
        {
            GameObject point = points[i];
            Card card = cards[i];
            card.InitialSetupPosition(point.transform);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
    private void InitialCardFlip(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];
            card.CallCardFlip();
        }
    }

    public IEnumerator InitialPlaying()
    {
        systemWorkingEvent.Trigger();
        yield return StartCoroutine(InitialCardPosition(allPoints,allCards));
        yield return new WaitForSeconds(2f);
        InitialCardFlip(allCards);
        yield return new WaitForSeconds(0.1f);
        systemWorkingEvent.Trigger();
    }

    private void ShuffleCard<T>(List<T> list)
    {
        Random random = new Random();
        allCards = allCards.OrderBy(x => random.Next()).ToList();
    }
    private void InstantiateCard()
    {
        GameObject cardParent = GameObject.Find("AllOfCard");
        List<ECardType> cardtypesList = InstantiateTypeList();
        for (int i = 0; i < cardtypesList.Count; i++)
        {
            GameObject cardvalue = soCard.cardDictionary[cardtypesList[i]];
            Card card =Instantiate(cardvalue,cardSpawner.position,cardvalue.transform.rotation,cardParent.transform).GetComponent<Card>();
            allCards.Add(card);
        }
        ShuffleCard(allCards);
    }
    private List<ECardType> InstantiateTypeList()
    {
        List<ECardType> cardtypesList = new List<ECardType>();
        for (int i = 0; i < allPoints.Count/collectIndex; i++)
        {
            int randomNumber = GetRandomNumber(cardtype.Count);
            int cardtypeNumber =ControlRandomForType(randomNumber);
            for (int j = 0; j < collectIndex; j++)
            {
                cardtypesList.Add(cardtype[cardtypeNumber]);
            }
        }
        return cardtypesList;
    }
    private int ControlRandomForType(int index)
    {
        if(controlTypeList.Count == soCard.cardDictionary.Count)controlTypeList.Clear();

        if(index > soCard.cardDictionary.Count-1) index = 0;

        if(controlTypeList.Contains(index)) return ControlRandomForType(index + 1);
        
        controlTypeList.Add(index);
        return index;
    }
    private int GetRandomNumber(int index)
    {
        Random random = new Random();
        
        int randomNumber = random.Next(0, index);
        
        return randomNumber;
    }
    private void InstantiatePoint()
    {
        GameObject pointParent = GameObject.Find("AllOfPoints");
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                Vector3 cardSpawnPosition = GetPointPosition(x,y);
                
                GameObject temp = Instantiate(pointObject, cardSpawnPosition, Quaternion.identity,pointParent.transform);
                allPoints.Add(temp);
            }
        }
    }
    private Vector3 GetPointPosition(int x,int y)
    {
        float spawnX = x*2 - (float)rows / 2;
        float spawnY = y*3 - (float)cols / 2;

        Vector3 originalSpawnPoint = new Vector3(spawnX, spawnY);

        Vector3 modifySpawnPoint = originalSpawnPoint - instantiateOffset*2;

        return modifySpawnPoint;
    }
}
