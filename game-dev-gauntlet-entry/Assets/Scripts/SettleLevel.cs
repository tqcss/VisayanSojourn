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
    public TextMeshProUGUI MessageText;
    public GameObject RecipeTextObj;
    public GameObject RoundFinishUI;
    public GameObject BackButton;
    public GameObject NextButton;

    public int maximumRound;
    private int currentRound = 1;



    private void Awake()
    {
        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        RecipeScroll.SetActive(true);
        FoodNameTextObj.SetActive(false);
        RecipeTextObj.SetActive(false);
        StartButton.SetActive(false);
        KitchenUI.SetActive(false);
        yield return new WaitForSeconds(scrollTimeSec);
        DisplayRecipe();
        StartButton.SetActive(true);
    }

    private void DisplayRecipe()
    {
        FoodNameTextObj.SetActive(true);
        RecipeTextObj.SetActive(true);
        if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 1)
        {
            if (currentRound == 1)
            {
                FoodNameText.text = "BANDI";
                RecipeText.text = "INGREDIENTS: \n   Roasted Peanuts \n   Muscovado Sugar \n   Sesame Seeds";
            }
            else if (currentRound == 2)
            {
                FoodNameText.text = "GINAT-AN NGA TAMBO KAG PATUYAW";
                RecipeText.text = "INGREDIENTS: \n   Bamboo Shoots (Tambo) \n   Shrimp \n   Coconut Milk \n   Garlic \n   Onion \n   Ginger \n   Green Chillies \n   Fish Sauce \n   Ground Black Pepper \n   Cooking Oil";
            }
        }
        else if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 2)
        {
            if (currentRound == 1)
            {
                FoodNameText.text = "SHRIMP KINILAW";
                RecipeText.text = "INGREDIENTS: \n   Shrimp \n   White Vinegar \n   Kalamansi Juice \n   Ginger \n   Onion \n   Red Bell Pepper \n   Jalapeno Chili Pepper \n   Red Chili Flakes \n   Green Onion \n   Salt and Ground Black Pepper";
            }
            else if (currentRound == 2)
            {
                FoodNameText.text = "CASSAVA PICHI-PICHI";
                RecipeText.text = "INGREDIENTS: \n   Cassava \n   Sugar \n   Water \n   Coconut \n   Lye Water \n   Buko-pandan Essence";
            }
        }
        else if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 3)
        {
            if (currentRound == 1)
            {
                FoodNameText.text = "ADOBO NGA BAKA";
                RecipeText.text = "INGREDIENTS: \n   Beef \n   Soy Sauce \n   Vinegar \n   Garlic \n   Peppercorns \n   Bay Leaves \n   Brown Sugar \n   Water \n   Salt";
            }
            else if (currentRound == 2)
            {
                FoodNameText.text = "RELLENONG PUSIT";
                RecipeText.text = "INGREDIENTS: \n   Ground Pork \n   Squid \n   Carrot \n   Celery \n   Garlic \n   Lemon \n   Onion \n   Tomato \n   Egg \n   Soy Sauce \n   All-purpose Flour \n   Salt and Ground Black Pepper \n   Cooking Oil";
            }
            else if (currentRound == 3)
            {
                FoodNameText.text = "BAKED SCALLOPS";
                RecipeText.text = "INGREDIENTS: \n   Scallops \n   Unsalted Butter \n   Mayonnaise \n   Salt and Ground Black Pepper \n   Cheddar Cheese \n   Parsley";
            }
        }
        else if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 4)
        {
            if (currentRound == 1)
            {
                FoodNameText.text = "UBE PIAYA";
                RecipeText.text = "INGREDIENTS: \n   All-purpose Flour \n   Salt \n   Lard \n   Vinegar \n   Water \n   Sesame Seeds \n   Ube Halaya \n   Cornstarch";
            }
            else if (currentRound == 2)
            {
                FoodNameText.text = "GUAPPLE PIE";
                RecipeText.text = "INGREDIENTS: \n   All-purpose Flour \n   Margarine \n   Lard \n   Water \n   Sugar \n   Ground Cinnamon \n   Chopped Lime Rind \n   Guapples \n   Cornstarch \n   Sugar \n   Unsalted Butter";
            }
            else if (currentRound == 3)
            {
                FoodNameText.text = "PANARA";
                RecipeText.text = "INGREDIENTS: \n   Cooking Oil \n   Garlic \n   Onion \n   Shrimp \n   Pork Broth Cube \n   Bottle Gourd \n   Lumpia Wrapper \n   Cooking Oil \n";
            }
            else if (currentRound == 4)
            {
                FoodNameText.text = "NAPOLEONES";
                RecipeText.text = "INGREDIENTS: \n   Puff Pastry \n   Egg Yolks \n   Sugar \n   Cornstarch \n   Fresh Milk \n   Powdered Sugar \n";
            }
        }
        else if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 5)
        {
            if (currentRound == 1)
            {
                FoodNameText.text = "PINASUGBO";
                RecipeText.text = "INGREDIENTS: \n   Raw Bananas (Saba) \n   Cooking Oil \n   Vanilla Flavoring \n   Brown Sugar \n   Water \n   Sesame Seeds";
            }
            else if (currentRound == 2)
            {
                FoodNameText.text = "MANGO PIZZA";
                RecipeText.text = "INGREDIENTS: \n   Pizza Sauce \n   Cheddar Cheese \n   Mozzarella Cheese \n   Green Bell Pepper \n   Cashew Nuts \n   Semi-Ripe Mango \n   Pizza Dough";
            }
            else if (currentRound == 3)
            {
                FoodNameText.text = "MANGO BIBINGKA";
                RecipeText.text = "INGREDIENTS: \n   Evaporated Milk \n   Coconut Milk \n   Rice Flour \n   All-purpose Flour \n   Baking Powder \n   Butter \n   Salt \n   White Sugar \n   Egg \n   Mango \n   Cheese \n   Banana Leaves";
            }
        }
        else if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 6)
        {
            if (currentRound == 1)
            {
                FoodNameText.text = "KAYDOS BABOY LANGKA";
                RecipeText.text = "INGREDIENTS: \n   Pork Hocks \n   Jackfruit \n   Pigeon Pea Kadyos \n   Sweet Potato Leaves \n   Sinigang Mix \n   Lemongrass \n   Pork Cube \n   Water \n   Salt and Pepper";
            }
            else if (currentRound == 2)
            {
                FoodNameText.text = "BATCHOY";
                RecipeText.text = "INGREDIENTS: \n   Noodles \n   Pork \n   Pig's Intestines \n   Pig Liver \n   Salt and Ground Black Pepper \n   Sugar \n   Shrimp Paste (Bagoong) \n   Onion Powder \n   Pork Cracklings (Chicharon) \n   Garlic \n   Water";
            }
            else if (currentRound == 3)
            {
                FoodNameText.text = "PANCIT MOLO";
                RecipeText.text = "INGREDIENTS: \n   Ground Pork \n   Shrimp \n   Wonton \n   Onion \n   Garlic Powder \n   Sesame Oil \n   Egg \n   Salt and Ground Black Pepper \n   Chicken \n   Chicken Broth \n   Water \n   Scallions \n   Fish Sauce";
            }
            else if (currentRound == 4)
            {
                FoodNameText.text = "APAN-APAN";
                RecipeText.text = "INGREDIENTS: \n   Kangkong \n   Garlic \n   Onion \n   Shrimp Paste \n   Vinegar \n   Sugar \n   Cooking Oil \n   Green Chillies";
            }
            else if (currentRound == 5)
            {
                FoodNameText.text = "ADOBO NGA TAKWAY";
                RecipeText.text = "INGREDIENTS: \n   Edible Fern (Takway) \n   Vinegar \n   Soy Sauce \n   Water \n   Garlic \n   Onion \n   Bay Leaves \n   Peppercorns \n   Cooking Oil \n   Salt and Pepper";
            }
            else if (currentRound == 6)
            {
                FoodNameText.text = "PINAMALHAN NA ISDA";
                RecipeText.text = "INGREDIENTS: \n   Lupoy (Fish) \n   Garlic \n   Vinegar \n   Cooking Oil \n   Salt and Groudn Black Pepper";
            }
        }
    }

    public void StartRound()
    {
        RecipeScroll.SetActive(false);
        KitchenUI.SetActive(true);
    }

    public void FinishRound()
    {
        RoundFinishUI.SetActive(true);
        if ((PlayerPrefs.GetInt("GlobalLives", 3) > 0))
        {
            MessageText.text = "ROUND FINISHED";
            NextButton.SetActive(true);
            BackButton.SetActive(false);
        }
        else
        {
            MessageText.text = "NO MORE LIVES";
            NextButton.SetActive(false);
            BackButton.SetActive(true);
        }
    }

}
