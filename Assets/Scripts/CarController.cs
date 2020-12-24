using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    private Rigidbody rb;
    private Transform body;

    public GameObject policeCar;
    public bool isDragged;
    public bool isBig;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        body = transform.Find("Body");

        isDragged = false;
        isBig = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        isDragged = true;

        // record initial mouse position
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    private void OnMouseDrag()
    {
        // get newest position of mouse
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        // update rigidbody but keep within confines of grid
        Vector3 newPosition = new Vector3(
            Mathf.Clamp(Mathf.Round(curPosition.x), -10, 10 - transform.localScale.x), 
            transform.position.y,
            Mathf.Clamp(Mathf.Round(curPosition.z), -9 + transform.localScale.z, 5)
        );

        // rotate in direction
        if (newPosition.z < transform.position.z)
        {
            body.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (newPosition.z > transform.position.z)
        {
            body.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (newPosition.x < transform.position.x)
        {
            body.transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else if (newPosition.x > transform.position.x)
        {
            body.transform.eulerAngles = new Vector3(0, -90, 0);
        }

        rb.MovePosition(newPosition);
    }

    private void OnMouseUp()
    {
        isDragged = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // only run on car being dragged
        if (!isDragged) return;

        // two of the same car touched
        if (transform.tag == collision.gameObject.tag)
        {
            // remove one
            Destroy(collision.gameObject);

            // and embiggen the other
            isBig = true;
            transform.localScale = new Vector3(2, 2, 2);
        }
        // opposite colors touched
        else if (transform.tag.Contains("Car") && collision.gameObject.tag.Contains("Car"))
        {
            if (isBig && collision.gameObject.GetComponent<CarController>().isBig)
            {
                // show police car
                Instantiate(policeCar, collision.gameObject.transform.position, Quaternion.identity);

                // remove others
                Destroy(collision.gameObject);
                Destroy(gameObject);

                // show end-card
                GameObject.Find("Canvas").GetComponent<CanvasController>().EndGame();
            }
        }
    }
}
