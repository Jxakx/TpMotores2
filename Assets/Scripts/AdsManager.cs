using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener
{
    [SerializeField] string gameID = "5728142";

    [SerializeField] string adID = "Interstitial_Android";

    public void OnInitializationComplete()
    {
        Advertisement.Load(adID);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {

    }

    private void Start()
    {
        Advertisement.Initialize(gameID, true, this);
    }

    public void ShowAD()
    {
        if (!Advertisement.isInitialized) return;

        Advertisement.Show(adID, this);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
    }

    public void OnUnityAdsShowStart(string placementId)
    {
    }

    public void OnUnityAdsShowClick(string placementId)
    {
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {

        if(showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("¡Obtuviste una carga de Stamina!");
        }
        else
        {
            Debug.Log("No obtuviste tu carga de Stamina :(");
        }

    }
}
