using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "CardClassify",menuName = "Card/CardClassify")]
public class SOCard : ScriptableObject
{
    [SerializedDictionary("Type", "Card")] 
    public SerializedDictionary<ECardType, GameObject> cardDictionary;
}
