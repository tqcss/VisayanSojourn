using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CookBookScript : MonoBehaviour
{
    public GameObject cookBookUi;
    public GameObject book;

    public GameObject buttons;
    public GameObject buttonLeft;
    public GameObject buttonRight;

    public GameObject ingredientsUi;
    public GameObject dishImage;
    public GameObject dishIngredientsText;

    public Text provinceText;
    public Image mapHighlight;
    public Sprite[] highlightedMaps;

    public GameObject dishList;
    public DishList listScript;
    private DishInfo[] allDishes;
    private LevelLoad _levelLoad;
    private PlayerProvince _playerProvince;

    private Vector3 defaultBookPosition;
    private Vector3 hiddenBookPosition;

    private const float ANIM_SPEED = 0.5f; // time it takes to play ui toggle animation
    private float progression = 0;
    private bool uiVisible = false;
    private bool onAnimation = false;
    private int currentPage;

    private void Awake()
    {
        _levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        _playerProvince = GameObject.FindGameObjectWithTag("playerProvince").GetComponent<PlayerProvince>();
    }
    
    private void Start()
    {
        currentPage = PlayerPrefs.GetInt("UnlockedDishes", 0);
        defaultBookPosition = book.transform.localPosition;
        hiddenBookPosition = book.transform.localPosition + Vector3.down * 720;

        listScript = dishList.transform.GetComponent<DishList>();
        allDishes = listScript.dishAntique.Concat(listScript.dishAklan).Concat(listScript.dishCapiz).Concat(listScript.dishNegrosOccidental).Concat(listScript.dishGuimaras).Concat(listScript.dishIloilo).ToArray();
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

    public void OpenCookbook()
    {
        onAnimation = true;
        uiVisible = true;
        cookBookUi.SetActive(true);
        ingredientsUi.SetActive(false);
        GetRecipesUnlocked();
    }

    public void CloseCookBook()
    {
        onAnimation = true;
        uiVisible = false;
        buttons.SetActive(false);
    }

    public void IncrementPage()
    {
        currentPage++;
        ChangePage(currentPage);
    }

    public void DecrementPage()
    {
        currentPage--;
        ChangePage(currentPage);
    }

    public void GetRecipesUnlocked()
    { 
        int recipesDone = 0;
        for (int i = 0; i < _playerProvince.recipeDoneKeyName.Length; i++)
            recipesDone += PlayerPrefs.GetInt(_playerProvince.recipeDoneKeyName[i], 1) - 1;

        int levelId = PlayerPrefs.GetInt("ProvinceCurrent", 0) - 1;
        try
        {
            if (PlayerPrefs.GetInt(_levelLoad.initialPlayedKeyNames[levelId + 1], 0) == 0 &&
                PlayerPrefs.GetInt(_playerProvince.recipeDoneKeyName[levelId], 1) > dishList.transform.GetComponent<DishList>().dishesLength)
                recipesDone -= 1;
        }
        catch (UnityException) {}

        Debug.Log(PlayerPrefs.GetInt(_playerProvince.recipeDoneKeyName[levelId], 1) > dishList.transform.GetComponent<DishList>().dishesLength);
        Debug.Log("Recipe Done: " + recipesDone);
        PlayerPrefs.SetInt("UnlockedDishes", recipesDone);
        ChangePage(currentPage);
    }

    public void ChangePage(int page)
    {   
        Debug.Log("Page: " + page);
        if (page == 0)
            buttonLeft.transform.GetComponent<UnityEngine.UI.Button>().interactable = false;
        else if (page == 1)
            buttonLeft.transform.GetComponent<UnityEngine.UI.Button>().interactable = true;
        
        if (page == PlayerPrefs.GetInt("UnlockedDishes"))
            buttonRight.transform.GetComponent<UnityEngine.UI.Button>().interactable = false;
        else if (page == PlayerPrefs.GetInt("UnlockedDishes") - 1)
            buttonRight.transform.GetComponent<UnityEngine.UI.Button>().interactable = true;

        dishImage.transform.GetComponent<RawImage>().texture = allDishes[page].framedSprite.texture;
        dishIngredientsText.transform.GetComponent<TextMeshProUGUI>().text = "";
        foreach (IngredientInfo ingredient in allDishes[page].recipe)
            dishIngredientsText.transform.GetComponent<TextMeshProUGUI>().SetText(dishIngredientsText.transform.GetComponent<TextMeshProUGUI>().text + "- " + ingredient.ToString().TrimEnd(" (IngredientInfo)").Replace('_', ' ') + "\n");
        
        for (int i = 0; i < allDishes[page].isDishAtProvince.Length; i++)
        {
            if (allDishes[page].isDishAtProvince[i])
            {
                mapHighlight.sprite = highlightedMaps[i];
                provinceText.text = highlightedMaps[i].name.Replace("highlighted_", "").Replace("_", "").ToUpper();
                return;
            }
        }
    }
}
