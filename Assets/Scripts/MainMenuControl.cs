using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuControl : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void StartGame()
    {

        StartCoroutine(StartButton());
    }

    IEnumerator StartButton()
    {
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene(1);
    }
}
