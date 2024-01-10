using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameRuleEvent",menuName = "ScriptObject/Event/GameRuleEvent")]
public class SOSystemWorkingEvent : ScriptableObject
{
    public event Action action;

    public void Trigger()
    {
        action?.Invoke();
    }
}

