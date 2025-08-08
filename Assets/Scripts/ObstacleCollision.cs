using UnityEngine;
using System.Collections;
public class ObstacleCollision : MonoBehaviour
{
    [SerializeField] AudioSource crashFx;

    void OnTriggerEnter(Collider other)
    {
        crashFx.Play();
        StartCoroutine(HandleGameOver());
    }

    IEnumerator HandleGameOver()
    {
        Object.FindFirstObjectByType<GameOverUI>().ShowGameOverScreen();
        yield return new WaitForSeconds(0.25f); // short buffer
        Time.timeScale = 0f;
    }
}
