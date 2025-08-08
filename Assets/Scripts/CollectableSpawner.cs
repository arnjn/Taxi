using UnityEngine;
using System.Collections.Generic;

public class CollectableSpawner : MonoBehaviour
{
    [Header("Collectable Prefabs & Player")]
    public GameObject[] collectablePrefabs; // Coin or other collectables
    public Transform player;

    [Header("Spawn Distance Settings")]
    public float spawnStartZ = 30f;
    public float initialSpawnInterval = 100f;
    public float minSpawnInterval = 30f;
    public float intervalDecreaseRate = 0.005f;
    //public float horizontalSpacing = 2.75f; // Space between coins in same row
    public float[] laneXPositions = { -2.75f, 2.75f };
    public float spawnDelay = 3.0f;

    [Header("Lane Z Positions (if spawning horizontally)")]
    public float[] laneZOffsets = { 0f, 10f, 20f };

    private float currentSpawnInterval;
    private float nextSpawnZ;
    private float spawnTimer = 0f;
    private List<GameObject> spawnedCollectables = new List<GameObject>();

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        nextSpawnZ = player.position.z + spawnStartZ;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnDelay)
        {
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - intervalDecreaseRate * spawnDelay);
            float baseZ = player.position.z + currentSpawnInterval;

            foreach (float zOffset in laneZOffsets)
            {
                SpawnCollectablesRow(baseZ + zOffset);
            }

            nextSpawnZ = baseZ + laneZOffsets[laneZOffsets.Length - 1];
            spawnTimer = 0f;
        }

        CleanupCollectables();
    }

    void SpawnCollectablesRow(float zPos)
    {
        foreach (float x in laneXPositions) // Spawn in 3 lanes: left, center, right
        {
            Vector3 spawnPos = new Vector3(x, 1.26f, zPos);

            int randomIndex = Random.Range(0, collectablePrefabs.Length);
            GameObject prefab = collectablePrefabs[randomIndex];
            Quaternion rotation = Quaternion.Euler(90f, 0f, 0f); // e.g. 180 degrees around Y
            GameObject collectable = Instantiate(prefab, spawnPos, rotation);
            collectable.SetActive(true);
            // GameObject collectable = Instantiate(prefab, spawnPos, Quaternion.identity);
            spawnedCollectables.Add(collectable);
        }
    }

    void CleanupCollectables()
    {
        for (int i = spawnedCollectables.Count - 1; i >= 0; i--)
        {
            if (spawnedCollectables[i].transform.position.z < player.position.z - 30f)
            {
                Destroy(spawnedCollectables[i]);
                spawnedCollectables.RemoveAt(i);
            }
        }
    }
}
