using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Gui;
using System;
using UnityEngine.UI;

public class HazardMenuManager : MonoBehaviour
{

    [SerializeField] Canvas[] _windows;
    [SerializeField] HoverSelect[] _buttons;
    [SerializeField] float windowOffset = 5f;
    [SerializeField] float verticalWindowOffset = 0.5f;
    [SerializeField] Transform player;

    [SerializeField] GameObject _reportWindow;

    [Header("Fields to Watch")]
    [SerializeField] QuestionController _questionController;
    [SerializeField] Canvas _canvas;

    private void Awake()
    {

        _buttons = GetComponentsInChildren<HoverSelect>();
        _questionController = GetComponentInChildren<QuestionController>();
        //player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        //_canvas = GetComponent<Canvas>();
        //_canvas.worldCamera = Camera.main;

    }
    private void Update()
    {
        //_canvas.transform.rotation = Camera.main.transform.rotation;

    }
    #region Callback_Subscriptions
    private void OnEnable()
    {
        HazardMenuEventSystem.instance.OnNextMenu += OnNextWindow;
        HazardMenuEventSystem.instance.OnMenuClose += OnMenuClose;
        HazardMenuEventSystem.instance.OnMenuOpen += OnMenuOpen;
        HazardMenuEventSystem.instance.OnTarpMenuOpen += OnTarpMenuOpen;
        HazardMenuEventSystem.instance.OnReportMenuOpen += OnReportMenuOpen;

        HazardMenuEventSystem.instance.OnBrowMenuOpen += OnBrowMenuOpen;
        HazardMenuEventSystem.instance.OnMatrixWindowOpen += OnMatrixWindowOpen;
        HazardMenuEventSystem.instance.OnConsequenceWindowOpen += OnConsequenceWindowOpen;
        HazardMenuEventSystem.instance.OnQuestionWindowOpen += OnQuestionWindowOpen;
        HazardMenuEventSystem.instance.OnReportMenuOpen += OnReportMenuOpen;
    }

    private void OnDisable()
    {
        HazardMenuEventSystem.instance.OnNextMenu -= OnNextWindow;
        HazardMenuEventSystem.instance.OnMenuClose -= OnMenuClose;
        HazardMenuEventSystem.instance.OnMenuOpen -= OnMenuOpen;
        HazardMenuEventSystem.instance.OnReportMenuOpen -= OnReportMenuOpen;
        HazardMenuEventSystem.instance.OnQuestionWindowOpen -= OnQuestionWindowOpen;

        HazardMenuEventSystem.instance.OnBrowMenuOpen -= OnBrowMenuOpen;
        HazardMenuEventSystem.instance.OnMatrixWindowOpen -= OnMatrixWindowOpen;
        HazardMenuEventSystem.instance.OnConsequenceWindowOpen -= OnConsequenceWindowOpen;
        HazardMenuEventSystem.instance.OnTarpMenuOpen -= OnTarpMenuOpen;
        HazardMenuEventSystem.instance.OnReportMenuOpen -= OnReportMenuOpen;
    }

    #endregion


    #region Callback_Methods
    private void OnMenuOpen(Hazard hazard)
    {
        SwitchWindow(1);
        transform.position = (Camera.main.transform.position + (Camera.main.transform.forward * windowOffset) + (Vector3.up * verticalWindowOffset));
        transform.LookAt(Camera.main.transform.position, Vector3.up);

    }

    private void OnMenuClose()
    {
        SwitchWindow(0);
    }

    private void OnNextWindow()
    {
        int currentWindow = (int)HazardMenuEventSystem.instance.State;
        int namesCount = Enum.GetNames(typeof(HazardMenuState)).Length;

        bool isAtEndOfChain = currentWindow == namesCount - 1 || currentWindow == -1;
        if (isAtEndOfChain)
        {
            SwitchWindow(0);
        }
        else
        {
            SwitchWindow(currentWindow + 1);
        }
    }
    private void OnReportMenuOpen()
    {
        SwitchWindow(4);
    }

    private void OnReportMenuClose()
    {
        _reportWindow.SetActive(false);
    }

    private void OnMatrixWindowOpen()
    {
        SwitchWindow(1);
    }

    private void OnQuestionWindowOpen()
    {
        SwitchWindow(2);
    }

    private void OnConsequenceWindowOpen()
    {
        SwitchWindow(3);
    }

    private void OnBrowMenuOpen(Hazard hazard){
        SwitchWindow(2);
        transform.position = (Camera.main.transform.position + (Camera.main.transform.forward * windowOffset) + (Vector3.up * verticalWindowOffset));
        transform.LookAt(Camera.main.transform.position, Vector3.up);
    }

    private void OnTarpMenuOpen(Hazard hazard){
        SwitchWindow(1);
        transform.position = (Camera.main.transform.position + (Camera.main.transform.forward * windowOffset) + (Vector3.up * verticalWindowOffset));
        transform.LookAt(Camera.main.transform.position, Vector3.up);
    }

    private void OnButtonHover()
    {

    }


    #endregion

    #region Window_Control
    public void DisableAllWindows()
    {
        foreach (Canvas window in _windows)
        {
            window.enabled = false;
        }
    }

    public void SwitchWindow(int index)
    {
        HazardMenuEventSystem.instance.SetWindowState(index);
        int namesCount = Enum.GetNames(typeof(HazardMenuState)).Length;
        if (index < 0 || index > namesCount - 1)
        {
            index = 0;

        }

        if (index == 0)
        {
            DisableAllWindows();
            return;
        }

        for (int loop = 0; loop < namesCount - 1; loop++)
        {
            bool shouldTurnOn = loop == index - 1;
            if (shouldTurnOn)
            {
                _windows[loop].enabled = true;
            }
            else
            {
                _windows[loop].enabled = false;
            }

        }

    }

    #endregion

    public int GetActiveButtonIndex(out int fillAmount)
    {
        fillAmount = 0;
        for (int loop = 0; loop < _buttons.Length; loop++)
        {
            if (_buttons[loop].IsActive)
            {
                fillAmount = _buttons[loop].GetFillAmoutInt();
                return loop;
            }
        }

        return -1;
    }

    #region Menu_Data_Exchange
    public MenuData GetCurrentMenuData()
    {
        int state = (int)HazardMenuEventSystem.instance.State;
        int fillAmount = 0;
        int currentButtonIndex = GetActiveButtonIndex(out fillAmount);
        Vector3 menuPosition = transform.position;
        MultipleChoiceQuestion question = _questionController.LastQuestion;

        MenuData data = new MenuData(state, currentButtonIndex, fillAmount, menuPosition, question.ToString());
        return data;

    }

    public void SyncWithMenuData(MenuData data)
    {
        SwitchWindow(data.MenuState);
        if (data.ActiveButtonIndex > -1 && data.ActiveButtonIndex < _buttons.Length - 1)
        {
            _buttons[data.ActiveButtonIndex].SetFillAmount(data.ActiveButtonFillPercentage);
        }
        transform.position = data.MenuPosition;
        transform.LookAt(Camera.main.transform.position);
        _questionController.InitializeQuestion(data.CurrentQuestion);
    }
    #endregion
}


