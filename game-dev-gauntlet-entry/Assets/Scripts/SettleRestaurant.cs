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
    public Text chatText;
    public GameObject chatBubble;
    public string[] orderMessage;
    public string[] correctMessage;
    public string[] incorrectMessage;
    public float sellTotal;
    private GameObject coinsParticle;

    private AudioManager _audioManager;
    private DishList _dishList;
    private LevelLoad _levelLoad;
    private OrderManager _orderManager;
    private PlayerCoins _playerCoins;

    private void Awake()
    {
        // Referencing the Scripts from GameObjects
        _audioManager = GameObject.FindGameObjectWithTag("audioManager").GetComponent<AudioManager>();
        _dishList = GameObject.FindGameObjectWithTag("dishList").GetComponent<DishList>();
        _levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        _orderManager = GameObject.FindGameObjectWithTag("orderManager").GetComponent<OrderManager>();
        _playerCoins = GameObject.FindGameObjectWithTag("playerCoins").GetComponent<PlayerCoins>();
        
        coinsParticle = Resources.Load("Prefabs/coinsParticle", typeof(GameObject)) as GameObject;
        coinsText.text = string.Format("{0:0.00}", _playerCoins.globalCoins);

        chatBubble.SetActive(false);
        dishColoredImage.SetActive(false);
        plate.SetActive(true);
        StartCoroutine(StartOrder());
        _audioManager.PlayBackgroundMusic(_audioManager.restaurantMusic);
    }

    public IEnumerator StartOrder()
    {
        // Start Customer's Order
        orderText.SetActive(false);
        orderDisplay.SetActive(false);
        checkButton.SetActive(false);

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
        // End Customer's Order
        if (success)
        {
            sellTotal = _orderManager.SellCompute();
            StartCoroutine(RewardCoins());
            StartCoroutine(ChatResponse(true));
            StartCoroutine(StartOrder());
        }
        else
        {
            StartCoroutine(ChatResponse(false));
            StartCoroutine(StartOrder());
        }
    }

    public void ChatOrder()
    {
        // Display the Order of a Customer
        chatBubble.SetActive(true);
        chatText.text = orderMessage[Random.Range(0, orderMessage.Length)] + _orderManager.currentOrderPrompt.name + "?";
    }

    public IEnumerator ChatResponse(bool success)
    {
        // Display the Response of a Customer
        if (success)
        {
            chatText.text = correctMessage[Random.Range(0, correctMessage.Length)] + string.Format("{0:0.00}", sellTotal) + " coins.";
            dishColoredImage.SetActive(true);
            plate.SetActive(false);
        }
        else
        {
            chatText.text = incorrectMessage[Random.Range(0, incorrectMessage.Length)];
        }

        yield return new WaitForSeconds((int)(waitTimeArrival / 2));
        chatBubble.SetActive(false);
        dishColoredImage.SetActive(false);
        plate.SetActive(true);
    }

    public IEnumerator RewardCoins()
    {
        // Instantiate coinsParticle when Rewarded
        GameObject newParticle = Instantiate(coinsParticle, chatBubble.transform.position, Quaternion.identity);
        newParticle.GetComponent<ParticleSystem>().Play();
        
        // Increase Coins by calculated sellTotal
        yield return new WaitForSeconds(1);
        _playerCoins.IncreaseCoins(sellTotal);
        coinsText.text = string.Format("{0:0.00}", PlayerPrefs.GetFloat("GlobalCoins", 0));
    }

    public void GoBackToMain()
    {
        _audioManager.StopMusic();
        restaurantUI.SetActive(false);
        _levelLoad.levelId = PlayerPrefs.GetInt("ProvinceCurrent", 0);
        _levelLoad.LoadBack(_levelLoad.mainScene);
    }
}
