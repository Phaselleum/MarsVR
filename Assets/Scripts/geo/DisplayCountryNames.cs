using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayCountryNames : MonoBehaviour
{

    /// <summary>list of ADM3s of countries that are interactable</summary>
    public List<string> countryADM3s = new List<string>();
    /// <summary>list of names of countries that are interactable</summary>
    public List<string> countryNames = new List<string>();
    /// <summary>list of countries that are interactable as EarthEngineCountry objects</summary>
    public List<EarthEngineCountry> countries = new List<EarthEngineCountry>();
    /// <summary>list of objects of names of countries that are interactable</summary>
    public List<GameObject> countryNameObjects = new List<GameObject>();
    /// <summary>prefab for the object that displays the country name</summary>
    public GameObject countryNamePrefab;
    /// <summary>the main Camera</summary>
    public Camera mainCamera;
    /// <summary>the up position of the camera when facing the globe</summary>
    private Vector3 mainCameraUp = new Vector3(0, 1, 0);

    /// <summary>The only instance of this script</summary>
    public static DisplayCountryNames Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //countries currently part of the program
        countryADM3s.Add("ARE"); //United Arab Emirates
        countryADM3s.Add("ARG"); //Argentina
        countryADM3s.Add("AUS"); //Australia
        countryADM3s.Add("AUT"); //Austria
        countryADM3s.Add("AZE"); //Azerbaijan
        countryADM3s.Add("BEL"); //Belgium
        countryADM3s.Add("BHS"); //Bahamas
        countryADM3s.Add("BRA"); //Brasil
        countryADM3s.Add("BRB"); //Barbados
        countryADM3s.Add("CAN"); //Canada
        countryADM3s.Add("CHE"); //Switzerland
        countryADM3s.Add("CHN"); //China
        countryADM3s.Add("CRI"); //Costa Rica
        countryADM3s.Add("CUB"); //Cuba
        countryADM3s.Add("CZE"); //Czech Republic
        countryADM3s.Add("DEU"); //Germany
        countryADM3s.Add("DNK"); //Denmark
        countryADM3s.Add("DOM"); //Dominican Rep.
        countryADM3s.Add("EGY"); //Egypt
        countryADM3s.Add("ESP"); //Spain
        countryADM3s.Add("FIN"); //Finland
        countryADM3s.Add("FRA"); //France
        countryADM3s.Add("GBR"); //United Kingdom
        countryADM3s.Add("GRC"); //Greece
        countryADM3s.Add("HUN"); //Hungary
        countryADM3s.Add("IDN"); //Indonesia
        countryADM3s.Add("IND"); //India
        countryADM3s.Add("IRL"); //Ireland
        countryADM3s.Add("ISL"); //Iceland
        countryADM3s.Add("ITA"); //Italy
        countryADM3s.Add("JAM"); //Jamaica
        countryADM3s.Add("JPN"); //Japan
        countryADM3s.Add("KEN"); //Kenya
        countryADM3s.Add("KOR"); //South Korea
        countryADM3s.Add("MAC"); //Macau
        countryADM3s.Add("MDV"); //Maledives
        countryADM3s.Add("MEX"); //Mexico
        countryADM3s.Add("MUS"); //Mauritius
        countryADM3s.Add("NLD"); //Netherlands
        countryADM3s.Add("NOR"); //Norway
        countryADM3s.Add("NZL"); //New Zealand
        countryADM3s.Add("PER"); //Peru
        countryADM3s.Add("PHL"); //Philippines
        countryADM3s.Add("POL"); //Poland
        countryADM3s.Add("PRT"); //Portugal
        countryADM3s.Add("PYF"); //French Polynesia
        countryADM3s.Add("ROU"); //Romania
        countryADM3s.Add("RUS"); //Russia
        countryADM3s.Add("SGP"); //Singapore
        countryADM3s.Add("SVK"); //Slovakia
        countryADM3s.Add("SWE"); //Sweden
        countryADM3s.Add("SYC"); //Seychelles
        countryADM3s.Add("THA"); //Thailand
        countryADM3s.Add("TUR"); //Turkey
        countryADM3s.Add("TZA"); //Tanzania
        countryADM3s.Add("URY"); //Uruguay
        countryADM3s.Add("USA"); //U.S.A
        countryADM3s.Add("ZAF"); //South Africa
        countryADM3s.Add("ZMB"); //Zambia

        countryNames.Add("United Arab Emirates");
        countryNames.Add("Argentina");
        countryNames.Add("Australia");
        countryNames.Add("Austria");
        countryNames.Add("Azerbaijan");
        countryNames.Add("Belgium");
        countryNames.Add("Bahamas");
        countryNames.Add("Brasil");
        countryNames.Add("Barbados");
        countryNames.Add("Canada");
        countryNames.Add("Switzerland");
        countryNames.Add("China");
        countryNames.Add("Costa Rica");
        countryNames.Add("Cuba");
        countryNames.Add("Czechia");
        countryNames.Add("Germany");
        countryNames.Add("Denmark");
        countryNames.Add("Dominican Rep.");
        countryNames.Add("Egypt");
        countryNames.Add("Spain");
        countryNames.Add("Finland");
        countryNames.Add("France");
        countryNames.Add("United Kingdom");
        countryNames.Add("Greece");
        countryNames.Add("Hungary");
        countryNames.Add("Indonesia");
        countryNames.Add("India");
        countryNames.Add("Ireland");
        countryNames.Add("Iceland");
        countryNames.Add("Italy");
        countryNames.Add("Jamaica");
        countryNames.Add("Japan");
        countryNames.Add("Kenya");
        countryNames.Add("South Korea");
        countryNames.Add("Macau");
        countryNames.Add("Maledives");
        countryNames.Add("Mexico");
        countryNames.Add("Mauritius");
        countryNames.Add("Netherlands");
        countryNames.Add("Norway");
        countryNames.Add("New Zealand");
        countryNames.Add("Peru");
        countryNames.Add("Philippines");
        countryNames.Add("Poland");
        countryNames.Add("Portugal");
        countryNames.Add("French Polynesia");
        countryNames.Add("Romania");
        countryNames.Add("Russia");
        countryNames.Add("Singapore");
        countryNames.Add("Slovakia");
        countryNames.Add("Sweden");
        countryNames.Add("Seychelles");
        countryNames.Add("Thailand");
        countryNames.Add("Turkey");
        countryNames.Add("Tanzania");
        countryNames.Add("Uruguay");
        countryNames.Add("United States of America");
        countryNames.Add("South Africa");
        countryNames.Add("Zambia");

        GeoLocator geo = new GeoLocator();

        //display the countries names
        /*for (int i=0;i<countryADM3s.Count; i++)
        {
            EarthEngineCountry mainCountry = EarthEngineCountryController.Instance.GetCountryByADM3(countryADM3s[i]);
            if (!mainCountry) { Debug.Log(countryADM3s[i] + " is not a valid ADM3 code"); continue;  }

            //exclude eponymous countries from display
            if(true/*countryNames[i] != "Macau" && countryNames[i] != "Singapore" && countryNames[i] != "Barbados" && countryNames[i] != "Seychelles")
            {

                //calculate center latitude/longitude of the country label
                /*int coordCount = 0;
                double latPos = 0;
                double longPos = 0;
                
                Component[] components = mainCountry.GetComponents(typeof(EarthEngineEarthVectors));
                foreach (EarthEngineEarthVectors component in components)
                {
                    for (int j = 0; j < component.poslat.Length; j++)
                    {
                        latPos += (float)component.poslat[j];
                        longPos += (float)component.poslong[j];
                        coordCount++;
                    }
                }

                latPos /= coordCount;
                longPos /= coordCount;

                //calculate initial positions and rotations
                Vector3 textPosition = transform.position + 
                    geo.GetVectorFromLatLong(transform.localScale.x * 100 + 0.1f, latPos, longPos) * .51f;

                Debug.Log(mainCountry.ADM0 + ": " + textPosition.x + ", " + textPosition.y + ", " + textPosition.z);

                GameObject countryNameObj = Instantiate(countryNamePrefab, mainCountry.labelPos, Quaternion.LookRotation(-(mainCountry.labelPos + mainCamera.transform.position), mainCameraUp), transform);
                countryNameObjects.Add(countryNameObj);
                countryNameObj.name = countryNames[i] + " (label)";

                TextMeshPro tmp = countryNameObj.GetComponent<TextMeshPro>();
                tmp.text = countryNames[i];
            }
        }*/
    }
}
