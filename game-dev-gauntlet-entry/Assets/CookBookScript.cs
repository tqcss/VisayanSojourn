using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CookBookScript : MonoBehaviour
{
    public GameObject cookBookUi;

    private RawImage c_uiImage;
    private VideoPlayer c_uiVideo;

    private bool shown = false;
    private bool bgFading = false;
    private int pageNumber = 0;

    void Start()
    {
        c_uiImage = cookBookUi.GetComponent<RawImage>();
        c_uiVideo = cookBookUi.GetComponent<VideoPlayer>();
        c_uiVideo.Prepare();
    }

    void Update()
    {
        if (!shown && Input.GetKeyDown(KeyCode.Space))
        {
            cookBookUi.SetActive(true);
            shown = true;
            bgFading = true;
        }
        else if (shown && Input.GetKeyUp(KeyCode.Space))
        {
            shown = false;
            bgFading = true;
        }

        if (!bgFading && shown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pageNumber--;
                Debug.Log("page: " + pageNumber);
            }
            if (Input.GetMouseButtonDown(1))
            {
                pageNumber++;
                Debug.Log("page: " + pageNumber);
            }
            return;
        }

        if (shown && c_uiImage.color.a < 1)
        {
            float nextValue = Time.deltaTime * 3;
            if (c_uiImage.color.a + nextValue >= 1)
            {
                c_uiImage.color = new Color(1, 1, 1, 1);
                bgFading = false;
                c_uiVideo.Play();
                return;
            }
            c_uiImage.color = new Color(1, 1, 1, (c_uiImage.color.a + nextValue));
        }
        else if (!shown && c_uiImage.color.a > 0)
        {
            float nextValue = Time.deltaTime * 3;
            if (c_uiImage.color.a - nextValue <= 0)
            {
                c_uiImage.color = new Color(1, 1, 1, 0);
                c_uiImage.gameObject.SetActive(false);
                bgFading = false;
                c_uiVideo.Stop();
                return;
            }
            c_uiImage.color = new Color(0, 0, 0, (c_uiImage.color.a - nextValue));
        }
    }
}
