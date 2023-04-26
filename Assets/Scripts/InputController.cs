using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
///     Controls inputs and associated manipulation concerning the earth object
/// </summary>
public class InputController : MonoBehaviour {

    /// <summary>the main Camera</summary>
    public Camera mainCamera;

    /// <summary>the up position of the camera when facing the globe</summary>
    private Vector3 mainCameraUp = new Vector3(0, 1, 0);
    /// <summary>Saves the last position of the book to later calculate the movement delta with</summary>
    Vector3 lastMousePos = new Vector3(0,0,0);
    /// <summary>the zoom level of the earth in relation to the main camera. Also influences the rotation speed</summary>
    public float zoomLevel = 1;/// <summary>the initial zoom level of the earth in relation to the main camera. Also influences the rotation speed</summary>
    private float startZoomLevel = 1;
    /// <summary>the position to rotate to, if necessary</summary>
    public Quaternion rotateTo;
    /// <summary>the position to tilt to, if necessary</summary>
    public Quaternion tiltTo;
    /// <summary>the position to tilt from, if necessary</summary>
    public Quaternion tiltFrom;
    /// <summary>the position to rotate from, if necessary</summary>
    public Quaternion rotateFrom;
    /// <summary>the time elapsed during rotation</summary>
    public float rotateTime = 0;
    /// <summary>Zoom while rotating?</summary>
    public bool noZoom = false;
    /// <summary>true when the mouse is hovering over the earth object</summary>
    bool mouseOver = false;
    /// <summary>true when the mouse was hovering over the earth object when the right mouse button was originally clicked</summary>
    bool validDrag = false;
    /// <summary>Right Oculus controller</summary>
    private InputDevice rightController;
    /// <summary>Left Oculus controller</summary>
    private InputDevice leftController;
    /// <summary>Last tick's value of the right controller primatry button</summary>
    private bool lastPrimaryRValue;
    /// <summary>Value of the left controller primatry button</summary>
    public bool triggerL;
    /// <summary>Value of the right controller primatry button</summary>
    public bool triggerR;

    private int tickCounter;

    /// <summary>  Console GameObject </summary>
    public GameObject consoleObject;

    private List<Transform> destinationLabels;
    private GameObject destinationLineHolder;

    public Transform earthPlane;

    /// <summary>The only instance of this script</summary>
    public static InputController Instance { get; private set; }

    /// <summary>String for longer/multipart Debug Messages</summary>
    string Debugtext = "";

    void Awake()
    {
        Instance = this;
        Debugtext += "Awake: ok;\n";
        //GameObject.Find("XtraSpecialText").GetComponent<TextMeshProUGUI>().text = Debugtext;
    }

    private void Start()
    {
        rotateTo = transform.localRotation;
        rotateFrom = transform.localRotation;
        tiltTo = earthPlane.rotation;
        tiltFrom = earthPlane.rotation;

        TryInitialise();
        Debugtext += "Start: ok;\n";

        //GameObject.Find("XtraSpecialText").GetComponent<TextMeshProUGUI>().text = Debugtext;
        destinationLineHolder = GameObject.Find("DestinationLines");

        destinationLabels = new List<Transform>();
        foreach(Transform t in GameObject.Find("EarthGeoHolder").transform)
        {
            destinationLabels.Add(t);
        }

        UpdateCountryNameRotations();

        if (DataHolderBehaviour.Instance.cityLabelName.Length > 0)
        {
            GameObject cityLabel = GameObject.Find(DataHolderBehaviour.Instance.cityLabelName);
            cityLabel.GetComponent<CityLabelBehaviour>().ClickEvent();
        }
    }

    void Update()
    {
        System.DateTime before = System.DateTime.Now;

        Debugtext = Time.time + "\n";
        //get mouseOver status from the actual earth object
        mouseOver = MouseOverDetect.Instance.mouseOver;

        if (Input.GetMouseButtonDown(1))
        {
            lastMousePos = Input.mousePosition;
            Vector3 eulers = transform.eulerAngles;
            transform.rotation = Quaternion.identity;
            transform.Rotate(eulers);
            if (mouseOver)
            {
                validDrag = true;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            validDrag = false;
        }


        //right controller axis input
        Vector2 primary2DAxisRValue = Vector2.zero;
        if (rightController != null)
        {
            rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out primary2DAxisRValue);
        }
        //left controller axis input
        Vector2 primary2DAxisLValue = Vector2.zero;
        if (leftController != null)
        {
            leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out primary2DAxisLValue);
        }

