using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class TravelOverviewBehaviour : MonoBehaviour
{
    public Text destinationText;
    public Text homeIATAText;
    public Text homeAirportText;
    public Text destinationIATAText;
    public Text destinationAirportText;
    public Text flightCalcText;
    public Text flightTotalText;
    public Text hotelText;
    public Text hotelCalcText;
    public Text hotelTotalText;
    public Text grandTotalText;

    public Dropdown guestsDD;
    public Dropdown nightsDD;
    public Dropdown roomsDD;

    private int flightCost = 0;
    private int hotelCost = 0;
    private int baseFlightCost = 0;
    private int baseHotelCost = 0;

    private CultureInfo ci;

    public static TravelOverviewBehaviour Instance;

    private void Awake()
    {
        Instance = this;
        ci = new CultureInfo("en-us");
    }

    public void UpdateDestinationText(string destination)
    {
        destinationText.text = destination;
    }

    public void UpdateHomeIATAText(string iata)
    {
        homeIATAText.text = iata;
    }

    public void UpdateHomeAirportText(string name)
    {
        homeAirportText.text = name;
    }

    public void UpdateDestinationIATAText(string iata)
    {
        destinationIATAText.text = iata;
    }

    public void UpdateDestinationAirportText(string name)
    {
        destinationAirportText.text = name;
    }

    public void UpdateFlightPrices(float price)
    {
        baseFlightCost = (int)price;
        flightCalcText.text = baseFlightCost.ToString("n0", ci) + "€ x " + guestsDD.options[guestsDD.value].text;
        flightCost = baseFlightCost * (guestsDD.value + 1);
        flightTotalText.text = flightCost.ToString("n0", ci) + "€";
        UpdateGrandTotalText();
    }

    public void UpdateDestinationAirport(Airport airport)
    {
        UpdateDestinationIATAText(airport.IATACode);
        UpdateDestinationAirportText(airport.airportName);
    }

    public void UpdateHotel()
    {
        Hotel hotel = DataHolderBehaviour.Instance.viewedHotel;
        //Debug.Log(hotel);
        if(hotel)
        {
            DataHolderBehaviour.Instance.selectedHotel = hotel;
            hotelText.text = hotel.hotelName + "\n";
            switch (hotel.hotelClass)
            {
                case 1:
                    hotelText.text += "\u22C6";
                    break;
                case 2:
                    hotelText.text += "\u22C6\u22C6";
                    break;
                case 3:
                    hotelText.text += "\u22C6\u22C6\u22C6";
                    break;
                case 4:
                    hotelText.text += "\u22C6\u22C6\u22C6\u22C6";
                    break;
                case 5:
                    hotelText.text += "\u22C6\u22C6\u22C6\u22C6\u22C6";
                    break;
                default: break;
            }
            baseHotelCost = hotel.price;
            hotelCalcText.text = baseHotelCost.ToString("n0", ci) + "€ x " + nightsDD.options[nightsDD.value].text + " x " + roomsDD.options[roomsDD.value].text;
            hotelCost = (baseHotelCost * (nightsDD.value + 1) * (roomsDD.value + 1));
            hotelTotalText.text = hotelCost.ToString("n0", ci) + "€";
            UpdateGrandTotalText();
        } else
        {
            hotelCalcText.text = "0€ x " + nightsDD.options[nightsDD.value].text + " x " + roomsDD.options[roomsDD.value].text;
        }
    }

    public void UpdateGrandTotalText()
    {
        grandTotalText.text = (flightCost + hotelCost).ToString("n0", ci) + "€";
    }

    /// <summary>
    /// Sets the number of guests in budget mode on Dropdown select
    /// </summary>
    public void ChangeGuestsNo(int value)
    {
        guestsDD.value = value;
        ChangeGuestsNo();
    }

    /// <summary>
    /// Sets the number of guests in budget mode on Dropdown select
    /// </summary>
    public void ChangeGuestsNo()
    {
        FilterBehaviour.Instance.SetGuestsNo(guestsDD.value + 1);
        roomsDD.options.Clear();
        roomsDD.options.Add(new Dropdown.OptionData("1 room"));
        for (int i = 1; i <= guestsDD.value; i++) roomsDD.options.Add(new Dropdown.OptionData((i + 1) + "rooms"));
        UpdateFlightPrices(baseFlightCost);
    }

    /// <summary>
    /// Sets the number of guests in budget mode on Dropdown select
    /// </summary>
    public void ChangeNightsNo()
    {
        FilterBehaviour.Instance.SetNightsNo(nightsDD.value + 1);
        UpdateHotel();
    }

    /// <summary>
    /// Sets the number of guests in budget mode on Dropdown select
    /// </summary>
    public void ChangeRoomsNo()
    {
        FilterBehaviour.Instance.SetRoomsNo(roomsDD.value + 1);
        UpdateHotel();
    }
}
