using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilterBehaviour : MonoBehaviour
{
    /// <summary>Array of displayed GUI seasons (winter, spring, summer, autumn)</summary>
    public GameObject[] monthObjects;

    /// <summary>Array of GUI season Sprites in the interactable main screen</summary>
    public Image[] monthSprites;

    /// <summary>Activates the budget mode section of the filters when true</summary>
    public bool budgetMode = false;

    /// <summary>Text showing the selected month on the main screen</summary>
    public TextMeshProUGUI monthTMP;
    /// <summary>Text showing the selected month on the lower screen</summary>
    public TextMeshProUGUI monthTMPLower;
    /// <summary>Text showing the selected temperature on the main screen</summary>
    public TextMeshProUGUI tempTMP;
    /// <summary>Text showing the selected temperature on the lower screen</summary>
    public TextMeshProUGUI tempTMPLower;
    /// <summary>Slider that determines the selected temperature</summary>
    public Slider tempSlider;
    /// <summary>Slider visualisation that displays the selected temperature on the lower screen</summary>
    public Slider tempSliderLower;
    /// <summary>Slider that determines the selected price</summary>
    public Slider priceSlider;
    /// <summary>Slider visualisation that displays the selected price on the lower screen</summary>
    public Slider priceSliderLower;
    /// <summary>Text showing the selected price on the main screen</summary>
    public TextMeshProUGUI priceTMP;
    /// <summary>Text showing the selected price on the lower screen</summary>
    public TextMeshProUGUI priceTMPLower;
    /// <summary>Dropdown with number of all-inclusive guests</summary>
    public Dropdown guestsNoDD;
    /// <summary>Dropdown with number of all-inclusive nights</summary>
    public Dropdown OVguestsNoDD;
    /// <summary>Dropdown with number of all-inclusive nights</summary>
    public Dropdown OVnightsNoDD;
    /// <summary>Dropdown with number of all-inclusive nights</summary>
    public Dropdown OVroomsNoDD;

    /// <summary>Info screen display to the left of the user</summary>
    public GameObject infoScreenLeft;
    /// <summary>Info screen display to the right of the user</summary>
    public GameObject infoScreenRight;
    /// <summary>Info screen display above the user</summary>
    public GameObject infoScreenTop;
    /// <summary>Displayed city name on higher in display</summary>
    public TextMeshProUGUI infoCityName;

    public static FilterBehaviour Instance { get; private set; }

    /// <summary>Enum of months used for filtering</summary>
    public enum Months
    {
        JAN,
        FEB,
        MAR,
        APR,
        MAY,
        JUN,
        JUL,
        AUG,
        SEP,
        OCT,
        NOV,
        DEC
    };

    /// <summary>Array of month names used for displaying month filter value</summary>
    public string[] monthNames = new string[] {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};

    /// <summary>selected month</summary>
    public Months monthFilter { get; private set; }
    /// <summary>currently selected max flight price</summary>
    public int priceFilter { get; private set; }
    /// <summary>currently selected max all-inclusive-price</summary>
    public int guestsNo { get; private set; }
    /// <summary>Number of nights for hotels</summary>
    public int nightsNo { get; private set; }
    /// <summary>Number of rooms for hotels</summary>
    public int roomsNo { get; private set; }
    /// <summary>currently selected temperature in Celsius</summary>
    public float temperatureFilter { get; private set; }

    private void Awake()
    {
        Instance = this;
        LoadFilters();
    }

    private void Start()
    {
        //initialize GUI to display last used filters
        monthObjects[(int)monthFilter].SetActive(true);
        monthSprites[(int)monthFilter].gameObject.GetComponent<MonthSelect>().Select();
        monthTMP.text = monthNames[(int)monthFilter];
        monthTMPLower.text = monthNames[(int)monthFilter];

        priceSlider.SetValueWithoutNotify(priceFilter / 100);
        priceSliderLower.SetValueWithoutNotify(priceFilter / 100);
        priceTMP.text = priceFilter + "€";
        priceTMPLower.text = priceFilter + "€";
        guestsNoDD.SetValueWithoutNotify(Math.Max(guestsNo - 1, 0));
        OVguestsNoDD.value = Math.Max(guestsNo - 1, 0);
        OVnightsNoDD.value = Math.Max(nightsNo - 1, 0);
        OVroomsNoDD.value = Math.Max(roomsNo - 1, 0);

        tempSlider.SetValueWithoutNotify(temperatureFilter);
        tempSliderLower.SetValueWithoutNotify(temperatureFilter);
        tempTMP.text = temperatureFilter + "°C";
        tempTMPLower.text = temperatureFilter + "°C";
    }

    private void OnMouseDown()
    {
        ClickEvent();
    }

    /// <summary>
    ///     Display the filter selection screen on the main screen
    /// </summary>
    public void ClickEvent()
    {
        MainScreenBehaviour.Instance.ChangeDisplay(MainScreenBehaviour.Displays.FILTERS);
    }

    /// <summary>
    ///     Load last used filters from PlayerPrefs to given variables
    /// </summary>
    private void LoadFilters()
    {
        if (PlayerPrefs.HasKey("months"))
        {
            monthFilter = (Months)PlayerPrefs.GetInt("months");
        }
        else {
            monthFilter = (Months)DateTime.Now.Month;
        }
        if (PlayerPrefs.HasKey("price"))
        {
            priceFilter = PlayerPrefs.GetInt("price");
        }
        if (PlayerPrefs.HasKey("guestsNo"))
        {
            guestsNo = PlayerPrefs.GetInt("guestsNo");
        } else guestsNo = 2;
        if (PlayerPrefs.HasKey("nightsNo"))
        {
            nightsNo = PlayerPrefs.GetInt("nightsNo");
        } else nightsNo = 1;
        if (PlayerPrefs.HasKey("roomsNo"))
        {
            nightsNo = PlayerPrefs.GetInt("roomsNo");
        } else roomsNo = 1;
        if (PlayerPrefs.HasKey("temperature"))
        {
            temperatureFilter = (float)PlayerPrefs.GetInt("temperature");
        }
    }

    /// <summary>
    /// Set the selected month
    /// </summary>
    /// <param name="month">Month to set to</param>
    public void SetMonth(Months month)
    {
        monthFilter = month;
        monthTMP.text = monthNames[(int)month];
        monthTMPLower.text = monthNames[(int)month];
        PlayerPrefs.SetInt("months", (int)month);
        for (int i = 0; i < monthObjects.Length; i++)
        {
            monthObjects[i].SetActive(i == (int)month);
        }
    }

    /// <summary>
    /// Set the selected temperature
    /// </summary>
    /// <param name="temp">Temperature to set to in Celsius</param>
    public void SetTemperature(int temp)
    {
        if(temp == -1)
        {
            temp = (int)tempSlider.value;
        }
        if(temp > 35)
        {
            tempTMP.text = ">35°C";
            tempTMPLower.text = ">35°C";
            PlayerPrefs.SetInt("temperature", 36);
        }
        else
        {
            tempTMP.text = temp + "°C";
            tempTMPLower.text = temp + "°C";
            PlayerPrefs.SetInt("temperature", temp);
        }
        temperatureFilter = temp;
        tempSliderLower.value = temp;
    }

    /// <summary>
    /// Set the selected max flight price
    /// </summary>
    /// <param name="price">Price to set to</param>
    public void SetPrice(int price)
    {
        priceFilter = price * 100;
        priceTMP.text = priceFilter + "€";
        priceSliderLower.value = price;
        priceTMPLower.text = priceFilter + "€";
        PlayerPrefs.SetInt("price", priceFilter);
    }

    /// <summary>
    /// Set the number of all-inclusive guests
    /// </summary>
    /// <param name="no">Number of guests</param>
    public void SetGuestsNo(int no)
    {
        guestsNo = no;
        PlayerPrefs.SetInt("guestsNo", no);
    }

    /// <summary>
    /// Set the number of hotel nights
    /// </summary>
    /// <param name="no">Number of nights</param>
    public void SetNightsNo(int no)
    {
        nightsNo = no;
        PlayerPrefs.SetInt("nightsNo", no);
    }

    /// <summary>
    /// Set the number of hotel rooms
    /// </summary>
    /// <param name="no">Number of rooms</param>
    public void SetRoomsNo(int no)
    {
        roomsNo = no;
        PlayerPrefs.SetInt("roomsNo", no);
    }

    /// <summary>
    /// Apply the chosen filters
    /// </summary>
    public void ApplyFilters()
    {
        if (!DataHolderBehaviour.Instance.flightPricesLoaded) return;

        //UserDataCollector.Instance.filterSearchCount++;

        Debug.Log("Filtering...");

        List<EarthEngineCity> filteredList = new List<EarthEngineCity>();
        /*foreach(EarthEngineCity eac in EarthEngineCityController.Instance.interactableCities)
        {
            eac.Label.GetComponent<CityLabelBehaviour>().DestroyDestinationLine();
            //
            /*Debug.Log((int)monthFilter + "/" + eac.minTemp.Length + "|" + eac.avgTemp.Length + "|" + eac.maxTemp.Length + ", " + AirportChoice.Instance.choice + "/" + eac.flightPrices.Length + ", " + eac.locationName);*/
            /*if (eac.locationName == "Mexico City") Debug.Log(eac.maxTemp[(int)monthFilter] + ";" + eac.minTemp[(int)monthFilter] + " (" + temperatureFilter + ") " + eac.flightPrices[AirportChoice.Instance.choice] + "/" + tempPriceFilter + "(" + guestsNo + ")");*/
            //
            /*if (eac.maxTemp[(int)monthFilter] > temperatureFilter 
                && eac.minTemp[(int)monthFilter] < temperatureFilter
                && eac.flightPrices[AirportChoice.Instance.choice] <= priceFilter / guestsNo
                && eac.flightPrices[AirportChoice.Instance.choice] > 0)
            {
                filteredList.Add(eac);
            }
        }
        filteredList = filteredList.OrderBy(c => Mathf.Abs(c.avgTemp[(int)monthFilter] - temperatureFilter)).ToList();*/
        /*foreach (EarthEngineCity eac in EarthEngineCityController.Instance.interactableCities) 
        {
            eac.Label.GetComponent<CityLabelBehaviour>().tmp.color = new Color(1, 1, 1, .25f);
            //eac.Label.GetComponent<CityLabelBehaviour>().button.GetComponent<Button>().enabled = false;
        }*/
        foreach (EarthEngineCity eac in filteredList)
        {
            eac.HighlightCity();
            //eac.Label.GetComponent<CityLabelBehaviour>().button.GetComponent<Button>().enabled = true;
            eac.Label.GetComponent<CityLabelBehaviour>().CreateDestinationLine();
            eac.Label.transform.localScale = Vector3.one * 1.5f;
            //Debug.Log(eac.locationName + " - " + Mathf.Abs(eac.avgTemp[(int)monthFilter] - temperatureFilter));
        }
        InputController.Instance.SetZoomLevel(InputController.Instance.zoomLevel);
        if (filteredList.Count > 0)
        {
            InputController.Instance.HomeIn(AirportChoice.Instance.currentAirport.latitude, AirportChoice.Instance.currentAirport.longitude, true);

            GameObject.Find("Console").SetActive(false);
        }
        else Debug.Log("no results found");
    }
}
