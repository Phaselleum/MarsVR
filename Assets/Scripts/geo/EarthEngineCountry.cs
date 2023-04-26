using UnityEngine;

/// <summary>
/// A country object
/// </summary>
public class EarthEngineCountry : MonoBehaviour
{
    /// <summary>coordinates of the country</summary>
	public Vector2[] coords;
    /// <summary>Country's ADM3 code</summary>
	public string ADM0;
    /// <summary>detail level of country borders (1 is most detailed)</summary>
	private int detail = 10;
    /// <summary>full name of the country</summary>
	public string countryName;
    /// <summary>default border line colour</summary>
    public Color32 lineColor = new Color32(255, 255, 255, 255);

	public Vector3 labelPos;

    /// <summary>LineRenderer that renders the border line</summary>
    private LineRenderer _line;
    /// <summary>individual parts of the country</summary>
	private Transform[] _countryParts;
    /// <summary>Linrenderers of individual country parts</summary>
	private LineRenderer[] _lineRenderers;
    /// <summary>has drawing the border lines been initiated?</summary>
	private bool _linescreated = false;
    /// <summary>Should border lines be drawn?</summary>
	public bool interactable = false;
	/// <summary>Array of airports in the country</summary>
	public Airport[] airports;

	void Start()
	{
		if (interactable)
		{
			//System.DateTime before = System.DateTime.Now;
			//StartHighLight();
			//int ms = System.DateTime.Now.Subtract(before).Milliseconds;
			//if(ms > 1) Debug.Log(ADM0 + ": " + ms);
		}
	}

    /// <summary>
    /// Manages the drawing of border lines throughout all country parts
    /// </summary>
	public void StartHighLight() {
		if (!_linescreated)
        {
            // Draw lines
            CreateLines();
			_linescreated = true;
		}

		for (int i = 0; i < _lineRenderers.Length-1; i++) {
            _lineRenderers[i].startWidth = StagitMainEarth.Instance.CountryBorderWidth;
			_lineRenderers[i].endWidth = StagitMainEarth.Instance.CountryBorderWidth;
            _lineRenderers[i].startColor = lineColor;
            _lineRenderers[i].endColor = lineColor;

			_lineRenderers[i].transform.localScale = new Vector3(1, 1, 1);
			_lineRenderers[i].transform.localPosition = new Vector3(0, 0, 0);
			_lineRenderers[i].useWorldSpace = false;
			//_lineRenderers[i].gameObject.transform.LookAt(labelPos);
			//_lineRenderers[i].alignment = LineAlignment.TransformZ;
		}
		//transform.LookAt(-labelPos);
	}

	/// <summary>
	/// Hides the boder lines
	/// </summary>
	public void StopHighLight() {
		for (int i = 0; i < _lineRenderers.Length-1; i++) {
			_lineRenderers[i].startWidth = 0f;
			_lineRenderers[i].endWidth =  0f;
		}
	}

    /// <summary>
    /// Draws the country border lines
    /// </summary>
	void CreateLines() {
		GameObject earth = GameObject.Find ("EarthObject");

		GeoLocator geo = new GeoLocator ();

		Component[] components = GetComponents(typeof(EarthEngineEarthVectors));

		// Since there can only be one linerender on an object we need to create different gameobjects
		// Countries  contain multiple parts for example islands
		foreach (EarthEngineEarthVectors component in components) {
			GameObject countrypart = new GameObject ("CountryPart");
			countrypart.transform.parent = transform;
		}

		_countryParts = GetComponentsInChildren<Transform> ();
		_lineRenderers = new LineRenderer[_countryParts.Length];

		// fill vector3 array with all country world positions from the lat long positions
		int c = 0;
		foreach (EarthEngineEarthVectors component in components) {
			Vector3[] vertices3D_main;
			vertices3D_main = new Vector3[component.poslat.Length / detail];

            for (int i = 0; i < (component.poslat.Length) / detail; i++) {
				if (i == 0) {
					transform.position = earth.transform.position + 
                        geo.GetVectorFromLatLong (earth.transform.localScale.x * 100 + StagitMainEarth.Instance.CountryBorderOffset, component.poslat [i], component.poslong [i]);
                }
                if (component.poslat [i ] != 0f) {
					Vector3 startpos = earth.transform.position + 
                        geo.GetVectorFromLatLong (earth.transform.localScale.x * 100 + StagitMainEarth.Instance.CountryBorderOffset, component.poslat [i * detail], component.poslong [i * detail]);

                    vertices3D_main[i] = startpos;
				}
			}

			_line = _countryParts[c].gameObject.AddComponent <LineRenderer> ();

			_line.material = StagitMainEarth.Instance.CountryLineMaterial;

			// Set the width to 0.0 first as we will use it later to finally highlight a country
			_line.startWidth = 0.0f;
			_line.endWidth = 0.0f;

			// Set all vector3 positions of the line
			//_line.SetVertexCount (vertices3D_main.Length);
			_line.positionCount = vertices3D_main.Length;
			for (int i = 0; i < vertices3D_main.Length; i++) {
				_line.SetPosition (i, vertices3D_main [i]);
			}
			_lineRenderers [c] = _line;
			c++;
		}
	}
}