using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientSpawner : MonoBehaviour
{
    public Vector2 DelayRangeBetweenSpawn;
    public int MaxPatientSpawn;
    public MoveTargetFinder TargetFinder;
    public Transform Container;

    [Space]

    public GameObject PatientPrefab;

    private int spawnCount;
    private float nextSpawn;
    private float timer;

    private void Start()
    {
        nextSpawn = Random.Range(DelayRangeBetweenSpawn.x, DelayRangeBetweenSpawn.y);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= nextSpawn)
        {
            Patient patient = Instantiate(PatientPrefab, transform.position, Quaternion.identity, Container).GetComponent<Patient>();
            patient.moveTarget = TargetFinder?.GetTarget();
            spawnCount++;

            if(spawnCount >= MaxPatientSpawn)
            {
                enabled = false;
            }
            else
            {
                nextSpawn = Random.Range(DelayRangeBetweenSpawn.x, DelayRangeBetweenSpawn.y);
                timer = 0;
            }
        }
    }
}