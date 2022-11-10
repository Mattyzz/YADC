using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class patrol : MonoBehaviour
{
 public float speed;
 public float distance;
 private bool movingRight = true;

 public Transform groundDetection;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Update(){
    transform.Translate(Vector2.right * speed * Time.deltaTime);
        anim.SetBool("isMoving", true);
    RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down,distance);

        if (groundInfo.collider == false)
        {
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }

        }
    }
 }

