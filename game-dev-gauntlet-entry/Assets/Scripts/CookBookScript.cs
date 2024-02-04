// spaghetti code kekw

using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CookBookScript : MonoBehaviour
{
    public GameObject cookBookUi;
    public GameObject book;

    public GameObject buttons;
    public GameObject buttonLeft;
    public GameObject buttonRight;

    public GameObject ingredientsUi;
    public GameObject dishImage;
    public GameObject dishNameLabel;
    public GameObject dishIngredientsText;

    public GameObject dishList;
    public DishList listScript;
    private DishInfo[] allDishes;

    private Vector3 defaultBookPosition;
    private Vector3 hiddenBookPosition;

    private const float ANIM_SPEED = 0.5f; // time it takes to play ui toggle animation
    private float progression = 0;
    private bool uiVisible = false;
    private bool onAnimation = false;
    private int currentPage;

    private void Start()
    {
        Debug.Log("Script Started");
        currentPage = PlayerPrefs.GetInt("UnlockedDishes", 0);
        defaultBookPosition = book.transform.localPosition;
        hiddenBookPosition = book.transform.localPosition + Vector3.down * 720;

        listScript = dishList.transform.GetComponent<DishList>();
        allDishes = listScript.dishAntique.Concat(listScript.dishAklan).Concat(listScript.dishCapiz).Concat(listScript.dishNegrosOccidental).Concat(listScript.dishGuimaras).Concat(listScript.dishIloilo).ToArray();

        changePage(0);
    }

    private void Update()
    {
        if (!onAnimation) return;

        progression += Time.deltaTime;
        if (progression >= ANIM_SPEED)
        {
            onAnimation = false;
            progression = 0;

            if (uiVisible)
            {
                cookBookUi.transform.GetComponent<RawImage>().color = new Color(0, 0, 0, 0.8f);
                book.transform.localPosition = defaultBookPosition;
                buttons.SetActive(true);
                return;
            }
            cookBookUi.transform.GetComponent<RawImage>().color = Color.clear;
            book.transform.localPosition = hiddenBookPosition;
            cookBookUi.SetActive(false);
            ingredientsUi.SetActive(true);
            return;
        }

        if (uiVisible)
        {
            cookBookUi.transform.GetComponent<RawImage>().color = new Color(0, 0, 0, progression / ANIM_SPEED * 0.8f);
            book.transform.localPosition = Vector3.Lerp(hiddenBookPosition, defaultBookPosition, progression / ANIM_SPEED);
            return;
        }
        cookBookUi.transform.GetComponent<RawImage>().color = new Color(0, 0, 0, 0.8f - progression / ANIM_SPEED * 0.8f);
        book.transform.localPosition = Vector3.Lerp(defaultBookPosition, hiddenBookPosition, progression / ANIM_SPEED);
    }

    public void openCookbook()
    {
        onAnimation = true;
        uiVisible = true;
        cookBookUi.SetActive(true);
        ingredientsUi.SetActive(false);
    }

    public void closeCookBook()
    {
        onAnimation = true;
        uiVisible = false;
        buttons.SetActive(false);
    }

    public void incrementPage()
    {
        currentPage++;
        changePage(currentPage);
    }

    public void decrementPage()
    {
        currentPage--;
        changePage(currentPage);
    }

    public void unlockNextRecipe()
    { 
        PlayerPrefs.SetInt("UnlockedDishes", PlayerPrefs.GetInt("UnlockedDishes") + 1);
        changePage(currentPage);
    }

    public void changePage(int page)
    {   
        if (page == 0)
        {
            buttonLeft.transform.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
        else if (page == 1)
        {
            buttonLeft.transform.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
        if (page == PlayerPrefs.GetInt("UnlockedDishes"))
        {
            buttonRight.transform.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
        else if (page == PlayerPrefs.GetInt("UnlockedDishes") - 1)
        {
            buttonRight.transform.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }

        dishImage.transform.GetComponent<UnityEngine.UI.RawImage>().texture = allDishes[page].sprite.texture;
        dishNameLabel.transform.GetComponent<TextMeshProUGUI>().SetText(allDishes[page].ToString().TrimEnd(" (DishInfo)").Replace('_', ' '));
        dishIngredientsText.transform.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        foreach (IngredientInfo ingredient in allDishes[page].recipe)
        {
            dishIngredientsText.transform.GetComponent<TextMeshProUGUI>().SetText(dishIngredientsText.transform.GetComponent<TextMeshProUGUI>().text + "- " + ingredient.ToString().TrimEnd(" (IngredientInfo)").Replace('_', ' ') + "\n");
        }
    }
}
