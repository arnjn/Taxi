using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    // === References to obstacle prefabs and the player ===
    [Header("Obstacle Prefabs & Player")]

    // Array of obstacle prefabs to randomly choose from
    public GameObject[] obstaclePrefabs;


    // Reference to the player (to spawn obstacles ahead of them)
    public Transform player;


// === Controls for how obstacles are spaced over time ===
[Header("Spawn Distance Settings")]

// The starting Z position at which obstacles will begin spawning
public float spawnStartZ = 60f;

// Initial distance between consecutive obstacle waves (along Z)
public float initialSpawnInterval = 300f;

// The minimum allowed spacing between waves (game becomes harder as spacing decreases)
public float minSpawnInterval = 50f;

// How quickly the spacing between waves decreases (difficulty ramp)
public float intervalDecreaseRate = 0.01f;

// Minimum vertical spacing between obstacles in the same wave (so they're not stacked too closely)
public float verticalSpacing = 50f;

// Time delay (in seconds) between obstacle wave spawns
public float spawnDelay = 5.0f;


// === Lane information (X positions where obstacles can appear) ===
[Header("Lane X Positions")]

// X-axis positions defining the horizontal lanes for obstacle spawning
public float[] laneXPositions = { -2.75f, 2.75f };


// === Internal state ===

// Current spacing used for spawning new waves (shrinks from initialSpawnInterval to minSpawnInterval)
private float currentSpawnInterval;

// Z position where the next set of obstacles will be spawned
private float nextSpawnZ;

// Tracks elapsed time since last spawn wave
private float spawnTimer = 0f;

// List to keep track of all spawned obstacles, so we can remove them when they're far behind the player
private List<GameObject> spawnedObstacles = new List<GameObject>();


    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        nextSpawnZ = player.position.z + currentSpawnInterval;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnDelay)
        {
            // Clamp spawn interval between min and initial
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - intervalDecreaseRate * spawnDelay);

            float baseZ = player.position.z + currentSpawnInterval;

            // Spawn 3 obstacles spaced vertically
            for (int i = 0; i < 3; i++)
            {
                float zPos = baseZ + i * verticalSpacing;
                SpawnObstacle(zPos);
            }

            nextSpawnZ = baseZ + 3 * verticalSpacing;
            spawnTimer = 0f;
        }

        CleanupObstacles();
    }

    void SpawnObstacle(float zPos)
    {
        float x = laneXPositions[Random.Range(0, laneXPositions.Length)];
        Vector3 spawnPos = new Vector3(x, 0.0f, zPos);

        int randomIndex = Random.Range(0, obstaclePrefabs.Length);
        GameObject prefab = obstaclePrefabs[randomIndex];
        GameObject obstacle = Instantiate(prefab, spawnPos, Quaternion.identity);
        spawnedObstacles.Add(obstacle);
    }

    void CleanupObstacles()
    {
        for (int i = spawnedObstacles.Count - 1; i >= 0; i--)
        {
            if (spawnedObstacles[i].transform.position.z < player.position.z - 50f)
            {
                Destroy(spawnedObstacles[i]);
                spawnedObstacles.RemoveAt(i);
            }
        }
    }
}
