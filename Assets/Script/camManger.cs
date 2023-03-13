using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camManger : MonoBehaviour
{
    Camera cam;
    [SerializeField] float minZoom = 5;
    [SerializeField] float maxZoom = 10;
    [SerializeField] GameObject border;
    private Vector3 lastPos;
    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        lastPos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Input.GetMouseButton(2))
            changeLoc();
        lastPos = cam.ScreenToWorldPoint(Input.mousePosition);
    }
    void Update()
    {
        
        if (Input.mouseScrollDelta.y != 0)
            changeZoom();
        
    }

    private void changeLoc() {
        cam.transform.position += (lastPos- cam.ScreenToWorldPoint(Input.mousePosition));
    
    }

    private void changeZoom()
    {
        cam.orthographicSize -= Input.mouseScrollDelta.y;
        cam.orthographicSize = Mathf.Max(Mathf.Min(cam.orthographicSize, maxZoom), minZoom);
        border.transform.localScale = Vector3.one * cam.orthographicSize / minZoom;
    }
}
