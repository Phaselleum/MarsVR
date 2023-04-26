using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FavouritesBehaviour : MonoBehaviour
{
    /// <summary> List of favourite hotels </summary>
    List<Hotel> hotels;
    /// <summary> List of codes of favourite hotels, as saved </summary>
    List<string> hotelCodes;
    /// <summary> List of country names of favourite hotels </summary>
    List<string> hotelCountries;
    /// <summary> List of countries of favourite hotels </summary>
    List<EarthEngineCountry> hotelEacs;
    /// <summary> Current page of favourites to display </summary>
    int currentPage = 0;

    /// <summary> Text components of favourites entries for hotel details </summary>
    public Text[] entries;
    /// <summary> Button to next page of favourites entries </summary>
    public Button nextEntries;
    /// <summary> Button to previous page of favourites entries </summary>
    public Button prevEntries;

    /// <summary> Screen to enter email (not used) </summary>
    public GameObject mailEntry;
    /// <summary> Screen after successfully sending email (not used) </summary>
    public GameObject mailSuccess;

    /// <summary> Regular star/favourite sprite </summary>
    public Sprite star;
    /// <summary> Greyed out star/favourite sprite </summary>
    public Sprite star2;

    private int counter;

    public GameObject keyboard;

    private void Start()
    {
        hotels = new List<Hotel>();
        hotelCodes = new List<string>();

        if (PlayerPrefs.HasKey("favourites") && !DataHolderBehaviour.Instance.hotelFavouritesLoaded)
        {
            StartCoroutine(GetHotelData((UnityWebRequest req) => {
                if (req.error != null)
                {
                    Debug.Log($"{req.error}: {req.downloadHandler.text}");
                }
                else
                {
                    try
                    {
                        string jsonString = req.downloadHandler.text.Substring(1, req.downloadHandler.text.Length - 2);
                        string[] jsonArray = jsonString.Split(",{");
                        List<Hotel> hotelList = new List<Hotel>();
                        Debug.Log("Received " + jsonArray.Length + " hotels for favourites list from the server.");
                        foreach (string json in jsonArray)
                        {
                            string json2 = json;
                            if (json[0] != '{') json2 = '{' + json2;
                            Hotel h = ScriptableObject.CreateInstance<Hotel>();
                            JsonUtility.FromJsonOverwrite(json2, h);
                            h.name = h.hotelCode;
                            hotelList.Add(h);
                        }
                        DataHolderBehaviour.Instance.allHotels.AddRange(hotelList);
                        DataHolderBehaviour.Instance.hotelFavouritesLoaded = true;
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Formatting problem with loaded data - " + e.Message);
                        Debug.Log(req.downloadHandler.text);
                    }
                }
            }, PlayerPrefs.GetString("favourites")));
        }
    }

    private void Update()
    {
        counter++;
        if(counter % 48 == 0 && MainScreenBehaviour.Instance.currentDisplay == MainScreenBehaviour.Displays.FAVOURITES)
        {
            ClickHandler();
        }
    }

    /// <summary>
    /// Handle Favourite menu selected. Loads the favourite hotels in lists and displays the favourite screen on the main screen.
    /// </summary>
    public void ClickHandler()
    {
        MainScreenBehaviour.Instance.ChangeDisplay(MainScreenBehaviour.Displays.FAVOURITES);
        if (PlayerPrefs.HasKey("favourites"))
        {
            hotels.Clear();
            hotelCodes.Clear();
            hotelCodes.AddRange(PlayerPrefs.GetString("favourites").Split(','));
            for (int i = 0; i < hotelCodes.Count; i++)
            {
                foreach(Hotel h in DataHolderBehaviour.Instance.allHotels)
                {
                    if(h.hotelCode == hotelCodes[i])
                    {
                        hotels.Add(h);
                    }
                }
            }
            DrawHotels();
        }
    }

    /// <summary>
    /// Displays the next page of favourites
    /// </summary>
    public void NextPage()
    {
        currentPage = Mathf.Min(hotels.Count / 4, currentPage + 1);
        DrawHotels();
    }

    /// <summary>
    /// Displays the previous page of favourites
    /// </summary>
    public void PrevPage()
    {
        currentPage = Mathf.Max(0, currentPage - 1);
        DrawHotels();
    }

    /// <summary>
    /// Displays the email entry screen (not currently used)
    /// </summary>
    public void ShowMailDialogue()
    {
        foreach(Text t in entries)
        {
            t.transform.parent.gameObject.SetActive(false);
        }
        mailEntry.SetActive(true);
        mailEntry.GetComponentInChildren<InputField>().text = PlayerPrefs.GetString("email", "");
        keyboard.SetActive(true);
        keyboard.GetComponent<KeyboardObjectBehaviour>().inputField = mailEntry.GetComponentInChildren<InputField>();
    }

    /// <summary>
    /// Displays the email success screen (not currently used)
    /// </summary>
    public void ShowMailSuccess()
    {
        mailEntry.SetActive(false);
        mailSuccess.SetActive(true);
    }

    /// <summary>
    /// Sends a list of favourites via email (not currently used)
    /// </summary>
    public void SendMail()
    {
        PlayerPrefs.SetString("email", mailEntry.GetComponentInChildren<InputField>().text);
        MailManager mm = gameObject.AddComponent<MailManager>();
        string contents = "This is your personal list of favourite hotels chosen in WeezyVR!\n\n";

        for (int i = 0; i < hotels.Count; i++)
        {
            contents += "<div class='hotelEntry' style='padding:10px 5px;margin:5px 10px;background:#DDD;font-family:sans-serif;width:fit-content;'><b>" + hotels[i].hotelName + "</b> " + new string('\u22C6', hotels[i].hotelClass) + "<br>"
                + hotels[i].price + "€ per night<br>";
			if(hotels[i].pool) contents += "pool - ";
			if(hotels[i].restaurant) contents += "restaurant - ";
			if(hotels[i].familyFriendly) contents += "family friendly - ";
			if(hotels[i].gym) contents += "gym - ";
			if(hotels[i].accessible) contents += "accessible - ";
			if(hotels[i].wifi) contents += "wifi - ";
			if(hotels[i].beachProximity) contents += "beach proximity - ";
			if(hotels[i].petFriendly) contents += "pet friendly - ";
			if(hotels[i].spa) contents += "spa - ";
			if(hotels[i].carPark) contents += "car park - ";
            contents += hotels[i].city + ", " + hotelCountries[i] + "</div>";
        }

        mm.SendMail(contents);
    }

    /// <summary>
    /// Closes the email entry screen, displays the favourites screen.
    /// </summary>
    public void CancelMail()
    {
        mailEntry.SetActive(false);
        DrawHotels();
    }

    /// <summary>
    /// Removes a hotel from the favourites lists and updates the display
    /// </summary>
    /// <param name="id">index in the favourits lists</param>
    public void UnFavourite(int id)
    {

        hotelCodes.RemoveAt(id + currentPage * 4);
        hotelCountries.RemoveAt(id + currentPage * 4);
        hotelEacs.RemoveAt(id + currentPage * 4);
        hotels.RemoveAt(id + currentPage * 4);
        PlayerPrefs.SetString("favourites", string.Join(",", hotelCodes.ToArray()));
        if (id == hotels.Count) PrevPage();
        else DrawHotels();
    }

    /// <summary>
    /// Navigate to the destination associated with a selected hotel and open the hotels list on the page displaying it.
    /// </summary>
    /// <param name="id"></param>
    public void SelectFavourite(int id)
    {
        id += currentPage * 4;
        EarthEngineCity city = null;
        float distance = float.MaxValue;
        foreach (EarthEngineCity eac in EarthEngineCityController.Instance.interactableCities)
        {
            if (eac.cityCode == hotels[id].hotelCode)
            {
				city = eac;
				break;
            } else if(eac.interactable && hotels[id].hotelCode.StartsWith(eac.cityCode))
			{
				city = eac;
				break;
			}
        }
        //Debug.Log(city);
        city.Label.GetComponent<CityLabelBehaviour>().ClickEvent();
        InfoScreenTopManager.Instance.ShowHotelDetails(hotels[id]);
    }

    /// <summary>
    /// Fill in the entries of the favourites screen according to the current selection and page.
    /// </summary>
    public void DrawHotels()
    {
        foreach(Text text in entries)
        {
            text.transform.parent.gameObject.SetActive(false);
        }
        for (int i = currentPage * 4; i < Mathf.Min(hotels.Count, 4 * (currentPage + 1)); i++)
        {
            if(i < hotels.Count)
            {
                entries[i % 4].transform.parent.gameObject.SetActive(true);
                entries[i % 4].text = hotels[i].hotelName + " " + new string('\u22C6', hotels[i].hotelClass) + "\n"
                    + hotels[i].city + ", " + hotelCountries[i];
            }
        }

        prevEntries.interactable = currentPage > 0;
        nextEntries.interactable = (currentPage + 1) * 4 < hotels.Count;
    }

    /// <summary>
    /// Get hotel image from server coroutine function
    /// </summary>
    /// <param name="callback">Callback after receiving server response</param>
    /// <returns></returns>
    private IEnumerator GetHotelData(Action<UnityWebRequest> callback, string hotelCodes)
    {
        string url = "http://" + DataHolderBehaviour.Instance.serverIP.Trim() + "/get-select-hotel-data/" + hotelCodes;
        Debug.Log("connecting to: " + url);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            callback(webRequest);
        }
    }
}
