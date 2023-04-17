using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatrixColumn : MonoBehaviour
{
    [SerializeField] int _column = 0;
    [SerializeField] TextMeshProUGUI[] _numbers;
    [SerializeField] HoverSelect[] _buttons;
    [SerializeField] MatrixWindowController _matrixWindowController;
    // Start is called before the first frame update
    void Start()
    {
        _matrixWindowController = GetComponentInParent<MatrixWindowController>();
        _numbers = GetComponentsInChildren<TextMeshProUGUI>();
        _buttons = GetComponentsInChildren<HoverSelect>();

        InitialiseButtonValues();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitialiseButtonValues()
    {
        for (int loop = 0; loop < _numbers.Length; loop++)
        {
            int value = (_numbers.Length - loop) * (_column + 1);
            _numbers[loop].text = value.ToString();
            _buttons[loop].select.AddListener(delegate { _matrixWindowController.SetHazardPriority(value); });
            _buttons[loop].select.AddListener(delegate { HazardMenuEventSystem.instance.NextWindow(); });
        }
    }
}
