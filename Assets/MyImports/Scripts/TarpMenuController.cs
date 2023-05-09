using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarpMenuController : MonoBehaviour
{
    [SerializeField] Hazard _currentHazard;
    [SerializeField] HoverSelect _buttonLow;
    [SerializeField] HoverSelect _buttonMedium;
    [SerializeField] HoverSelect _buttonHigh;

    // Start is called before the first frame update
    void Start()
    {
        InitiliseButtons();
    }
    private void OnEnable() {
        
        HazardMenuEventSystem.instance.OnTarpMenuOpen += OnTarpMenuOpen;
    }

    private void OnDisable() {
        HazardMenuEventSystem.instance.OnTarpMenuOpen -= OnTarpMenuOpen;
    }
    private void OnTarpMenuOpen(Hazard hazard){
        _currentHazard = hazard;
        //InitiliseButtons();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitiliseButtons(){
        if(_currentHazard == null){
            return;
        }
        

        _buttonLow.select.AddListener(() => _currentHazard.UserTarpRating = 1);
        _buttonMedium.select.AddListener(() => _currentHazard.UserTarpRating = 2);
        _buttonHigh.select.AddListener(() => _currentHazard.UserTarpRating = 3);

        _buttonLow.select.AddListener(() => HazardMenuEventSystem.instance.QuestionWindowOpen());
        _buttonMedium.select.AddListener(() => HazardMenuEventSystem.instance.QuestionWindowOpen());
        _buttonHigh.select.AddListener(() => HazardMenuEventSystem.instance.QuestionWindowOpen());
    }

    public void SetHazardTarpRating(int rating){
        _currentHazard.UserTarpRating = rating;

    }
}
