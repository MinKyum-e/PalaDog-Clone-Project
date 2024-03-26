using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;
    [SerializeField]
    private bool lerpCamera = false;
    [SerializeField]
    private float cameraMoveSpeed = 3.0f;
    

    private Vector3 cameraPosition;
    

    public Vector2 center;
    public Vector2 size;

    float height;
    float width;
 

    private void Start()
    {
        target = Player.Instance.transform;
        cameraPosition = new Vector3(0,3.3f,-10);
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }
    private void FixedUpdate()
    {
        moveCamera();
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }

    void moveCamera()
    {
        if (!lerpCamera)
        {
            transform.position = target.position + cameraPosition;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, target.position + cameraPosition, Time.deltaTime * cameraMoveSpeed);
        
        }
        float lx = size.x * 0.5f - width;
        float clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);
        transform.position = new Vector3(clampX,transform.position.y, -10f);
    }

}
