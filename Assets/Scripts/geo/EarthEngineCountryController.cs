using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the EarthEngineCountries
/// </summary>
public class EarthEngineCountryController : MonoBehaviour
{
    /// <summary>The only instance of this script</summary>
    public static EarthEngineCountryController Instance { get; private set; }

	private List<EarthEngineCountry> countries;
	private List<EarthEngineSovCountry> sovCountries;

    void Awake() {
		Instance = this;

		countries = new List<EarthEngineCountry>();
		sovCountries = new List<EarthEngineSovCountry>();
		foreach (Transform child in transform)
		{
			countries.Add(child.GetComponent<EarthEngineCountry>());
		}
	}

    /// <summary>
    /// Accumulates all countries with a given name
    /// </summary>
    /// <param name="countryName">the name of the country to search for</param>
    /// <returns>Returns all countries associated with the given name. Returns null if none are found.</returns>
    public EarthEngineCountry SearchCountryByName(string countryName) {
		EarthEngineCountryData earthEngineCountryData = new EarthEngineCountryData ();

		string adm3Country = earthEngineCountryData.GetAdmin3FromCountry(countryName);
		if (adm3Country != "") {
			Debug.Log ("FOUND ADM3:" + adm3Country);
			return GetCountryByADM3(adm3Country);
		}
		return null;
	}

    /// <summary>
    /// Accumulates all countries with a given ADM3 code
    /// </summary>
    /// <param name="ADM3">the ADM3 code of the country to search for</param>
    /// <returns>Returns all countries associated with the given ADM3 code. Returns null if none are found.</returns>
    public EarthEngineCountry GetCountryByADM3(string ADM3) {
		foreach(EarthEngineCountry eac in countries)
		{
			if (eac.ADM0 == ADM3) {
				return eac;
			}
		}
		return null;
	}

    public EarthEngineCountry[] GetSiblingCountriesByADM3(string ADM3) {
		foreach(EarthEngineSovCountry easc in sovCountries)
		{
			if (easc.adminA3 == ADM3) {
				return easc.earthEngineCountry;
			}
		}
		return null;
	}
}

