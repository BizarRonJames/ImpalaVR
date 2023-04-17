using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public class HoverSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float _selectTime = 1f;
    [SerializeField] private Image _fillCircle;
    [SerializeField] private Button _button;
    [SerializeField] private int index = 0;
    [SerializeField] private bool _isActive;


    private float time;
    private bool entered = false;
    private Coroutine selectRoutine;

    public UnityEvent select;

    public int Index { get => index; set => index = value; }
    public bool IsActive { get => _isActive; set => _isActive = value; }

    public UnityEvent GetSelectEvent(){
        return select;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        entered = true;
        StartSelectRoutine();

    }
    public bool GetCurrentState(out float filled){
        
        filled = _fillCircle.fillAmount;
        return entered;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        entered = false;
    }

    public void SetFillAmount(float amount){
        _fillCircle.fillAmount = amount;
    }
    public int GetFillAmoutInt(){
        return (int)(_fillCircle.fillAmount * 1000);
    }
    public float GetFillAmout(){
        return _fillCircle.fillAmount;
    }
    // Start is called before the first frame update
    void Start()
    {
        IsActive = false;
    }
    private void Awake() {
        _fillCircle = transform.Find("FillCircle").GetComponent<Image>();
        _button = GetComponent<Button>();
        _fillCircle.fillAmount = 0;
        selectRoutine = null;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (entered)
        {
            time += Time.deltaTime;
            fillCircle.fillAmount = (time / timeToSelect);
            if (time > timeToSelect)
            {
                time = 0.0f;
                select.Invoke();
            }
        }
        else
        {
            time = 0.0f;
            fillCircle.fillAmount = 0;
        }
        */
    }

    private void StartSelectRoutine()
    {
        if (selectRoutine == null)
        {
            selectRoutine = StartCoroutine(SelectRoutine(_selectTime));
        }
        else
        {
            return;
        }
    }


    IEnumerator SelectRoutine(float period)
    {
        float time = 0;
        IsActive = true;
        //HazardMenuEventSystem.instance.ButtonHover(index);
        while (true)
        {
            //HazardMenuEventSystem.instance.ButtonHover(index);
            if (time >= period)
            {
                IsActive = false;
                select.Invoke();
                _fillCircle.fillAmount = 0;
                selectRoutine = null;
                //HazardMenuEventSystem.instance.ButtonHoverExit(index);
                break;
            }
            else if (!entered)
            {
                _fillCircle.fillAmount = 0;
                selectRoutine = null;
                //HazardMenuEventSystem.instance.ButtonHoverExit(index);
                break;
            }
            else
            {
                time += Time.deltaTime;
                _fillCircle.fillAmount = time / period;
                yield return null;
            }
        }
    }






}
