using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Hotel", menuName = "Weezy/Hotel", order = 1)]
[Serializable]
public class Hotel : ScriptableObject
{
    /// <summary> Name of the Hotel </summary>
    public string hotelName;
    /// <summary> Internal code of the Hotel </summary>
    public string hotelCode;
    /// <summary> Name of the Hotel's city </summary>
    public string city;
    /// <summary> Image of the Hotel </summary>
    public Sprite hotelImage;
    public string[] imageURLs;
    /// <summary> Video of the Hotel </summary>
    public Video video;

    /// <summary> Latitude of the Hotel </summary>
    public double latitude;
    /// <summary> Longitude of the Hotel </summary>
    public double longitude;

    /// <summary> Price per night in Euros </summary>
    public int price;
    public string currency;
    /// <summary> Guest rating out of 10 </summary>
    public float rating;
    /// <summary> Address of the hotel property </summary>
    public string propertyAddress;
    /// <summary> Hotel class rating (3,4,5) </summary>
    public byte hotelClass;

    public string[] ammenities;

    /// <summary> Does the hotel have a pool? </summary>
    public bool pool;
    /// <summary> Does the hotel have a restaurant? </summary>
    public bool restaurant;
    /// <summary> Is the hotel family friendly? </summary>
    public bool familyFriendly;
    /// <summary> Does the hotel have a gym? </summary>
    public bool gym;
    /// <summary> Is the hotel accessible? </summary>
    public bool accessible;
    /// <summary> Does the hotel have WiFi? </summary>
    public bool wifi;
    /// <summary> Is the hotel close to a beach? </summary>
    public bool beachProximity;
    /// <summary> Is the hotel pet friendly? </summary>
    public bool petFriendly;
    /// <summary> Does the hotel have a spa? </summary>
    public bool spa;
    /// <summary> Does the hotel have a car park? </summary>
    public bool carPark;

    /// <summary> distance in km (only used internally) </summary>
    public double distance;

    /// <summary> Tour of the Hotel </summary>
    public Slideshow slideshow;

    /// <summary> Descriptive text of the Hotel </summary>
    [TextArea(6, 6)]
    [FormerlySerializedAs("infoText")]
    public string description;
}
