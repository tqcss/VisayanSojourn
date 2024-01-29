
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using System.Linq;

public class IngredientManager : MonoBehaviour
{
    private GameObject ingredientButton;
    private GameObject ingredientBase;
    public GameObject uiContent; // Parent of ingredientButton
    public Scrollbar scrollBar;
    
    // For Drag and Drop Physics
    public LayerMask m_DragLayers; // Layer dedicated for loose items
    public int deadZoneX = -11;
    private TargetJoint2D m_TargetJoint;
    private Rigidbody2D body; // Loose item held by mouse - rigidbody2D component

    [Range(0.0f, 100.0f)]
    public float m_Damping = 1.0f;

    [Range(0.0f, 100.0f)]
    public float m_Frequency = 5.0f;

    private AudioManager _audioManager;
    private IngredientModule _ingredientModule;

    private void Start()
    {
        // Referencing the Scripts from GameObjects
        _audioManager = GameObject.FindGameObjectWithTag("audioManager").GetComponent<AudioManager>();
        _ingredientModule = GameObject.FindGameObjectWithTag("ingredientModule").GetComponent<IngredientModule>();
        ingredientButton = Resources.Load("Prefabs/ingredientButton", typeof(GameObject)) as GameObject;
        ingredientBase = Resources.Load("Prefabs/ingredientBase", typeof(GameObject)) as GameObject;

        // Ingredient Slot Loader
        foreach (IngredientInfo ingredientInfo in _ingredientModule.ingredients.OrderBy(item => item.name).ToList())
        {
            GameObject newButton = Instantiate(ingredientButton, uiContent.transform);
                
            newButton.transform.GetChild(2).GetComponent<Text>().text = ingredientInfo.name.Replace("_", " ");
            newButton.transform.GetChild(0).GetComponent<Image>().sprite = ingredientInfo.sprite;

            EventTrigger clickTrigger = newButton.GetComponent<EventTrigger>();
            EventTrigger.Entry clickEvent = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerDown
            };

            clickEvent.callback.AddListener((data) => { SpawnIngredient(ingredientInfo); });
            clickTrigger.triggers.Add(clickEvent);
        }
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        // Update the Cell Size of Ingredient Slots and ScrollBar 
        uiContent.GetComponent<RectTransform>().sizeDelta = new Vector2
            (uiContent.GetComponent<RectTransform>().sizeDelta.x, 
            (uiContent.GetComponent<GridLayoutGroup>().cellSize.y + uiContent.GetComponent<GridLayoutGroup>().spacing.y) * Mathf.CeilToInt(_ingredientModule.ingredients.Count / 2.0f));
        scrollBar.value = 1.0f;
    }

    private void Update()
    {
        // Loose Item Behavior
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
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
        else if (Input.GetMouseButtonUp(0))
        {
            Destroy(m_TargetJoint);
            m_TargetJoint = null;

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
        // Spawns Ingredient when Dragged from the Slot
        _audioManager.popSfx.Play();
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        GameObject newIngredient = Instantiate(ingredientBase, mousePosition, Quaternion.identity);
        newIngredient.name = ingredientInfo.name;

        if (ingredientInfo.randomRotation)
            newIngredient.transform.rotation = Quaternion.Euler(0, 0, (Random.Range(0f, 360f)));

        if (ingredientInfo.sprite == null)
            return;

        newIngredient.transform.localScale = new Vector3(ingredientInfo.scaleX, ingredientInfo.scaleY, 0);
        newIngredient.GetComponent<SpriteRenderer>().sprite = ingredientInfo.sprite;
        newIngredient.GetComponent<BoxCollider2D>().size = new Vector2(ingredientInfo.colliderSizeX, ingredientInfo.colliderSizeY);
    }
}
