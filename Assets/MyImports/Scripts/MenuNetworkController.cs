using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Normal.Realtime;

[System.Serializable]
public class MenuData{
    [SerializeField] private int menuState;
    [SerializeField] private int activeButtonIndex;
    [SerializeField] private float activeButtonFillPercentage;
    [SerializeField] private Vector3 menuPosition;
    [SerializeField] private MultipleChoiceQuestion currentQuestion;

    public int MenuState { get => menuState; set => menuState = value; }
    public int ActiveButtonIndex { get => activeButtonIndex; set => activeButtonIndex = value; }
    public float ActiveButtonFillPercentage { get => activeButtonFillPercentage; set => activeButtonFillPercentage = value; }
    public Vector3 MenuPosition { get => menuPosition; set => menuPosition = value; }
    public MultipleChoiceQuestion CurrentQuestion { get => currentQuestion; set => currentQuestion = value; }

    public MenuData(int menuState, int activeButtonIndex, int fillPercentage, Vector3 menuPosition, string multipleChoiceQuestion)
    {
        this.MenuState = menuState;
        this.ActiveButtonIndex = activeButtonIndex;
        this.activeButtonFillPercentage = fillPercentage/1000;
        this.MenuPosition = menuPosition;
        MultipleChoiceQuestion question = new MultipleChoiceQuestion();
        
        this.currentQuestion = question.DecodeString(multipleChoiceQuestion);
    }

    public UserMenuModel ConvertToUserMenuModel(){
        UserMenuModel model = new UserMenuModel();
        model.menuState = this.MenuState;
        model.activeButtonIndex = this.ActiveButtonIndex;
        model.activeButtonFillPercentage = (int) (this.activeButtonFillPercentage * 1000);
        model.menuPosition = this.MenuPosition;
        model.hazardQuestion = this.currentQuestion.ToString(); 

        return model;
    }

    public static MenuData ConvertFromUserMenuModel(UserMenuModel model){
        MenuData data = new MenuData(model.menuState, model.activeButtonIndex, model.activeButtonFillPercentage, model.menuPosition, model.hazardQuestion);
        return data;
    }
}
public class MenuNetworkController : RealtimeComponent<UserMenuDictionaryModel>
{

    [SerializeField] MenuData _currentMenuData;
    QuestionController _questionController;
    HazardMenuManager _hazardMenuManager;
    private void Awake() {
        _questionController = GetComponentInChildren<QuestionController>();
        _hazardMenuManager = GetComponentInChildren<HazardMenuManager>();
    }
    private void Start() {
        
    }

    public MenuData GetCurrentPlayerMenuData(uint playerId){
        if(!PlayerExistsInDict(playerId)){
            return null;
        }
        UserMenuModel menuModel = model.usersMenuData[playerId];
        MenuData data = MenuData.ConvertFromUserMenuModel(menuModel);
        return data;
    }

    public void AddPlayerMenuDataToDict(uint playerId, MenuData data){
        UserMenuModel userModel = data.ConvertToUserMenuModel();
        model.usersMenuData.Add(playerId, userModel);
    }

    public void UpdateModelData(uint playerId, MenuData data){
        if(!PlayerExistsInDict(playerId)){
            Debug.Log("Player ID not found");
            return;
        }

        UserMenuModel newModel = data.ConvertToUserMenuModel();
        model.usersMenuData[playerId] = newModel;
    }

    private bool PlayerExistsInDict(uint playerId)
    {
        try
        {
          UserMenuModel _ = model.usersMenuData[playerId];
          return true;
        }
        catch
        {
          return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
