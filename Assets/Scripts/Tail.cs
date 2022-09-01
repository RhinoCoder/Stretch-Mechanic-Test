using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Tail : MonoBehaviour
{

    [SerializeField] private Vector3 followOffset;
    [SerializeField] private GameObject parentObj;  
    
    public GameObject nextObj;
    public float maxDistance = 2.5f;

    private TailParent tailParent;
    
    private void OnEnable()
    {
        parentObj = FindObjectOfType<Player>().gameObject;
        tailParent = FindObjectOfType<TailParent>();
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, nextObj.transform.position) <= maxDistance)
        {
            transform.DOMove(nextObj.transform.position + (-parentObj.transform.forward*1.5f), 0.3f, false);
        }
        
    }
}
