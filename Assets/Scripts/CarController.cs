using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    private Rigidbody rb;

    public GameObject policeCar;
    public GameObject linePrefab;
    public LineRenderer line;

    public bool isDragged;
    public bool isBig;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

        screenPoint = Camera.main.WorldToScreenPoint(transform.position);

        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        // create line
        line = Instantiate(linePrefab, transform.position, Quaternion.identity).GetComponent<LineRenderer>();
        line.SetPosition(0, transform.position);
    }

    private void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        // avoid going offscreen
        if (curScreenPoint.x < 0 || curScreenPoint.x > Screen.width || curScreenPoint.y < 0 || curScreenPoint.y > Screen.height)
        {
            return;
        }

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        Vector3 newPosition = new Vector3(Mathf.Round(curPosition.x), transform.position.y, Mathf.Round(curPosition.z));

        // check potential collision
        float diff = transform.Find("Body").transform.position.x - transform.position.x;
        Collider[] colliders = Physics.OverlapBox(new Vector3(newPosition.x + diff, 0, newPosition.z - diff), transform.lossyScale / 4, Quaternion.identity);

        foreach (Collider collider in colliders)
        {
            // deny movement
            if (collider.tag == "Wall")
            {
                return;
            }
        }

        rb.MovePosition(newPosition);

        // adjust position of line
        line.SetPosition(1, newPosition);
    }

    private void OnMouseUp()
    {
        isDragged = false;

        Destroy(line.gameObject);
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
