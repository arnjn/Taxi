using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public GameObject collisionImageGameObject; 
    public GameObject gameOverPanel;
    public GameObject starContainer;
    public GameObject fullStarPrefab;
    public GameObject halfStarPrefab;

    public TMP_Text reviewText;

    void Start()
    {
        fullStarPrefab.SetActive(false);
        halfStarPrefab.SetActive(false);
        gameOverPanel.SetActive(false);
        if (collisionImageGameObject != null)
            collisionImageGameObject.SetActive(false);
    }

    public void ShowGameOverScreen()
    {
        collisionImageGameObject.SetActive(true);
        gameOverPanel.SetActive(true);

        float stars = GenerateRandomStarRating(); // e.g., 3.5 or 4.0
        ShowStarImages(stars);
        reviewText.text = GenerateReview();
    }

    void ShowStarImages(float starCount)
    {
        // Clear old stars
        foreach (Transform child in starContainer.transform)
        {
            Destroy(child.gameObject);
        }

        int fullStars = Mathf.FloorToInt(starCount);
        bool hasHalfStar = (starCount - fullStars) >= 0.5f;

        for (int i = 0; i < fullStars; i++)
        {
            GameObject star = Instantiate(fullStarPrefab, starContainer.transform);
            star.SetActive(true);

        }

        if (hasHalfStar)
        {
            GameObject star = Instantiate(halfStarPrefab, starContainer.transform);
            star.SetActive(true);

        }
    }

    float GenerateRandomStarRating()
    {
        // Random rating: 0, 0.5, 1, 1.5, ..., 5
        int steps = Random.Range(0, 11); // 0 to 10
        return steps * 0.5f;
    }

    string GenerateReview()
    {
        List<string> veryPositive = new List<string> { "Perfect Ride", "Five Stars" };
        List<string> slightlyPositive = new List<string> {
            "Smooth Trip", "Clean Cab", "On Time", "Polite Driver",
            "Nice Music", "Quick Route", "Fair Fare", "Decent Ride"
        };
        List<string> slightlyNegative = new List<string> {
            "Late Arrival", "Smelly Cab", "Rough Ride", "Missed Turn",
            "No AC", "Weird Music", "Expensive Fare", "Driver Silent"
        };
        List<string> veryNegative = new List<string> { "Horrible Ride", "Never Again" };

        float r = Random.value;
        if (r < 0.1f) return PickRandom(veryPositive);
        else if (r < 0.5f) return PickRandom(slightlyPositive);
        else if (r < 0.9f) return PickRandom(slightlyNegative);
        else return PickRandom(veryNegative);
    }

    string PickRandom(List<string> list)
    {
        return list[Random.Range(0, list.Count)];
    }
}
