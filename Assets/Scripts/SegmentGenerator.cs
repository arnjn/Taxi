using System.Collections.Generic;
using UnityEngine;

public class InfiniteRoadGenerator : MonoBehaviour
{
    public GameObject roadSegmentPrefab;
    public Transform player;

    public float spawnInterval = 200f; // New segment every 200 units
    public int maxSegments = 6;

    private float nextSpawnZ = 0f;
    private Queue<GameObject> segments = new Queue<GameObject>();

    void Start()
    {
        // Pre-spawn starting segments
        for (int i = 0; i < maxSegments; i++)
        {
            SpawnSegment();
        }
    }

    void Update()
    {
        // Spawn new segment every 200 units
        if (player.position.z > nextSpawnZ - (maxSegments - 1) * spawnInterval)
        {
            SpawnSegment();

            if (segments.Count > maxSegments)
                RemoveOldestSegment();
        }
    }

    void SpawnSegment()
    {
        Vector3 spawnPos = new Vector3(0, 0, nextSpawnZ);
        GameObject segment = Instantiate(roadSegmentPrefab, spawnPos, Quaternion.identity);
        segments.Enqueue(segment);
        nextSpawnZ += spawnInterval;
    }

    void RemoveOldestSegment()
    {
        Destroy(segments.Dequeue());
    }
}
