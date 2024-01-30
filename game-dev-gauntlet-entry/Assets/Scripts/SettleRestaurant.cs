using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettleRestaurant : MonoBehaviour
{
    public int waitTimeArrival;
    public GameObject restaurantUI;
    public GameObject orderText;
    public GameObject orderDisplay;
    public GameObject checkButton;
    public GameObject plate;
    public GameObject dishColoredImage;
    public Text coinsText;
    public Text streakBonusText;
    public Text provinceBonusText;
    public Text chatText;
    public GameObject chatBubble;
    public string[] orderMessage;
    public string[] correctMessage;
    public string[] incorrectMessage;

    public float sellTotal;
    public int streakCount;
    public float streakBonus;
    public float bonusIncrement;
    public float[] provinceBonus;
    private GameObject coinsParticle;

    private AudioManager _audioManager;
    private DishList _dishList;
    private LevelLoad _levelLoad;
    private OrderManager _orderManager;
    private PlayerCoins _playerCoins;

    private void Awake()
    {
        // Reference the scripts from game objects
        _audioManager = GameObject.FindGameObjectWithTag("audioManager").GetComponent<AudioManager>();
        _dishList = GameObject.FindGameObjectWithTag("dishList").GetComponent<DishList>();
        _levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        _orderManager = GameObject.FindGameObjectWithTag("orderManager").GetComponent<OrderManager>();
        _playerCoins = GameObject.FindGameObjectWithTag("playerCoins").GetComponent<PlayerCoins>();
        
        // Reference the prefab particle
        coinsParticle = Resources.Load("Prefabs/coinsParticle", typeof(GameObject)) as GameObject;

        // Set initial values to the variables and set the game objects
        coinsText.text = string.Format("{0:0.00}", _playerCoins.globalCoins);
        DisplayBonus();
        chatBubble.SetActive(false);
        dishColoredImage.SetActive(false);
        plate.SetActive(true);
        StartCoroutine(StartOrder());
        _audioManager.PlayBackgroundMusic(_audioManager.restaurantMusic, true);
    }

    public IEnumerator StartOrder()
    {
        // Set the game objects
        orderText.SetActive(false);
        orderDisplay.SetActive(false);
        checkButton.SetActive(false);

        // Set the order of a customer
        yield return new WaitForSeconds(waitTimeArrival);
        orderText.SetActive(true);
        orderDisplay.SetActive(true);
        checkButton.SetActive(true);
        _dishList.RandomPromptOrder();
        _orderManager.StartTimer();
        ChatOrder();
    }

    public void EndOrder(bool success)
    {
        // End the order of a customer
        // Compute the increased bonus from the serve streak 
        streakCount = (success) ? streakCount + 1 : 0;
        streakBonus = (success) ? 1 + (streakCount * bonusIncrement) : 1;
        
        if (success)
        {
            int provinceCurrent = PlayerPrefs.GetInt("ProvinceCurrent", 0);
            // Compute the sell total with serve streak and province current bonuses
            sellTotal = _orderManager.SellCompute(streakBonus, provinceBonus[provinceCurrent - 1]);
            StartCoroutine(RewardCoins());
        }

        DisplayBonus();
        StartCoroutine(ChatResponse(success));
        StartCoroutine(StartOrder());
    }

    public void DisplayBonus()
    {
        // Update display the serve streak and current province bonuses
        int provinceCurrent = PlayerPrefs.GetInt("ProvinceCurrent", 0);
        streakBonusText.text = string.Format("Streak Bonus: {0:0.00}x", streakBonus);
        provinceBonusText.text = string.Format("Province Bonus: {0:0.00}x", provinceBonus[provinceCurrent - 1]);
    }

    public void ChatOrder()
    {
        // Display the order of a customer
        chatBubble.SetActive(true);
        chatText.text = orderMessage[Random.Range(0, orderMessage.Length)] + _orderManager.currentOrderPrompt.name + "?";
    }

    public IEnumerator ChatResponse(bool success)
    {
        // Display the response of a customer
        chatText.text = (success)
                        ? correctMessage[Random.Range(0, correctMessage.Length)] + string.Format("{0:0.00}", sellTotal) + " coins."
                        : incorrectMessage[Random.Range(0, incorrectMessage.Length)];
        
        // Display the image of a prompted dish if success
        dishColoredImage.SetActive(success);
        plate.SetActive(!success);

        // Set the game objects after half the time of wait time arrival
        yield return new WaitForSeconds((int)(waitTimeArrival / 2));
        chatBubble.SetActive(false);
        dishColoredImage.SetActive(false);
        plate.SetActive(true);
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
