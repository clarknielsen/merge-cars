using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public Image shine;
    public GameObject confetti;

    private int delay;
    private bool isEnded;
    private bool isStarted;

    // Start is called before the first frame update
    void Start()
    {
        delay = 0;
        isEnded = false;
        isStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStarted && Input.GetMouseButtonDown(0))
        {
            isStarted = true;
            transform.Find("Instructions").gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (!isEnded) return;

        shine.transform.Rotate(new Vector3(0, 0, .8f));

        delay++;

        if (delay >= 10)
        {
            delay = 0;

            // randomly generate new confetti
            Instantiate(confetti, new Vector3(Random.Range(-Screen.width / 2, Screen.width / 2), Screen.height / 2, 0), Quaternion.identity);
        }
    }

    public void ClickToPlay()
    {
        Application.OpenURL("https://play.google.com/store");
    }

    public void EndGame()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        // pause
        yield return new WaitForSeconds(1f);

        isEnded = true;

        transform.Find("HUD").gameObject.SetActive(false);
        transform.Find("Finale").gameObject.SetActive(true);
    }
}
