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

    private GameObject particles;
    private DishList dishList;
    private LevelLoad levelLoad;
    private OrderManager orderManager;
    private RecipeManager recipeManager;
    private PlayerCoins playerCoins;
    private PlayerProvince playerProvince;

    private void Awake()
    {
        dishList = GameObject.FindGameObjectWithTag("dishList").GetComponent<DishList>();
        levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        orderManager = GameObject.FindGameObjectWithTag("orderManager").GetComponent<OrderManager>();
        recipeManager = GameObject.FindGameObjectWithTag("recipeManager").GetComponent<RecipeManager>();
        playerCoins = GameObject.FindGameObjectWithTag("playerCoins").GetComponent<PlayerCoins>();
        playerProvince = GameObject.FindGameObjectWithTag("playerProvince").GetComponent<PlayerProvince>();
        
        particles = Resources.Load("Prefabs/coinsParticle", typeof(GameObject)) as GameObject;
        coinsText.text = string.Format("{0:0.00}", playerCoins.globalCoins);

        chatBubble.SetActive(false);
        dishColoredImage.SetActive(false);
        plate.SetActive(true);
        StartCoroutine(StartOrder());
    }

    public IEnumerator StartOrder()
    {
        orderText.SetActive(false);
        orderDisplay.SetActive(false);
        checkButton.SetActive(false);

        yield return new WaitForSeconds(waitTimeArrival);
        orderText.SetActive(true);
        orderDisplay.SetActive(true);
        checkButton.SetActive(true);
        dishList.RandomPromptOrder();
        orderManager.StartTimer();
        ChatOrder();
    }

    public void EndOrder(bool success)
    {
        if (success)
        {
            sellTotal = orderManager.SellCompute();
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
        chatBubble.SetActive(true);
        chatText.text = orderMessage[Random.Range(0, orderMessage.Length)] + orderManager.currentOrderPrompt.name + "?";
    }

    public IEnumerator ChatResponse(bool success)
    {
        if (success)
        {
            chatText.text = correctMessage[Random.Range(0, correctMessage.Length)] + string.Format("{0:0.00}", sellTotal) + " coins.";
            dishColoredImage.SetActive(true);
            plate.SetActive(false);
        }
        else
            chatText.text = incorrectMessage[Random.Range(0, incorrectMessage.Length)];
        
        yield return new WaitForSeconds((int)(waitTimeArrival / 2));
        chatBubble.SetActive(false);
        dishColoredImage.SetActive(false);
        plate.SetActive(true);
    }

    public IEnumerator RewardCoins()
    {
        GameObject newParticle = Instantiate(particles, chatBubble.transform.position, Quaternion.identity);
        newParticle.GetComponent<ParticleSystem>().Play();
        
        yield return new WaitForSeconds(1);
        playerCoins.IncreaseCoins(sellTotal);
        coinsText.text = string.Format("{0:0.00}", PlayerPrefs.GetFloat("GlobalCoins", 0));
    }

    public void GoBackToMain()
    {
        restaurantUI.SetActive(false);
        levelLoad.levelId = PlayerPrefs.GetInt("ProvinceCurrent", 0);
        levelLoad.LoadBack(levelLoad.mainScene);
    }
}
