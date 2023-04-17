using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Normal.Realtime;
using System;

[RequireComponent(typeof(Menu))]
public class Information : RealtimeComponent<InfoModel>
{
    public static Information instance;
    public void ClaimInstance()
    {
        instance = this;
    }

    public Transform tTest;

    [SerializeField] private TMP_Text lblHeader;
    [SerializeField] private TMP_Text lblSubHeader;
    [SerializeField] private TMP_Text lblText;
    [SerializeField] private HorizontalLayoutGroup equipmentRequiredLayout;

    [SerializeField] private Information childInformant;

    [Header("Completed")]
    public Image imgMainBackground;
    public Color colCompleted;


    [Header("TOPix")]
    public GameObject pnlTOPix;
    public Image imgTOPix;

    [Header("Tools")]
    public TMP_Text lblTools;

    [Header("PPE")]
    public Image[] imgPPE;
    public Sprite[] ppeNotActive;
    public Sprite[] ppeActive;
    public GameObject[] ppeChecked;

    public string ppeStatus = "";
    public string ppeWorn = "00000";

    [Header("Results")]
    public GameObject pnlResults;
    public TMP_Text lbl_result_Part;
    public TMP_Text lbl_result_Step;
    public TMP_Text lbl_result_PPE;
    public TMP_Text lbl_result_Tool;
    public TMP_Text lbl_result_Torque;
    public TMP_Text lbl_result_Procedure;
    public TMP_Text lbl_result_Cleaned;
    public TMP_Text lbl_Required_PPE;

    [Header("Topix")]
    public Sprite[] topixItems;

    [Header("Information")]
    public GameObject btnOK;
    //private InformationalStep stepInfo;


    private void Update()
    {
        if (tTest)
            lbl_Required_PPE.text = tTest.position.ToString() + " _ " + System.DateTime.Now.Second.ToString();
    }

