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
        startingZ = player.position.z;

        if (writeToFile)
        {
            filePath = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllText(filePath, "Distance Traveled: 0\n");
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
