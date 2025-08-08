using UnityEngine;
using TMPro;
using System.IO;

public class DistanceDisplay : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI distanceText;
    public string fileName = "distance.txt";
    public bool writeToFile = false;

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

        if (writeToFile)
        {
            File.WriteAllText(filePath, $"Distance Traveled: {distance:F2} m");
        }
    }
}
