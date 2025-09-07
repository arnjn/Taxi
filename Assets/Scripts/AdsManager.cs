using System;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections; 
public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdManager Instance;

    [Header("Game IDs (set from Dashboard)")]
    public string androidGameId = "YOUR_ANDROID_GAME_ID";
    public string iOSGameId     = "YOUR_IOS_GAME_ID";
    public bool testMode = true;

    [Header("Placement IDs")]
    public string bannerPlacement = "Banner_Android";        // replace with your placement IDs
    public string interstitialPlacement = "Interstitial_Android";
    public string rewardedPlacement = "Rewarded_Android";

    // loaded flags
    private bool interstitialLoaded = false;
    private bool rewardedLoaded = false;

    // callback to call when rewarded ad completes
    private Action onRewardedSuccess;

    void Awake()
    {
         if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // keep across scenes
    }

    void Start()
    {
        InitializeAds();
    }

    #region Initialization
    public void InitializeAds()
    {
        string gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iOSGameId : androidGameId;
        if (!Advertisement.isInitialized)
        {
            Advertisement.Initialize(gameId, testMode, this);
        }
        else
        {
            // If already initialized, start loading
            SetBannerPositionTop();
            LoadBanner();
            LoadInterstitial();
            LoadRewarded();
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        SetBannerPositionTop();
        LoadBanner();        // load so banner is ready when you want it
        LoadInterstitial();  // preload interstitial
        LoadRewarded();      // preload rewarded
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogWarning($"Unity Ads Initialization Failed: {error} - {message}");
    }
    #endregion

    #region Banner
    void SetBannerPositionTop()
    {
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
    }

    public void LoadBanner()
    {
        var options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };
        Advertisement.Banner.Load(bannerPlacement, options);
    }

    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded.");
        Advertisement.Banner.Show(bannerPlacement);
    }

    void OnBannerError(string message)
    {
        Debug.LogWarning($"Banner failed to load: {message}");
    }

    // --- Banner helpers (add to your existing Banner region) ---
    public void ShowBanner()
    {
        // ensure banner position is set (you already have this helper)
        SetBannerPositionTop();

        // LoadBanner will call OnBannerLoaded -> Advertisement.Banner.Show(...)
        // so calling LoadBanner guarantees the banner will be shown as soon as it's ready.
        LoadBanner();
    }

    public void HideBanner()
    {
        // Hide but don't destroy by default (keeps it available)
        Advertisement.Banner.Hide(false);
    }


    public void HideBanner(bool destroy = false)
    {
        Advertisement.Banner.Hide(destroy);
    }
    #endregion

    #region Interstitial
    public void LoadInterstitial()
    {
        Advertisement.Load(interstitialPlacement, this);
    }

    public bool IsInterstitialReady() => interstitialLoaded;

    public void ShowInterstitial()
    {
        if (interstitialLoaded)
        {
            Advertisement.Show(interstitialPlacement, this);
        }
        else
        {
            Debug.Log("Interstitial not ready; calling LoadInterstitial().");
            LoadInterstitial();
        }
    }
    #endregion

    #region Rewarded
    public void LoadRewarded()
    {
        Advertisement.Load(rewardedPlacement, this);
    }

    public bool IsRewardedReady() => rewardedLoaded;

    // Call this from your Revive button script (or hook ShowRewardedForRevive to the Button)
   public void ShowRewardedForRevive()
    {
        ShowRewarded(() =>
        {
            Debug.Log("Rewarded ad completed — Reviving player.");
            GameState.isReviving = true; // ✅ set flag before reload
            Time.timeScale = 1f;
            SceneManager.LoadScene(1);
        });
    }



    public void ShowRewarded(Action onComplete)
    {
        onRewardedSuccess = onComplete;
        if (rewardedLoaded)
        {
            Advertisement.Show(rewardedPlacement, this);
        }
        else
        {
            Debug.Log("Rewarded ad not loaded yet; loading now.");
            LoadRewarded();
        }
    }
    #endregion

    #region Load/Show listeners
    // IUnityAdsLoadListener
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Ad loaded: {placementId}");
        if (placementId == interstitialPlacement) interstitialLoaded = true;
        if (placementId == rewardedPlacement) rewardedLoaded = true;
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogWarning($"Failed to load Ad: {placementId} - {error} - {message}");
        if (placementId == interstitialPlacement) interstitialLoaded = false;
        if (placementId == rewardedPlacement) rewardedLoaded = false;
        // TODO: exponential backoff / retry later
    }

    // IUnityAdsShowListener
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogWarning($"Ad show failed: {placementId} - {error} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId) { Debug.Log($"Ad show start: {placementId}"); }
    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"Ad show complete: {placementId} -> {showCompletionState}");

        // Reward on completion (only for rewarded placement)
        if (placementId == rewardedPlacement && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            onRewardedSuccess?.Invoke();
            onRewardedSuccess = null;
        }

        // after a show, reload that placement
        if (placementId == interstitialPlacement) { interstitialLoaded = false; LoadInterstitial(); }
        if (placementId == rewardedPlacement) { rewardedLoaded = false; LoadRewarded(); }
    }
    #endregion
}
