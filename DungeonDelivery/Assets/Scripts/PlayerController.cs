using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 6f;
    public LayerMask floorLayer;

    private Vector3 mousePos;

    void Awake() 
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        float horz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        
        // move player relative to the camera's forward vector
        var camera = Camera.main;
        var forward = camera.transform.forward;
        var right = camera.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        var desiredMoveDirection = forward * vert + right * horz;
        if (desiredMoveDirection.magnitude >= 0.1f)
        {
            controller.Move(desiredMoveDirection * speed * Time.deltaTime);
        }

        // Face mouse position direction
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f, floorLayer))
        {
            mousePos = hit.point;
            Vector3 direction = mousePos - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            rotation.z = 0f;
            rotation.x = 0f;
            transform.rotation = rotation;
        }
    }

    void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(mousePos, 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, mousePos);
    }
}
