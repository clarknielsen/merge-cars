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
    private Rect rect;

    // Start is called before the first frame update
    void Start()
    {
        delay = 0;
        isEnded = false;
        isStarted = false;

        rect = GetComponent<RectTransform>().rect;
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

            // randomly generate new confetti within canvas frame
            Instantiate(confetti, new Vector3(Random.Range(-rect.width / 2, rect.width / 2), rect.height / 2, 0), Quaternion.identity);
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
