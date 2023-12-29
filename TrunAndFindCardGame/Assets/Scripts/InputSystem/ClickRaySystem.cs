using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(ClickDetectSystem))]
public class ClickRaySystem : MonoBehaviour
{
    [SerializeField] private ClickDetectSystem _clickDetectSystem;
    private RaycastHit2D hitObject;

    //Test debug
    [Space(10)][Header("EditorTest")]
    public bool debugCall;

    private void Awake()
    {
        _clickDetectSystem = GetComponent<ClickDetectSystem>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (debugCall)
            {
                Debug.Log("LeftClick is working");
            }

            Vector3 mouseClickPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
            Vector3 mouseClickWorldPoint = Camera.main.ScreenToWorldPoint(mouseClickPosition);

            hitObject = Physics2D.Raycast(mouseClickWorldPoint, mouseClickPosition, 10f);
            if (hitObject)
            {
                _clickDetectSystem.Detect(hitObject);
            }
        }
    }
}
