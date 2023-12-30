using UnityEngine;

public class ClickDetectSystem : MonoBehaviour
{
    public SOClickEvent clickEvent;
    
    public void Detect(RaycastHit2D raycastHit)
    {

        GameObject objectGethit = raycastHit.collider.gameObject;
        Card card = objectGethit.GetComponent<Card>();

        if (card)
        {
            clickEvent.Trigger(card);
        }
    }
}
