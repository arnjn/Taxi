using TMPro;
using UnityEngine;

public class MasterInfo : MonoBehaviour
{
    //public static MasterInfo Instance;
    public static int coinCount = 0;
    public static int totalCoinCount = 0; // All-time total coins
    [SerializeField] GameObject coinDisplay;

    void Start()
    {
    }

    // void Awake()
    // {
    //     if (Instance == null)
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    // Update is called once per frame
    void Update()
    {
        coinDisplay.GetComponent<TMPro.TMP_Text>().text = "x" + coinCount;
    }
    
    // Call this when returning to Scene 0
    public static void SaveRunCoins()
    {
        totalCoinCount += coinCount;
        coinCount = 0;
    }
}
