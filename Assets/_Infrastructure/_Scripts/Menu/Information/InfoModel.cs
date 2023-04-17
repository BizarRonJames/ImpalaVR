using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class InfoModel
{
    [RealtimeProperty(1, true, false)]
    string _header;

    [RealtimeProperty(2, true, false)]
    string _subHeader;

    [RealtimeProperty(3, true, true)]
    string _text;

    [RealtimeProperty(4, true, false)]
    bool _shouldAnimate = false;



    [RealtimeProperty(9, true, true)]
    bool _showingInfoOK = false;

    [RealtimeProperty(10, true, true)]
    bool _completed = false;
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class InfoModel : RealtimeModel {
    public string header {
        get {
            return _headerProperty.value;
        }
        set {
            if (_headerProperty.value == value) return;
            _headerProperty.value = value;
            InvalidateReliableLength();
        }
    }
    
    public string subHeader {
        get {
            return _subHeaderProperty.value;
        }
        set {
            if (_subHeaderProperty.value == value) return;
            _subHeaderProperty.value = value;
            InvalidateReliableLength();
        }
    }
    
    public string text {
        get {
            return _textProperty.value;
        }
        set {
            if (_textProperty.value == value) return;
            _textProperty.value = value;
            InvalidateReliableLength();
            FireTextDidChange(value);
        }
    }
    
    public bool shouldAnimate {
        get {
            return _shouldAnimateProperty.value;
        }
        set {
            if (_shouldAnimateProperty.value == value) return;
            _shouldAnimateProperty.value = value;
            InvalidateReliableLength();
        }
    }
    
    public bool showingInfoOK {
        get {
            return _showingInfoOKProperty.value;
        }
        set {
            if (_showingInfoOKProperty.value == value) return;
            _showingInfoOKProperty.value = value;
            InvalidateReliableLength();
            FireShowingInfoOKDidChange(value);
        }
    }
    
    public bool completed {
        get {
            return _completedProperty.value;
        }
        set {
            if (_completedProperty.value == value) return;
            _completedProperty.value = value;
            InvalidateReliableLength();
            FireCompletedDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(InfoModel model, T value);
    public event PropertyChangedHandler<string> textDidChange;
    public event PropertyChangedHandler<bool> showingInfoOKDidChange;
    public event PropertyChangedHandler<bool> completedDidChange;
    
    public enum PropertyID : uint {
        Header = 1,
        SubHeader = 2,
        Text = 3,
        ShouldAnimate = 4,
        ShowingInfoOK = 9,
        Completed = 10,
    }
    
    #region Properties
    
    private ReliableProperty<string> _headerProperty;
    
    private ReliableProperty<string> _subHeaderProperty;
    
    private ReliableProperty<string> _textProperty;
    
    private ReliableProperty<bool> _shouldAnimateProperty;
    
    private ReliableProperty<bool> _showingInfoOKProperty;
    
    private ReliableProperty<bool> _completedProperty;
    
    #endregion
    
    public InfoModel() : base(null) {
        _headerProperty = new ReliableProperty<string>(1, _header);
        _subHeaderProperty = new ReliableProperty<string>(2, _subHeader);
        _textProperty = new ReliableProperty<string>(3, _text);
        _shouldAnimateProperty = new ReliableProperty<bool>(4, _shouldAnimate);
        _showingInfoOKProperty = new ReliableProperty<bool>(9, _showingInfoOK);
        _completedProperty = new ReliableProperty<bool>(10, _completed);
    }
    
    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        _headerProperty.UnsubscribeCallback();
        _subHeaderProperty.UnsubscribeCallback();
        _textProperty.UnsubscribeCallback();
        _shouldAnimateProperty.UnsubscribeCallback();
        _showingInfoOKProperty.UnsubscribeCallback();
        _completedProperty.UnsubscribeCallback();
    }
    
    private void FireTextDidChange(string value) {
        try {
            textDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireShowingInfoOKDidChange(bool value) {
        try {
            showingInfoOKDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireCompletedDidChange(bool value) {
        try {
            completedDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        var length = 0;
        length += _headerProperty.WriteLength(context);
        length += _subHeaderProperty.WriteLength(context);
        length += _textProperty.WriteLength(context);
        length += _shouldAnimateProperty.WriteLength(context);
        length += _showingInfoOKProperty.WriteLength(context);
        length += _completedProperty.WriteLength(context);
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var writes = false;
        writes |= _headerProperty.Write(stream, context);
        writes |= _subHeaderProperty.Write(stream, context);
        writes |= _textProperty.Write(stream, context);
        writes |= _shouldAnimateProperty.Write(stream, context);
        writes |= _showingInfoOKProperty.Write(stream, context);
        writes |= _completedProperty.Write(stream, context);
        if (writes) InvalidateContextLength(context);
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        var anyPropertiesChanged = false;
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            var changed = false;
            switch (propertyID) {
                case (uint) PropertyID.Header: {
                    changed = _headerProperty.Read(stream, context);
                    break;
                }
                case (uint) PropertyID.SubHeader: {
                    changed = _subHeaderProperty.Read(stream, context);
                    break;
                }
                case (uint) PropertyID.Text: {
                    changed = _textProperty.Read(stream, context);
                    if (changed) FireTextDidChange(text);
                    break;
                }
                case (uint) PropertyID.ShouldAnimate: {
                    changed = _shouldAnimateProperty.Read(stream, context);
                    break;
                }
                case (uint) PropertyID.ShowingInfoOK: {
                    changed = _showingInfoOKProperty.Read(stream, context);
                    if (changed) FireShowingInfoOKDidChange(showingInfoOK);
                    break;
                }
                case (uint) PropertyID.Completed: {
                    changed = _completedProperty.Read(stream, context);
                    if (changed) FireCompletedDidChange(completed);
                    break;
                }
                default: {
                    stream.SkipProperty();
                    break;
                }
            }
            anyPropertiesChanged |= changed;
        }
        if (anyPropertiesChanged) {
            UpdateBackingFields();
        }
    }
    
    private void UpdateBackingFields() {
        _header = header;
        _subHeader = subHeader;
        _text = text;
        _shouldAnimate = shouldAnimate;
        _showingInfoOK = showingInfoOK;
        _completed = completed;
    }
    
}
/* ----- End Normal Autogenerated Code ----- */