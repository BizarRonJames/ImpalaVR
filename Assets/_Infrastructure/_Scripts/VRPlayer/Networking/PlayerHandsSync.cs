using Autohand;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class PlayerHandsSync : RealtimeComponent<PlayerHandsModel>
{
    enum Handedness {Left, Right }

    [SerializeField] Handedness handedness;
    
    [SerializeField] Finger model_thumb;
    [SerializeField] Finger model_index;
    [SerializeField] Finger model_middle;
    [SerializeField] Finger model_ring;
    [SerializeField] Finger model_pinky;

    Finger origin_thumb;
    Finger origin_index;
    Finger origin_middle;
    Finger origin_ring;
    Finger origin_pinky;

    /*float _thumb_bend_offset => model.thumb_bend_offset;
    float _index_bend_offset => model.index_bend_offset;
    float _middle_bend_offset => model.middle_bend_offset;
    float _ring_bend_offset => model.ring_bend_offset;
    float _pinky_bend_offset => model.pinky_bend_offset;*/

    private bool _isSelf;

    float thumb_Offset;
    float index_Offset;
    float middle_Offset;
    float ring_Offset;
    float pinky_Offset;

    private void Start()
    {
        if (GetComponent<RealtimeAvatar>().isOwnedLocallyInHierarchy)
        {
            if (handedness == Handedness.Left)
            {
                origin_thumb = Player.instance.l_Thumb;
                origin_index = Player.instance.l_Index;
                origin_middle = Player.instance.l_Middle;
                origin_ring = Player.instance.l_Ring;
                origin_pinky = Player.instance.l_Pinky;
            }
            else
            {
                origin_thumb = Player.instance.r_Thumb;
                origin_index = Player.instance.r_Index;
                origin_middle = Player.instance.r_Middle;
                origin_ring = Player.instance.r_Ring;
                origin_pinky = Player.instance.r_Pinky;
            }
            _isSelf = true;
        }
    }

    private void Update()
    {
        if (_isSelf)
        {
            if(thumb_Offset != origin_thumb.bendOffset)
            {
                thumb_Offset = origin_thumb.bendOffset;
                model.thumb_bend_offset = thumb_Offset;
            }

            if (index_Offset != origin_index.bendOffset)
            {
                index_Offset = origin_index.bendOffset;
                model.index_bend_offset = index_Offset;
            }

            if (middle_Offset != origin_thumb.bendOffset)
            {
                middle_Offset = origin_middle.bendOffset;
                model.middle_bend_offset = middle_Offset;
            }

            if (ring_Offset != origin_ring.bendOffset)
            {
                ring_Offset = origin_ring.bendOffset;
                model.ring_bend_offset = ring_Offset;
            }

            if (pinky_Offset != origin_thumb.bendOffset)
            {
                pinky_Offset = origin_pinky.bendOffset;
                model.pinky_bend_offset = pinky_Offset;
            }
        }
    }

    protected override void OnRealtimeModelReplaced(PlayerHandsModel previousModel, PlayerHandsModel currentModel)
    {
        if(previousModel != null)
        {
            previousModel.thumb_bend_offsetDidChange -= Thumb_bend_offsetDidChange;
            previousModel.index_bend_offsetDidChange -= Index_bend_offsetDidChange;
            previousModel.middle_bend_offsetDidChange -= Middle_bend_offsetDidChange;
            previousModel.ring_bend_offsetDidChange -= Ring_bend_offsetDidChange;
            previousModel.pinky_bend_offsetDidChange -= Pinky_bend_offsetDidChange;
        }

        if(currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.thumb_bend_offset = 0;
                currentModel.index_bend_offset = 0;
                currentModel.middle_bend_offset = 0;
                currentModel.ring_bend_offset = 0;
                currentModel.pinky_bend_offset = 0;
            }

            Update_Thumb();
            Update_Index();
            Update_Middle();
            Update_Ring();
            Update_Pinky();

            currentModel.thumb_bend_offsetDidChange += Thumb_bend_offsetDidChange;
            currentModel.index_bend_offsetDidChange += Index_bend_offsetDidChange;
            currentModel.middle_bend_offsetDidChange += Middle_bend_offsetDidChange;
            currentModel.ring_bend_offsetDidChange += Ring_bend_offsetDidChange;
            currentModel.pinky_bend_offsetDidChange += Pinky_bend_offsetDidChange;
        }

        base.OnRealtimeModelReplaced(previousModel, currentModel);
    }

    private void Thumb_bend_offsetDidChange(PlayerHandsModel model, float value)
    {        
        Update_Thumb();
    }

    private void Update_Thumb()
    {        
        model_thumb.bendOffset = model.thumb_bend_offset;
        model_thumb.SetFingerBend(model.thumb_bend_offset);
    }

    private void Index_bend_offsetDidChange(PlayerHandsModel model, float value)
    {
        Update_Index();
    }

    private void Update_Index()
    {
        model_index.bendOffset = model.index_bend_offset;
        model_index.SetFingerBend(model.index_bend_offset);
    }

    private void Middle_bend_offsetDidChange(PlayerHandsModel model, float value)
    {
        Update_Middle();
    }

    private void Update_Middle()
    {
        model_middle.bendOffset = model.middle_bend_offset;
        model_middle.SetFingerBend(model.middle_bend_offset);
    }

    private void Ring_bend_offsetDidChange(PlayerHandsModel model, float value)
    {
        Update_Ring();
    }

    private void Update_Ring()
    {
        model_ring.bendOffset = model.ring_bend_offset;
        model_ring.SetFingerBend(model.ring_bend_offset);
    }

    private void Pinky_bend_offsetDidChange(PlayerHandsModel model, float value)
    {
        Update_Pinky();
    }

    private void Update_Pinky()
    {       
        model_pinky.bendOffset = model.pinky_bend_offset;
        model_pinky.SetFingerBend(model.pinky_bend_offset);
    }
}
