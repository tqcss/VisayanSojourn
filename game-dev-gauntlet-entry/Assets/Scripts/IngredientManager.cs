using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class IngredientManager : MonoBehaviour
{
    private IngredientModule ingredientModule;
    private GameObject ingredientButton;
    private GameObject ingredientBase;
    public GameObject uiContent; // parent of ingredientButton
    
    // FOR DRAG AND DROP PHYSICS
    public LayerMask m_DragLayers; // layer dedicated for loose items
    public int deadZoneX = -11;
    private TargetJoint2D m_TargetJoint;
    private Rigidbody2D body; // loose item held by mouse - rigidbody2d component

    [Range(0.0f, 100.0f)]
    public float m_Damping = 1.0f;

    [Range(0.0f, 100.0f)]
    public float m_Frequency = 5.0f;

    private void Start()
    {
        ingredientModule = GameObject.FindWithTag("ingredientModule").GetComponent<IngredientModule>();
        ingredientButton = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ingredientButton.prefab");
        ingredientBase = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ingredientBase.prefab");

        // INGREDIENT SLOT LOADER
        foreach (IngredientInfo ingredientInfo in ingredientModule.ingredients)
        {
            GameObject newButton = Instantiate(ingredientButton, uiContent.transform);
            newButton.transform.GetChild(2).GetComponent<Text>().text = ingredientInfo.name;
            newButton.transform.GetChild(0).GetComponent<Image>().sprite = ingredientInfo.sprite;

            EventTrigger clickTrigger = newButton.GetComponent<EventTrigger>();
            EventTrigger.Entry clickEvent = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerDown
            };

            clickEvent.callback.AddListener((data) => { spawnIngredient(ingredientInfo); });
            clickTrigger.triggers.Add(clickEvent);
        }
    }

    void Update() // LOOSE ITEM BEHAVIOR
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D collider = Physics2D.OverlapPoint(mousePosition, m_DragLayers);
            if (!collider) return;

            body = collider.attachedRigidbody;
            if (!body) return;
            // Debug.Log(body.name);

            if (m_TargetJoint) Destroy(m_TargetJoint);

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
            {
                Destroy(body.gameObject);
            }
            body = null;
            return;
        }

        if (m_TargetJoint)
        {
            m_TargetJoint.target = mousePosition;
        }
    }

    public void spawnIngredient(IngredientInfo ingredientInfo)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        GameObject newIngredient = Instantiate(ingredientBase, mousePosition, Quaternion.identity);
        newIngredient.GetComponent<ObjectInfo>().id = ingredientInfo.id;

        newIngredient.transform.localScale = new Vector3(ingredientInfo.scaleX, ingredientInfo.scaleY, 0);
        newIngredient.GetComponent<SpriteRenderer>().sprite = ingredientInfo.sprite;
        newIngredient.GetComponent<BoxCollider2D>().size = new Vector2(ingredientInfo.colliderSizeX, ingredientInfo.colliderSizeY);

        if (ingredientInfo.randomRotation)
        {
            newIngredient.transform.rotation = Quaternion.Euler(0, 0, (Random.Range(0f, 360f)));
        }
    }
}
