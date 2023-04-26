using TMPro;
using UnityEngine;

public class DestinationLineBehaviour : MonoBehaviour
{
    /// <summary> LineRenderer of the destinationLine </summary>
    public LineRenderer lineRenderer;
    public GameObject priceMarkerPrefab;
    private GameObject priceMarker;
	private int pos = 30;

    /// <summary>
    /// Interpolate the destination line from home base to destination in 100 steps
    /// </summary>
    /// <param name="destinationLat">latitude of the destination</param>
    /// <param name="destinationLong">longitude of the destination</param>
    public void NewLine(double destinationLat, double destinationLong, string price) {
        lineRenderer.positionCount = 100;
        Airport currentAirport = AirportChoice.Instance.currentAirport;
        lineRenderer.SetPosition(0, 
            GeoLocator.GeodeticToVector3(currentAirport.latitude, currentAirport.longitude) * (50 + (21 - InputController.Instance.zoomLevel) * .1f));
        for(int i = 1; i < 99; i++)
        {
            lineRenderer.SetPosition(i, (GeoLocator.GeodeticToVector3(destinationLat, destinationLong) * i * .01f + GeoLocator.GeodeticToVector3(currentAirport.latitude, currentAirport.longitude) * (1 - i * .01f))
                * ((75 - Mathf.Abs(i - 50)) + (21 - InputController.Instance.zoomLevel) * .1f));
        }
        lineRenderer.SetPosition(99, GeoLocator.GeodeticToVector3(destinationLat, destinationLong) 
            * (50 + (21 - InputController.Instance.zoomLevel) * .1f));
        CreatePriceMarker(price);
    }

    private Vector3 oldvector;
    private int tickCounter;
    private void Update()
    {
        if(tickCounter++ % 8 == 0 && lineRenderer.positionCount > 30 && !oldvector.Equals(lineRenderer.GetPosition(30)))
        {
            UpdateLabelCoords();
        }
        oldvector = lineRenderer.GetPosition(30);
    }

    private void CreatePriceMarker(string price)
    {
        priceMarker = Instantiate(priceMarkerPrefab, transform);
        //Quaternion.LookRotation(new Vector3(0, 1.75f, 150), new Vector3(0, 1, 0))
        //priceMarker.transform.position -= priceMarker.transform.forward * 5;
        priceMarker.GetComponent<Canvas>().worldCamera = InputController.Instance.mainCamera;
        priceMarker.GetComponentInChildren<TextMeshProUGUI>().text = price + "€";
        //Debug.Log("lrpos: " + lineRenderer.GetPosition(30).x + ", " + lineRenderer.GetPosition(30).y + ", " + lineRenderer.GetPosition(30).z);
        //Debug.Log("markerPos: " + priceMarker.transform.localPosition.x + ", " + priceMarker.transform.localPosition.y + ", " + priceMarker.transform.localPosition.z);
        UpdateLabelCoords();
		pos += (int)Random.Range(0,10);
    }

    private void UpdateLabelCoords()
    {
        priceMarker.transform.localPosition = lineRenderer.GetPosition(pos);
        priceMarker.transform.LookAt(InputController.Instance.mainCamera.transform, Vector3.up);
    }
}
