using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailParent : MonoBehaviour
{
    [SerializeField] private Player player;
    
    public GameObject followLastObj;


    private void Start()
    {
        followLastObj = player.gameObject;
    }

    private void Update()
    {
         
        if (Input.GetMouseButtonUp(0))
        {
            
            PositionChanger();

        }
        
        
    }


    private void PositionChanger()
    {
        if (player.spawnedTails.Count <= 0)
        {
            return;
        }

        transform.position = player.transform.position + (-player.transform.forward * 2f);

    }
}
