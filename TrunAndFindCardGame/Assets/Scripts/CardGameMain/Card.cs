using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using Sequence = DG.Tweening.Sequence;

public class Card : MonoBehaviour
{
    [Header("CardType")]
    public ECardType cardType;

    [Space(10)][Header("Flip")]
    [Min(1)] public float flipDuration;
    [SerializeField] private bool isFlip;
    
    [Space(10)][Header("Collection")]
    [Min(1)] public float moveDuration;
    public bool isCollected;
    
    [Space(10)][Header("Icon")]
    [SerializeField] private GameObject cardIcon;
    [SerializeField] private bool iconActive;

    public void CallCardFlip()
    {
        StartCoroutine(CardFlip());
    }
    private IEnumerator CardFlip()
    {
        Vector3 target = GetFlipTo();

        Tween flipTween = transform.DORotate(target, flipDuration, RotateMode.Fast);
        yield return flipTween.WaitForPosition(flipDuration/2-0.25f);
        EnableIcon();
    }
    public void CardCollecting(Transform collectingPlace)
    {
        isCollected = !isCollected;

        transform.DOMove(collectingPlace.position, moveDuration);
    }

    private Vector3 GetFlipTo()
    {
        Vector3 turnOut = new Vector3(0, 180, 0);
        Vector3 trunBack = new Vector3(0, 0, 0);

        isFlip = !isFlip;
        return isFlip ? turnOut : trunBack;
    }

    private void EnableIcon()
    {
        cardIcon.SetActive(!iconActive);
        iconActive = !iconActive;
    }
}

public enum ECardType
{
    Elphelt,
    Bridget,
    Ramlethal,
    JackO,
    Baiben,
    May,
    Millia,
    Giovanna,
    I,
    Jam,
}
