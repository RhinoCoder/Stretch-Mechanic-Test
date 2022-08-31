using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Tail : MonoBehaviour
{

    [SerializeField] private Vector3 followOffset;
      
    public GameObject nextObj;
    public float maxDistance = 2.5f;
    
    

    private void FixedUpdate()
    {
        
        transform.DOMove(nextObj.transform.position + (-nextObj.transform.forward), 0.5f, false);
    }
}
