using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using System.Linq;

public class IngredientManager : MonoBehaviour
{
    private GameObject purchaseButton;
    private GameObject freeButton;
    private GameObject ingredientButton;
    private GameObject ingredientBase;
    public GameObject uiContent; // Parent of ingredientButton
    public Scrollbar scrollBar;
    
    // For drag and drop physics
    public LayerMask m_DragLayers; // Layer dedicated for loose items
    public int deadZoneX = -11;
    private TargetJoint2D m_TargetJoint;
    private Rigidbody2D body; // Loose item held by mouse - rigidbody2D component

    [Range(0.0f, 100.0f)]
    public float m_Damping = 1.0f;

    [Range(0.0f, 100.0f)]
    public float m_Frequency = 5.0f;

    private AudioManager _audioManager;
    private LevelLoad _levelLoad;
    private IngredientModule _ingredientModule;
    private SettleRestaurant _settleRestaurant;

    private void Start()
    {
        // Reference the scripts from game objects
        _audioManager = GameObject.FindGameObjectWithTag("audioManager").GetComponent<AudioManager>();
        _levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        _ingredientModule = GameObject.FindGameObjectWithTag("ingredientModule").GetComponent<IngredientModule>();
        _settleRestaurant = GameObject.FindGameObjectWithTag("mainScript").GetComponent<SettleRestaurant>();

        // Reference the prefab objects
        purchaseButton = Resources.Load("Prefabs/purchaseIngredientButton", typeof(GameObject)) as GameObject;
        freeButton = Resources.Load("Prefabs/freeIngredientButton", typeof(GameObject)) as GameObject;
        ingredientBase = Resources.Load("Prefabs/ingredientBase", typeof(GameObject)) as GameObject;

        // Check the current mode
        // Kitchen Mode: Ingredients should have no cost
        // Restaurant Mode: Ingredients should have cost based on the sell point
        switch (_levelLoad.CheckModeId())
        {
            case 1:
                ingredientButton = freeButton;
                break;
            case 2:
                ingredientButton = purchaseButton;
                break;
        }

        // Load the slots of ingredients on the ingredient tab
        foreach (IngredientInfo ingredientInfo in _ingredientModule.ingredients.OrderBy(item => item.name).ToList())
        {
            // Instantiate ingredient buttons
            GameObject newButton = Instantiate(ingredientButton, uiContent.transform);
            
            // Set children to each button to display the ingredient's name and sprite
            newButton.transform.GetChild(0).GetComponent<Image>().sprite = ingredientInfo.sprite;
            newButton.transform.GetChild(2).GetComponent<Text>().text = ingredientInfo.name.Replace("_", " ");
            if (_levelLoad.CheckModeId() == 2)
                newButton.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = string.Format("{0:0.00}", Mathf.Round(ingredientInfo.sellPoint * _settleRestaurant.costRatio * 100.0f) * 0.01f);

            // Add an event trigger to each button
            EventTrigger clickTrigger = newButton.GetComponent<EventTrigger>();
            EventTrigger.Entry clickEvent = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerDown
            };

            // Add a listener SpawnIngredient(ingredientInfo) to each button for its functionality
            clickEvent.callback.AddListener((data) => { SpawnIngredient(ingredientInfo); });
            clickTrigger.triggers.Add(clickEvent);
        }
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        // Update the cell size of ingredient tab based on the number of ingredient slots,
        // and reset the scroll bar value (not working)
        uiContent.GetComponent<RectTransform>().sizeDelta = new Vector2
            (uiContent.GetComponent<RectTransform>().sizeDelta.x, 
            (uiContent.GetComponent<GridLayoutGroup>().cellSize.y + uiContent.GetComponent<GridLayoutGroup>().spacing.y) * Mathf.CeilToInt(_ingredientModule.ingredients.Count / 2.0f));
        scrollBar.value = 1f;
    }

    private void Update()
    {
        // Loose item behavior
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Set the position of a selected ingredient based on the cursor pointed
        // if the mouse left click is in hold position
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D collider = Physics2D.OverlapPoint(mousePosition, m_DragLayers);
            if (!collider)
                return;

            body = collider.attachedRigidbody;
            if (!body)
                return;

            if (m_TargetJoint)
                Destroy(m_TargetJoint);

            m_TargetJoint = body.gameObject.AddComponent<TargetJoint2D>();
            m_TargetJoint.dampingRatio = m_Damping;
            m_TargetJoint.frequency = m_Frequency;
            m_TargetJoint.anchor = m_TargetJoint.transform.InverseTransformPoint(mousePosition);
        }
        // Drop the selected ingredient if the mouse left click is not in hold position
        else if (Input.GetMouseButtonUp(0))
        {
            Destroy(m_TargetJoint);
            m_TargetJoint = null;

            // Destroy the ingredient if the position X is in less than the dead zone
            if (body && body.gameObject.transform.position.x <= deadZoneX)
                Destroy(body.gameObject);

            body = null;
            return;
        }

        if (m_TargetJoint)
            m_TargetJoint.target = mousePosition;

    }

    public void SpawnIngredient(IngredientInfo ingredientInfo)
    {
        _audioManager.popSfx.Play();
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Instantiate a selected ingredient when it is dragged from the ingredient slot
        GameObject newIngredient = Instantiate(ingredientBase, mousePosition, Quaternion.identity);

        // Set the name, local scale, sprite, and collider size of the spawned ingredient
        newIngredient.name = ingredientInfo.name;

        if (ingredientInfo.randomRotation)
            newIngredient.transform.rotation = Quaternion.Euler(0, 0, (Random.Range(0f, 360f)));

        if (ingredientInfo.sprite == null)
            return;

        newIngredient.transform.localScale = new Vector3(ingredientInfo.scaleX, ingredientInfo.scaleY, 0);
        newIngredient.GetComponent<SpriteRenderer>().sprite = ingredientInfo.sprite;
        newIngredient.GetComponent<BoxCollider2D>().size = new Vector2(ingredientInfo.colliderSizeX, ingredientInfo.colliderSizeY);

        // Execute decreasing player global coins if the current mode is restaurant
        if (_levelLoad.CheckModeId() == 2)
            _settleRestaurant.PurchaseIngredient(ingredientInfo);
    }
}
