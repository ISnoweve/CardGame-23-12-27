using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(ClickDetectSystem))]
public class InputSystem : MonoBehaviour
{
    private ClickDetectSystem _clickDetectSystem;
    private RaycastHit2D hitObject;

    [SerializeField] private SOCardGameRuleWorkingEvent gameRuleWorkingEvent;

    [SerializeField] private bool canClickInput = true;

    private void Awake()
    {
        _clickDetectSystem = GetComponent<ClickDetectSystem>();
    }

    private void Update()
    {

        if(!canClickInput)return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseClickPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
            Vector3 mouseClickWorldPoint = Camera.main.ScreenToWorldPoint(mouseClickPosition);

            hitObject = Physics2D.Raycast(mouseClickWorldPoint, mouseClickPosition, 10f);
            if (hitObject)
            {
                _clickDetectSystem.Detect(hitObject);
            }
        }
    }

    private void DetectClickInput()
    {
        canClickInput = !canClickInput;
    }

    private void OnEnable()
    {
        gameRuleWorkingEvent.action += DetectClickInput;
    }

    private void OnDisable()
    {
        gameRuleWorkingEvent.action -= DetectClickInput;
    }
}
