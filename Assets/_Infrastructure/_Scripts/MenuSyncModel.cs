using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public class MenuSyncModel {
    [RealtimeProperty(1, true, true)] private int _currentMenu;
    [RealtimeProperty(2, true, true)] private int _currentButton;
    [RealtimeProperty(3, true, true)] private int _currentButtonPercentage;
    [RealtimeProperty(4, true, true)] private string _currentQuestion;
    [RealtimeProperty(5, true, true)] private Vector3 _currentPosition;
    

}
