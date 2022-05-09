using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] float cameraMoveSpeed = 25f;

    void Update()
    {
        if(Input.GetKey(KeyCode.W)) {
            transform.position += Vector3.forward * cameraMoveSpeed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.S)) {
            transform.position += Vector3.back * cameraMoveSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.D)) {
            transform.position += Vector3.right * cameraMoveSpeed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.A)) {
            transform.position += Vector3.left * cameraMoveSpeed * Time.deltaTime;
        }
    }
}
