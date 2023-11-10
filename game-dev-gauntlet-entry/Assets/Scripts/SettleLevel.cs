using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SettleLevel : MonoBehaviour
{
    
    public GameObject RecipeScroll;
    public int scrollTimeSec;
    public GameObject KitchenUI;
    public GameObject StartButton;
    public TextMeshProUGUI FoodNameText;
    public GameObject FoodNameTextObj;
    public TextMeshProUGUI RecipeText;
    public GameObject RecipeTextObj;
    public TextMeshProUGUI MessageText;
    public GameObject RoundFinishUI;
    public GameObject BackButton;
    public GameObject PrevButton;
    public GameObject NextButton;
    public DishInfo firstDish;
    public DishInfo secondDish;
    public DishInfo thirdDish;
    public DishInfo fourthDish;
    public DishInfo fifthDish;
    public DishInfo sixthDish;
    public GameObject firstDishObj;
    public GameObject secondDishObj;
    public GameObject thirdDishObj;
    public GameObject fourthDishObj;
    public GameObject fifthDishObj;
    public GameObject sixthDishObj;

    private OrderManager orderManager;

    public int maximumRound;
    private int currentRound = 1;



    private void Awake()
    {
        orderManager = GameObject.FindGameObjectWithTag("orderManager").GetComponent<OrderManager>();
        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        RecipeScroll.SetActive(true);
        FoodNameTextObj.SetActive(false);
        RecipeTextObj.SetActive(false);
        StartButton.SetActive(false);
        KitchenUI.SetActive(false);
        RoundFinishUI.SetActive(false);
        if (currentRound == 1)
        {
            yield return new WaitForSeconds(scrollTimeSec);
        }
        DisplayRecipe();
        StartButton.SetActive(true);
    }

    private void DisplayRecipe()
    {
        FoodNameTextObj.SetActive(true);
        RecipeTextObj.SetActive(true);
        if (PlayerPrefs.GetInt("ProvinceCurrent", 1) == 1)
        {
            if (currentRound == 1)
            {
                FoodNameText.text = "BANDI";
                RecipeText.text = "INGREDIENTS: \n\n   Roasted Peanuts \n   Brown Sugar \n   Sesame Seeds";
            }
            else if (currentRound == 2)
            {
                FoodNameText.text = "GINAT-AN NGA TAMBO KAG PATUYAW";
                RecipeText.text = "INGREDIENTS: \n\n   Bamboo Shoots (Tambo) \n   Shrimp \n   Coconut Milk \n   Garlic \n   Onion \n   Ginger \n   Green Chillies \n   Fish Sauce \n   Ground Black Pepper \n   Cooking Oil";
            }
        }
        else if (PlayerPrefs.GetInt("ProvinceCurrent", 1) == 2)
        {
            if (currentRound == 1)
            {
                FoodNameText.text = "SHRIMP KINILAW";
                RecipeText.text = "INGREDIENTS: \n\n   Shrimp \n   White Vinegar \n   Calamansi Juice \n   Ginger \n   Onion \n   Red Bell Pepper \n   Green Mango \n   Jalapeno Chili Pepper \n   Red Chili Flakes \n   Green Onion \n   Salt and Ground Black Pepper";
            }
            else if (currentRound == 2)
            {
                FoodNameText.text = "CASSAVA PICHI-PICHI";
                RecipeText.text = "INGREDIENTS: \n\n   Cassava \n   Sugar \n   Water \n   Coconut (Grated) \n   Buko-pandan Essence";
            }
        }
        else if (PlayerPrefs.GetInt("ProvinceCurrent", 1) == 3)
        {
            if (currentRound == 1)
            {
                FoodNameText.text = "ADOBO NGA BAKA";
                RecipeText.text = "INGREDIENTS: \n\n   Beef \n   Soy Sauce \n   Vinegar \n   Garlic \n   Peppercorns \n   Bay Leaves \n   Brown Sugar \n   Water \n   Salt";
            }
            else if (currentRound == 2)
            {
                FoodNameText.text = "RELLENONG PUSIT";
                RecipeText.text = "INGREDIENTS: \n\n   Ground Pork \n   Squid \n   Carrot \n   Celery \n   Garlic \n   Lemon \n   Onion \n   Tomato \n   Egg \n   Soy Sauce \n   All-purpose Flour \n   Salt and Ground Black Pepper \n   Cooking Oil";
            }
            else if (currentRound == 3)
            {
                FoodNameText.text = "BAKED SCALLOPS";
                RecipeText.text = "INGREDIENTS: \n\n   Scallops \n   Butter \n   Mayonnaise \n   Salt and Ground Black Pepper \n   Cheddar Cheese \n   Parsley";
            }
        }
        else if (PlayerPrefs.GetInt("ProvinceCurrent", 1) == 4)
        {
            if (currentRound == 1)
            {
                FoodNameText.text = "UBE PIAYA";
                RecipeText.text = "INGREDIENTS: \n\n   All-purpose Flour \n   Salt \n   Lard \n   Vinegar \n   Water \n   Sesame Seeds \n   Ube Halaya \n   Cornstarch";
            }
            else if (currentRound == 2)
            {
                FoodNameText.text = "GUAPPLE PIE";
                RecipeText.text = "INGREDIENTS: \n\n   All-purpose Flour \n   Margarine \n   Lard \n   Water \n   White Sugar \n   Ground Cinnamon \n   Lime Rind \n   Guapples \n   Cornstarch \n   Butter";
            }
            else if (currentRound == 3)
            {
                FoodNameText.text = "PANARA";
                RecipeText.text = "INGREDIENTS: \n\n   Cooking Oil \n   Garlic \n   Onion \n   Shrimp \n   Pork Broth Cube \n   Bottle Gourd \n   Lumpia Wrapper \n   Cooking Oil \n";
            }
            else if (currentRound == 4)
            {
                FoodNameText.text = "NAPOLEONES";
                RecipeText.text = "INGREDIENTS: \n\n   Puff Pastry \n   Egg Yolks \n   Sugar \n   Cornstarch \n   Fresh Milk \n   White Sugar \n";
            }
        }
        else if (PlayerPrefs.GetInt("ProvinceCurrent", 1) == 5)
        {
            if (currentRound == 1)
            {
                FoodNameText.text = "PINASUGBO";
                RecipeText.text = "INGREDIENTS: \n\n   Raw Bananas (Saba) \n   Cooking Oil \n   Vanilla Flavoring \n   Brown Sugar \n   Water \n   Sesame Seeds";
            }
            else if (currentRound == 2)
            {
                FoodNameText.text = "MANGO PIZZA";
                RecipeText.text = "INGREDIENTS: \n\n   Pizza Sauce \n   Cheese \n   Green Bell Pepper \n   Cashew Nuts \n   Semi-Ripe Mango \n   Pizza Dough";
            }
            else if (currentRound == 3)
            {
                FoodNameText.text = "MANGO BIBINGKA";
                RecipeText.text = "INGREDIENTS: \n\n   Evaporated Milk \n   Coconut Milk \n   Rice Flour \n   All-purpose Flour \n   Baking Powder \n   Butter \n   Salt \n   White Sugar \n   Egg \n   Mango \n   Cheese \n   Banana Leaves";
            }
        }
        else if (PlayerPrefs.GetInt("ProvinceCurrent", 1) == 6)
        {
            if (currentRound == 1)
            {
                FoodNameText.text = "KAYDOS BABOY LANGKA";
                RecipeText.text = "INGREDIENTS: \n\n   Pork Hocks \n   Jackfruit \n   Pigeon Pea Kadyos \n   Sweet Potato Leaves \n   Sinigang Mix \n   Lemongrass \n   Pork Cube \n   Water \n   Salt and Pepper";
            }
            else if (currentRound == 2)
            {
                FoodNameText.text = "BATCHOY";
                RecipeText.text = "INGREDIENTS: \n\n   Noodles \n   Pork \n   Pig's Intestines \n   Pig Liver \n   Salt and Ground Black Pepper \n   Sugar \n   Shrimp Paste (Bagoong) \n   Onion Powder \n   Pork Cracklings (Chicharon) \n   Garlic \n   Water";
            }
            else if (currentRound == 3)
            {
                FoodNameText.text = "PANCIT MOLO";
                RecipeText.text = "INGREDIENTS: \n\n   Ground Pork \n   Shrimp \n   Wonton \n   Onion \n   Garlic Powder \n   Sesame Oil \n   Egg \n   Salt and Ground Black Pepper \n   Chicken \n   Chicken Broth \n   Water \n   Scallions \n   Fish Sauce";
            }
            else if (currentRound == 4)
            {
                FoodNameText.text = "APAN-APAN";
                RecipeText.text = "INGREDIENTS: \n\n   Kangkong \n   Garlic \n   Onion \n   Shrimp Paste \n   Vinegar \n   Sugar \n   Cooking Oil \n   Green Chillies";
            }
            else if (currentRound == 5)
            {
                FoodNameText.text = "ADOBO NGA TAKWAY";
                RecipeText.text = "INGREDIENTS: \n\n   Edible Fern (Takway) \n   Vinegar \n   Soy Sauce \n   Water \n   Garlic \n   Onion \n   Bay Leaves \n   Peppercorns \n   Cooking Oil \n   Salt and Pepper";
            }
            else if (currentRound == 6)
            {
                FoodNameText.text = "PINAMALHAN NA ISDA";
                RecipeText.text = "INGREDIENTS: \n\n   Lupoy (Fish) \n   Garlic \n   Vinegar \n   Cooking Oil \n   Salt and Groudn Black Pepper";
            }
        }
    }

    public void StartRound()
    {
        RecipeScroll.SetActive(false);
        KitchenUI.SetActive(true);
        if (currentRound == 1)
        {
            orderManager.changeOrderPrompt(firstDish);
        }
        else if (currentRound == 2)
        {
            orderManager.changeOrderPrompt(secondDish);
        }
        else if (currentRound == 3)
        {
            orderManager.changeOrderPrompt(thirdDish);
        }
        else if (currentRound == 4)
        {
            orderManager.changeOrderPrompt(fourthDish);
        }
        else if (currentRound == 5)
        {
            orderManager.changeOrderPrompt(fifthDish);
        }
        else if (currentRound == 6)
        {
            orderManager.changeOrderPrompt(sixthDish);
        }
    }

    public void FinishRound()
    {
        if (PlayerPrefs.GetInt("RoundCorrect", 0) == 1)
        {
            RoundFinishUI.SetActive(true);
            if ((PlayerPrefs.GetInt("GlobalLives", 3) > 0))
            {
                MessageText.text = "ROUND CORRECT";
                NextButton.SetActive(true);
                PrevButton.SetActive(false);
                BackButton.SetActive(true);
                DisplayDish(1);
            }
            PlayerPrefs.SetInt("RoundCorrect", 0);
        }
        else if (PlayerPrefs.GetInt("RoundCorrect", 0) == 0)
        {
            RoundFinishUI.SetActive(true);
            if ((PlayerPrefs.GetInt("GlobalLives", 3) > 0))
            {
                MessageText.text = "ROUND INCORRECT";
                NextButton.SetActive(false);
                PrevButton.SetActive(true);
                BackButton.SetActive(true);
                DisplayDish(0);
            }
            else
            {
                MessageText.text = "NO MORE LIVES";
                NextButton.SetActive(false);
                PrevButton.SetActive(false);
                BackButton.SetActive(true);
                DisplayDish(0);
            }
        }
    }

    public void DisplayDish(int display)
    {
        firstDishObj.SetActive(false);
        secondDishObj.SetActive(false);
        thirdDishObj.SetActive(false);
        fourthDishObj.SetActive(false);
        fifthDishObj.SetActive(false);
        sixthDishObj.SetActive(false);
        if (display == 1)
        {
            if (currentRound == 1)
            {
                firstDishObj.SetActive(true);
            }
            else if (currentRound == 2)
            {
                secondDishObj.SetActive(true);
            }
            else if (currentRound == 3)
            {
                thirdDishObj.SetActive(true);
            }
            else if (currentRound == 4)
            {
                fourthDishObj.SetActive(true);
            }
            else if (currentRound == 5)
            {
                fifthDishObj.SetActive(true);
            }
            else if (currentRound == 6)
            {
                sixthDishObj.SetActive(true);
            }
        }
    }

    public void OntoNextRound()
    {
        if (currentRound < maximumRound)
        {
            currentRound++;
            StartCoroutine(PlayAnimation());
        }
        else
        {
            PlayerPrefs.SetInt("ProceedNextProvince", 1);
            Debug.Log("Go to Next Province");
        }
    }

    public void OntoPreviousRound()
    {
        StartCoroutine(PlayAnimation());
    }
}