using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDetectSystem : MonoBehaviour
{
    public SOClickEvent clickEvent;
    
    //Test debug
    [Space(10)][Header("EditorTest")]
    public bool debugCall;
    public void Detect(RaycastHit2D raycastHit)
    {
        if (debugCall)
        {
            Debug.Log(raycastHit.collider.gameObject);
        }

        GameObject objectGethit = raycastHit.collider.gameObject;
        Card card = objectGethit.GetComponent<Card>();

        if (card)
        {
            clickEvent.Trigger(card);
        }
    }
}