        //rotate on right mouse down
        if ((validDrag && Input.GetMouseButton(1)) || Mathf.Abs(primary2DAxisRValue.magnitude) > .1f)
        {
            Debugtext += "rotating: " + primary2DAxisRValue.magnitude + "\n";

            Vector2 mousePos = Input.mousePosition;

            float speed = 0;
            if (primary2DAxisRValue.magnitude > 0)
            {
                speed = Mathf.Abs(primary2DAxisRValue.magnitude * 250) * .2f * 1 / (5 * zoomLevel);
            }
            else if (lastMousePos != Vector3.zero)
            {
                speed = Mathf.Min(Mathf.Abs(Vector3.Distance(lastMousePos, mousePos)) * .2f,10f) * 1/(5 * zoomLevel);
            }

            //limit globe rotation to 75° vertical in each direction
            float mouseTravelY = mousePos.y - lastMousePos.y;
            if (primary2DAxisRValue.magnitude > 0)
            {
                mouseTravelY = -primary2DAxisRValue.y * 15;
            }
            //Debug.Log("xrot: " + transform.eulerAngles.x + "; zrot: " + transform.eulerAngles.z + "; yMT: " + mouseTravelY);
            float eulerX = transform.eulerAngles.x;
            float eulerZ = transform.eulerAngles.z;

            if (eulerX > 180) eulerX -= 360;
            if (eulerZ > 180) eulerZ -= 360;

            if (Mathf.Sqrt((eulerX * eulerX) + (eulerZ * eulerZ)) > 65)
            {
                if ((eulerX > 0 && (transform.eulerAngles.y < 90 || transform.eulerAngles.y > 270) && mouseTravelY < 0) 
                    || (eulerX < 0 && (transform.eulerAngles.y < 90 || transform.eulerAngles.y > 270) && mouseTravelY > 0)
                    || (eulerX > 0 && transform.eulerAngles.y > 90 && transform.eulerAngles.y < 270 && mouseTravelY > 0)
                    || (eulerX < 0 && transform.eulerAngles.y > 90 && transform.eulerAngles.y < 270 && mouseTravelY < 0)) mouseTravelY = 0;
            }

            earthPlane.RotateAround(transform.position, Vector3.left, mouseTravelY * Time.deltaTime * speed);
            if (primary2DAxisRValue.magnitude > 0)
            {
                transform.RotateAround(transform.position, -transform.up, -primary2DAxisRValue.x * 10 * Time.deltaTime * speed);
            } else
            {
                transform.RotateAround(transform.position, -transform.up, (mousePos.x - lastMousePos.x) * Time.deltaTime * speed);
            }

            //update country names
            if(tickCounter++ % 16 == 0)
                UpdateCountryNameRotations();

            lastMousePos = Input.mousePosition;
            rotateTo = transform.localRotation;
            rotateFrom = transform.localRotation;
            tiltTo = earthPlane.rotation;
            tiltFrom = earthPlane.rotation;
            rotateTime = 0;
        } else
        {
            lastMousePos = Vector3.zero;

            if(Quaternion.Angle(rotateTo,transform.rotation) > .001f
                && Quaternion.Angle(tiltTo, earthPlane.rotation) > .001f)
            {
                //rotate towards position in 1.5s
                rotateTime = Mathf.Min(rotateTime + Time.deltaTime * .75f,1);
                transform.localRotation = Quaternion.Slerp(rotateFrom, rotateTo, rotateTime);
                earthPlane.rotation = Quaternion.Slerp(tiltFrom, tiltTo, rotateTime);
                if (Quaternion.Angle(rotateTo, transform.localRotation) <= .001f
                    && Quaternion.Angle(tiltTo, earthPlane.rotation) <= .001f)
                {
                    rotateTo = transform.localRotation;
                    rotateFrom = transform.localRotation;
                    tiltTo = earthPlane.rotation;
                    tiltFrom = earthPlane.rotation;
                    rotateTime = 0;
                }
                if (!noZoom)
                {
                    //always ends at max zoom level (19)
                    SetZoomLevel(zoomLevel + Time.deltaTime * (19 - startZoomLevel) * .5f);
                    if (tickCounter % 8 == 0)
                        UpdateCountryNameSize();
                }
                if (tickCounter++ % 16 == 0)
                    UpdateCountryNameRotations();
            } else
            {
                noZoom = false;
                rotateTo = transform.localRotation;
                rotateFrom = transform.localRotation;
                tiltTo = earthPlane.rotation;
                tiltFrom = earthPlane.rotation;
                rotateTime = 0;
                startZoomLevel = zoomLevel;
                if (tickCounter++ % 32 == 0)
                    UpdateCountryNameRotations();
            }
        }

