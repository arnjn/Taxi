using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
public class MainMenuControl : MonoBehaviour
{
    [SerializeField] TMP_Text totalCoinText;
    [SerializeField] TMP_Text highestDistance;
     
    public static double CurrentHighestDistance = 0.0;
    public static double CurrentDistance = 0.0;
    void Start()
    {
        Debug.Log(Time.timeScale);
        Time.timeScale = 1f;

        // Show banner when main menu loads
        if (AdManager.Instance != null)
        {
            AdManager.Instance.ShowBanner();
    }
    }

    void Update()
    {
        if (CurrentHighestDistance < CurrentDistance)
        {
            //Debug.Log("hi");
            CurrentHighestDistance = CurrentDistance;
            highestDistance.text = "Score: " + CurrentHighestDistance;
        }
        else
        {
            highestDistance.text = "Score: " + CurrentHighestDistance;
        }
        totalCoinText.text = "$ " + MasterInfo.totalCoinCount;
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
