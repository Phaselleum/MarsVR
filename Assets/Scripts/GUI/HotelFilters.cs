using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HotelFilters : MonoBehaviour
{
    /// <summary> Slider for minimum price per night </summary>
    public Slider minPrice;
    /// <summary> Text displaying minimum price per night </summary>
    public Text minPriceText;
    /// <summary> Slider for maximum price per night </summary>
    public Slider maxPrice;
    /// <summary> Text displaying maximum price per night </summary>
    public Text maxPriceText;
    /// <summary> GameObject holding guest rating selection buttons </summary>
    public GameObject rating;
    /// <summary> Slider for maximum distance from selected position/place </summary>
    public Slider maxDistance;
    /// <summary> Text displaying maximum distance from selected position/place </summary>
    public Text maxDistanceText;
    /// <summary> GameObject holding hotel class rating selection buttons </summary>
    public GameObject hotelClass;
    /// <summary> Toggle to filter for hotels with a pool </summary>
    public Toggle pool;
    /// <summary> Toggle to filter for hotels with a restaurant </summary>
    public Toggle restaurant;
    /// <summary> Toggle to filter for family friendly hotels </summary>
    public Toggle familyFriendly;
    /// <summary> Toggle to filter for hotels with a gym </summary>
    public Toggle gym;
    /// <summary> Toggle to filter for accessible hotels </summary>
    public Toggle accessible;
    /// <summary> Toggle to filter for hotels with wifi </summary>
    public Toggle wifi;
    /// <summary> Toggle to filter for hotels in close proximity to a beach </summary>
    public Toggle beachProximity;
    /// <summary> Toggle to filter for pet friendly hotels </summary>
    public Toggle petFriendly;
    /// <summary> Toggle to filter for hotels with a spa </summary>
    public Toggle spa;
    /// <summary> Toggle to filter for hotels with a cark park </summary>
    public Toggle carPark;

    /// <summary> Toggle to set the distance parameter to the city center </summary>
    public Toggle distanceCity;
    /// <summary> Toggle to set the distance parameter to a selected airport </summary>
    public Toggle distanceAirport;
    /// <summary> Toggle to set the distance parameter to a selected attraction </summary>
    public Toggle distanceAttraction;
    /// <summary> Dropdown menu of available nearby airports for distane parameter </summary>
    public Dropdown airportDrop;
    /// <summary> Dropdown menu of available nearby attractions for distane parameter </summary>
    public Dropdown attractionDrop;

    /// <summary> Value of minimum price per night slider </summary>
    private int minPriceVal;
    /// <summary> Value of maximum price per night slider </summary>
    private int maxPriceVal;
    /// <summary> Value of selected guest rating option </summary>
    private int ratingVal;
    /// <summary> Value of maximum distance slider </summary>
    private int maxDistanceVal;
    /// <summary> Value of selected hotel class rating option </summary>
    private int hotelClassVal;
    /// <summary> Pool filter value </summary>
    private bool poolVal;
    /// <summary> Restaurant filter value </summary>
    private bool restaurantVal;
    /// <summary> Family friendly filter value </summary>
    private bool familyFriendlyVal;
    /// <summary> Gym filter value </summary>
    private bool gymVal;
    /// <summary> Accessible filter value </summary>
    private bool accessibleVal;
    /// <summary> Wifi filter value </summary>
    private bool wifiVal;
    /// <summary> Beach proximity filter value </summary>
    private bool beachProximityVal;
    /// <summary> Pet friendly filter value </summary>
    private bool petFriendlyVal;
    /// <summary> Spa filter value </summary>
    private bool spaVal;
    /// <summary> Car park filter value </summary>
    private bool carParkVal;

    /// <summary> List of nearby airports </summary>
    private List<Airport> airportList;
    /// <summary> List of nearby attractions </summary>
    private List<Attraction> attractionList;
    /// <summary> Latitude of selected distance object </summary>
    private double selectedLat;
    /// <summary> Longitude of selected distance object </summary>
    private double selectedLong;

    /// <summary> List of hotels in the destination country </summary>
    public List<Hotel> hotels;
    /// <summary> List of hotels left over after applying the filters </summary>
    public List<Hotel> filteredList;
    /// <summary> Selected destination </summary>
    public EarthEngineCity city;
    /// <summary> Country of selected destination </summary>
    public EarthEngineCountry country;

    /// <summary> GameObject holding the default text for when no hotels are available given the current filters </summary>
    public GameObject noHotelsText;
    public GameObject loadingText;

    /// <summary> Array of Text components for names of the currently displayed hotel selection </summary>
    public Text[] elementNames;
    /// <summary> Array of Text components for prices of the currently displayed hotel selection </summary>
    public Text[] elementPrices;
    /// <summary> Array of Text components for guest ratings of the currently displayed hotel selection </summary>
    public Text[] elementRatings;
    /// <summary> Array of Text components for hotel class ratings of the currently displayed hotel selection </summary>
    public Text[] elementClasses;
    /// <summary> Array of Text components for ammenities of the currently displayed hotel selection </summary>
    public Text[] elementExtras;
    /// <summary> Array of Image components for images of the currently displayed hotel selection </summary>
    public Image[] elementImages;
    /// <summary> Array of Image components for 360 video overlays of the currently displayed hotel selection </summary>
    public Image[] elementButtons;
    /// <summary> Array of Image components for favourite buttons of the currently displayed hotel selection </summary>
    public Image[] favouriteButtons;

    /// <summary> List of hotel codes of favourited hotels </summary>
    public List<string> favouriteHotels;
    /// <summary> Sprite for hotel favourite button </summary>
    public Sprite star;
    /// <summary> Greyed out Sprite for hotel favourite button </summary>
    public Sprite star2;
    /// <summary> Placeholder Sprite for hotel image </summary>
    public Sprite weezySprite;

    /// <summary> GameObject holding buttons to select options to sort the hotel selection by </summary>
    public GameObject sortBy;
    /// <summary> Option to sort by (0: default, 1: rating, 2: distance, 3: price) </summary>
    private byte sortByVal;
    /// <summary> Filter by availability of a 360 video? </summary>
    private bool filter360;
    /// <summary> Button to display next page of filter results </summary>
    public Button nextPage;
    /// <summary> Button to display previous page of filter results </summary>
    public Button previousPage;

    /// <summary> Is the filter setup running? Prevents unnessecary redraws of filter display during setup </summary>
    private bool setup;
    /// <summary> Page of filter results to display </summary>
    public int hotelPage;

    /// <summary> Checks every second if the price slider has updated. Prevents redraws every tick of slider movement </summary>
    private bool priceChange;
    /// <summary> Keeps track of the time passed until cheecking the price slider </summary>
    private float priceChangeCounter;

    /// <summary> Array of jsons of hotels by country </summary>
    public TextAsset[] HotelsByCountry = new TextAsset[59]; //sorted by CountriesEnum

    /// <summary> Popup message to display infos/warnings/errors to user </summary>
    public GameObject messagePopup;

    private bool hotelsLoaded;

    public static HotelFilters Instance;

    private void Awake()
    {
        Instance = this;
        airportList = new List<Airport>();
        attractionList = new List<Attraction>();
        hotels = new List<Hotel>();
    }

    private void Start()
    {
        ResetFilters(false);

        favouriteHotels = new List<string>();
        if (PlayerPrefs.HasKey("favourites")) {
            favouriteHotels.AddRange(PlayerPrefs.GetString("favourites").Split(',')); 
        }
    }

    private void Update()
    {
        //checks for price slider changes
        priceChangeCounter += Time.deltaTime;
        if (priceChangeCounter >= 1)
        {
            priceChangeCounter = 0;
            if (priceChange)
            {
                priceChange = false;
                priceChangeUpdate();
            }
        }
    }

    /// <summary>
    /// Has the minimum price slider value changed?
    /// </summary>
    public void OnMinPriceChange()
    {
        priceChange = true;
    }

    /// <summary>
    /// Has the maximum price slider value changed?
    /// </summary>
    public void OnMaxPriceChange()
    {
        priceChange = true;
    }

    /// <summary>
    /// Update price filter and associated Text components. Update filtered hotel display.
    /// </summary>
    private void priceChangeUpdate()
    {
        minPrice.value = Mathf.Min(minPrice.value, maxPrice.value);
        minPriceVal = (int)minPrice.value;
        minPriceText.text = minPriceVal + "€";

        maxPrice.value = Mathf.Max(minPrice.value, maxPrice.value);
        maxPriceVal = (int)maxPrice.value;
        if (maxPriceVal >= 600)
        {
            maxPriceText.text = "600€+";
        }
        else
        {
            maxPriceText.text = maxPriceVal + "€";
        }

        if(!setup) ApplyFilters(city);
    }

    /// <summary>
    /// Updates the selected guest ratings filter. Updates filtered hotel display.
    /// </summary>
    public void SetRating(int r)
    {
        for(int i = 0; i < rating.transform.childCount; i++)
        {
            rating.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.white;
        }
        rating.transform.GetChild(r).gameObject.GetComponent<Image>().color = new Color(.7f, .7f, .7f);
        ratingVal = (byte)r;

        if (!setup) ApplyFilters(city);
    }

    /// <summary>
    /// Updates the distance filter and associated Text components. Updates filtered hotel display.
    /// </summary>
    public void OnDistanceChange()
    {
        maxDistanceVal = (byte)maxDistance.value;
        if (maxDistanceVal >= 25)
        {
            maxDistanceText.text = "25km+";
        }
        else
        {
            maxDistanceText.text = maxDistanceVal + "km";
        }

        if (!setup) ApplyFilters(city);
    }

    /// <summary>
    /// Toggles the favourite status of a hotel. Updates the asssociated PlayerPrefs entry.
    /// </summary>
    public void OnFavourite(int id)
    {
        if (favouriteHotels.Contains(filteredList[id].hotelCode))
        {
            favouriteHotels.Remove(filteredList[id].hotelCode);
            PlayerPrefs.SetString("favourites", string.Join(",", favouriteHotels.ToArray()));
            favouriteButtons[id].sprite = star2;
        } else
        {
            favouriteHotels.Add(filteredList[id].hotelCode);
            PlayerPrefs.SetString("favourites", string.Join(",", favouriteHotels.ToArray()));
            favouriteButtons[id].sprite = star;
        }
    }

    /// <summary>
    /// Updates the selected hotel class ratings filter. Updates filtered hotel display.
    /// </summary>
    public void SetClass(int c)
    {
        for (int i = 0; i < hotelClass.transform.childCount; i++)
        {
            hotelClass.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.white;
        }
        hotelClass.transform.GetChild(c).gameObject.GetComponent<Image>().color = new Color(.7f, .7f, .7f);
        hotelClassVal = (byte)c;

        if (!setup) ApplyFilters(city);
    }

    /// <summary>
    /// Updates the selected ammenities or distance option filter. Updates filtered hotel display.
    /// </summary>
    public void OnToggle(int t)
    {
        switch (t)
        {
            case 0:
                poolVal = pool.isOn;
                break;
            case 1:
                wifiVal = wifi.isOn;
                break;
            case 2:
                beachProximityVal = beachProximity.isOn;
                break;
            case 3:
                restaurantVal = restaurant.isOn;
                break;
            case 4:
                petFriendlyVal = petFriendly.isOn;
                break;
            case 5:
                familyFriendlyVal = familyFriendly.isOn;
                break;
            case 6:
                spaVal = spa.isOn;
                break;
            case 7:
                gymVal = gym.isOn;
                break;
            case 8:
                accessibleVal = accessible.isOn;
                break;
            case 9:
                carParkVal = carPark.isOn;
                break;
            case 100:
                if(!setup)
                {
                    selectedLat = city.latitude;
                    selectedLong = city.longitude;
                }
                distanceCity.SetIsOnWithoutNotify(true);
                distanceAirport.SetIsOnWithoutNotify(false);
                distanceAttraction.SetIsOnWithoutNotify(false);
                break;
            case 101:
                selectedLat = airportList[airportDrop.value].latitude;
                selectedLong = airportList[airportDrop.value].longitude;
                distanceCity.SetIsOnWithoutNotify(false);
                distanceAirport.SetIsOnWithoutNotify(true);
                distanceAttraction.SetIsOnWithoutNotify(false);
                break;
            case 102:
                selectedLat = attractionList[attractionDrop.value].latitude;
                selectedLong = attractionList[attractionDrop.value].longitude;
                distanceCity.SetIsOnWithoutNotify(false);
                distanceAirport.SetIsOnWithoutNotify(false);
                distanceAttraction.SetIsOnWithoutNotify(true);
                break;
            default:
                return;
        }

        if (!setup) ApplyFilters(city);
    }

    /// <summary>
    /// Updates the selection sort by options. Updates filtered hotel display.
    /// </summary>
    public void SetSortBy(int s)
    {
        for (int i = 0; i < sortBy.transform.childCount; i++)
        {
            sortBy.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.white;
        }
        sortBy.transform.GetChild(s).gameObject.GetComponent<Image>().color = new Color(.7f, .7f, .7f);
        sortByVal = (byte)s;

        if (!setup) ApplyFilters(city);
    }

    /// <summary>
    /// Updates the filter by available 360 video option. Updates filtered hotel display.
    /// </summary>
    public void Filter360()
    {
        filter360 = !filter360;
        if (!setup) ApplyFilters(city);
    }

    public void ResetFilters(bool applyFilters)
    {
        setup = true;
        SetRating(0);
        minPrice.value = 0;
        maxPrice.value = 600;
        maxPriceVal = 600;
        SetClass(0);
        pool.isOn = false;
        wifi.isOn = false;
        beachProximity.isOn = false;
        restaurant.isOn = false;
        petFriendly.isOn = false;
        familyFriendly.isOn = false;
        spa.isOn = false;
        gym.isOn = false;
        accessible.isOn = false;
        carPark.isOn = false;
        maxDistanceVal = 25;
        maxDistance.SetValueWithoutNotify(25);
        airportDrop.SetValueWithoutNotify(0);
        attractionDrop.SetValueWithoutNotify(0);
        SetSortBy(0);
        filter360 = false;
        setup = false;
        if (applyFilters) ApplyFilters(city);
    }

    /// <summary>
    /// Displays the next page of filter page results. Updates filtered hotel display.
    /// </summary>
    public void NextPage()
    {
        hotelPage++;
        DisplayHotelList();
        InfoScreenTopManager.Instance.LoadMap();

    }

    /// <summary>
    /// Displays the previous page of filter page results. Updates filtered hotel display.
    /// </summary>
    public void PreviousPage()
    {
        hotelPage--;
        DisplayHotelList();
        InfoScreenTopManager.Instance.LoadMap();
    }

    /// <summary>
    /// Resets all hotel filters to default values and updates airport and attraction lists. Determines if budget mode is being used
    /// </summary>
    /// <param name="city">Destination to set as selected</param>
    public void PrepareFilters(EarthEngineCity city)
    {
        hotelsLoaded = false;
        Debug.Log("Loading hotels for " + city.locationName);

        if (!city.hotelsLoaded && !DataHolderBehaviour.Instance.hotelsLoadingFrom.Contains(city.cityCode))
        /*{
            if (!DataHolderBehaviour.Instance.destinationssWithHotelsLoaded.Contains(city.cityCode))
            {
                Debug.Log("Loading hotels from server");

                loadingText.SetActive(true);
                StartCoroutine(GetHotelData((UnityWebRequest req, EarthEngineCity eac) =>
                {
                    if (req.error != null)
                    {
                        Debug.Log($"{req.error}: {req.downloadHandler.text}");

                        /*messagePopup.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Error retrieving hotel data";
                        messagePopup.transform.GetChild(2).gameObject.GetComponent<Text>().text = "(" + req.error + ")";
                        messagePopup.SetActive(true);*//*
                        if (city.hotels.Length == 0) { loadingText.SetActive(false); noHotelsText.SetActive(true); }
                    }
                    else
                    {
                        try
                        {
                            //Hotel[] newHotels = JsonUtility.FromJson<Hotel[]>(req.downloadHandler.text);
                            string jsonString = req.downloadHandler.text.Substring(1, req.downloadHandler.text.Length - 2);
                            string[] jsonArray = jsonString.Split(",{");
                            List<Hotel> hotelList = new List<Hotel>();
                            hotelList.AddRange(eac.hotels);
                            Debug.Log("Received " + jsonArray.Length + " hotels for " + eac.locationName + " from the server.");
                            foreach (string json in jsonArray)
                            {
                                string json2 = json;
                                if (json.Length > 0 && json[0] != '{') json2 = '{' + json2;
                                Hotel h = ScriptableObject.CreateInstance<Hotel>();
                                JsonUtility.FromJsonOverwrite(json2, h);
                                h.name = h.hotelCode;
                                foreach(string str in h.ammenities)
                                {
                                    switch(str)
                                    {
                                        case "Heated Pool(s)": 
                                        case "Indoor Pool": 
                                        case "Outdoor Pool(s)":
                                            h.pool = true;
                                            break;
                                        case "Family-friendly Hotel":
                                            h.familyFriendly = true;
                                            break;
                                        case "Gym":
                                            h.gym = true;
                                            break;
                                        case "WLAN access":
                                            h.wifi = true;
                                            break;
                                        case "Beach":
                                        case "Beach Hotel":
                                        case "Rocky Beach":
                                        case "Sandy Beach":
                                            h.beachProximity = true;
                                            break;
                                        case "Pets allowed":
                                            h.petFriendly = true;
                                            break;
                                        case "Massage":
                                        case "Sauna":
                                        case "Spa":
                                            h.spa = true;
                                            break;
                                        case "Car Park":
                                            h.carPark = true;
                                            break;
                                    }
                                    if (str.StartsWith("Restaurant")) h.restaurant = true;
                                    if (str.StartsWith("Wheelchair")) h.accessible = true;
                                }
                                hotelList.Add(h);
                            }
                            eac.hotels = hotelList.ToArray();
                            DataHolderBehaviour.Instance.destinationssWithHotelsLoaded.Add(eac.cityCode);
                            DataHolderBehaviour.Instance.hotelsLoadingFrom.Remove(eac.cityCode);
                            foreach (Hotel newhotel in hotelList)
                            {
                                bool skip = false;
                                foreach (Hotel oldhotel in DataHolderBehaviour.Instance.allHotels)
                                {
                                    if (oldhotel.hotelCode == newhotel.hotelCode)
                                    {
                                        skip = true;
                                        continue;
                                    }
                                }
                                if (!skip)
                                {
                                    DataHolderBehaviour.Instance.allHotels.Add(newhotel);
                                }
                            }
                            loadingText.SetActive(false);
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Formatting problem with loaded data - " + e.Message + e.StackTrace);
                            Debug.Log(req.downloadHandler.text);
                            /*messagePopup.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Error reading hotel data";
                            messagePopup.transform.GetChild(2).gameObject.GetComponent<Text>().text = "(" + e.Message + ")";
                            messagePopup.SetActive(true);*//*
                            if (city.hotels.Length == 0) { loadingText.SetActive(false); noHotelsText.SetActive(true); }
                        }
                    }
                    hotelsLoaded = true;
                    eac.hotelsLoaded = true;
                    ApplyFilters(city);
                }, city));
            } else
            {
                Debug.Log("Loading hotels from cache");
                List<Hotel> hotels = new List<Hotel>();
                hotels.AddRange(city.hotels);
                foreach(Hotel hotel in DataHolderBehaviour.Instance.allHotels)
                {
                    if(hotel.hotelCode.StartsWith(city.cityCode))
                    {
                        hotels.Add(hotel);
                    }
                }
                city.hotels = hotels.ToArray();
                hotelsLoaded = true;
                city.hotelsLoaded = true;
            }
        } else
        {
            hotelsLoaded = true;
        }*/

        //set selected city
        this.city = city;
        selectedLat = city.latitude;
        selectedLong = city.longitude;

        //clear filters
        airportDrop.options.Clear();
        attractionDrop.options.Clear();
        airportList.Clear();
        attractionList.Clear();
        filter360 = false;
        hotelPage = 0;

        //set airport list
        /*EarthEngineCountry eac = EarthEngineCountryController.Instance.GetCountryByADM3(city.adminA3);
        List<string> airportNameList = new List<string>();
            foreach (Airport a in eac.airports)
            {
                if (GeoLocator.GeodeticDistance(city.latitude, city.longitude, a.latitude, a.longitude) < 100e3)
                {
                    airportNameList.Add(a.airportName + " (" + a.IATACode + ")");
                    airportList.Add(a);
                }
            }
        airportDrop.AddOptions(airportNameList);*/

        //set attraction list
        /*List<string> attractionNameList = new List<string>();
        foreach (Attraction a in city.attractions)
        {
            if (GeoLocator.GeodeticDistance(city.latitude, city.longitude, a.latitude, a.longitude) < 100e3)
            {
                attractionNameList.Add(a.attractionName);
                attractionList.Add(a);
            }
        }

        attractionDrop.AddOptions(attractionNameList);*/

        if(hotelsLoaded) ApplyFilters(city);
    }

    /// <summary>
    /// Filter the hotel list by given filter settings
    /// </summary>
    /// <param name="city">Destination to filter from</param>
    public void ApplyFilters(EarthEngineCity city)
    {
        this.city = city;
        List<Hotel> countryHotels = new List<Hotel>();
        //use only hotels within 100km of the destination
            //Debug.Log(eac);
        /*foreach (Hotel h in city.hotels)
        {
            //Debug.Log(h);
            if (GeoLocator.GeodeticDistance(city.latitude, city.longitude, h.latitude, h.longitude) < 100e3)
            {
                countryHotels.Add(h);
            }
        }
        Debug.Log("Number of hotels nearby: " + countryHotels.Count);
        hotels = countryHotels;

        if (countryHotels.Count > 0)
        {
            noHotelsText.SetActive(false);

            filteredList = new List<Hotel>();
            List<Hotel> removeList = new List<Hotel>();
            filteredList.AddRange(hotels);

            //apply 360 video availability filter
            if (filter360)
            {
                foreach (Hotel h in filteredList)
                {
                    if (!h.video)
                    {
                        filteredList.Remove(h);
                    }
                }
            }

            foreach (Hotel h in filteredList)
            {
                Debug.Log(h.hotelName);
                //price filters
                if (h.price < minPriceVal)
                {
                    removeList.Add(h);
                    continue;
                }
                if (h.price > maxPriceVal && maxPriceVal != 600)
                {
                    removeList.Add(h);
                    Debug.Log("Checkpoint 2");
                    continue;
                }

                //guest ratings filter
                switch (ratingVal)
                {
                    case 1:
                        if(h.rating < 50)
                        {
                            removeList.Add(h);
                            continue;
                        }
                        break;
                    case 2:
                        if (h.rating < 70)
                        {
                            removeList.Add(h);
                            continue;
                        }
                        break;
                    case 3:
                        if (h.rating < 90)
                        {
                            removeList.Add(h);
                            continue;
                        }
                        break;
                    default:break;
                }

                //distance filter
                h.distance = GeoLocator.GeodeticDistance(h.latitude, h.longitude, selectedLat, selectedLong);
                if (h.distance > maxDistanceVal * 1000 && maxDistanceVal != 25)
                {
                    removeList.Add(h);
                    continue;
                }

                //hotel class rating filter
                if(h.hotelClass < hotelClassVal + 3)
                {
                    removeList.Add(h);
                    continue;
                }

                //amenities filters
                if (poolVal && !h.pool)
                {
                    removeList.Add(h);
                    continue;
                }
                if (wifiVal && !h.wifi)
                {
                    removeList.Add(h);
                    continue;
                }
                if (beachProximityVal && !h.beachProximity)
                {
                    removeList.Add(h);
                    continue;
                }
                if (restaurantVal && !h.restaurant)
                {
                    removeList.Add(h);
                    continue;
                }
                if (petFriendlyVal && !h.petFriendly)
                {
                    removeList.Add(h);
                    continue;
                }
                if (familyFriendlyVal && !h.familyFriendly)
                {
                    removeList.Add(h);
                    continue;
                }
                if (spaVal && !h.spa)
                {
                    removeList.Add(h);
                    continue;
                }
                if (gymVal && !h.gym)
                {
                    removeList.Add(h);
                    continue;
                }
                if (accessibleVal && !h.accessible)
                {
                    removeList.Add(h);
                    continue;
                }
                if (carParkVal && !h.carPark)
                {
                    removeList.Add(h);
                    continue;
                }
            }

            //remove hotels that don't match filters
            foreach (Hotel h in removeList)
            {
                filteredList.Remove(h);
            }

            //sort filtered hotels
            switch (sortByVal)
            {
                case 1:
                    filteredList.Sort((h1, h2) => h2.rating.CompareTo(h1.rating));
                    break;
                case 2:
                    filteredList.Sort((h1, h2) => h1.distance.CompareTo(h2.distance));
                    break;
                case 3:
                    filteredList.Sort((h1, h2) => h1.price.CompareTo(h2.price));
                    break;
                case 0:
                default:
                    break;
            }

            Debug.Log("Number of hotels filtered: " + filteredList.Count);
            //update hotel display
            if(hotelsLoaded) DisplayHotelList();
        } else
        {
            //display no-hotels-available-text
            foreach(Text text in elementNames) text.transform.parent.parent.gameObject.SetActive(false);
            noHotelsText.SetActive(true);
        }

        InfoScreenTopManager.Instance.LoadMap();*/
    }

    /// <summary>
    /// Display the updated list of filtered hotels
    /// </summary>
    void DisplayHotelList()
    {

        //set buttons active based on page count and number of filtered hotels
        nextPage.interactable = filteredList.Count > 6 + 6 * hotelPage;
        previousPage.interactable = hotelPage > 0;

        //hide all entries
        for(int j = 0; j < 6; j++)
        {
            elementNames[j].transform.parent.parent.gameObject.SetActive(false);
        }

        //fill all entries with necessary info
        for (int i = 6 * hotelPage; i < Mathf.Min(6 + 6 * hotelPage, filteredList.Count); i++)
        {
            int j = i % 6;
            if (filteredList.Count > i)
            {
                elementNames[j].text = filteredList[i].hotelName;
                elementPrices[j].text = filteredList[i].price + "€";
                if (filteredList[i].rating >= 90)
                {
                    elementRatings[j].text = filteredList[i].rating + "/100 Excellent";
                    elementRatings[j].color = new Color(0, .575f, .0087f);
                }
                else if (filteredList[i].rating >= 70)
                {
                    elementRatings[j].text = filteredList[i].rating + "/100 Very Good";
                    elementRatings[j].color = new Color(.13f, .858f, .143f);
                }
                else if (filteredList[i].rating >= 50)
                {
                    elementRatings[j].text = filteredList[i].rating + "/100 Good";
                    elementRatings[j].color = new Color(1, .84f, 0);
                }
                else if (filteredList[i].rating > 0)
                {
                    elementRatings[j].text = filteredList[i].rating + "/100";
                    elementRatings[j].color = new Color(1, .42f, 0);
                } else
                {
                    elementRatings[j].text = "";
                }
                elementClasses[j].text = filteredList[i].hotelClass + " star hotel";

                Transform extras = elementExtras[j].transform;
                extras.GetChild(0).gameObject.SetActive(filteredList[i].pool);
                extras.GetChild(1).gameObject.SetActive(filteredList[i].wifi);
                extras.GetChild(2).gameObject.SetActive(filteredList[i].beachProximity);
                extras.GetChild(3).gameObject.SetActive(filteredList[i].restaurant);
                extras.GetChild(4).gameObject.SetActive(filteredList[i].petFriendly);
                extras.GetChild(5).gameObject.SetActive(filteredList[i].familyFriendly);
                extras.GetChild(6).gameObject.SetActive(filteredList[i].spa);
                extras.GetChild(7).gameObject.SetActive(filteredList[i].gym);
                extras.GetChild(8).gameObject.SetActive(filteredList[i].accessible);
                extras.GetChild(9).gameObject.SetActive(filteredList[i].carPark);
                elementExtras[j].text = "";

                ///

                //Debug.Log(i + "/" + filteredList.Count);
                //Get hotel image from cache or server
                if(filteredList[i].hotelImage)
                {
                    try
                    {
                        elementImages[j].sprite = filteredList[i].hotelImage;
                    }
                    catch (MissingReferenceException e)
                    {
                        Debug.Log(filteredList[i].name + ": " + e.Message);
                        filteredList[i].hotelImage = weezySprite;
                        elementImages[j].sprite = weezySprite;
                    }
                } else if(filteredList[i].imageURLs.Length > 0)
                {
                    StartCoroutine(GetHotelImage((UnityWebRequest req, Hotel hotel, Image img) =>
                    {
                        if (req.error != null)
                        {
                            Debug.Log($"{req.error}: {req.downloadHandler.text}");

                            /*messagePopup.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Error retrieving hotel image";
                            messagePopup.transform.GetChild(2).gameObject.GetComponent<Text>().text = "(" + req.error + ")";
                            messagePopup.SetActive(true);*/

                            img.sprite = weezySprite;
                            hotel.hotelImage = weezySprite;
                        }
                        else
                        {
                            Texture2D tex = new Texture2D(0, 0);
                            tex.LoadImage(req.downloadHandler.data);
                            Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
                            img.sprite = spr;
                            hotel.hotelImage = spr;
                        }
                    }, filteredList[i], elementImages[j]));
                    filteredList[i].hotelImage = weezySprite;
                    elementImages[j].sprite = weezySprite;
                } else
                {
                    filteredList[i].hotelImage = weezySprite;
                    elementImages[j].sprite = weezySprite;
                }

                ///

                //display video overlay
                if (filteredList[i].video)
                {
                    elementButtons[j].gameObject.SetActive(true);
                    DataHolderBehaviour.Instance.YThotel[j] = (Video)filteredList[i].video;
                    DataHolderBehaviour.Instance.hotelVideoTitles[j] = city.locationName + " - " + filteredList[i].hotelName;
                }
                else
                {
                    elementButtons[j].gameObject.SetActive(false);
                }
                //set favourite button
                if (favouriteHotels.Contains(filteredList[i].hotelCode))
                {
                    favouriteButtons[j].sprite = star;
                }
                else
                {
                    favouriteButtons[j].sprite = star2;
                }
                //display entry
                elementNames[j].transform.parent.parent.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Get hotel image from server coroutine function
    /// </summary>
    /// <param name="callback">Callback after receiving server response</param>
    /// <param name="hotel">Hotel the image is loaded for</param>
    /// <param name="img">Image component to load the image into</param>
    /// <returns></returns>
    private IEnumerator GetHotelImage(Action<UnityWebRequest,Hotel,Image> callback, Hotel hotel, Image img)
    {
        string url = hotel.imageURLs[0];
        Debug.Log("connecting to: " + url);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            callback(webRequest, hotel, img);
        }
    }

    /// <summary>
    /// Get hotel image from server coroutine function
    /// </summary>
    /// <param name="callback">Callback after receiving server response</param>
    /// <param name="eacx">EarthEngineCountry the hotels are getting loaded to</param>
    /// <returns></returns>
    private IEnumerator GetHotelData(Action<UnityWebRequest,EarthEngineCity> callback, EarthEngineCity eac)
    {
        DataHolderBehaviour.Instance.hotelsLoadingFrom.Add(eac.cityCode);
        //Debug.Log(eac.locationName + ":" + eac.cityCode);
        string url = "http://" + DataHolderBehaviour.Instance.serverIP.Trim() + "/get-hotel-data/" + eac.cityCode;
        Debug.Log("connecting to: " + url);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            callback(webRequest, eac);
        }
    }
}
