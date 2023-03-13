using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class point : MonoBehaviour
{
    [SerializeField]bool isMoving = false;
    [SerializeField] Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }
    private void OnCollisionEnter2D(Collision2D c)
    {
        if ((transform.position - c.transform.position) == Vector3.zero)
            transform.position += new Vector3(0.1f, 0, 0);

    }

    private void OnMouseDrag()
    {
        isMoving = Clustering.inMoving = true;
        transform.position = cam.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
    }
    private void OnMouseUp()
    {
        StartCoroutine(stoppedMoving());
        /*isMoving = Clustering.inMoving = false;*/
    }
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) {
            Clustering.deletePoint(gameObject);
            Destroy(gameObject);
        }

    }

    IEnumerator stoppedMoving() {

        yield return new WaitForEndOfFrame();
        isMoving = Clustering.inMoving = false;
    }
}
