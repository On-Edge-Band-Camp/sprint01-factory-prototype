using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 keyInputs;
    public float moveForce;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Keyboard wasd/arrow input vector
        keyInputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        rb.AddForce(keyInputs * moveForce, ForceMode2D.Force);
    }
}
