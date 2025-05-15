using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform notch;
    public PullInteractable bow;

    private bool isArrowNotched = false;
    private GameObject currentArrow = null;

    private void Awake()
    {
        bow.OnRelease += OnBowRelease;
    }

    private void OnDestroy()
    {
        bow.OnRelease -= OnBowRelease;
    }

    public void SpawnObject()
    {
        if (currentArrow == null && !isArrowNotched) {
            currentArrow = Instantiate(arrowPrefab, notch);
            isArrowNotched = true;
        }
    }

    private void OnBowRelease(float pullAmount)
    {
        EmptyNotch();
    }

    private void EmptyNotch()
    {
        isArrowNotched = false;
        currentArrow = null;
    }

    public void DestroyObject()
    {
        if (currentArrow != null)
        {
            Destroy(currentArrow);
            EmptyNotch();
        }
    }
}
