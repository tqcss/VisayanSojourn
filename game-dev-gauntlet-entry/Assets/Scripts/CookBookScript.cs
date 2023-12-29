using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CookBookScript : MonoBehaviour
{
    public GameObject cookBookUi;
    public GameObject book;
    public GameObject guides;
    private Image c_uiImage;
    private Image c_uiBookImage;

    private bool shown = false;
    private bool bgFading = false;
    private bool turningPage = false;
    private int lastPage = 1;
    private int pageNumber = 1;

    private void Start()
    {
        c_uiImage = cookBookUi.GetComponent<Image>();
        c_uiBookImage = book.GetComponent<Image>();
        GameObject page = book.transform.GetChild(pageNumber).gameObject;
        page.SetActive(true);
        page.GetComponent<CanvasGroup>().alpha = 1;
       
    }

    private bool HasPageRelative(int b)
    {
        try
        {
            book.transform.GetChild(pageNumber + b);
            return true;
        }
        catch (UnityException)
        {
            return false;
        }
    }

    private void Update()
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

        if (turningPage)
        {
            float nextValue = Time.deltaTime * 2;
            CanvasGroup newCanvasGroup = book.transform.GetChild(pageNumber).GetComponent<CanvasGroup>();
            CanvasGroup lastCanvasGroup = book.transform.GetChild(lastPage).GetComponent<CanvasGroup>();

            if (newCanvasGroup.alpha + nextValue >= 1)
            {
                newCanvasGroup.alpha = 1;
                lastCanvasGroup.alpha = 0;
                book.transform.GetChild(lastPage).gameObject.SetActive(false);
                turningPage = false;
            }
            newCanvasGroup.alpha += nextValue;
            lastCanvasGroup.alpha -= nextValue;
        }

        if (!bgFading)
        {
            if (!shown)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0) && !turningPage && HasPageRelative(-2))
            {
                turningPage = true;
                lastPage = pageNumber;
                pageNumber--;
                book.transform.GetChild(pageNumber).gameObject.SetActive(true);
                Debug.Log("page: " + pageNumber);
            }

            if (Input.GetMouseButtonDown(1) && !turningPage && HasPageRelative(1))
            {
                turningPage = true;
                lastPage = pageNumber;
                pageNumber++;
                book.transform.GetChild(pageNumber).gameObject.SetActive(true);
                Debug.Log("page: " + pageNumber);
            }
            return;
        }

        if (shown && c_uiImage.color.a < .95f)
        {
            float nextValue = Time.deltaTime * 1.9f;
            if (c_uiImage.color.a + nextValue >= .95f)
            {
                c_uiImage.color = new Color(0, 0, 0, .95f);
                c_uiBookImage.transform.position = new Vector3(0, 0, 0);
                bgFading = false;
                return;
            }
            c_uiImage.color = new Color(0, 0, 0, (c_uiImage.color.a + nextValue));
            c_uiBookImage.transform.position = new Vector3(0, c_uiBookImage.transform.position.y + Time.deltaTime * 1400, 0);
        }
        else if (!shown && c_uiImage.color.a > 0)
        {
            float nextValue = Time.deltaTime * 1.9f;
            if (c_uiImage.color.a - nextValue <= 0)
            {
                c_uiImage.color = new Color(0, 0, 0, 0);
                c_uiImage.gameObject.SetActive(false);
                c_uiBookImage.transform.position = new Vector3(0, -700f, 0);
                Debug.Log(c_uiBookImage.transform.position.y);
                bgFading = false;
                return;
            }
            c_uiImage.color = new Color(0, 0, 0, (c_uiImage.color.a - nextValue));
            c_uiBookImage.transform.position = new Vector3(0, c_uiBookImage.transform.position.y - Time.deltaTime * 1400, 0);
        }
    }
}
