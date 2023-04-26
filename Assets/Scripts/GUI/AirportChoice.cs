using UnityEngine;
using TMPro;
using System;

public class AirportChoice : MonoBehaviour
{
    public Airport CPH;
    public Airport RKE;
    public Airport HEL;
    public Airport KEF;
    public Airport RKV;
    public Airport BGO;
    public Airport OSL;
    public Airport RYG;
    public Airport TRF;
    public Airport ARN;
    public Airport BMA;
    public Airport GOT;
    public Airport NYO;
    public Airport VST;

    public Airport FRA;
    public Airport BER;
    public Airport GRZ;
    public Airport HAM;
    public Airport MUC;
    public Airport ZRH;
    public Airport SZG;
    public Airport VIE;
    public Airport CDG;
    public Airport BOD;
    public Airport BVA;
    public Airport MRS;
    public Airport ORY;
    public Airport GVA;

    public Airport AMS;
    public Airport ANR;
    public Airport BRU;
    public Airport CRL;
    public Airport RTM;
    public Airport LHR;
    public Airport DUB;
    public Airport CWL;
    public Airport EDI;
    public Airport LCY;
    public Airport LGW;
    public Airport LTN;
    public Airport SEN;
    public Airport STN;

    public Airport SVO;
    public Airport PRG;
    public Airport BUD;
    public Airport WAW;
    public Airport OTP;
    public Airport DME;
    public Airport LED;
    public Airport VKO;
    public Airport ZIA;
    public Airport BTS;

    public Airport BGY;
    public Airport FCO;
    public Airport CIA;
    public Airport LIN;
    public Airport MXP;
    public Airport NAP;
    public Airport TSF;
    public Airport VCE;
    public Airport LIS;
    public Airport OPO;
    public Airport ATH;
    public Airport BCN;
    public Airport MAD;
    public Airport PMI;

    public Airport DXB;
    public Airport HND;
    public Airport DEL;
    public Airport IST;
    public Airport GYD;
    public Airport PVG;
    public Airport SHA;
    public Airport BOM;
    public Airport CCU;
    public Airport DPS;
    public Airport CTS;
    public Airport ITM;
    public Airport KIX;
    public Airport NRT;
    public Airport OKD;
    public Airport MFM;
    public Airport GAN;
    public Airport HAQ;
    public Airport MLE;
    public Airport MNL;
    public Airport SIN;
    public Airport GMP;
    public Airport ICN;
    public Airport PUS;
    public Airport BKK;
    public Airport DMK;
    public Airport HKT;
    public Airport AYT;
    public Airport ESB;
    public Airport SAW;
    public Airport DWC;

    public Airport AEP;
    public Airport EZE;
    public Airport CGH;
    public Airport GIG;
    public Airport GRU;
    public Airport SDU;
    public Airport CSL;
    public Airport CUN;
    public Airport MEX;
    public Airport LIM;
    public Airport PDP;

    public Airport NAS;
    public Airport BGI;
    public Airport SJO;
    public Airport SYQ;
    public Airport HAV;
    public Airport JBQ;
    public Airport POP;
    public Airport PUJ;
    public Airport SDQ;
    public Airport KIN;

    public Airport JFK;
    public Airport LAX;
    public Airport YHM;
    public Airport YHU;
    public Airport YKF;
    public Airport YMX;
    public Airport YTZ;
    public Airport YUL;
    public Airport YYZ;
    public Airport BWI;
    public Airport IAD;
    public Airport EWR;
    public Airport SWF;
    public Airport LGA;
    public Airport LAS;
    public Airport MIA;
    public Airport DCA;
    public Airport SFO;

    public Airport AVV;
    public Airport BNE;
    public Airport MEB;
    public Airport MEL;
    public Airport PER;
    public Airport SYD;
    public Airport BOB;
    public Airport AKL;
    public Airport CHC;
    public Airport WLG;

    public Airport CAI;
    public Airport SSH;
    public Airport NBO;
    public Airport MBA;
    public Airport WIL;
    public Airport MRU;
    public Airport RRG;
    public Airport PRI;
    public Airport SEZ;
    public Airport CPT;
    public Airport HLA;
    public Airport JNB;
    public Airport JRO;
    public Airport LVI;

    /// <summary>Text in the GUI showing the current home airport name</summary>
    public TextMeshProUGUI TMPAirportName;

