using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    //Interaction Type
    public enum InteractionType
    {
        NONE,
        PickUp,
        Examine,
        LoadLevelTwo,
        LoadLevelThree,
        Finish
    }
    public enum ItemType
    {
        Static,
        Consumable
    }

    [Header("Attributes")]
    public InteractionType interactType;
    public ItemType type;

    [Header("Examine")]
    public string descriptionText;

    [Header("Custom Evenets")]
    public UnityEvent customEvent;
    public UnityEvent consumeEvent;

    //Collider Trigger
    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 7;
    }

    public void Interact()
    {
        switch (interactType)
        {
            case InteractionType.PickUp:
                //Add the object to the PickedUpItems list and then delete/disable the object
                FindObjectOfType<InventorySystem>().PickUp(gameObject);
                gameObject.SetActive(false);
                break;
            case InteractionType.Examine:
                //Call the Examine itiem in the interaction system
                FindObjectOfType<InteractionSystem>().ExamineItem(this);
                break;
            case InteractionType.LoadLevelTwo:
                SceneManager.LoadScene("LevelTwo");
                break;
            case InteractionType.LoadLevelThree:
                SceneManager.LoadScene("LevelThree");
                break;
            case InteractionType.Finish:
                SceneManager.LoadScene("Finish");
                break;
            default:
                Debug.Log("NULL ITEM");
                break;
        }

        customEvent.Invoke();
    }
}
