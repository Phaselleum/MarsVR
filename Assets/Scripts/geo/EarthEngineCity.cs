using UnityEngine;
using TMPro;
using System;

public class EarthEngineCity : MonoBehaviour
{
    /// <summary>Name of the city</summary>
	public string locationName;
    /// <summary>IATA Code associated with the city</summary>
    public string cityCode;
    public string adminA3;
    /// <summary>latitude of the city</summary>
	public double latitude;
    /// <summary>longitude of the city</summary>
	public double longitude;
    /// <summary>is this city interactable?</summary>
	public bool interactable;

    /// <summary>label of the city, which displays its name</summary>
    public GameObject Label { get; private set; }
    /// <summary>holder object for the label object, determining position and rotation of the label</summary>
	private GameObject labelHolder;
    
    /// <summary>prefab of the city label, preconfiguring the TextMeshPro component</summary>
    public GameObject labelPrefab;
    /// <summary>the up position of the camera when facing the globe</summary>
    private Vector3 mainCameraUp = new Vector3(0, 1, 0);

    /// <summary>Sprites to be shown on the city detail page (main top info screen)</summary>
    public Sprite[] images;
    /// <summary>Text to be shown on the city detail page (main top info screen)</summary>
    [TextArea(6, 6)]
    public string infoText;
    /// <summary>Videos to be shown on click on the thumbnail on the city detail page (main top info screen)</summary>
    public Video[] videos;
    /// <summary>Attractions associated with the city (to be pinned on the static maps later)</summary>
    public Slideshow[] tours;

    /// <summary>Is deactivated in LateUpdate - to prevent premature usage of objects initialized in ShowCity()</summary>
    public bool initializedThisTick;

    public bool hotelsLoaded;


    private void LateUpdate()
    {
        initializedThisTick = false;
    }

    /// <summary>
    /// Creates and displays the city name label as well as its holder object
    /// <return>returns the labelHolder for the city</return>
    /// </summary>
	public GameObject ShowCity ()
    {
        //create objects
        if(labelHolder == null) labelHolder = new GameObject(locationName);
        labelHolder.name = "labelHolder (" + name + ")";
		labelHolder.transform.parent = EarthEngineCityController.Instance.earthGeoHolderT;
        if (Label == null) Label = Instantiate(labelPrefab);
        Label.name = "label (" + name + ")";
        Label.SetActive (false);

        Label.GetComponent<CityLabelBehaviour>().city = this;
        
        //transform objects
        Label.transform.SetParent(labelHolder.transform, false);
        Vector3 camPos = new Vector3(0, 1.5f, 150);
        //labelHolder.transform.position = this.transform.position - (earthCenter.transform.position - transform.position) * .01f;
        //labelHolder.transform.position = new Vector3(-labelHolder.transform.position.x, -labelHolder.transform.position.y, labelHolder.transform.position.z);
        labelHolder.transform.position = GeoLocator.GeodeticToVector3(latitude, longitude) * 51.9f;
        labelHolder.transform.rotation = Quaternion.LookRotation(-(labelHolder.transform.position + camPos), mainCameraUp);
        
        //customize TextMeshPro
        TextMeshProUGUI tmp = Label.GetComponent<CityLabelBehaviour>().tmp;
        tmp.text = locationName;
        //tmp.fontSize = SetRankToScale (rank) * 50f;
        tmp.fontSize = 7;

        Label.SetActive (true);
        initializedThisTick = true;
        return Label;
    }

    /// <summary>
    /// Highlights the city
    /// </summary>
    public void HighlightCity()
    {
        TextMeshProUGUI tmp = Label.GetComponent<CityLabelBehaviour>().tmp;
        tmp.color = new Color(1, 0.3686275f, 0);
        tmp.fontStyle = FontStyles.Bold;
    }

    /// <summary>
    /// Unhighlights the city
    /// </summary>
    public void UnhighlightCity()
    {
        TextMeshProUGUI tmp = Label.GetComponent<CityLabelBehaviour>().tmp;
        tmp.color = new Color(1, 1, 1);
        tmp.fontStyle = FontStyles.Normal;
    }

    /// <summary>
    /// Destroys the city name label
    /// </summary>
	public void HideCity() {
        //GameObject.Destroy (labelHolder);
        if (labelHolder) labelHolder.SetActive(false);
        if (Label) Label.SetActive(false);
        Debug.Log("Hide " + locationName);
    }

    /// <summary>
    /// calculates the city label font size
    /// </summary>
    /// <param name="rank">The given city rank, based on population</param>
    /// <returns></returns>
	float SetRankToScale(int rank) {
		float scale = 0;
		switch (rank) {
		case 0:
			scale = 0.16f;
			break;
		case 1:
			scale = 0.14f;
			break;
		case 2:
			scale = 0.11f;
			break;
		case 3:
			scale = 0.09f;			
			break;
		case 4:
		case 5:
		case 6:
			scale = 0.06f;
			break;
		case 7:
		case 8:
			scale = 0.05f;
			break;

		default:
			scale = 0.04f;
			break;
		}
		return scale;
    }
}