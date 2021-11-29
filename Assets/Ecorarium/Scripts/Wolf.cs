using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wolf : MonoBehaviour
{
    public Vector3 targetPoint;
    RaycastHit hit;
    [SerializeField] float range = 2;
    [SerializeField] LayerMask hitMask;
    bool moving;
    bool hasCollided = false;
    NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        DetectFence();
        Move();
    }


    void DetectFence()
    {
        Vector3 forward = transform.forward;
        Debug.DrawRay(transform.position, forward * range, Color.red);

        if (Physics.Raycast(transform.position, forward, out hit, range, hitMask))
        {


            Vector3 direction = hit.point - transform.position;
            Debug.DrawRay(transform.position, direction, Color.red);
            // TODO: Make rotation work
            //float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            //if (angle != 0)
            //{
            //    Quaternion angleAxis = Quaternion.AngleAxis(0, Vector3.up);
            //    transform.rotation = Quaternion.Slerp(transform.rotation, angleAxis, Time.deltaTime * 10);
            //}
            moving = false;

            if (hasCollided == false) GameEventsManager.current.WolfFoundTarget(true);
            hasCollided = true;

        }
        else
        {
            moving = true;
            if (hasCollided == true) GameEventsManager.current.WolfFoundTarget(false);
            hasCollided = false;
        }
    }

    private void OnDestroy()
    {
        if (hasCollided == true) GameEventsManager.current.WolfFoundTarget(false);
    }

    private void Move()
    {

        if (moving)
        {
            navMeshAgent.destination = targetPoint;

            //float step = speed * Time.deltaTime;
            //Vector3 targetPostition = new Vector3(targetPoint.x, transform.position.y, targetPoint.z);
            //transform.LookAt(targetPostition);
            //transform.position = Vector3.MoveTowards(transform.position, targetPostition, step);
        }
        else
        {
            navMeshAgent.destination = transform.position;
        }
    }
}
