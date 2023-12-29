using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptObject/Event/Click")]
public class SOClickEvent : ScriptableObject
{
    public event Action<Card> action;

    public void Trigger(Card card)
    {
        action?.Invoke(card);
    }
}
