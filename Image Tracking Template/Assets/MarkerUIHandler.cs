using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MarkerUIHandler : MonoBehaviour
{
    public GameObject uiPrefab;
    private GameObject spawnedUI;
    private ARTrackedImageManager imageManager;

    void Awake()
    {
        imageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
        {
            if (trackedImage.referenceImage.name == "ImageLib")
            {
                if (spawnedUI == null)
                    spawnedUI = Instantiate(uiPrefab);

                spawnedUI.SetActive(true);
                UpdateUIPosition(trackedImage);
            }
        }

        foreach (var trackedImage in args.updated)
        {
            if (trackedImage.referenceImage.name == "ImageLib")
            {
                if (trackedImage.trackingState == TrackingState.Tracking)
                {
                    if (spawnedUI == null)
                        spawnedUI = Instantiate(uiPrefab);

                    spawnedUI.SetActive(true);
                    UpdateUIPosition(trackedImage);
                }
                else if (spawnedUI != null)
                {
                    spawnedUI.SetActive(false);
                }
            }
        }

        foreach (var trackedImage in args.removed)
        {
            if (spawnedUI != null)
                spawnedUI.SetActive(false);
        }
    }

    void UpdateUIPosition(ARTrackedImage trackedImage)
    {
        spawnedUI.transform.SetPositionAndRotation(
            trackedImage.transform.position,
            trackedImage.transform.rotation
        );
    }
}
