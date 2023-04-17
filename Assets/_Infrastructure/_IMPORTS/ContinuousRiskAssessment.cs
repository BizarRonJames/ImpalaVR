using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Globalization;

public class ContinuousRiskAssessment : MonoBehaviour
{
    public static ContinuousRiskAssessment instance;
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Debug.LogError("There is more than one instance of ContinuousRiskAssessment in this scene");
    }


    string date;
    string time;

    //string foundHazards = "Burning LHD##Broken Light##DustHanging Pipe##Cabels On Ground##Loose Signs##Missing Signs##Unlocked Electrical Box##Ventilation Not Running##Dripping Water##";
    public string[] values = {
        "a",
        "a",
        "a",
        "a",
        "a",
        "a",
        "a",
        "a",
        "a",
        "a",
        "a",
        "a",
        "a",
        "a",
        "a"
    };

    [SerializeField] string[] hazardNames;
    List<string> missedHazards;
    public string[] ratingsNames;
    public int[] wcgw;
    [SerializeField] int[] ratingIndex;
    [SerializeField] int currentHazard = -1;

    [SerializeField] string hazardString;

    IEnumerator enumerator;

    [SerializeField] float waitTime = 1.5f;

    [Header("Init")]
    public RectTransform pnlInit;
    [SerializeField] Image imgPnlInit;

    #region pnlSheet
    [Header("Sheet")]
    public RectTransform pnlSheet;
    [SerializeField] Text txtDate;
    [SerializeField] Text txtTime;
    [SerializeField] Image[] highlightYN;
    [SerializeField] Image[] xYN;
    [SerializeField] Image[] highlightRatings;
    [SerializeField] Text[] lblHazards;
    [SerializeField] Text[] lblHazardRatings;
    [SerializeField] Text[] lblWCGW;
    [SerializeField] Image[] imgWCGW;
    [SerializeField] string testString;


    #endregion

    [Header("User Details")]
    [SerializeField] Text[] txtUserNames;
    [SerializeField] Text[] txtUserIDs;    

    #region Matrix
    [Header("Matrix")]
    public RectTransform pnlMatrix;
    public Text txtMatrixName;
    public RectTransform[] matrixRects;
    public Image matrixHighlight;
    public Image matrixLoadingBar;
    #endregion

    #region Wcgw
    [Header("WCGW")]
    [SerializeField] RectTransform pnlWCGW;
    [SerializeField] Text wcgwHazardName;
    [SerializeField] Image[] wcgwHighlights;
    [SerializeField] Text[] wcgwLabels;
    #endregion

    [Header("ShowResults")]
    [SerializeField] GameObject btnShowResults;
    [SerializeField] RectTransform pnlResults;

    public Material matReportCorrect;
    public Material matReportIncorrect;
    public Material[] matReportPriority;

    [SerializeField] Text txtUserName;
    [SerializeField] Text txtUserID;
    [SerializeField] Text txtReportResults;
    

    [ContextMenu("Test Initialize")]
    public void ShowMenu()
    {
        values[0] = "1";
        values[1] = "0";
        SetViewerValues();
    }

    private void Start()
    {
    }

    public void HideMenu()
    {
        values[0] = "-1";
        SetViewerValues();
    }

    public void IDNotFound()
    {
        pnlMatrix.localScale = Vector3.zero;
        pnlInit.localScale = Vector3.zero;
        pnlSheet.localScale = Vector3.zero;
        pnlResults.localScale = Vector3.zero;
    }

    #region Init
    public void MenuInit_Entered()
    {
        if (enumerator != null)
            StopCoroutine(enumerator);
        enumerator = MenuInit_Entered_I();

        StartCoroutine(enumerator);
    }

    IEnumerator MenuInit_Entered_I()
    {
        values[1] = "0";
        float rate = 1 / waitTime;
        float i = 0;
        while (i < 1)
        {
            i += Time.unscaledDeltaTime * rate;
            values[1] = i.ToString();
            SetViewerValues();
            yield return null;
        }

        values[0] = "4";
        date = System.DateTime.Now.ToString("yyyy/MM/dd");
        time = System.DateTime.Now.ToString("HH:mm");
        values[1] = date;
        values[2] = time;
        for (int j = 3; j < values.Length; j++)
            values[j] = "a";
        string line = "";
        missedHazards = new List<string>();
       // foreach (Hazard h in Ha.instance.hazardsAnswered)
           // line += h.hazardName + "###";
     //   foreach (Hazard h in Hira.instance.hazards)
         //   if (h.isShowing && !h.answered)
           //     missedHazards.Add(h.hazardName);
        values[11] = line;
      //  Hira.instance.TurnOffAllHazards();
        SetViewerValues();
    }

    public void MenuInit_Exited()
    {
        if (enumerator != null)
            StopCoroutine(enumerator);
        imgPnlInit.fillAmount = 0;
        values[1] = "0";
        SetViewerValues();
    }

    public void InitializeMenu(string options)
    {
        string[] list = options.Split('#');
        int i = 0;

        int target = list.Length;
        if (target % 3 == 1)
            target--;
        hazardNames = new string[target / 3];
        ratingIndex = new int[target / 3];
        wcgw = new int[target / 3];

        for (i = 0; i < target; i+=3)
        { 
            //Assign hazard name
            lblHazards[i/3].text = list[i];
            hazardNames[i / 3] = list[i];

            //Parse Hazard Rating
            highlightRatings[i / 3].gameObject.SetActive(true);
            highlightRatings[(i / 3)+lblHazards.Length].gameObject.SetActive(true);
            if (list[i + 1] == "" || list[i + 1]=="-1")
            {
                ratingIndex[i / 3] = -1;
                lblHazardRatings[i / 3].text = "X";
            }
            else
            {
                ratingIndex[i / 3] = int.Parse(list[i + 1]);                
                lblHazardRatings[i / 3].text = ratingsNames[ratingIndex[i/3]];
            }

            //Parse WCGW
            if(list[i+2]=="" || list[i+2]=="-1")
            {
                wcgw[i / 3] = -1;
                lblWCGW[i / 3].text = "X";

                highlightRatings[i / 3].gameObject.SetActive(false);
                lblHazardRatings[i / 3].text = "";

            }
            else
            {
                wcgw[i / 3] = int.Parse(list[i+2]);
               // lblWCGW[i / 3].text = Hira.instance.GetHazardByName(hazardNames[i / 3]).wcgw[wcgw[i / 3]];
            }
        }

        target /= 3;
        i = target;

        for (; i < lblHazardRatings.Length; i++)
        {
            lblHazards[i].text = "";
            lblHazardRatings[i].text = "";
            lblWCGW[i].text = "";
            highlightRatings[i].gameObject.SetActive(false);            
            highlightRatings[i + lblHazards.Length].gameObject.SetActive(false);            
        }    
    }
    #endregion

    #region Rating
    public void HighlightRating(int i)
    {
        if (enumerator != null)
            StopCoroutine(enumerator);

        enumerator = HighlightRating_I(i);
        StartCoroutine(enumerator);
    }

    public void DeHighlightRating(int i)
    {
        if (enumerator != null)
            StopCoroutine(enumerator);

        highlightRatings[i].enabled = false;
        values[12] = "a";
        SetViewerValues();
    }

    IEnumerator HighlightRating_I(int i)
    {
        values[12] = i.ToString();
        SetViewerValues();
        yield return new WaitForSeconds(waitTime);
        currentHazard = i;
        values[0] = "5";        
        values[13] = hazardNames[i];
        values[14] = "a";
        values[15] = "a";
        SetViewerValues();

    }
    #endregion

    #region ShowResults    
    public void HighlightShowResults()
    {
        int i = highlightRatings.Length - 1;
        if (enumerator != null)
            StopCoroutine(enumerator);

        enumerator = HighlightShowResults_I(i);
        StartCoroutine(enumerator);
    }

    public void DeHighlightShowResults(int i)
    {
        if (enumerator != null)
            StopCoroutine(enumerator);

        highlightRatings[i].enabled = false;
        values[12] = "a";
        SetViewerValues();
    }

    IEnumerator HighlightShowResults_I(int i)
    {
        values[12] = i.ToString();
        SetViewerValues();
        yield return new WaitForSeconds(waitTime);
        currentHazard = i;
        values[0] = "7";

        string line = "";
        for(int j = 0; j < hazardNames.Length; j++)
        {
            line += hazardNames[j] + "#";
           // Hazard h = Hira.instance.GetHazardByName(hazardNames[j]);
          //  line += h.answer.ToString() + "#";
            line += ratingIndex[j].ToString() + "#";
            line += wcgw[j].ToString() + "#";
        }

        foreach (string s in missedHazards)
        {
            Debug.Log("Hazard Adding: " + s);
            line += s + "#-1###";
        }
            

        values[11] = date + "#" + time;
        values[12] = time;
        values[13] = line;
        values[14] = Player.instance.first_name;
        values[15] = Player.instance.id_number;
        SetViewerValues();

    }
    #endregion

    #region WCGW
    public void HighlightWCGW(int i)
    {
        if (enumerator != null)
            StopCoroutine(enumerator);

        enumerator = HighlightWCGW_I(i);
        StartCoroutine(enumerator);
    }

    public void DeHighlightWCGW(int i)
    {
        if (enumerator != null)
            StopCoroutine(enumerator);

        highlightRatings[i].enabled = false;
        values[12] = "a";
        SetViewerValues();
    }

    IEnumerator HighlightWCGW_I(int i)
    {
        values[12] = i.ToString();
        SetViewerValues();
        yield return new WaitForSeconds(waitTime);
        currentHazard = i- lblHazards.Length;
        values[0] = "6";
        values[13] = hazardNames[currentHazard];

        string optionList = "";
        //foreach (string s in Hira.instance.GetHazardByName(hazardNames[currentHazard]).wcgw)
        //    optionList += s + "#";
        values[14] = optionList;
        values[15] = "-1";
        SetViewerValues();
    }
    #endregion

    #region WCGW_Menu
    bool highlighted = false;

    public void HighlighWCGW_Menu(int i)
    {
        highlighted = true;
        if (enumerator != null)
            StopCoroutine(enumerator);

        enumerator = HighlightWCGW_Menu_I(i);
        StartCoroutine(enumerator);

    }

    IEnumerator HighlightWCGW_Menu_I(int i)
    {
        string line = "";
        for (int k = 0; k < hazardNames.Length; k++)
            line += hazardNames[k] + "#" + ratingIndex[k] + "#" + wcgw[k] + "#";
        wcgw[currentHazard] = i;
        values[15] = i.ToString();
        SetViewerValues();
        float j = 0;
        float rate = 1 / waitTime;        
        while (j < 1)
        {
            j += Time.unscaledDeltaTime * rate;
            yield return null;
        }
        
        line = "";
        for (int k = 0; k < hazardNames.Length; k++)
            line += hazardNames[k] + "#" + ratingIndex[k] + "#" + wcgw[k] + "#";
        values[11] = line;
        values[0] = "4";
        SetViewerValues();
    }

    public void DehighlightWCGW_Menu(int i)
    {
        highlighted = false;
        if (enumerator != null)
            StopCoroutine(enumerator);
        enumerator = WaitToDehighlightWCGW_Menu();
        StartCoroutine(enumerator);
    }

    IEnumerator WaitToDehighlightWCGW_Menu()
    {
        yield return new WaitForSeconds(0.05f);
        values[15] = "a";
            highlighted = false;
            SetViewerValues();
        
    }
    #endregion

    #region YesNo
    public void HighlightYesNo(int i)
    {
        if (enumerator != null)
            StopCoroutine(enumerator);

        enumerator = HighlightYesNo_I(i);
        StartCoroutine(enumerator);

    }

    public void DehighlightYesNo(int i)
    {
        if (enumerator != null)
            StopCoroutine(enumerator);

        if (i % 2 == 1)
            i--;
        values[i + 4] = "a";
        SetViewerValues();
    }

    IEnumerator HighlightYesNo_I(int i)
    {
        bool isEven = true;
        if (i % 2 == 1) 
        {
            i--;
            isEven = false;
        }
        string outcome = isEven ? "y" : "n";
        values[i + 4] = outcome;
        SetViewerValues();
        yield return new WaitForSeconds(waitTime);
        values[i + 4] = "a";
        values[i + 3] = outcome;
        SetViewerValues();
    }
    #endregion

    #region Matrix
    public void HighlightMatrix(int i)
    {
        if (enumerator != null)
            StopCoroutine(enumerator);        

        enumerator = HighlightMatrix_I(i);
        StartCoroutine(enumerator);

    }

    IEnumerator HighlightMatrix_I(int i)
    {
        values[14] = i.ToString() ;
        values[15] = "0";
        float j = 0;
        float rate = 1 / waitTime;
        SetViewerValues();
        while (j < 1)
        {
            j += Time.unscaledDeltaTime * rate;
            values[15] = j.ToString();
            //matrixLoadingBar.fillAmount = j;
            SetViewerValues();
            yield return null;
        }
        values[15] = "1";
        ratingIndex[currentHazard] = i;

        //Sort hazards
        for(int a= 0; a<ratingIndex.Length-1; a++)
            for(int b =a+1; b<ratingIndex.Length; b++)
                if (ratingIndex[a] < ratingIndex[b])
                {
                    string tmpName = hazardNames[a];
                    int tmpIndex = ratingIndex[a];
                    int tmpWCGW = wcgw[a];
                    hazardNames[a] = hazardNames[b];
                    ratingIndex[a] = ratingIndex[b];
                    wcgw[a] = wcgw[b];

                    hazardNames[b] = tmpName;
                    ratingIndex[b] = tmpIndex;
                    wcgw[b] = tmpWCGW;
                }


        string line = "";
        for (int k = 0; k < hazardNames.Length; k++)
            line += hazardNames[k] + "#" + ratingIndex[k] + "#" + wcgw[k] + "#";
        values[0] = "4";
        values[11] = line;
        SetViewerValues();
        
    }

    public void DehighlightMatrix(int i)
    {
        if (enumerator != null)
            StopCoroutine(enumerator);
        enumerator = WaitToDehighlightMatrix();
        StartCoroutine(enumerator);
    }

    IEnumerator WaitToDehighlightMatrix()
    {
        yield return new WaitForSeconds(0.05f);
        values[14] = "a";
        SetViewerValues();
    }
    #endregion

    void SetViewerValues()
    {
        string line = "";
        foreach (string s in values)
            line += s + " ";
        //if (RemoteViewerManager.instance.IsOwner())
        //    remoteViewer.ValueChanged(RemoteViewerManager.instance.viewerOriginalID, line);
    }

    //void ListUsers()
    //{
    //    if(txtUserNames[0].text == "")
    //    {
    //        Player[] players = FindObjectsOfType<Player>();
    //        int n = 0;
    //        foreach(Player p in players)
    //        {
    //            if (!p.isServer)
    //            {
    //                txtUserNames[n].text = p.playerName;
    //                txtUserIDs[n].text = p.playerID;
    //                n++;
    //            }
    //        }
    //    }
    //}

    #region Parse Values
    public void ParseValues()
    {        
        try
        {
            //string[] values = remoteViewer.GetValue(RemoteViewerManager.instance.viewerID).Split(' ');
            if (values.Length == 0)
            {
                return;
            }
            if (values[0] == "1")
                Parse_1(values);
            else if (values[0] == "4")
                Parse_4(values);
            else if (values[0] == "5")
                Parse_5(values);
            else if (values[0] == "6")
                Parse_6(values);
            else if (values[0] == "7")
                Parse_7(values);
            else
            {
                IDNotFound();
            }
        }
        catch(Exception ex)
        {
            Debug.LogError(ex);
            IDNotFound();
        }

    }

    void Parse_1(string[] values)
    {
        pnlMatrix.localScale = Vector3.zero;
        pnlInit.localScale = Vector3.one;
        pnlSheet.localScale = Vector3.zero;
        pnlWCGW.localScale = Vector3.zero;
        pnlResults.localScale = Vector3.zero;

        pnlMatrix.gameObject.SetActive(false);
        pnlInit.gameObject.SetActive(true);
        pnlSheet.gameObject.SetActive(false);
        pnlWCGW.gameObject.SetActive(false);
        pnlResults.gameObject.SetActive(false);

        if (values[1] != "a" && values[1] != "")
            imgPnlInit.fillAmount = float.Parse(values[1], NumberStyles.Any, CultureInfo.InvariantCulture);
    }

    void Parse_4(string[] values)
    {
      //  ListUsers();
        pnlMatrix.localScale = Vector3.zero;
        pnlInit.localScale = Vector3.zero;
        pnlSheet.localScale = Vector3.one;
        pnlWCGW.localScale = Vector3.zero;
        pnlResults.localScale = Vector3.zero;

        pnlMatrix.gameObject.SetActive(false);
        pnlInit.gameObject.SetActive(false);
        pnlSheet.gameObject.SetActive(true);
        pnlWCGW.gameObject.SetActive(false);
        pnlResults.gameObject.SetActive(false);

        txtDate.text = System.DateTime.Now.ToString("yyyy/MM/dd");
        txtTime.text = values[2];

        for (int i = 3; i < 10; i += 2)
        {
            //Enable/Disables X's on form
            if (values[i] == "y")
            {
                xYN[i - 3].gameObject.SetActive(true);
                xYN[i - 2].gameObject.SetActive(false);
            }
            else if (values[i] == "n")
            {
                xYN[i - 3].gameObject.SetActive(false);
                xYN[i - 2].gameObject.SetActive(true);
            }
            else
            {
                xYN[i - 3].gameObject.SetActive(false);
                xYN[i - 2].gameObject.SetActive(false);
            }

            //Enabling/Disables highlighters on X's

            if (values[i + 1] == "y")
            {
                highlightYN[i - 3].enabled = true;
                highlightYN[i - 2].enabled = false;
            }
            else if (values[i + 1] == "n")
            {
                highlightYN[i - 3].enabled = false;
                highlightYN[i - 2].enabled = true;
            }
            else
            {
                highlightYN[i - 3].enabled = false;
                highlightYN[i - 2].enabled = false;
            }
        }

        //List hazards and priorities
        if (hazardString != values[11])
        {
            if (values[11] != "a" && values[11]!="")
            {
                InitializeMenu(values[11]);
                hazardString = values[11];
            }
        }


        //Highlighting Priority
        for (int j = 0; j < highlightRatings.Length; j++)
        {
            highlightRatings[j].enabled = false;
        }

        if (values[12] != "a")
        {
            int i = int.Parse(values[12]);
            highlightRatings[i].enabled = true;
        }
    }

    void Parse_5(string[] values)
    {
        pnlMatrix.localScale = Vector3.one;
        pnlInit.localScale = Vector3.zero;
        pnlSheet.localScale = Vector3.zero;
        pnlWCGW.localScale = Vector3.zero;
        pnlResults.localScale = Vector3.zero;

        pnlMatrix.gameObject.SetActive(true);
        pnlInit.gameObject.SetActive(false);
        pnlSheet.gameObject.SetActive(false);
        pnlWCGW.gameObject.SetActive(false);
        pnlResults.gameObject.SetActive(false);

        txtMatrixName.text = values[13];
        
        if (values[14] != "a")
        {
            int index = int.Parse(values[14]);
            matrixHighlight.rectTransform.position = matrixRects[index].position;
            matrixLoadingBar.rectTransform.position = matrixRects[index].position;
            matrixHighlight.enabled = true;
            if (values[15] != "a")
            {
                float progress = float.Parse(values[15], NumberStyles.Any, CultureInfo.InvariantCulture);
                matrixLoadingBar.fillAmount = progress;
            }
            else
            {
                matrixHighlight.enabled = false;
                matrixLoadingBar.fillAmount = 0;
            }
        }
        else
        {
            matrixHighlight.enabled = false;
            matrixLoadingBar.fillAmount = 0;
        }

    }

    void Parse_6(string[] values)
    {
        pnlMatrix.localScale = Vector3.zero;
        pnlInit.localScale = Vector3.zero;
        pnlSheet.localScale = Vector3.zero;
        pnlWCGW.localScale = Vector3.one;
        pnlResults.localScale = Vector3.zero;

        pnlMatrix.gameObject.SetActive(false);
        pnlInit.gameObject.SetActive(false);
        pnlSheet.gameObject.SetActive(false);
        pnlWCGW.gameObject.SetActive(true);
        pnlResults.gameObject.SetActive(false);

        wcgwHazardName.text = values[13];
        string[] options = values[14].Split('#');
        if (options.Length >= 3)
            for (int i = 0; i < wcgwLabels.Length && i < options.Length; i++)
            {
                wcgwLabels[i].text = options[i];
                //Debug.Log("Dehighlighting: " + i + " (" + values[15] + ")");
                wcgwHighlights[i].enabled = false;
            }
        else
            for (int i = 0; i < wcgwLabels.Length; i++)
                wcgwLabels[i].text = "";

        if (values[15] != "a")
        {
            int selected = int.Parse(values[15]);
            for (int i = 0; i < wcgwHighlights.Length; i++)
                wcgwHighlights[i].enabled = (i == selected);
        }
    }

    void Parse_7(string[] values)
    {
        pnlMatrix.localScale = Vector3.zero;
        pnlInit.localScale = Vector3.zero;
        pnlSheet.localScale = Vector3.zero;
        pnlWCGW.localScale = Vector3.zero;
        pnlResults.localScale = Vector3.one;

        pnlMatrix.gameObject.SetActive(false);
        pnlInit.gameObject.SetActive(false);
        pnlSheet.gameObject.SetActive(false);
        pnlWCGW.gameObject.SetActive(false);
        pnlResults.gameObject.SetActive(true);

        string[] line = values[13].Split('#');
        //foreach (CRA_ReportItem i in reportItems)
        //    i.gameObject.SetActive(false);
        if (line.Length > 0 && (line.Length) % 4 == 1)
        {
            float score = 0;
            float total = 0;
            for(int i=0;i < line.Length - 1; i += 4)
            {                
                int j = i / 4;                
              //  reportItems[j].gameObject.SetActive(true);
              //  score += reportItems[j].Initialize(line[i], line[i + 1], line[i + 2], line[i + 3]);
                total++;                
            }
            float finalScore = score / total;
            txtReportResults.text = (finalScore*100.00).ToString("0.00") + "%";
            txtUserName.text = values[14];
            txtUserID.text = values[15];
        }
        else
            Debug.Log("Results could not be shown: " + line.Length);

    }
    #endregion

    private void Update()
    {
       // LookAtManager.instance.LookAt(transform);
    }

    public void RecordValues(string userID, string value)
    {
        //Debug.Log("Recording Values: " + Player.instance.isserver + "(" + userID + ")" + ": " + value);
        //if (Player.instance.isserver && value.StartsWith("7 "))
        //{
        //    string[] values = value.Split(' ');
        //    string[] dt = values[11].Split('#');
        //   // ReportManager.instance.FileReport(dt[0].Replace('/','_').Replace('\\','_'), dt[1].Replace(':','_'), values[14], values[15], values[13]);
             
        //}
        
    }
    
}
