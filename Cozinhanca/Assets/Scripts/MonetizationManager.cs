using System;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Purchasing;

public class MonetizationManager : MonoBehaviour, IUnityAdsInitializationListener
{
    //
    public static Action<string> OnPurchaseCompleted;

    public string _androidGameId;
    public string _androidBanerId;
    public string _androidInterstitialId;


    public string _iOSGameId;
    public string _iOSBanerId;
    public string _iOSInterstitialId;


    public bool _testMode = true;

    private string _gameId;
    private string _banerId;
    private string _interstitialId;

    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
#if UNITY_IOS
            _gameId = _iOSGameId;
            _banerId = _iOSBanerId;
            _interstitialId = _iOSInterstitialId;
#elif UNITY_ANDROID
            _gameId = _androidGameId;
        _banerId = _androidBanerId;
        _interstitialId = _androidInterstitialId;
#else
        Debug.LogError("PLATAFORMA NAO SUPORTADA");
        return;
#endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, Debug.isDebugBuild, this);
        }
    }


    public void OnInitializationComplete()
    {
#if ENABLE_LOG
        Debug.Log("Unity Ads initialization complete.");
#endif
       //Advertisement.Load(_interstitialId, this);
        LoadBanner();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void LoadBanner()
    {
        if (HasPurchased("removerbaner")) return;
        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load(_banerId, options);
    }
    void OnBannerLoaded()
    {
        if (HasPurchased("removerbaner")) return;
        Debug.Log("Banner loaded");
        Advertisement.Banner.Show(_banerId, null);

    }

    // Implement code to execute when the load errorCallback event triggers:
    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        // Optionally execute additional code, such as attempting to load another ad.
    }

    public void ShowInterstitialAd()
    {
        if (HasPurchased("removerbaner")) return;
        //Advertisement.Show(_interstitialId, this);
        //show interstitial

    }

    public static void ShowInterstitial()
    {
        if (HasPurchased("removerbaner")) return;
        MonetizationManager manager = FindObjectOfType<MonetizationManager>(true);
        manager.ShowInterstitialAd();
    }

    public void PurchaseCompleted(Product product)
    {
        PlayerPrefs.SetInt("PURCHASED" + product.definition.id, 1);
        PlayerPrefs.Save();

        if(HasPurchased("removerbaner")) Advertisement.Banner.Hide();

        if(OnPurchaseCompleted != null) OnPurchaseCompleted(product.definition.id);
    }

    public static bool HasPurchased(string productID)
    {
        return PlayerPrefs.GetInt("PURCHASED" + productID) == 1;
    }
}