        //scroll/zoom behaviour
        if (Mathf.Abs(primary2DAxisLValue.y) > .1f)
        {
            SetZoomLevel(zoomLevel + primary2DAxisLValue.y);
            if (tickCounter % 8 == 0)
                UpdateCountryNameSize();
        }
        else if (mouseOver == true && Input.mouseScrollDelta.y != 0)
        {
            SetZoomLevel(zoomLevel + Input.mouseScrollDelta.y);
            if (tickCounter % 8 == 0)
                UpdateCountryNameSize();
        }
        mouseOver = false;

        bool primaryButtonValue = false;
        if (rightController != null)
        {
            rightController.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonValue);
        }
        //on right primary button down
        /*if (Input.GetKeyDown(KeyCode.C) || (primaryButtonValue && (primaryButtonValue ^ lastPrimaryRValue)))
        {
            consoleObject.SetActive(!consoleObject.activeSelf);
            if(consoleObject.activeSelf && MainScreenBehaviour.Instance.currentDisplay == MainScreenBehaviour.Displays.FILTERS)
            {
                UserDataCollector.Instance.filterUseCount++;
            }
        }
        lastPrimaryRValue = primaryButtonValue;
        /*if (Input.GetKeyDown(KeyCode.R))
        {
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            rotateTo = transform.rotation;
            rotateFrom = transform.rotation;
            rotateTime = 0;
            SetZoomLevel(1);
        }*/

        if (!rightController.isValid || !leftController.isValid)
        {
            TryInitialise();
        }
        //GameObject.Find("XtraSpecialText").GetComponent<TextMeshProUGUI>().text = Debugtext;

        int ms = (System.DateTime.Now - before).Milliseconds;
        if(ms > 3) Debug.Log("Update: " + ms + "ms (" + tickCounter % 32 + ")");
    }

    private void LateUpdate()
    {
        //Get inputs from controllers
        if (rightController != null)
        {
            rightController.TryGetFeatureValue(CommonUsages.triggerButton, out triggerR);
        }
        if (leftController != null)
        {
            leftController.TryGetFeatureValue(CommonUsages.triggerButton, out triggerL);
        }
    }

    /// <summary>
    ///     Initialise controller setup
    /// </summary>
    private void TryInitialise()
    {
        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, inputDevices);
        if (inputDevices.Count > 0)
        {
            rightController = inputDevices[0];
        }
        inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left, inputDevices);
        if (inputDevices.Count > 0)
        {
            leftController = inputDevices[0];
        }
    }

    /// <summary>
    ///     Positions the country and city name tags to face upwards & towards the camera
    /// </summary>
    void UpdateCountryNameRotations()
    {
        List<GameObject> cnos = DisplayCountryNames.Instance.countryNameObjects;
        foreach (GameObject go in cnos)
        {
            go.transform.rotation = Quaternion.LookRotation(-(go.transform.position + mainCamera.transform.position), mainCameraUp);
            go.SetActive(go.transform.position.z > 0);
        }

        foreach (Transform t in destinationLabels)
        {
            t.rotation = Quaternion.LookRotation(-(t.position + mainCamera.transform.position), mainCameraUp);
        }

        /*foreach (Transform t in destinationLineHolder.transform)
        {
            t.GetChild(0).rotation = Quaternion.LookRotation((t.position + mainCamera.transform.position), mainCameraUp);
        }*/
    }

    /// <summary>
    ///     Positions the country and city name tags to face upwards & towards the camera
    /// </summary>
    void UpdateCountryNameSize()
    {
        Vector3 scale;
        if (zoomLevel > 10) {
            scale = Vector3.one * (1 - (.08f * (zoomLevel - 10)));
        } else
        {
            scale = Vector3.one;
        }
        List<GameObject> cnos = DisplayCountryNames.Instance.countryNameObjects;
        foreach (GameObject go in cnos)
        {
            go.transform.localScale = scale;
        }

        foreach (Transform t in destinationLabels)
        {
            t.localScale = scale * 2;
        }
    }

    /// <summary>
    ///     Adjusts the scale of the earth object on a scale from 0 (original size) to 20 (triple size)
    /// </summary>
    /// <param name="level">level to adjust to</param>
    public void SetZoomLevel(float level)
    {
        zoomLevel = Mathf.Min(Mathf.Max(1, level), 15f);

        //transform.position = Vector3.MoveTowards(initialPosition, Camera.main.transform.position, zoomLevel * 6);
        transform.localScale = Vector3.one * (.9f + zoomLevel / 10);

        //update country names
        List<GameObject> cnos = DisplayCountryNames.Instance.countryNameObjects;
        foreach (GameObject go in cnos)
        {
            go.GetComponent<TextMeshPro>().fontSize = 12 * Mathf.Pow(.95f, zoomLevel);
            go.transform.position = new Vector3(
            (50 + (21 - zoomLevel) * .1f) * go.transform.position.normalized.x,
            (50 + (21 - zoomLevel) * .1f) * go.transform.position.normalized.y,
            (50 + (21 - zoomLevel) * .1f) * go.transform.position.normalized.z) * transform.localScale.x;
        }

        foreach (Transform t in destinationLabels)
        {
            t.position = new Vector3(
            (50 + (20 - zoomLevel) * .1f) * t.position.normalized.x,
            (50 + (20 - zoomLevel) * .1f) * t.position.normalized.y,
            (50 + (20 - zoomLevel) * .1f) * t.position.normalized.z) * transform.localScale.x;
        }

        foreach (LineRenderer lr in destinationLineHolder.GetComponentsInChildren<LineRenderer>())
        {
            for (int i = 0; i < lr.positionCount; i++)
            {
                lr.SetPosition(i, lr.GetPosition(i).normalized * (50 + (20 - zoomLevel) * .1f));
            }
        }

        //set airport label size
        float zoomScale = 2.2f - (zoomLevel * .1f);
        //AirportChoice.Instance.Label.transform.localScale = Vector3.one * zoomScale;
    }

    /// <summary>
    /// Send a haptic signal to the left controller
    /// </summary>
    public void DoHapticsLeft()
    {
        leftController.SendHapticImpulse(0, .5f, .2f);
    }

    /// <summary>
    /// Send a haptic signal to the right controller
    /// </summary>
    public void DoHapticsRight()
    {
        rightController.SendHapticImpulse(0, .5f, .2f);
    }

    /// <summary>
    /// Rotate to and zoom in to a given position
    /// </summary>
    /// <param name="lat">Latitude to rotate to</param>
    /// <param name="lon">Longitude to rotate to</param>
    /// <param name="noZoom">Zoom in when rotating?</param>
    public void HomeIn(double lat, double lon, bool noZoom)
    {
        rotateTo = Quaternion.Euler(0, (float)lon, 0);
        tiltTo = Quaternion.Euler((float)lat, 0, 0);
        this.noZoom = noZoom;
    }

    /// <summary>
    /// Rotate globe left a set amount
    /// </summary>
    public void ClickLeft()
    {
        rotateTo = Quaternion.Euler(0, rotateTo.eulerAngles.y - (5 - zoomLevel / 5), 0);
        transform.localRotation = rotateTo;
        rotateFrom = rotateTo;
        noZoom = true;
        UpdateCountryNameRotations();
    }

    /// <summary>
    /// Rotate globe right a set amount
    /// </summary>
    public void ClickRight()
    {
        rotateTo = Quaternion.Euler(0, rotateTo.eulerAngles.y + (5 - zoomLevel / 5), 0);
        transform.localRotation = rotateTo;
        rotateFrom = rotateTo;
        noZoom = true;
        UpdateCountryNameRotations();
    }

    /// <summary>
    /// Rotate globe up a set amount
    /// </summary>
    public void ClickUp()
    {
        tiltTo = Quaternion.Euler(tiltTo.eulerAngles.x + (5 - zoomLevel / 5), 0, 0);
        earthPlane.localRotation = tiltTo;
        tiltFrom = tiltTo;
        noZoom = true;
        UpdateCountryNameRotations();
    }

    /// <summary>
    /// Rotate globe down a set amount
    /// </summary>
    public void ClickDown()
    {
        tiltTo = Quaternion.Euler(tiltTo.eulerAngles.x - (5 - zoomLevel / 5), 0, 0);
        earthPlane.localRotation = tiltTo;
        tiltFrom = tiltTo;
        noZoom = true;
        UpdateCountryNameRotations();
    }

    /// <summary>
    /// Zoom in to the globe by a set amount
    /// </summary>
    public void ClickIn()
    {
        SetZoomLevel(zoomLevel + 1);
        UpdateCountryNameSize();
    }

    /// <summary>
    /// Zoom out of the globe by a set amount
    /// </summary>
    public void ClickOut()
    {
        SetZoomLevel(zoomLevel - 1);
        UpdateCountryNameSize();
    }
}
