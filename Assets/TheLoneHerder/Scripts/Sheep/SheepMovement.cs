using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace TheLoneHerder
{
    public class SheepMovement : MonoBehaviour, IDestroyable
    {
        [SerializeField]
        private GameObject gorePrefab;
        NavMeshAgent animal;
        [SerializeField]
        Vector3 walkPoint;
        [SerializeField]
        bool walkPointSet;
        private AudioSource baa;
        public float walkPointRange = 2;
        float timer;
        float soundTimer;
        float freeWillBaa;

        public LayerMask whatisWalkable;
        void Start()
        {
            animal = GetComponent<NavMeshAgent>();
            baa = GetComponent<AudioSource>();
            freeWillBaa = Random.Range(1.0f, 80.0f);
            soundTimer = Time.time;
        }

        void Update()
        {
            Walk();
            if ((Time.time - timer) > 2.0f)
            {
                walkPointSet = false;
            }

            if ((Time.time - soundTimer) > freeWillBaa)
            {
                try
                {
                    baa.Play();
                }
                catch
                {
                    Debug.LogWarning("Sheep audio source not found");
                }

                freeWillBaa = Random.Range(10.0f, 80.0f);
                soundTimer = Time.time;
            }
        }

        private void Walk()
        {
            if ((!walkPointSet)) SeachForWalkPoint();

            if (walkPointSet) animal.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;
            if (distanceToWalkPoint.magnitude < 1.0f)
            {
                walkPointSet = false;
            }

        }

        private void SeachForWalkPoint()
        {
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            if (Physics.Raycast(walkPoint, -transform.up, 2f, whatisWalkable))
            {
                walkPointSet = true;
                timer = Time.time;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            walkPointSet = false;
        }

        public void Damage()
        {
            Instantiate(gorePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}