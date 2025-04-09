using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultiTrack : MonoBehaviour
{

    [SerializeField] private ARTrackedImageManager arMultiTrack;
    [SerializeField] private GameObject[] modelsTRack;

    private Dictionary<string, GameObject> modelsAr = new Dictionary<string, GameObject>();
    private Dictionary<string, bool> arState = new Dictionary<string, bool>();

    void Start()
    {
        foreach (var modelsAR in modelsTRack) {

            GameObject newTrack = Instantiate(modelsAR, Vector3.zero, Quaternion.identity);
            newTrack.name = modelsAR.name;
            modelsAr.Add(modelsAR.name, newTrack);
            newTrack.SetActive(true);
            arState.Add(modelsAR.name, newTrack);
        }
        
    }

    private void OnEnable()
    {
        arMultiTrack.trackedImagesChanged += ImageFound;
    }

    private void OnDisable()
    {
        arMultiTrack.trackedImagesChanged -= ImageFound;
    }

    private void ImageFound(ARTrackedImagesChangedEventArgs EventData)
    {
        foreach (var trackImage in EventData.added) {
            ShowModel(trackImage);        
        }

        foreach (var trackImage in EventData.updated) {
            if (trackImage.trackingState == TrackingState.Tracking) {

                HideModel(trackImage);
            }
        }
    }


    public void ShowModel(ARTrackedImage trackingImage) {
        bool arActive = arState[trackingImage.referenceImage.name];
        if (!arActive){

            GameObject arModelTrack = modelsAr[trackingImage.referenceImage.name];
            arModelTrack.transform.position = trackingImage.transform.position;
            arModelTrack.SetActive(true);
            arState[trackingImage.referenceImage.name] = true;
        }
        else {
            GameObject arModelTrack = modelsAr[trackingImage.referenceImage.name];
            arModelTrack.transform.position = trackingImage.transform.position;

        }
    
    }

    public void HideModel(ARTrackedImage trackingImage)
    {
        bool arActive = arState[trackingImage.referenceImage.name];
        if (arActive)
        {
            GameObject arModelTrack = modelsAr[trackingImage.referenceImage.name];
            arModelTrack.SetActive(false);
            arState[trackingImage.referenceImage.name] = false;
        }

    }
}
