using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CityLabelBehaviour : MonoBehaviour
{
    /// <summary> City associated with the label </summary>
    public EarthEngineCity city;
    /// <summary> Texture to replace standard mouse texture when hovering over Label. Used in desktop only. </summary>
    public Texture2D pointer;
    /// <summary> MainEarth GameObject </summary>
    public GameObject earth;
    /// <summary> Main Camera GameObject </summary>
    public GameObject mainCamera;
    /// <summary> Prefab of lines to filtered destination labels </summary>
    public GameObject destinationLinePrefab;
    /// <summary> Line from home base to this label </summary>
    private GameObject destinationLine;

    /// <summary> Is this destination highlighted? </summary>
    bool highlighted = false;
    /// <summary> Has ClickEvent been triggered in this frame? </summary>
    bool mouseDown = false;
    /// <summary> Ensures orientation and sizing setup happens only fter a few frames have passed to ensure dependent objects have loaded </summary>
    int tickCounter = 0;

    /// <summary> Button used to select the label </summary>
    public GameObject button;
    /// <summary> Text component displaying the destination name </summary>
    public TextMeshProUGUI tmp;

    /// <summary> InfoScreens Object (used to initially find individual InfoScreens that may be deactiated) </summary>
    public GameObject infoScreens;

    /// <summary> Left InfoScreens Object </summary>
    public GameObject infoScreenLeft;
    /// <summary> Right InfoScreens Object </summary>
    public GameObject infoScreenRight;
    /// <summary> Top InfoScreens Object </summary>
    public GameObject infoScreenTop;
    /// <summary> Text component displaying the destination name on top info display </summary>
    public TextMeshProUGUI infoCityName;

    /// <summary> City latitude </summary>
    private double latitude;
    /// <summary> City longitude </summary>
    private double longitude;

    public bool display;
    private bool queueClick = false;


    /// <summary> Have the flight prices been loaded into the city Object? </summary>
    bool flightPricesLoaded = false;

    private void Awake()
    {
        infoScreens = GameObject.Find("Infoscreens");

        infoScreenLeft = infoScreens.transform.GetChild(0).gameObject;
        infoScreenRight = infoScreens.transform.GetChild(1).gameObject;
        infoScreenTop = infoScreens.transform.GetChild(2).gameObject;
    }

    private void Start()
    {
        //Populate GameObjects
        earth = EarthEngineCityController.Instance.earth;
        mainCamera = EarthEngineCityController.Instance.mainCamera;

        longitude = city.longitude;
        latitude = city.latitude;

        transform.GetChild(0).GetComponent<Canvas>().worldCamera = mainCamera.GetComponent<Camera>();
    }

    void Update()
    {
        //Load flight prices to city Object
        if (!flightPricesLoaded && DataHolderBehaviour.Instance.flightPricesLoaded)
        {
            //Debug.Log(city.locationName + ":" + city.cityCode);
            //if (city.cityCode == "JNB") Debug.Log(string.Join(',', DataHolderBehaviour.Instance.destinationAirports));
            int index = Array.IndexOf(DataHolderBehaviour.Instance.destinationAirports, city.cityCode);
            //Debug.Log(city.cityCode + ":" + index);
            //if (index >= 0 && city) city.flightPrices = DataHolderBehaviour.Instance.flightPrices[index];
            flightPricesLoaded = true;
            if (queueClick) ClickEvent();
        }

        //End highlight routine if anything else was clicked
        if (highlighted && !city.initializedThisTick && Input.GetMouseButtonDown(0) && !mouseDown)
        {
            city.UnhighlightCity();
            highlighted = false;
            Destroy(destinationLine);
        }
        mouseDown = false;

        //Setup Label display
        tickCounter++;
        if (tickCounter == 3)
        {
            button.GetComponent<RectTransform>().sizeDelta = tmp.GetRenderedValues() * 1.5f;
            tmp.GetComponent<RectTransform>().sizeDelta = tmp.GetRenderedValues() * 1.5f;
        }

        //Display Button Image only if the Label is on the side of the Earth Object facing the Player and at a minimum zoom level
        //if (city.locationName == "Cardiff" && tickCounter % 50 == 0) Debug.Log(transform.GetChild(0).GetChild(0).position.z);

        if(tickCounter % 32 == 0)
        {
            display = display && InputController.Instance.zoomLevel > 7.5f;
            if (display != button.GetComponent<Button>().enabled)
            {
                //Debug.Log("Toggled " + city.locationName);
                button.GetComponent<Button>().enabled = display;
                button.GetComponent<Image>().enabled = display;
                //buttonx.transform.parent.gameObject.GetComponent<TrackedDeviceGraphicRaycaster>().enabled = display;
            }
            button.SetActive(button.transform.position.z > 0);
        }
    }

    /// <summary>
    /// Handles actions on Click/Trigger. Displays and populates Info screens and updates User Data and Advancement data. Rotates and zooms in the Earth to have the Label face the Player.
    /// </summary>
    public void ClickEvent()
    {
        /*if(!flightPricesLoaded)
        {
            queueClick = true;
            return;
        }*/
        //UserDataCollector.Instance.destinationList.Add(city.locationName);
        //AdvancementBehaviour.Instance.DestinationVisited(city.locationName);
        //TravelOverviewBehaviour.Instance.UpdateDestinationText(city.locationName + ", " + city.adminA3);
        //TravelOverviewBehaviour.Instance.UpdateDestinationAirport(AirportChoice.Instance.GetAirportFromString(city.cityCode, false));
        //Debug.Log(AirportChoice.Instance.choice + "/" + city.flightPrices.Length);
        //TravelOverviewBehaviour.Instance.UpdateFlightPrices(city.flightPrices[AirportChoice.Instance.choice]);

        //Highlight Label
        foreach (EarthEngineCity eac in EarthEngineCityController.Instance.interactableCities)
        {
            eac.Label.transform.localScale = Vector3.one;
            eac.UnhighlightCity();
        }
        city.HighlightCity();

        //Display Info Screens
        ShowInfoScreens();
        infoScreenTop.GetComponent<InfoScreenTopManager>().Setup(city);
        //infoScreens.GetComponent<HotelFilters>().PrepareFilters(city);

        if (!infoCityName)
        {
            infoCityName = infoScreenTop.GetComponentInChildren<TextMeshProUGUI>();
        }
        infoCityName.text = city.locationName;

        //Rotate and zoom the Earth Object
        HomeIn();
        /*foreach (EarthEngineCity eac in EarthEngineCityController.Instance.interactableCities)
        {
            eac.Label.GetComponent<CityLabelBehaviour>().DestroyDestinationLine();
        }
        CreateDestinationLine();*/

        highlighted = true;
        mouseDown = true;
    }

    /// <summary>
    /// Activate the three main InfoScreens
    /// </summary>
    public void ShowInfoScreens()
    {
        //infoScreenLeft.SetActive(true);
        //infoScreenRight.SetActive(true);
        infoScreenTop.SetActive(true);
    }

    /// <summary>
    /// Create a destinationLine from home base to the Label
    /// </summary>
    public void CreateDestinationLine()
    {
        //destinationLine = Instantiate(destinationLinePrefab, GameObject.Find("DestinationLines").transform);
        //destinationLine.GetComponent<DestinationLineBehaviour>().NewLine(city.latitude, city.longitude, city.flightPrices[AirportChoice.Instance.choice].ToString());
    }

    /// <summary>
    /// Remove the destination Line associated with the Label
    /// </summary>
    public void DestroyDestinationLine()
    {
        //Destroy(destinationLine);
    }

    /// <summary>
    /// Set target coordinates to the city label
    /// </summary>
    public void HomeIn()
    {
        InputController.Instance.HomeIn(latitude, longitude, false);
    }
}
