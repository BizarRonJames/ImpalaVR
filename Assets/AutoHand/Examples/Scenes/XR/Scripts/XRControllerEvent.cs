using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Autohand.Demo {
    public class XRControllerEvent : MonoBehaviour
    {
        public XRHandControllerLink link;
        public CommonButton button;
        public UnityEvent Pressed;
        public UnityEvent Released;
        bool pressed = false;
        private void Update()
        {
            //Debug.Log("Updating");
            if (link.ButtonPressed(button) && !pressed)
            {
                Debug.Log("Button Pressed");
                Pressed?.Invoke();
                pressed = true;
            }
            else if (!link.ButtonPressed(button) && pressed)
            {
                //Debug.Log("Button released");
                Released?.Invoke();
                pressed = false;
            }
        }
    }
}