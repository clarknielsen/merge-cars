using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(GameObject.Find("Canvas").transform, false);

        // pick random color
        transform.GetChild(Random.Range(0, transform.childCount)).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // cleanup
        if (transform.position.y < 0)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        transform.Rotate(1, 0, 1);
    }
}
