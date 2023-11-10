using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookBookScript : MonoBehaviour
{
    public GameObject cookBookUi;

    private Image c_uiImage;

    private bool shown = false;
    private bool bgFading = false;
    private int pageNumber = 0;

    void Start()
    {
        c_uiImage = cookBookUi.GetComponent<Image>();
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

        if (shown && c_uiImage.color.a < .9f)
        {
            float nextValue = Time.deltaTime * 2.7f;
            if (c_uiImage.color.a + nextValue >= .9f)
            {
                c_uiImage.color = new Color(0, 0, 0, .9f);
                bgFading = false;
                return;
            }
            c_uiImage.color = new Color(0, 0, 0, (c_uiImage.color.a + nextValue));
        }
        else if (!shown && c_uiImage.color.a > 0)
        {
            float nextValue = Time.deltaTime * 2.7f;
            if (c_uiImage.color.a - nextValue <= 0)
            {
                c_uiImage.color = new Color(0, 0, 0, 0);
                c_uiImage.gameObject.SetActive(false);
                bgFading = false;
                return;
            }
            c_uiImage.color = new Color(0, 0, 0, (c_uiImage.color.a - nextValue));
        }
    }
}