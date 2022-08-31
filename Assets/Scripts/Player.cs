using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;


public class Player : MonoBehaviour
{
    [SerializeField] private float maxVelocity;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject tailPrefab;
    [SerializeField] private GameObject firstObj;
    [SerializeField] private float maxRange = 7f;


    private Camera mainCamera;
    private Vector3 moveDirection;
    private Rigidbody playerRb;


    private bool canMove;
    private bool canSpawn;
    private Vector3 startPoint;
    private List<GameObject> spawnedTails = new List<GameObject>();
    private RaycastHit hit;


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
                RotationHandler();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            firstObj.GetComponent<MeshRenderer>().enabled = true;
            if (canSpawn)
            {
                StartCoroutine(SpawnTails());
            }
        }

        else if (Input.GetMouseButtonUp(0))
        {
            startPoint = transform.position;
            firstObj.GetComponent<MeshRenderer>().enabled = false;
            SpawnedTailsDestroyer();
            StopAllCoroutines();
        }
    }


    private bool DistanceCalculator()
    {
        if (Vector3.Distance(startPoint, transform.position) <= maxRange)
        {
            canMove = true;
            canSpawn = true;
        }
        else
        {
            canMove = false;
            canSpawn = false;
        }


        return canMove;
    }

    private IEnumerator SpawnTails()
    {
        while (true)
        {
            GameObject tail = Instantiate(tailPrefab, transform.position, Quaternion.identity);

            if (spawnedTails.Count <= 0)
            {
                tail.GetComponent<Tail>().nextObj = firstObj;
            }
            else
            {
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

    private void CharacterMover()
    {
        var step = moveSpeed * Time.deltaTime;
        Vector3 offset = new Vector3(0f, 1f, 0f);

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                transform.position = Vector3.MoveTowards(transform.position, hit.point + offset, step);
                transform.LookAt(hit.point);
            }
        }
    }


    private void RotationHandler()
    {
        if (playerRb.velocity == Vector3.zero)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, Vector3.back);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        //Resets the rotation 
        transform.DORotate(new Vector3(0f, transform.rotation.y, 0f), 0.1f);
    }
}