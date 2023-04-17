using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class HazardNetworkManager : RealtimeComponent<HazardStateListModel>
{

    [SerializeField] GameObject _hazardContainer;
    [SerializeField] Hazard [] _hazards;
    // Start is called before the first frame update
    private void Awake() {
        _hazards = _hazardContainer.GetComponentsInChildren<Hazard>();
    }
    void Start()
    {
        _hazards = _hazardContainer.GetComponentsInChildren<Hazard>();
        //Debug.Log(_hazards.Length.ToString() + " " + GetHazardStateData());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [ContextMenu("Get Hazards")]
    public void GetHazards(){
        _hazards = _hazardContainer.GetComponentsInChildren<Hazard>();
    }

    [ContextMenu("Get Hazards Data")]
    public string GetHazardStateData(){

        string data = "";
        for(int loop = 0; loop < _hazards.Length; loop++){
            if(loop == _hazards.Length - 1){
                data += _hazards[loop].GetHazardData();
            }
            else{
                data += _hazards[loop].GetHazardData() + "%";
            }
        }
        Debug.Log(_hazards.Length.ToString() + " " + data);
        return data;
    }
    public void SetHazardsStates(string data){
        string [] info = data.Split('%');
        if(info.Length != _hazards.Length){
            Debug.Log("not enough data " + info.Length);
        }
        else{
            for(int loop = 0; loop < _hazards.Length; loop++){
                _hazards[loop].SetHazardState(info[loop]);
            }
        }
    }

    public void SetNetworkHazardData(uint id){
        if(!PlayerExistsInDict(id)){
            Debug.Log("Invalid ID");
            return;
        }

        HazardStateModel newModel = new HazardStateModel();
        newModel.hazardStates = GetHazardStateData();
        model.playerHazards[id] = newModel;

    }

    public void ApplyNetworkHazardData(uint id){
        if(!PlayerExistsInDict(id)){
            Debug.Log("Invalid ID");
            return;
        }
        string data = model.playerHazards[id].hazardStates;
        SetHazardsStates(data);
    }
    private bool PlayerExistsInDict(uint id)
    {
        try
        {
          HazardStateModel _ = model.playerHazards[id];
          return true;
        }
        catch
        {
          return false;
        }
    }

    
}
