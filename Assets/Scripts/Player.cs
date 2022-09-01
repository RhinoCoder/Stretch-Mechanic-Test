using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;


public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private DynamicJoystick dynamicJoystick;
    [SerializeField] private float turnSpeed = 5f;
    [SerializeField] private GameObject tailPrefab;
    [SerializeField] private GameObject firstObj; // TailParent
    [SerializeField] private float maxRange = 7f;


    private Camera mainCamera;
    private Vector3 moveDirection;
    private Rigidbody playerRb;
    private bool canMove;
    private bool canSpawn;
    private Vector3 startPoint;


    public List<GameObject> spawnedTails = new List<GameObject>();

    public Vector3 StartPoint
    {
        get { return startPoint; }
    }


    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        canMove = true;
        DOTween.Init();
    }

    private void Start()
    {
        startPoint = transform.position;
    }


    void Update()
    {
        if (!canSpawn)
        {
            StopAllCoroutines();
        }


        if (Input.GetMouseButton(0))
        {
            if (DistanceCalculator())
            {
                CharacterMover();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (canSpawn)
            {
                StartCoroutine(SpawnTails());
            }
        }

        else if (Input.GetMouseButtonUp(0))
        {
            GroundCheck();
            startPoint = transform.position;
            SpawnedTailsDestroyer();
            StopAllCoroutines();
        }
    }


    private void CharacterMover()
    {
        float horizontal = dynamicJoystick.Horizontal;
        float vertical = dynamicJoystick.Vertical;
        Vector3 newPos = new Vector3(horizontal * moveSpeed * Time.deltaTime, 0, vertical * moveSpeed * Time.deltaTime);
        transform.position += newPos;

        Vector3 direction = Vector3.forward * vertical + Vector3.right * horizontal;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
            turnSpeed * Time.deltaTime);
    }

    private bool DistanceCalculator()
    {
        var distanceToCenter = Vector3.Distance(startPoint, transform.position);


        if (distanceToCenter <= maxRange)
        {
            canMove = true;
            canSpawn = true;
        }
        else
        {
            canMove = false;
            canSpawn = false;
        }

        Debug.Log(distanceToCenter);
        return canMove;
    }

 
    private void GroundCheck()
    {
        RaycastHit hitGround;
        if (Physics.Raycast(transform.position, Vector3.down, out hitGround, Mathf.Infinity))
        {
            if (hitGround.collider.CompareTag("Ground"))
            {
                transform.position = hitGround.point + new Vector3(0f,(0.5f),0f);
            }
        }
        else
        {
            transform.position = startPoint;
        }
    }

    private IEnumerator SpawnTails()
    {
        while (true)
        {
            GameObject tail = Instantiate(tailPrefab, transform.position, Quaternion.identity);
            if (spawnedTails.Count <= 0)
            {
                tail.GetComponent<Tail>().nextObj = gameObject;
            }
            else
            {
                tail.transform.position = spawnedTails[spawnedTails.Count - 1].transform.position;
                tail.GetComponent<Tail>().nextObj = spawnedTails[spawnedTails.Count - 1];
            }


            spawnedTails.Add(tail);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void SpawnedTailsDestroyer()
    {
        foreach (GameObject tail in spawnedTails)
        {
            tail.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.1f);
            Destroy(tail, 0.2f);
        }

        spawnedTails.Clear();
    }
}