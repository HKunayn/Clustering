using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class point : MonoBehaviour
{
    bool isMoving =  false;

    private void OnCollisionEnter2D(Collision2D c)
    {
        if ((transform.position - c.transform.position) == Vector3.zero)
            transform.position += new Vector3 (0.1f, 0, 0);

    }

    void OnMouseOver()
    {
        Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        float f = Mathf.Pow(Mathf.Pow(v.x - transform.position.x, 2) + Mathf.Pow(v.y - transform.position.y, 2), 0.5f);
        if (Input.GetMouseButton(0) && (  f< 0.5f) && !isMoving)
        {
            isMoving = Clustering.inMoving = true;
            //Debug.Log("in " + f);
        }
        else if (Input.GetMouseButtonDown(1)) {
            Clustering.deletePoint(gameObject);
            Destroy(gameObject);
        }else if (!isMoving)
            isMoving = Clustering.inMoving = false;

    }

    private void Update()
    {
        if (isMoving)
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        if (Input.GetMouseButtonUp(0))
            isMoving = Clustering.inMoving = false;


    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "area") {
            Debug.Log("exited ?");
            transform.position = other.transform.position;
        }

    }
}
