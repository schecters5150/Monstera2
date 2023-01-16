using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusModel : MonoBehaviour
{
    public bool isDodging { get; set; } = false;
    public bool isHitstun { get; set; } = false;
    public bool isAttacking { get; set; } = false;
    public bool isBump { get; set; } = false;
    public bool isGrounded { get; set; } = false;
    public bool disableDodge { get; set; } = false;
    public bool disableAttack { get; set; } = false;
    public bool disableJump { get; set; } = false;
    public bool disableMove { get; set; } = false;
    public bool isCling { get; set; } = false;
    public bool isClingJump { get; set; } = false;
    public bool isInvincible { get; set; } = false;
    public bool staminaDepleted { get; set; } = false;
}
