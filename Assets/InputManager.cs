using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InputMaster _inputMaster;
    public InventoryModel _inventoryModel;

    public bool JumpIsPressed()
    {
        return _inputMaster.Platformer.Jump.IsPressed();
    }
    public bool HoverIsPressed()
    {
        if (!_inventoryModel.jsonModel.hover) return false;
        return _inputMaster.Platformer.Hover.IsPressed();
    }
    /* public bool FireIsPressed()
     {
         return _inputMaster.Platformer.
     }*/
    public bool DodgeIsPressed()
    {
        if (!_inventoryModel.jsonModel.dodge) return false;
        return _inputMaster.Platformer.Dodge.IsPressed();
    }
    public bool StartIsPressed()
    {
        return _inputMaster.Platformer.Start.IsPressed();
    }
    public bool ZoomIsPressed()
    {
        return _inputMaster.Platformer.Zoom.IsPressed();
    }
    public bool JumpTriggered()
    {
        return _inputMaster.Platformer.Jump.triggered;
    }
    public bool HoverTriggered()
    {
        if (!_inventoryModel.jsonModel.hover) return false;
        return _inputMaster.Platformer.Hover.triggered;
    }
    public bool SpellTriggered()
    {
        return _inputMaster.Platformer.Spell.triggered;
    }
    public bool SpellSwapTriggered()
    {
        return _inputMaster.Platformer.SpellSwap.triggered;
    }
    public bool HealTriggered()
    {
        return _inputMaster.Platformer.Heal.triggered;
    }
    public bool ZoomTriggered()
    {
        return _inputMaster.Platformer.Zoom.triggered;
    }
    public bool DodgeTriggered()
    {
        if (!_inventoryModel.jsonModel.dodge) return false;
        return _inputMaster.Platformer.Dodge.triggered;
    }

    public bool ParryTriggered()
    {
        if (!_inventoryModel.jsonModel.parry) return false;
        return _inputMaster.Platformer.Parry.triggered;
    }

    public bool AttackLeftTriggered()
    {
        if (!_inventoryModel.jsonModel.sword) return false;
        return _inputMaster.Platformer.AttackLeft.triggered;
    }
    public bool AttackRightTriggered()
    {
        if (!_inventoryModel.jsonModel.sword) return false;
        return _inputMaster.Platformer.AttackRight.triggered;
    }
    public bool AttackUpTriggered()
    {
        if (!_inventoryModel.jsonModel.sword) return false;
        return _inputMaster.Platformer.AttackUp.triggered;
    }
    public bool AttackDownTriggered()
    {
        if (!_inventoryModel.jsonModel.sword) return false;
        return _inputMaster.Platformer.AttackDown.triggered;
    }


    public float MovementX()
    {
        return _inputMaster.Platformer.Movement.ReadValue<Vector2>().x;
    }
    public float MovementY()
    {
        return _inputMaster.Platformer.Movement.ReadValue<Vector2>().y;
    }


    public void Start()
    {
        _inventoryModel = GetComponent<InventoryModel>();
    }
    public void Awake()
    {
        _inputMaster = new InputMaster();
    }

    private void OnEnable()
    {
        _inputMaster.Enable();
    }
    private void OnDisable()
    {
        _inputMaster.Disable();
    }
}
