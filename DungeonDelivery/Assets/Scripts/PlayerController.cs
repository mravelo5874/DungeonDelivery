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
        Vector3 movement = new Vector3(horz, 0f, vert).normalized;

        if (movement.magnitude >= 0.1f)
        {
            controller.Move(movement * speed * Time.deltaTime);
        }

        // Direction
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
