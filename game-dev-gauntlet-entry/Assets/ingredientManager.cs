using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ingredientManager : MonoBehaviour
{
    public ingredientModule ingredientModule;
    public GameObject ingredientButton;
    public GameObject uiContent;
    public GameObject ingredientBase;
    public LayerMask m_DragLayers; // layer dedicated for loose items
    public int deadZoneX = -11;
    public int deadZoneY = -13;

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

            EventTrigger eventTrigger = newButton.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventData) => {spawnIngredient(i);});
            eventTrigger.triggers.Add(entry);

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
        // Debug.Log("Spawned ingredient: " + ingredientModule.getIngredient(id).name);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        GameObject newIngredient = Instantiate(ingredientBase, mousePosition, Quaternion.identity);
        
        // insert sprite assigner here
    }
}
