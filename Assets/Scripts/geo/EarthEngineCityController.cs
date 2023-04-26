using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine.UI;

public class EarthEngineCityController : MonoBehaviour
{
    public static EarthEngineCityController Instance { get; private set; }

    /// <summary>List of cities currently selectinteractable</summary>
    public List<EarthEngineCity> interactableCities;
    /// <summary>List of cities currently selectable</summary>
    public List<EarthEngineCity> activeCities;
    /// <summary>Right Oculus controller</summary>
    private InputDevice rightController;
    /// <summary>Used to deterine if the right controller's secondary button has been pressed down in this frame or not</summary>
    private bool lastSecondaryRValue;

    /// <summary>Info screen display to the left of the user</summary>
    public GameObject infoScreenLeft;
    /// <summary>Info screen display to the right of the user</summary>
    public GameObject infoScreenRight;
    /// <summary>Info screen display above the user</summary>
    public GameObject infoScreenTop;
    public GameObject earth;
    /// <summary> Main Camera GameObject </summary>
    public GameObject mainCamera;
    /// <summary> InfoScreens Object (used to initially find individual InfoScreens that may be deactiated) </summary>
    public GameObject infoScreens;

    private int tickCounter;
    public Transform earthGeoHolderT;

    void Awake()
    {
        Instance = this;

        activeCities = new List<EarthEngineCity>();

        earthGeoHolderT = GameObject.Find("EarthGeoHolder").transform;
        earth = GameObject.Find("MainEarth");
        mainCamera = GameObject.Find("Main Camera");
        infoScreens = GameObject.Find("Infoscreens");
    }

    private void Start()
    {
        interactableCities = GetInteractableCities();
        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, inputDevices);
        if (inputDevices.Count > 0)
        {
            rightController = inputDevices[0];
        }

        foreach (EarthEngineCity eac in interactableCities)
        {
            eac.ShowCity();
        }

    }

    private void Update()
    {
        bool secondaryButtonValue = false;
        if (rightController != null)
        {
            rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButtonValue);
        }
        if (Input.GetKeyDown(KeyCode.Escape) || (secondaryButtonValue && (secondaryButtonValue ^ lastSecondaryRValue)))
        {
            foreach (EarthEngineCity eac in interactableCities)
            {
                eac.Label.transform.localScale = Vector3.one;
                eac.Label.transform.GetComponent<CityLabelBehaviour>().button.GetComponent<Button>().enabled = true;
                eac.UnhighlightCity();
            }
            infoScreenLeft.SetActive(false);
            infoScreenRight.SetActive(false);
            infoScreenTop.SetActive(false);
        }
        lastSecondaryRValue = secondaryButtonValue;

        tickCounter++;
        if(tickCounter % 32 == 0 && InputController.Instance.zoomLevel > 7.5f)
        {
            List<GameObject> labels = new List<GameObject>();
            for (int i = 0; i < earthGeoHolderT.childCount; i++)
            {
                labels.Add(earthGeoHolderT.GetChild(i).GetChild(0).gameObject);
            }
            labels.Sort((a, b) => b.transform.GetChild(0).GetChild(0).position.z
                .CompareTo(a.transform.GetChild(0).GetChild(0).position.z));
            for (int i = 0; i < earthGeoHolderT.childCount; i++)
            {
                labels[i].GetComponent<CityLabelBehaviour>().display = i < 20;
            }
            //Debug.Log("1 " + labels[0] + ": " + labels[0].transform.GetChild(0).GetChild(0).position.z);
            //Debug.Log("2 " + labels[1] + ": " + labels[1].transform.GetChild(0).GetChild(0).position.z);
        }
    }

    /// <summary>
    /// Returns a city given its name
    /// </summary>
    /// <param name="cityname">Name of the city</param>
    /// <param name="ADM3">ADM3 code of the country</param>
    /// <returns>returns the city object, or null if none is found</returns>
	public EarthEngineCity GetCity(string cityname, string ADM3 = "") {
		foreach(Transform child in transform)
		{
			if (child.name.ToLower() == cityname.ToLower()) {
				Debug.Log("Found city: " + cityname );
				EarthEngineCity earthEngineCity = child.GetComponent<EarthEngineCity> () as EarthEngineCity;
				if (ADM3 == "") {
					return earthEngineCity;
				} else {
					if (earthEngineCity.adminA3.ToLower() == ADM3.ToLower()) {
						return earthEngineCity;
					}
				}
			}
		}
		return null;
    }


    /// <summary>
    ///     Gets all cities in a given country with a minimum rank
    /// </summary>
    /// <param name="ADM3">Country administrative code</param>
    /// <param name="minRank">minimum rank the cities need to be considered</param>
    /// <returns>Returns an array of the cities in a country with a given minimum rank</returns>
    public EarthEngineCity[] GetCitiesByCountry(string ADM3, int minRank = 10)
    {
        List<EarthEngineCity> eacs = new List<EarthEngineCity>();
        /*foreach (Transform child in transform)
        {
            arthEngineCity earthEngineCity = child.GetComponent<EarthEngineCity>() as EarthEngineCity;
            if (earthEngineCity.adminA3.ToLower() == ADM3.ToLower() && earthEngineCity.rank <= minRank)
            {
                eacs.Add(earthEngineCity);
            }
        }*/
        return eacs.ToArray();
    }


    /// <summary>
    ///     Gets all interactable cities in a given country
    /// </summary>
    /// <param name="ADM3">Country administrative code</param>
    /// <returns>Returns an array of the interactable cities in a country</returns>
    public EarthEngineCity[] GetInteractableCitiesByCountry(string ADM3)
    {
        List<EarthEngineCity> eacs = new List<EarthEngineCity>();
        foreach (EarthEngineCity eac in interactableCities)
        {
            if (eac.adminA3.ToLower() == ADM3.ToLower() && eac.interactable)
            {
                eacs.Add(eac);
            }
        }
        return eacs.ToArray();
    }


    /// <summary>
    ///     Gets all interactable cities
    /// </summary>
    /// <returns>Returns an array of the interactable cities in a country</returns>
    public List<EarthEngineCity> GetInteractableCities()
    {
        if (interactableCities.Count > 0) return interactableCities;
        List<EarthEngineCity> eacs = new List<EarthEngineCity>();
        foreach (Transform child in transform)
        {
            EarthEngineCity earthEngineCity = child.GetComponent<EarthEngineCity>();
            if (earthEngineCity.interactable)
            {
                eacs.Add(earthEngineCity);
                /*EarthEngineCountry eac = EarthEngineCountryController.Instance.GetCountryByADM3(earthEngineCity.adminA3);
                if (eac)
                    eac.interactable = true;
                else Debug.Log(earthEngineCity.locationName + " has an invalid country attatched") ;*/
            }
        }
        return eacs;
    }

}

