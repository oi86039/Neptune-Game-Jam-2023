using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(ControllerHandler))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1;
    public float maxSpeed = 0.1f;
    public float breakSpeed = 10;
    public float jumpHeight = 1.4f;
    public float jumpTime = 2f;
    public float gravMax = 10;
    public float jumpMax = 10;
    public float sprintModifier = 2;
    float jumpForce; 
    float gravity;
    bool crouching = false;
    bool sprinting = false;
    public bool facingF = true;
    Vector3 velocity = new Vector3(0, 0, 0);
    public Vector2 input = new Vector2();
    ControllerHandler controller;
    Transform sprite;
    public Object prefab;
    public float flashCool = 4;
    bool canFlash = true;
    float flashCoolTotal;

    void Start()
    {
        controller = GetComponent<ControllerHandler>();
        sprite = transform.Find("Sprite");
        jumpForce = (2f * jumpHeight) / (jumpTime / 2f);
        gravity = (-2f * jumpHeight) / Mathf.Pow(jumpTime / 2f, 2f);
        flashCoolTotal = flashCool;
    }

    void Update()
    {
        if(canFlash == false)
        {
            flashCool -= Time.deltaTime;
            Debug.Log("FALSE");
        }
        if(flashCool < 0)
        {
            canFlash = true;
            flashCool = flashCoolTotal;
        }
        if (!crouching)
        {
            if (Input.GetKey(KeyCode.Mouse0) && canFlash)
            {
                Instantiate(prefab, sprite.transform.rotation.y == 0 ? transform.position + Vector3.right * 5 : transform.position + Vector3.right * -5, Quaternion.identity);
                canFlash = false;
                Debug.Log("FLASHING");
            }
        }
    }
    private void FixedUpdate()
    {
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (input.y < 0 && controller.collisions.below)
        {
            //duck animation trigger
            transform.localScale = new Vector3(0.85f, 0.75f, 1);
            if (!crouching)
            {
                transform.Translate(Vector3.down * 0.225f);
                breakSpeed /= sprintModifier;
            }
            input.x = 0;
            crouching = true;
        }
        else
        {
            //stand animation trigger
            transform.localScale = new Vector3(0.7f, 1.2f, 1);
            if (crouching)
            {
                transform.Translate(Vector3.up * 0.225f);
                breakSpeed *= sprintModifier;
            }
            crouching = false;
        }
      if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Mouse1))
        {
            if (!sprinting) { 
                moveSpeed *= sprintModifier;
                maxSpeed *= sprintModifier;
            }
            sprinting = true;
        }
        else
        {
            if (sprinting)
            {
                moveSpeed /= sprintModifier;
                maxSpeed /= sprintModifier;
            }
            sprinting = false;
        }
            if (input.x == 0)
            {
                float step = (velocity.x - 0) / breakSpeed;
                if (step < 0.0001 && step > -0.0001)
                {
                    velocity.x = 0;
                }
                else
                {
                    velocity.x -= step;
                }
            }
        if (!crouching)
        {
            velocity.x = Mathf.Clamp(velocity.x + input.x * moveSpeed * Time.deltaTime, -maxSpeed, maxSpeed);
            if (velocity.x > 0f)
            {
                sprite.transform.eulerAngles = Vector3.zero;
                facingF = true;
            }
            else if (velocity.x < 0f)
            {
                sprite.transform.eulerAngles = new Vector3(0f, 180f, 0f);
                facingF = false;
            }
        }
        if ((Input.GetKey(KeyCode.Space) || input.y > 0) && controller.collisions.below && !crouching)
        {
            velocity.y = jumpForce;
        }
        velocity.y = Mathf.Clamp(velocity.y + (gravity * Time.deltaTime), -gravMax, jumpMax);
        velocity = controller.Move(velocity);
    }
}
