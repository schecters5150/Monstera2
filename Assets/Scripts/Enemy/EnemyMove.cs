using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Enemy;

public class EnemyMove
{
    public GameObject lineOfSight;
    public EnemyController enemyController;
    public MoveType moveType;
    public float startPositionY;
    MoveFunction move;

    public EnemyMove(EnemyController enemyController)
    {
        this.enemyController = enemyController;
        startPositionY = enemyController.transform.position.y;
    }

    public void TriggerMove()
    {
        if (moveType == MoveType.Walk) move = new MoveWalk();
        else if (moveType == MoveType.Hover) move = new MoveHover();
        else if (moveType == MoveType.Jump)
        {
            move = new Jump();
            move.isGrounded = enemyController.isGrounded;
        }
        else if (moveType == MoveType.MoveLockHorizontal) 
        { 
            move = new MoveLockHorizontal();
            move.startPositionY = startPositionY;
        }
        else move = new Idle();
        move.Move(enemyController.rb, enemyController.moveSpeed);
    }

    public void TriggerHitstun(float speed)
    {
        var move = new Hitstun();
        move.Move(enemyController.rb, speed);
    }
}

public class MoveFunction
{
    internal bool isGrounded;
    internal float startPositionY;

    public virtual void Move(Rigidbody2D rb, float speed)
    {
        throw new System.NotImplementedException("This function must not be called. It must be overridden.");
    }

    public virtual void Move(Rigidbody2D rb, float speed, bool isGrounded)
    {
        throw new System.NotImplementedException("This function must not be called. It must be overridden.");
    }
}

public class Idle : MoveFunction
{
    public override void Move(Rigidbody2D rb, float speed)
    {
        return;
    }
}

public class MoveWalk : MoveFunction
{
    public override void Move(Rigidbody2D rb, float speed)
    {
        var playerPosition = GameObject.Find("Player").transform.position;
        var direction = rb.position.x > playerPosition.x ? -1 : 1;
        Vector3 newPosition = new Vector3(rb.position.x + speed * direction * Time.deltaTime, rb.position.y, 0); 
        rb.position = newPosition;
    }
}

public class MoveLockHorizontal : MoveFunction
{
    public override void Move(Rigidbody2D rb, float speed)
    {
        var playerPosition = GameObject.Find("Player").transform.position;
        var direction = rb.position.x > playerPosition.x ? -1 : 1;
        Vector3 newPosition = new Vector3(rb.position.x + speed * direction * Time.deltaTime, startPositionY, 0);
        rb.position = newPosition;
    }
}

public class MoveHover : MoveFunction
{
    public override void Move(Rigidbody2D rb, float speed)
    {
        var playerPosition = GameObject.Find("Player").transform.position;
        var directionX = rb.transform.position.x > playerPosition.x ? -1 : 1;
        var directionY = rb.position.y > playerPosition.y ? -1 : 1;
        var magX = rb.position.x - playerPosition.x;
        var magY = rb.position.y - playerPosition.y;
        var angle = Mathf.Atan(magY / magX);

        var displaceX = speed * Time.deltaTime * directionX * Mathf.Abs(Mathf.Cos(angle));
        var displaceY = speed * Time.deltaTime * directionY * Mathf.Abs(Mathf.Sin(angle));

        Vector3 newPosition = new Vector3(rb.position.x + displaceX, rb.position.y + displaceY, 0);
        rb.position = newPosition;
    }
}

public class Hitstun : MoveFunction
{
    public override void Move(Rigidbody2D rb, float speed)
    {
        if (GameObject.Find("Player").GetComponent<PlayerController>()._statusModel.isDodging) return;

        var playerPosition = GameObject.Find("Player").transform.position;
        bool horizontal = Mathf.Abs(playerPosition.x - rb.position.x) >= Mathf.Abs(playerPosition.y - rb.position.y);

        if (horizontal)
        {
            var direction = rb.position.x < playerPosition.x ? -1 : 1;
            Vector3 newPosition = new Vector3(rb.position.x + speed * direction * Time.deltaTime, rb.position.y, 0);
            rb.position = newPosition;
        }
        else
        {
            var direction = rb.position.y < playerPosition.y ? -1 : 1;
            Vector3 newPosition = new Vector3(rb.position.x, rb.position.y + speed * direction * Time.deltaTime, 0);
            rb.position = newPosition;
        }
    }
}

public class Jump : MoveFunction
{
    public override void Move(Rigidbody2D rb, float speed)
    {
        var playerPosition = GameObject.Find("Player").transform.position;
        var direction = rb.position.x > playerPosition.x ? -1 : 1;
        var speedY = rb.velocity.y - 50 * Time.deltaTime;
        if (isGrounded) speedY = speed *2;

        Vector3 velocity = new Vector3(speed * direction, speedY, 0);
        rb.velocity = velocity;
    }
}