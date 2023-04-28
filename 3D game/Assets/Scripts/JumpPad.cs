using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{   
    [SerializeField] PlayerMovement movement;
    public float bounceForce = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            movement.rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
            Debug.Log("jumpPad");
        
        }
    }
}

