using UnityEngine;
using TMPro;
using System.IO;
using System;
public class DistanceDisplay : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI distanceText;
    public string fileName = "distance.txt";
    public bool writeToFile = false;
    public static double d = 0.0;
    public static double od = 0.0;
    private float startingZ;
    private string filePath;

    void Start()
    {
        void Start()
{
            if (!GameState.isReviving) // only reset on fresh start
            {
                startingZ = player.position.z;
                d = 0.0;
                od = 0.0;

                if (writeToFile)
                {
                    filePath = Path.Combine(Application.persistentDataPath, fileName);
                    File.WriteAllText(filePath, "Distance Traveled: 0\n");
                }
            }
            else
            {
                // keep old distance — don’t reset
                // Revive → continue from old distance 
                GameState.isReviving = false; // clear flag so future runs are fresh
                startingZ = player.position.z;
    }
}

    }

    void Update()
    {
        float distance = player.position.z - startingZ;
        distanceText.text = $"{distance:F1} m";
        d = Math.Round(distance, 1);;
        if (writeToFile)
        {
            File.WriteAllText(filePath, $"Distance Traveled: {distance:F2} m");
        }
    }
}
