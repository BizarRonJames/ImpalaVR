using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum HazardMenuState
{
    off = 0,
    MatrixRating = 1,
    MCQ = 2,
    Consequence = 3,
    report = 4
}
public class HazardMenuEventSystem : MonoBehaviour
{
    [SerializeField] Hazard _testHazard;
    [SerializeField] HazardMenuState state;
    public static HazardMenuEventSystem instance;

    public HazardMenuState State { get => state; set => state = value; }

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        MenuClose();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MenuOpen(_testHazard);
        }

        if(Input.GetKeyDown(KeyCode.N)){
            NextWindow();
        }
    }
    #region Events
    public event Action<Hazard> OnMenuOpen;
    [ContextMenu("Menu Open")]
    public void TestMenu(){
        MenuOpen(_testHazard);
    }
    public void MenuOpen(Hazard hazard)
    {
        if (OnMenuOpen != null)
        {
            OnMenuOpen(hazard);
            state = HazardMenuState.MatrixRating;
        }
    }

    public event Action<Hazard> OnTarpMenuOpen;
    
    public void TarpMenuOpen(Hazard hazard)
    {
        if (OnTarpMenuOpen != null)
        {
            OnTarpMenuOpen(hazard);
            state = HazardMenuState.MatrixRating;
        }
    }
    
    public event Action OnMenuClose;
    public void MenuClose()
    {
        if (OnMenuClose != null)
        {
            OnMenuClose();
        }
    }
    public event Action OnNextMenu;
    public void NextWindow()
    {
        if (OnMenuClose != null)
        {
            OnNextMenu();

        }
    }
    public event Action<int> OnButtonHover;
    public void ButtonHover(int index)
    {
        if (OnButtonHover != null)
        {
            OnButtonHover(index);
        }
    }

    public event Action<Hazard> OnBrowMenuOpen;
    public void BrowMenuOpen(Hazard hazard){
        if(OnBrowMenuOpen != null){
            OnBrowMenuOpen(hazard);
        }
    }

    public event Action<int> OnButtonHoverExit;
    public void ButtonHoverExit(int index)
    {
        if (OnButtonHoverExit != null)
        {
            OnButtonHoverExit(index);
        }
    }



    public event Action OnReportMenuOpen;
    public void ReportMenuOpen()
    {
        if (OnReportMenuOpen != null)
        {
            state = HazardMenuState.off;
            OnReportMenuOpen();
        }
    }
    public event Action OnReportMenuClose;
    public void ReportMenuClose()
    {
        if (OnReportMenuOpen != null)
        {
            OnReportMenuClose();
        }
    }

    public event Action OnMatrixWindowOpen;
    public void MatrixWindowOpen(){
        if(OnMatrixWindowOpen != null){
            OnMatrixWindowOpen();
        }
    }


    public event Action OnQuestionWindowOpen;
    public void QuestionWindowOpen(){
        if(OnQuestionWindowOpen != null){
            OnQuestionWindowOpen();
        }
    }

    public event Action OnConsequenceWindowOpen;
    public void ConsequenceWindowOpen(){
        if(OnConsequenceWindowOpen != null){
            OnConsequenceWindowOpen();
        }
    }

    public event Action OnMenuNetworkDataUpdate;
    public void MenuNetworkDataUpdate()
    {
        if (OnMenuNetworkDataUpdate != null)
        {
            OnMenuNetworkDataUpdate();
        }
    }
    public event Action OnMenuNetworkSync;
    public void MenuNetworkSync()
    {
        if (OnMenuNetworkSync != null)
        {
            OnMenuNetworkSync();
        }
    }
    #endregion

    public void SetWindowState(int index){
        if(index < 0 || index > Enum.GetNames(typeof(HazardMenuState)).Length  - 1){
            index = 0;
        }


        this.State = (HazardMenuState) index;
    }

    public void IncrementHazardMenuState()
    {
        int currentWindow = (int)state + 1;
        int namesCount = Enum.GetNames(typeof(HazardMenuState)).Length;
        if (currentWindow > namesCount - 1)
        {
            currentWindow = 0;
        }

        state = (HazardMenuState)currentWindow;
    }

}