    /// <summary>The prefab for the image label for the airport</summary>
    public GameObject labelPrefab;
    /// <summary>The image label for the airport</summary>
    public GameObject Label { get; private set; }
    /// <summary>the up position of the camera when facing the globe</summary>
    private Vector3 mainCameraUp = new Vector3(0, 1, 0);
    /// <summary>the currently selected airport (defaults to Los Santos International)</summary>
    public Airport currentAirport;

    /// <summary>Holds individual regional selection screens for airports</summary>
    public GameObject SelectionTree;
    /// <summary>Overview screen for region selection</summary>
    public GameObject RegionButtons;

    /// <summary>ID of currently chosen airport</summary>
    public int choice;


    public static AirportChoice Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        //setup the current airport and its label#
        string code = "ZRH";
        if (PlayerPrefs.HasKey("Airport"))
            code = PlayerPrefs.GetString("Airport");
        else PlayerPrefs.SetString("Airport", code);
        SetAirport(code);
    }

    private void Start()
    {
        //fly to airport at start or fly to last visited city on return from another scene
        if (DataHolderBehaviour.Instance.cityLabelName.Length == 0)
        {
            InputController.Instance.noZoom = true;
            InputController.Instance.rotateTo = Quaternion.Euler(0, (float)currentAirport.longitude, 0);
            InputController.Instance.tiltTo = Quaternion.Euler((float)currentAirport.latitude, 0, 0);
        } else
        {
            GameObject cityLabel = GameObject.Find(DataHolderBehaviour.Instance.cityLabelName);
            cityLabel.GetComponent<CityLabelBehaviour>().ClickEvent();
            InputController.Instance.consoleObject.SetActive(false);
        }
    }

    /// <summary>
    ///     Handles Click/Trigger events
    /// </summary>
    public void ClickHandler()
    {
        MainScreenBehaviour.Instance.ChangeDisplay(MainScreenBehaviour.Displays.AIRPORTS);
    }

    /// <summary>
    /// Sets the home airport
    /// </summary>
    /// <param name="code">IATA code of the airport to set</param>
    public void SetAirport(string code)
    {
        currentAirport = GetAirportFromString(code, true);
        //use predefined list to get ID on initial startup
        if (DataHolderBehaviour.Instance.flightPricesLoaded)
            choice = Array.IndexOf(DataHolderBehaviour.Instance.departureAirports, code);
        else choice = Array.IndexOf("AEP,EZE,BNE,MEL,MEB,AVV,PER,SYD,GRZ,SZG,VIE,GYD,NAS,BGI,ANR,BRU,CRL,SDU,GIG,GRU,CGH,YMX,YHU,YUL,YHM,YKF,YTZ,YYZ,SHA,PVG,SJO,SYQ,HAV,PRG,CPH,RKE,POP,PUJ,SDQ,JBQ,CAI,SSH,HEL,BOD,MRS,CDG,ORY,BVA,BOB,BER,FRA,HAM,MUC,ATH,BUD,RKV,KEV,CCU,BOM,DEL,DPS,DUB,LIN,MXP,BGY,NAP,FCO,CIA,TSF,VCE,KIN,ITM,KIX,OKD,CTS,HND,NRT,MBA,NBO,WIL,MFM,MLE,GAN,HAQ,MRU,RRG,CSL,CUN,MEX,AMS,RTM,AKL,CHC,WLG,BGO,OSL,RYG,TRF,LIM,MNL,WAW,LIS,OPO,OTP,LED,SVO,DME,VKO,ZIA,SEZ,PRI,SIN,BTS,CPT,JNB,HLA,PUS,GMP,ICN,BCN,MAD,PMI,GOT,ARN,BNA,NYO,VST,GVA,ZRH,JRO,BKK,HKT,DMK,AYT,ESB,IST,SAW,DXB,DWC,CWL,EDI,LHR,LCY,LGW,LTN,SEN,STN,LAS,LAX,MIA,JFK,SFO,DCA,BWI,EWR,IAD,LGA,SWF,PDP,LVI".Split(','), code);
        PlayerPrefs.SetString("Airport", code);
        Debug.Log("Set " + code + " as the home airport!");

        //rotate the Earth so the airport faces the player
        InputController.Instance.noZoom = true;
        InputController.Instance.rotateTo = Quaternion.Euler(0, (float)currentAirport.longitude, 0);
        InputController.Instance.tiltTo = Quaternion.Euler((float)currentAirport.latitude, 0, 0);

        TravelOverviewBehaviour.Instance.UpdateHomeIATAText(currentAirport.IATACode);
        TravelOverviewBehaviour.Instance.UpdateHomeAirportText(currentAirport.airportName);

        SetAirportUI();
    }

    /// <summary>
    /// Get an airport from its IATA Code
    /// </summary>
    /// <param name="code">IATA Code</param>
    /// <returns></returns>
    public Airport GetAirportFromString(string code, bool setHome)
    {
        Airport airport = null;
        switch (code)
        {
            case "CPH":
                if(setHome) TMPAirportName.text += CPH.airportName;
                airport = CPH;
                break;
            case "RKE":
                if(setHome) TMPAirportName.text += RKE.airportName;
                airport = RKE;
                break;
            case "HEL":
                if(setHome) TMPAirportName.text += HEL.airportName;
                airport = HEL;
                break;
            case "KEF":
                if(setHome) TMPAirportName.text += KEF.airportName;
                airport = KEF;
                break;
            case "RKV":
                if(setHome) TMPAirportName.text += RKV.airportName;
                airport = RKV;
                break;
            case "BGO":
                if(setHome) TMPAirportName.text += BGO.airportName;
                airport = BGO;
                break;
            case "OSL":
                if(setHome) TMPAirportName.text += OSL.airportName;
                airport = OSL;
                break;
            case "RYG":
                if(setHome) TMPAirportName.text += RYG.airportName;
                airport = RYG;
                break;
            case "TRF":
                if(setHome) TMPAirportName.text += TRF.airportName;
                airport = TRF;
                break;
            case "ARN":
                if(setHome) TMPAirportName.text += ARN.airportName;
                airport = ARN;
                break;
            case "BMA":
                if(setHome) TMPAirportName.text += BMA.airportName;
                airport = BMA;
                break;
            case "GOT":
                if(setHome) TMPAirportName.text += GOT.airportName;
                airport = GOT;
                break;
            case "NYO":
                if(setHome) TMPAirportName.text += NYO.airportName;
                airport = NYO;
                break;
            case "VST":
                if(setHome) TMPAirportName.text += VST.airportName;
                airport = VST;
                break;

            case "FRA":
                if(setHome) TMPAirportName.text += FRA.airportName;
                airport = FRA;
                break;
            case "BER":
                if(setHome) TMPAirportName.text += BER.airportName;
                airport = BER;
                break;
            case "ZRH":
                if(setHome) TMPAirportName.text += ZRH.airportName;
                airport = ZRH;
                break;
            case "CDG":
                if(setHome) TMPAirportName.text += CDG.airportName;
                airport = CDG;
                break;
            case "BOD":
                if(setHome) TMPAirportName.text += BOD.airportName;
                airport = BOD;
                break;
            case "BVA":
                if(setHome) TMPAirportName.text += BVA.airportName;
                airport = BVA;
                break;
            case "MRS":
                if(setHome) TMPAirportName.text += MRS.airportName;
                airport = MRS;
                break;
            case "ORY":
                if(setHome) TMPAirportName.text += ORY.airportName;
                airport = ORY;
                break;
            case "HAM":
                if(setHome) TMPAirportName.text += HAM.airportName;
                airport = HAM;
                break;
            case "MUC":
                if(setHome) TMPAirportName.text += MUC.airportName;
                airport = MUC;
                break;
            case "GRZ":
                if(setHome) TMPAirportName.text += GRZ.airportName;
                airport = GRZ;
                break;
            case "SZG":
                if(setHome) TMPAirportName.text += SZG.airportName;
                airport = SZG;
                break;
            case "VIE":
                if(setHome) TMPAirportName.text += VIE.airportName;
                airport = VIE;
                break;
            case "GVA":
                if(setHome) TMPAirportName.text += GVA.airportName;
                airport = GVA;
                break;

            case "AMS":
                if(setHome) TMPAirportName.text += AMS.airportName;
                airport = AMS;
                break;
            case "LHR":
                if(setHome) TMPAirportName.text += LHR.airportName;
                airport = LHR;
                break;
            case "ANR":
                if(setHome) TMPAirportName.text += ANR.airportName;
                airport = ANR;
                break;
            case "BRU":
                if(setHome) TMPAirportName.text += BRU.airportName;
                airport = BRU;
                break;
            case "CRL":
                if(setHome) TMPAirportName.text += CRL.airportName;
                airport = CRL;
                break;
            case "RTM":
                if(setHome) TMPAirportName.text += RTM.airportName;
                airport = RTM;
                break;
            case "DUB":
                if(setHome) TMPAirportName.text += DUB.airportName;
                airport = DUB;
                break;
            case "CWL":
                if(setHome) TMPAirportName.text += CWL.airportName;
                airport = CWL;
                break;
            case "EDI":
                if(setHome) TMPAirportName.text += EDI.airportName;
                airport = EDI;
                break;
            case "LGW":
                if(setHome) TMPAirportName.text += LGW.airportName;
                airport = LGW;
                break;
            case "LCY":
                if(setHome) TMPAirportName.text += LCY.airportName;
                airport = LCY;
                break;
            case "LTN":
                if(setHome) TMPAirportName.text += LTN.airportName;
                airport = LTN;
                break;
            case "SEN":
                if(setHome) TMPAirportName.text += SEN.airportName;
                airport = SEN;
                break;
            case "STN":
                if(setHome) TMPAirportName.text += STN.airportName;
                airport = STN;
                break;

            case "ATH":
                if(setHome) TMPAirportName.text += ATH.airportName;
                airport = ATH;
                break;
            case "BGY":
                if(setHome) TMPAirportName.text += BGY.airportName;
                airport = BGY;
                break;
            case "CIA":
                if(setHome) TMPAirportName.text += CIA.airportName;
                airport = CIA;
                break;
            case "FCO":
                if(setHome) TMPAirportName.text += FCO.airportName;
                airport = FCO;
                break;
            case "LIN":
                if(setHome) TMPAirportName.text += LIN.airportName;
                airport = LIN;
                break;
            case "MXP":
                if(setHome) TMPAirportName.text += MXP.airportName;
                airport = MXP;
                break;
            case "NAP":
                if(setHome) TMPAirportName.text += NAP.airportName;
                airport = NAP;
                break;
            case "TSF":
                if(setHome) TMPAirportName.text += TSF.airportName;
                airport = TSF;
                break;
            case "VCE":
                if(setHome) TMPAirportName.text += VCE.airportName;
                airport = VCE;
                break;
            case "LIS":
                if(setHome) TMPAirportName.text += LIS.airportName;
                airport = LIS;
                break;
            case "OPO":
                if(setHome) TMPAirportName.text += OPO.airportName;
                airport = OPO;
                break;
            case "OTP":
                if(setHome) TMPAirportName.text += OTP.airportName;
                airport = OTP;
                break;
            case "BTS":
                if(setHome) TMPAirportName.text += BTS.airportName;
                airport = BTS;
                break;
            case "BCN":
                if(setHome) TMPAirportName.text += BCN.airportName;
                airport = BCN;
                break;
            case "MAD":
                if(setHome) TMPAirportName.text += MAD.airportName;
                airport = MAD;
                break;
            case "PMI":
                if(setHome) TMPAirportName.text += PMI.airportName;
                airport = PMI;
                break;

            case "BUD":
                if(setHome) TMPAirportName.text += BUD.airportName;
                airport = BUD;
                break;
            case "SVO":
                if(setHome) TMPAirportName.text += SVO.airportName;
                airport = SVO;
                break;
            case "PRG":
                if(setHome) TMPAirportName.text += PRG.airportName;
                airport = PRG;
                break;
            case "WAW":
                if(setHome) TMPAirportName.text += WAW.airportName;
                airport = WAW;
                break;
            case "DME":
                if(setHome) TMPAirportName.text += DME.airportName;
                airport = DME;
                break;
            case "LED":
                if(setHome) TMPAirportName.text += LED.airportName;
                airport = LED;
                break;
            case "VKO":
                if(setHome) TMPAirportName.text += VKO.airportName;
                airport = VKO;
                break;
            case "ZIA":
                if(setHome) TMPAirportName.text += ZIA.airportName;
                airport = ZIA;
                break;

            case "DXB":
                if(setHome) TMPAirportName.text += DXB.airportName;
                airport = DXB;
                break;
            case "HND":
                if(setHome) TMPAirportName.text += HND.airportName;
                airport = HND;
                break;
            case "DEL":
                if(setHome) TMPAirportName.text += DEL.airportName;
                airport = DEL;
                break;
            case "IST":
                if(setHome) TMPAirportName.text += IST.airportName;
                airport = IST;
                break;
            case "GYD":
                if(setHome) TMPAirportName.text += GYD.airportName;
                airport = GYD;
                break;
            case "PVG":
                if(setHome) TMPAirportName.text += PVG.airportName;
                airport = PVG;
                break;
            case "SHA":
                if(setHome) TMPAirportName.text += SHA.airportName;
                airport = SHA;
                break;
            case "BOM":
                if(setHome) TMPAirportName.text += BOM.airportName;
                airport = BOM;
                break;
            case "CCU":
                if(setHome) TMPAirportName.text += CCU.airportName;
                airport = CCU;
                break;
            case "DPS":
                if(setHome) TMPAirportName.text += DPS.airportName;
                airport = DPS;
                break;
            case "CTS":
                if(setHome) TMPAirportName.text += CTS.airportName;
                airport = CTS;
                break;
            case "ITM":
                if(setHome) TMPAirportName.text += ITM.airportName;
                airport = ITM;
                break;
            case "KIX":
                if(setHome) TMPAirportName.text += KIX.airportName;
                airport = KIX;
                break;
            case "NRT":
                if(setHome) TMPAirportName.text += NRT.airportName;
                airport = NRT;
                break;
            case "OKD":
                if(setHome) TMPAirportName.text += OKD.airportName;
                airport = OKD;
                break;
            case "MFM":
                if(setHome) TMPAirportName.text += MFM.airportName;
                airport = MFM;
                break;
            case "GAN":
                if(setHome) TMPAirportName.text += GAN.airportName;
                airport = GAN;
                break;
            case "HAQ":
                if(setHome) TMPAirportName.text += HAQ.airportName;
                airport = HAQ;
                break;
            case "MLE":
                if(setHome) TMPAirportName.text += MLE.airportName;
                airport = MLE;
                break;
            case "MNL":
                if(setHome) TMPAirportName.text += MNL.airportName;
                airport = MNL;
                break;
            case "SIN":
                if(setHome) TMPAirportName.text += SIN.airportName;
                airport = SIN;
                break;
            case "GMP":
                if(setHome) TMPAirportName.text += GMP.airportName;
                airport = GMP;
                break;
            case "ICN":
                if(setHome) TMPAirportName.text += ICN.airportName;
                airport = ICN;
                break;
            case "PUS":
                if(setHome) TMPAirportName.text += PUS.airportName;
                airport = PUS;
                break;
            case "BKK":
                if(setHome) TMPAirportName.text += BKK.airportName;
                airport = BKK;
                break;
            case "HKT":
                if(setHome) TMPAirportName.text += HKT.airportName;
                airport = HKT;
                break;
            case "DMK":
                if(setHome) TMPAirportName.text += DMK.airportName;
                airport = DMK;
                break;
            case "AYT":
                if(setHome) TMPAirportName.text += AYT.airportName;
                airport = AYT;
                break;
            case "ESB":
                if(setHome) TMPAirportName.text += ESB.airportName;
                airport = ESB;
                break;
            case "SAW":
                if(setHome) TMPAirportName.text += SAW.airportName;
                airport = SAW;
                break;
            case "DWC":
                if(setHome) TMPAirportName.text += DWC.airportName;
                airport = DWC;
                break;

            case "AEP":
                if(setHome) TMPAirportName.text += AEP.airportName;
                airport = AEP;
                break;
            case "EZE":
                if(setHome) TMPAirportName.text += EZE.airportName;
                airport = EZE;
                break;
            case "CGH":
                if(setHome) TMPAirportName.text += CGH.airportName;
                airport = CGH;
                break;
            case "GIG":
                if(setHome) TMPAirportName.text += GIG.airportName;
                airport = GIG;
                break;
            case "GRU":
                if(setHome) TMPAirportName.text += GRU.airportName;
                airport = GRU;
                break;
            case "SDU":
                if(setHome) TMPAirportName.text += SDU.airportName;
                airport = SDU;
                break;
            case "LIM":
                if(setHome) TMPAirportName.text += LIM.airportName;
                airport = LIM;
                break;
            case "PDP":
                if(setHome) TMPAirportName.text += PDP.airportName;
                airport = PDP;
                break;

            case "NAS":
                if(setHome) TMPAirportName.text += NAS.airportName;
                airport = NAS;
                break;
            case "BGI":
                if(setHome) TMPAirportName.text += BGI.airportName;
                airport = BGI;
                break;
            case "SJO":
                if(setHome) TMPAirportName.text += SJO.airportName;
                airport = SJO;
                break;
            case "SYQ":
                if(setHome) TMPAirportName.text += SYQ.airportName;
                airport = SYQ;
                break;
            case "HAV":
                if(setHome) TMPAirportName.text += HAV.airportName;
                airport = HAV;
                break;
            case "JBQ":
                if(setHome) TMPAirportName.text += JBQ.airportName;
                airport = JBQ;
                break;
            case "POP":
                if(setHome) TMPAirportName.text += POP.airportName;
                airport = POP;
                break;
            case "PUJ":
                if(setHome) TMPAirportName.text += PUJ.airportName;
                airport = PUJ;
                break;
            case "SDQ":
                if(setHome) TMPAirportName.text += SDQ.airportName;
                airport = SDQ;
                break;
            case "KIN":
                if(setHome) TMPAirportName.text += KIN.airportName;
                airport = KIN;
                break;
            case "CSL":
                if(setHome) TMPAirportName.text += CSL.airportName;
                airport = CSL;
                break;
            case "CUN":
                if(setHome) TMPAirportName.text += CUN.airportName;
                airport = CUN;
                break;
            case "MEX":
                if(setHome) TMPAirportName.text += MEX.airportName;
                airport = MEX;
                break;

            case "JFK":
                if(setHome) TMPAirportName.text += JFK.airportName;
                airport = JFK;
                break;
            case "LAX":
                if(setHome) TMPAirportName.text += LAX.airportName;
                airport = LAX;
                break;
            case "YHM":
                if(setHome) TMPAirportName.text += YHM.airportName;
                airport = YHM;
                break;
            case "YHU":
                if(setHome) TMPAirportName.text += YHU.airportName;
                airport = YHU;
                break;
            case "YKF":
                if(setHome) TMPAirportName.text += YKF.airportName;
                airport = YKF;
                break;
            case "YMX":
                if(setHome) TMPAirportName.text += YMX.airportName;
                airport = YMX;
                break;
            case "YTZ":
                if(setHome) TMPAirportName.text += YTZ.airportName;
                airport = YTZ;
                break;
            case "YUL":
                if(setHome) TMPAirportName.text += YUL.airportName;
                airport = YUL;
                break;
            case "YYZ":
                if(setHome) TMPAirportName.text += YYZ.airportName;
                airport = YYZ;
                break;
            case "BWI":
                if(setHome) TMPAirportName.text += BWI.airportName;
                airport = BWI;
                break;
            case "IAD":
                if(setHome) TMPAirportName.text += IAD.airportName;
                airport = IAD;
                break;
            case "EWR":
                if(setHome) TMPAirportName.text += EWR.airportName;
                airport = EWR;
                break;
            case "SWF":
                if(setHome) TMPAirportName.text += SWF.airportName;
                airport = SWF;
                break;
            case "LGA":
                if(setHome) TMPAirportName.text += LGA.airportName;
                airport = LGA;
                break;
            case "LAS":
                if(setHome) TMPAirportName.text += LAS.airportName;
                airport = LAS;
                break;
            case "MIA":
                if(setHome) TMPAirportName.text += MIA.airportName;
                airport = MIA;
                break;
            case "DCA":
                if(setHome) TMPAirportName.text += DCA.airportName;
                airport = DCA;
                break;
            case "SFO":
                if(setHome) TMPAirportName.text += SFO.airportName;
                airport = SFO;
                break;

            case "AVV":
                if(setHome) TMPAirportName.text += AVV.airportName;
                airport = AVV;
                break;
            case "MEB":
                if(setHome) TMPAirportName.text += MEB.airportName;
                airport = MEB;
                break;
            case "MEL":
                if(setHome) TMPAirportName.text += MEL.airportName;
                airport = MEL;
                break;
            case "PER":
                if(setHome) TMPAirportName.text += PER.airportName;
                airport = PER;
                break;
            case "SYD":
                if(setHome) TMPAirportName.text += SYD.airportName;
                airport = SYD;
                break;
            case "BOB":
                if(setHome) TMPAirportName.text += BOB.airportName;
                airport = BOB;
                break;
            case "AKL":
                if(setHome) TMPAirportName.text += AKL.airportName;
                airport = AKL;
                break;
            case "CHC":
                if(setHome) TMPAirportName.text += CHC.airportName;
                airport = CHC;
                break;
            case "WLG":
                if(setHome) TMPAirportName.text += WLG.airportName;
                airport = WLG;
                break;

            case "CAI":
                if(setHome) TMPAirportName.text += CAI.airportName;
                airport = CAI;
                break;
            case "SSH":
                if(setHome) TMPAirportName.text += SSH.airportName;
                airport = SSH;
                break;
            case "NBO":
                if(setHome) TMPAirportName.text += NBO.airportName;
                airport = NBO;
                break;
            case "MBA":
                if(setHome) TMPAirportName.text += MBA.airportName;
                airport = MBA;
                break;
            case "WIL":
                if(setHome) TMPAirportName.text += WIL.airportName;
                airport = WIL;
                break;
            case "MRU":
                if(setHome) TMPAirportName.text += MRU.airportName;
                airport = MRU;
                break;
            case "RRG":
                if(setHome) TMPAirportName.text += RRG.airportName;
                airport = RRG;
                break;
            case "PRI":
                if(setHome) TMPAirportName.text += PRI.airportName;
                airport = PRI;
                break;
            case "SEZ":
                if(setHome) TMPAirportName.text += SEZ.airportName;
                airport = SEZ;
                break;
            case "CPT":
                if(setHome) TMPAirportName.text += CPT.airportName;
                airport = CPT;
                break;
            case "JNB":
                if(setHome) TMPAirportName.text += JNB.airportName;
                airport = JNB;
                break;
            case "HLA":
                if(setHome) TMPAirportName.text += HLA.airportName;
                airport = HLA;
                break;
            case "JRO":
                if(setHome) TMPAirportName.text += JRO.airportName;
                airport = JRO;
                break;
            case "LVI":
                if(setHome) TMPAirportName.text += LVI.airportName;
                airport = LVI;
                break;
        }

        return airport;
    }

    /// <summary>
    /// Sets UI representation to match the selected home airport
    /// </summary>
    private void SetAirportUI()
    {
        string airport = PlayerPrefs.GetString("Airport");
        Debug.Log("Home Airport: " + airport);
        TMPAirportName.text = "<b>" + airport + "</b>\n";
        
        CreateAirportLabel();
    }

    /// <summary>
    /// Creates and displays the airport image label
    /// </summary>
	public void CreateAirportLabel()
    {
        if(Label != null)
        {
            Destroy(Label);
        }

        GameObject mainEarth = GameObject.Find("MainEarth");

        //create objects
        Label = Instantiate(labelPrefab);
        Label.name = "AirportLabel";
        Label.SetActive(false);
        Label.transform.position = GetPos(currentAirport);
        Label.transform.SetParent(mainEarth.transform, false);
        Label.transform.localScale = Vector3.one * (2.2f - (InputController.Instance.zoomLevel * .1f));

        //transform objects
        Label.transform.rotation = Quaternion.LookRotation(-(Label.transform.position + mainEarth.transform.position), mainCameraUp);

        Label.SetActive(true);
    }

    /// <summary>
    /// Returns the initial position of a given airport
    /// </summary>
    /// <param name="airport">the airport</param>
    public Vector3 GetPos(Airport airport)
    {
        return new Vector3(
            (float)(50 * Math.Cos(airport.latitude * Math.PI / 180) * Math.Sin(-airport.longitude * Math.PI / 180)),
            (float)(50 * Math.Sin(airport.latitude * Math.PI / 180)),
            (float)(50 * Math.Cos(airport.latitude * Math.PI / 180) * Math.Cos(-airport.longitude * Math.PI / 180)));
    }

    /// <summary>
    /// Show the selected regional airport selection screen
    /// </summary>
    /// <param name="id">region id</param>
    public void SelectHomeAirportRegion(int id)
    {
        RegionButtons.SetActive(false);
        SelectionTree.transform.GetChild(id).gameObject.SetActive(true);
    }

    /// <summary>
    /// Return from the current regional selection screen to the region overview
    /// </summary>
    public void ReturnToRegionSelection()
    {
        RegionButtons.SetActive(true);
        for(int i=0; i< SelectionTree.transform.childCount; i++)
        {
            SelectionTree.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
