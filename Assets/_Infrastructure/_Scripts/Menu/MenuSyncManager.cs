using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentMenu{
    none = 0,
    MatrixMenu = 1,
    MCQ = 2,
    Consequence = 3
}

public class MenuSyncData{
    private CurrentMenu currentMenu = CurrentMenu.none;
    private int currentButton = -1;
    private float currentButtonPercentage = 0;
    private Transform menuPosition;
    private MultipleChoiceQuestion currentQuestion;

    public MenuSyncData(CurrentMenu currentMenu, int currentButton, float currentButtonPercentage, Transform menuPosition, MultipleChoiceQuestion currentQuestion)
    {
        this.CurrentMenu = currentMenu;
        this.CurrentButton = currentButton;
        this.CurrentButtonPercentage = currentButtonPercentage;
        this.MenuPosition = menuPosition;
        this.currentQuestion = currentQuestion;
    }

    public float CurrentButtonPercentage { get => currentButtonPercentage; set => currentButtonPercentage = value; }
    public int CurrentButton { get => currentButton; set => currentButton = value; }
    public CurrentMenu CurrentMenu { get => currentMenu; set => currentMenu = value; }
    public Transform MenuPosition { get => menuPosition; set => menuPosition = value; }
    public MultipleChoiceQuestion CurrentQuestion { get => currentQuestion; set => currentQuestion = value; }
}
public class MenuSyncManager : MonoBehaviour
{
    [SerializeField] CurrentMenu _currentMenu;
    [SerializeField] HoverSelect [] _hoverButtons;
    [SerializeField] Canvas [] _rateMenus = new Canvas[3];

    [SerializeField] Canvas [] menus;


    private void Awake() {
        _hoverButtons = GetComponentsInChildren<HoverSelect>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitialiseButtons(){
        for(int loop = 0; loop < _hoverButtons.Length; loop++){
            _hoverButtons[loop].Index = loop;
        }
    }
    public int GetCurrentSelectedButton(out float fillAmount){
        fillAmount = 0;
        for(int loop = 0; loop < _hoverButtons.Length; loop++){
            bool isSelected = _hoverButtons[loop].GetCurrentState(out fillAmount);
            if(isSelected){
                return loop;
            }
            else{
                continue;
            }
        }

        return -1;
        
    }

    public MenuSyncData GetCurrentSyncData(){
        float fillAmount = 0;
        int currentButtonIndex = GetCurrentSelectedButton(out fillAmount);
        MultipleChoiceQuestion currentQuestion = QuestionManager.instance.LastQuestion;
        Transform currentPostion = GetCurrentMenuTransform(_currentMenu);
        MenuSyncData menuSyncData = new MenuSyncData(_currentMenu, currentButtonIndex, fillAmount, currentPostion, currentQuestion);
        return menuSyncData;
    }


    public void InitialiseCurrentMenu(MenuSyncData syncData){
        SetActiveMenu(syncData.CurrentMenu);
        
        if(syncData.CurrentButton >= 0){
            _hoverButtons[syncData.CurrentButton].SetFillAmount(syncData.CurrentButtonPercentage);
        }

        if((int) syncData.CurrentMenu > 0){
            SetCurrentMenuTransform(syncData.MenuPosition);
            QuestionManager.instance.InitializeQuestion(syncData.CurrentQuestion);
        }
        
    }



    public void SetActiveMenu(CurrentMenu currentMenu){
        int index = (int) currentMenu;
        EnableMenuAtIndex(index - 1);
    }

    public Transform GetCurrentMenuTransform(CurrentMenu currentMenu){
        int index = (int) currentMenu - 1;
        return menus[index].transform;
    }

    public void SetCurrentMenuTransform(Transform transform){
        int index = (int) _currentMenu - 1;
        menus[index].transform.position = transform.position;
        menus[index].transform.rotation = transform.rotation;
    }

    public void DisableAllMenus(){
        for(int loop = 0; loop < menus.Length; loop++){
            menus[loop].enabled = false;
        }
    }

    public void EnableMenuAtIndex(int index){
        if(index < 0 || index > menus.Length-1){
            Debug.Log("Index out of range");
            return;
        }
        for(int loop = 1; loop < menus.Length; loop++){
            if(loop == index){
                menus[loop].enabled = true;
            }
            else{
                menus[loop].enabled = false;
            }
        }

    }

    public void UploadCurrentMenuData(){
        
        float currentFillAmount = 0;
        int currentButtonIndex = GetCurrentSelectedButton(out currentFillAmount);
        Transform currentMenuTransform = GetCurrentMenuTransform(_currentMenu);
        MultipleChoiceQuestion currentQuestion = QuestionManager.instance.LastQuestion;
        MenuSyncData menuSyncData = new MenuSyncData(_currentMenu, currentButtonIndex, currentFillAmount, currentMenuTransform, currentQuestion);
        int playerIndex = Player.instance.playerIndex;
    }



}
