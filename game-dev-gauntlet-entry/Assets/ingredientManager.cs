using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ingredientManager : MonoBehaviour
{
    // REFERENCES
    public ingredientModule ingredientModule;
    public GameObject ingredientButton;
    public GameObject uiContent;
    public GameObject ingredientBase;

    public LayerMask m_DragLayers; // layer dedicated for loose items
    public int deadZoneX = -11;
    // public int deadZoneY = -13;

    // // DRAG DROP PHYSICS
    [Range(0.0f, 100.0f)]
    public float m_Damping = 1.0f;

    [Range(0.0f, 100.0f)]
    public float m_Frequency = 5.0f;

    private TargetJoint2D m_TargetJoint;
    private Rigidbody2D body; // loose item held by mouse - rigidbody2d component

    private void Start() // INGREDIENT SLOT MANAGER
    {
        for (int i = 0; i < ingredientModule.ingredients.Length; i++)
        {
            GameObject newButton = Instantiate(ingredientButton, uiContent.transform);
            newButton.GetComponentInChildren<Text>().text = ingredientModule.getIngredient(i).name;

            // ASSIGN TRIGGER EVENT
            EventTrigger clickTrigger = newButton.GetComponent<EventTrigger>();
            EventTrigger.Entry clickEvent = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerDown
            };

            int localValue = i; // temp var for lambda
            clickEvent.callback.AddListener((data) => { spawnIngredient(localValue); });
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

    public void spawnIngredient(int id)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        GameObject newIngredient = Instantiate(ingredientBase, mousePosition, Quaternion.identity);
        newIngredient.GetComponent<info>().id = id; // assign id to game object for ingredientchecker
        
        // insert sprite assigner here
    }
}
