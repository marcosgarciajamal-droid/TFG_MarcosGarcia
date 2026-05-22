using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Header("Elevador")]
    public CharacterController Elevador;

    public float vel = 15f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       float y = Input.GetAxis("Vertical");
       float x = Input.GetAxis("Horizontal");

       Vector3 mover = transform.right * x + transform.forward*y;
       Elevador.Move(mover*vel*Time.deltaTime);
    }
}
