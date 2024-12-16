using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class BackpackOpen : MonoBehaviour
{
    public GameObject panel;
    public bool isMouseOverPanel = false;
    public bool isMouseOverObject = false;
    public bool isPanelOpen = false;
    public Image[] backpackSlot;
    public Transform[] backpackPlaces;
    private Image lastHoveredSlot = null;
    public GameObject backpack;
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            panel.SetActive(false);
            isPanelOpen = false;

            if (isMouseOverPanel)
            {
                isMouseOverPanel = false;
            }
            if (lastHoveredSlot != null)
            {
                OnMouseReleasedOnSlot(lastHoveredSlot);
                lastHoveredSlot = null;
            }
        }
        if (Input.GetMouseButton(0) && (isMouseOverPanel || isMouseOverObject) && !isPanelOpen)
        {
            panel.SetActive(true);
            isPanelOpen = true;
        }
    }
    public void OnMouseEnterPanel()
    {
        isMouseOverPanel = true;
    }
    public void OnMouseExitPanel()
    {
        isMouseOverPanel = false;
        panel.SetActive(false);
        isPanelOpen = false;
    }
    private void OnMouseEnter()
    {
        isMouseOverObject = true;
    }
    private void OnMouseExit()
    {
        isMouseOverObject = false;
    }
    public void OnMouseEnterSlot(Image slot)
    {
        if (slot != null)
        {
            lastHoveredSlot = slot;
        }
    }
    public void OnMouseExitSlot(Image slot)
    {
        if (lastHoveredSlot == slot)
        {
            lastHoveredSlot = null;
        }
    }

    private void OnMouseReleasedOnSlot(Image slot)
    {
        int slotIndex = System.Array.IndexOf(backpackSlot, slot);
        if (slotIndex >= 0 && slotIndex < backpackPlaces.Length)
        {
            Transform place = backpackPlaces[slotIndex];
            if (place.childCount > 0)
            {
                Vector3 backpackCenter = backpack.GetComponent<Collider>().bounds.center;
                Transform child = place.GetChild(0);
                child.SetParent(null);
                float horizontalOffset = 0f;
                if (child.position.x < backpackCenter.x)
                {
                    horizontalOffset = -2f;
                }
                else
                {
                    horizontalOffset = 2f;
                }
                Vector3 targetPosition = new Vector3(child.position.x + horizontalOffset, child.position.y, child.position.z);
                StartCoroutine(MoveObjectSidewaysAndCheck(child, targetPosition, horizontalOffset));
            }
            else
            {
                Debug.Log($"Backpack place {slotIndex} is Empty.");
            }
        }
        else
        {
            Debug.LogError("Slot not found  or  index not valid.");
        }
    }

    private IEnumerator MoveObjectSidewaysAndCheck(Transform objectToMove, Vector3 targetPosition, float horizontalOffset)
    {
        float timeElapsed = 0f;
        float smoothTime = 0.5f;
        Vector3 startPosition = objectToMove.position;
        while (timeElapsed < smoothTime)
        {
            objectToMove.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / smoothTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        objectToMove.position = targetPosition;
        if (Mathf.Abs(objectToMove.position.x - startPosition.x) > 0.5f)
        {
            PerformActionAfterMovement(objectToMove);
        }
    }

    private void PerformActionAfterMovement(Transform objectToMove)
    {
        DragAndDrop dragDrop = objectToMove.GetComponent<DragAndDrop>();
        dragDrop.PutOutBackpack();
    }

}