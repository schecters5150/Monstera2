using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveService 
{
    // Start is called before the first frame update
    public void ControllerMove(float walkSpeed, float jumpHeight, float gravity);
    public void HitStun(float stunSpeed, int directionX);
    public void Dodge( float dodgeSpeed, float gravity);
    public void SetJumps(int jumps);
    public int GetDirectionX();
}