    protected override void OnRealtimeModelReplaced(InfoModel previousModel, InfoModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.textDidChange -= TextUpdated;
            previousModel.showingInfoOKDidChange -= ShowInfoButton;
            previousModel.completedDidChange -= CompletedDiDChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                //StartCoroutine(WaitToReEnable());
            }
            currentModel.textDidChange += TextUpdated;
            currentModel.completedDidChange += CompletedDiDChange;
        }
    }

    IEnumerator WaitToReEnable()
    {
        yield return new WaitForSeconds(2);
        TextUpdateLocal();

    }

    public void Parse()
    {
        StartCoroutine(WaitToShow());
    }

    IEnumerator WaitToShow()
    {
        Debug.Log("About to show info");
        yield return new WaitForSeconds(2);
        TextUpdateLocal();

    }

    private void CompletedDiDChange(InfoModel model, bool value)
    {
        Debug.Log("Information: CompletedDidChange: " + value);
        if (model.completed == true)
        {
            ShowEnd();
        }
    }

    public void ShowEnd()
    {
        if (imgMainBackground)
        {
            imgMainBackground.color = colCompleted;
            if (childInformant)
                childInformant.ShowEnd();
        }
    }


    private void TextUpdated(InfoModel model, string value)
    {
        TextUpdateLocal();
    }

    void TextUpdateLocal()
    {
        ShowInfo(model.header, model.subHeader, model.text, model.shouldAnimate);
    }


    //private InventoryService InventoryService => InstanceLocator.Get<InventoryService>();

    public void ShowInstructions(string header, string subHeader, string text, bool shouldAnimate = true)
    {
        recHeader = header;
        recSubHeader = subHeader;
        recText = text;
        //Debug.Log($"Setting Instructions {recHeader}, {subHeader}, {recText}");

        if (NetworkStatus.instance && NetworkStatus.instance.IsConnected && model != null)
        {
            model.header = header;
            model.subHeader = subHeader;
            model.shouldAnimate = shouldAnimate;
            model.text = text;
        }
        else
            ShowInfo(header, subHeader, text, shouldAnimate);
    }

    public void ShowInfo(string header, string subHeader, string text, bool shouldAnimate = true)
    {
        lblHeader.gameObject.SetActive(true);
        lblSubHeader.gameObject.SetActive(true);

        if (gameObject.activeInHierarchy && shouldAnimate)
            StartCoroutine(GetComponent<Menu>().FullAnimation(false));
        lblHeader.text = header;
        lblSubHeader.text = subHeader;
        lblText.text = text;

        if (childInformant && childInformant.gameObject.activeSelf)
            childInformant.ShowInfo(header, subHeader, text, shouldAnimate);
    }

    //public void ShowEquipmentRequired(List<InventoryItemIds> requiredItems)
    //{
    //    recRequiredItems = requiredItems;

    //    lblHeader.gameObject.SetActive(false);
    //    lblSubHeader.gameObject.SetActive(false);

    //    lblText.text = "Please, equip the required protection gear!";

    //    foreach (GameObject equipmentRequiredGameObject in equipmentRequiredGameObjects)
    //    {
    //        Destroy(equipmentRequiredGameObject);
    //    }

    //    equipmentRequiredGameObjects.Clear();

    //    foreach (InventoryItemIds inventoryItemId in requiredItems)
    //    {
    //        GameObject equipmentRequiredGameObject = GameObjectHelpers.Instantiate(InventoryService.InventoryConfig.GetItemConfig(inventoryItemId.ToString()).GetPrefab(), equipmentRequiredLayout.transform);
    //        equipmentRequiredGameObjects.Add(equipmentRequiredGameObject);
    //    }
    //}












    #region Results
    public void ShowResult(string part, string step, string ppe, string tool, string torque, string procedure, string cleaned, string requiredPPE)
    {
        if (!pnlResults)
            return;

        lbl_result_Part.text = part;
        lbl_result_Step.text = step;
        lbl_result_PPE.text = "PPE: " + ppe;
        lbl_result_Tool.text = "Tool: " + tool;
        lbl_result_Torque.text = "Torque: " + torque;
        lbl_result_Procedure.text = "Procedure: " + procedure;
        lbl_result_Cleaned.text = "Cleaned: " + cleaned;
        if (lbl_Required_PPE)
            lbl_Required_PPE.text = requiredPPE;
    }
    #endregion

    #region Informational Steps
    //public void ShowInformationalStep(InformationalStep inf)
    //{
    //    if (inf != null)
    //    {
    //        stepInfo = inf;
    //        model.showingInfoOK = true;
    //        btnOK.SetActive(true);
    //    }
    //}

    //public void EndInformationalStep()
    //{
    //    if(stepInfo != null)
    //    {
    //        model.showingInfoOK = false;
    //        btnOK.SetActive(false);
    //        stepInfo.EndStep();
    //    }
    //}

    private void ShowInfoButton(InfoModel model, bool value)
    {
        btnOK.SetActive(value);
    }

    #endregion

    #region Completed

    public void CompletedLocally()
    {
        Debug.Log("Information: CompletedLocally");
        model.completed = true;
    }
    #endregion

    #region OfflineValues
    string recPpe;
    string[] recTools;
    // public List<InventoryItemIds> recRequiredItems;
    public string recHeader;
    public string recSubHeader;
    public string recText;
    public string recPpeWorn;

    public void Reconnect()
    {
        // Debug.Log($"Reconnecting: {recPpe}, {recTools}, {recRequiredItems}, {recHeader}, {recSubHeader}, {recText}" );
        //StartCoroutine(ReconnectI());
    }

    IEnumerator ReconnectI()
    {
        yield return new WaitForSeconds(2);
        // Debug.Log($"Reconnecting: {recPpe}, {recTools}, {recRequiredItems}, {recHeader}, {recSubHeader}, {recText}");
        //SetPPE(recPpe);
        //SetToolRequirements(recTools);
        //ShowEquipmentRequired(recRequiredItems);
        //ShowInstructions(recHeader, recSubHeader, recText);
    }
    #endregion
}
