using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettleRestaurant : MonoBehaviour
{
    public int waitTimeArrival;
    public GameObject restaurantUI;
    public GameObject modeEndUI;
    public GameObject orderText;
    public GameObject orderDisplay;
    public GameObject checkButton;
    public GameObject plate;
    public GameObject dishColoredImage;
    public Text coinsText;
    public Text streakBonusText;
    public Text provinceBonusText;
    public Image customerBody;
    public Sprite[] manBody;
    public Sprite[] womanBody;
    public Image customerFace;
    public Sprite[] manFace;
    public Sprite[] womanFace;
    public int genderId;
    public int customerDistanceApart;
    public Text chatText;
    public GameObject chatBubble;
    public string[] orderMessage;
    public string[] correctMessage;
    public string[] incorrectMessage;

    public float costRatio;
    public float sellTotal;
    public int streakCount;
    public float streakBonus;
    public float bonusIncrement;
    public float[] provinceBonus;
    private GameObject coinsParticle;

    private AudioManager _audioManager;
    private DishList _dishList;
    private IngredientManager _ingredientManager;
    private LevelLoad _levelLoad;
    private OrderManager _orderManager;
    private PlayerCoins _playerCoins;
    private RecipeManager _recipeManager;

    private void Awake()
    {
        // Reference the scripts from game objects
        _audioManager = GameObject.FindGameObjectWithTag("audioManager").GetComponent<AudioManager>();
        _dishList = GameObject.FindGameObjectWithTag("dishList").GetComponent<DishList>();
        _levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        _orderManager = GameObject.FindGameObjectWithTag("orderManager").GetComponent<OrderManager>();
        _playerCoins = GameObject.FindGameObjectWithTag("playerCoins").GetComponent<PlayerCoins>();
        _recipeManager = GameObject.FindGameObjectWithTag("recipeManager").GetComponent<RecipeManager>();
        
        // Reference the prefab particle
        coinsParticle = Resources.Load("Prefabs/coinsParticle", typeof(GameObject)) as GameObject;

        // Set initial values to the variables and set the game objects
        _audioManager.PlayBackgroundMusic(_audioManager.restaurantMusic, true);
        coinsText.text = string.Format("{0:0.00}", _playerCoins.globalCoins);

        restaurantUI.SetActive(true);
        modeEndUI.SetActive(false);
        chatBubble.SetActive(false);
        dishColoredImage.SetActive(false);
        plate.SetActive(true);

        StartCoroutine(StartOrder(true));
        DisplayBonus();
    }

    public IEnumerator StartOrder(bool ableToStart)
    {
        // Set the game objects
        if (ableToStart)
        {
            StartCoroutine(CustomerArrival());

            // Set the order of a customer
            yield return new WaitForSeconds(waitTimeArrival);
            orderText.SetActive(true);
            orderDisplay.SetActive(true);
            checkButton.SetActive(true);
            _dishList.RandomPromptOrder();
            _orderManager.StartTimer();
            CustomerOrder();
        }
    }

    public IEnumerator CustomerArrival()
    {
        RandomGenderId();
        switch (genderId)
        {
            // FaceId: 0 - Happy
            // Execute if the customer is a man
            case 1:
                customerBody.sprite = manBody[Random.Range(0, manBody.Length)];
                customerFace.sprite = manFace[0];
                break;
            // Execute if the customer is a woman
            case 2:
                customerBody.sprite = womanBody[Random.Range(0, womanBody.Length)];
                customerFace.sprite = womanFace[0];
                break;
        }
        
        // Get the first position of the customer
        float firstCustomerPosX = customerBody.GetComponent<RectTransform>().localPosition.x;
        float firstCustomerPosY = customerBody.GetComponent<RectTransform>().localPosition.y;
        // Reset the position of the customer
        customerBody.GetComponent<RectTransform>().localPosition = new Vector2(firstCustomerPosX - (2 * customerDistanceApart), firstCustomerPosY);

        // Get the current position of the customer
        float currentCustomerPosX = customerBody.GetComponent<RectTransform>().localPosition.x;
        float timeArrival = 0;
        while (timeArrival < 1f)
        {
            // Move the character from the origin position to the center position
            timeArrival = Mathf.MoveTowards(timeArrival, Mathf.Clamp01(1f), Time.deltaTime / 4.0f);
            float moveCustomerPosX = currentCustomerPosX + (timeArrival * customerDistanceApart);
            customerBody.GetComponent<RectTransform>().localPosition = new Vector2((int)moveCustomerPosX, firstCustomerPosY);
            yield return null;
        }
    }

    public IEnumerator CustomerDeparture(bool ableToStart)
    {
        // Get the first position of the customer
        float firstCustomerPosX = customerBody.GetComponent<RectTransform>().localPosition.x;
        float firstCustomerPosY = customerBody.GetComponent<RectTransform>().localPosition.y;
        
        float timeArrival = 0;
        while (timeArrival < 1f)
        {
            // Move the character from the center position to the final position
            timeArrival = Mathf.MoveTowards(timeArrival, Mathf.Clamp01(1f), Time.deltaTime / 3.14f);
            float moveCustomerPosX = firstCustomerPosX + (timeArrival * customerDistanceApart);
            customerBody.GetComponent<RectTransform>().localPosition = new Vector2((int)moveCustomerPosX, firstCustomerPosY);
            yield return null;
        }

        StartCoroutine(StartOrder(ableToStart));
    }

    public void RandomGenderId()
    {
        /*
            Gender Id
            Man: 1
            Woman: 2
        */
        // Generate a random id for the gender
        genderId = Random.Range(1, 3);
    }

    public void EndOrder(bool orderSuccess, bool ableToStart)
    {
        // End the order of a customer
        // Compute the increased bonus from the serve streak 
        streakCount = (orderSuccess) ? streakCount + 1 : 0;
        streakBonus = (orderSuccess) ? 1 + (streakCount * bonusIncrement) : 1;
        
        if (orderSuccess)
        {
            int provinceCurrent = PlayerPrefs.GetInt("ProvinceCurrent", 0);
            // Compute the sell total with serve streak and province current bonuses
            sellTotal = _orderManager.SellCompute(streakBonus, provinceBonus[provinceCurrent - 1]);
            StartCoroutine(RewardCoins());
        }

        orderText.SetActive(false);
        orderDisplay.SetActive(false);
        checkButton.SetActive(false);
        DisplayBonus();
        StartCoroutine(CustomerResponse(orderSuccess, ableToStart));
    }

    public void DisplayBonus()
    {
        // Update display the serve streak and current province bonuses
        int provinceCurrent = PlayerPrefs.GetInt("ProvinceCurrent", 0);
        streakBonusText.text = string.Format("Streak Bonus: {0:0.00}x", streakBonus);
        provinceBonusText.text = string.Format("Province Bonus: {0:0.00}x", provinceBonus[provinceCurrent - 1]);
    }

    public void PurchaseIngredient(IngredientInfo ingredientInfo)
    {
        // Compute the buy point of an ingredient with the cost ratio
        float buyPoint = Mathf.Round(ingredientInfo.sellPoint * costRatio * 100.0f) * 0.01f;
        
        // Decrease player global coins by the buy point of an ingredient if it has enough coins to purchase
        if (PlayerPrefs.GetFloat("GlobalCoins", _playerCoins.initialCoins) >= buyPoint)
        {
            _playerCoins.DecreaseCoins(buyPoint);
            coinsText.text = string.Format("{0:0.00}", PlayerPrefs.GetFloat("GlobalCoins", _playerCoins.initialCoins));
        }
        // Display "not enough coins" if the player has not enough coins to purchase an ingredient
        else
        {
            _recipeManager.FailPlayer();
            EndOrder(false, false);
            _audioManager.StopMusic();
            modeEndUI.SetActive(true);
        }
    }

    public void ReturnIngredient(IngredientInfo ingredientInfo)
    {
        // Give player global coins based on the selected ingredient if that ingredient returns to the ingredient tab
        return;
    }

    public void CustomerOrder()
    {
        // Display the order of a customer
        chatBubble.SetActive(true);
        chatText.text = orderMessage[Random.Range(0, orderMessage.Length)] + _orderManager.currentOrderPrompt.name + "?";
    }

    public IEnumerator CustomerResponse(bool orderSuccess, bool ableToStart)
    {
        // Display the emotion of a customer
        switch (genderId)
        {
            // FaceId: 0 - Happy
            // FaceId: 2 - Mad
            // Execute if the customer is a man
            case 1:
                customerFace.sprite = (orderSuccess) ? manFace[0] : manFace[2];
                break;
            // Execute if the customer is a woman
            case 2:
                customerFace.sprite = (orderSuccess) ? womanFace[0] : womanFace[2];
                break;
        }
        
        // Display the response of a customer
        chatText.text = (orderSuccess)
                        ? correctMessage[Random.Range(0, correctMessage.Length)] + string.Format("{0:0.00}", sellTotal) + " coins."
                        : incorrectMessage[Random.Range(0, incorrectMessage.Length)];
        
        // Display the image of a prompted dish if the order is success
        dishColoredImage.SetActive(orderSuccess);
        plate.SetActive(!orderSuccess);

        // Set the game objects after half the time of wait time arrival
        yield return new WaitForSeconds((int)(waitTimeArrival / 2));
        chatBubble.SetActive(false);
        dishColoredImage.SetActive(false);
        plate.SetActive(true);
        StartCoroutine(CustomerDeparture(ableToStart));
    }

    public IEnumerator RewardCoins()
    {
        // Instantiate coins particle if the player is rewarded
        GameObject newParticle = Instantiate(coinsParticle, chatBubble.transform.position, Quaternion.identity);
        newParticle.GetComponent<ParticleSystem>().Play();
        
        // Increase coins by the computed sell total
        yield return new WaitForSeconds(1);
        _playerCoins.IncreaseCoins(sellTotal);
        coinsText.text = string.Format("{0:0.00}", PlayerPrefs.GetFloat("GlobalCoins", _playerCoins.initialCoins));
    }

    public void GoBackToMain()
    {
        // Set the game objects
        _audioManager.StopMusic();
        restaurantUI.SetActive(false);
        _levelLoad.levelId = PlayerPrefs.GetInt("ProvinceCurrent", 0);
        _levelLoad.LoadBack(_levelLoad.mainScene);
    }
}
