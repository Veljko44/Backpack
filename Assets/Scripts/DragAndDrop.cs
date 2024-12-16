using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class DragAndDrop : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging;
    public float minY = -10f;
    public float maxY = 10f;
    public float minZ = -10f;
    public float maxZ = 10f;
    public float maxHeight = 15f;
    private Rigidbody rb;
    private float startYPosition;
    public bool canDrag = true;
    public Transform backpackTransform;
    public Vector3 posOnBackpack;
    public Sprite itemSprite;
    public GameObject backpackPanel;
    public Sprite defaultBgSlot;
    private Backpack backpackScript;
    private void Start()
    {
        backpackScript = backpackTransform.GetComponent<Backpack>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        startYPosition = transform.position.y;
        if (backpackTransform == null)
        {
            Debug.LogError(" Set Backpack Transform!");
        }
    }

    private void OnMouseDown()
    {
        if (canDrag == true)
        {
            isDragging = true;
        }
    }

    private void OnMouseUp()
    {
        if (canDrag == true)
        {
            isDragging = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isDragging == false && canDrag == true)
        {
            if (collision.transform.tag == "Backpack")
            {
                if (backpackScript != null)
                {
                    InventoryItem item = GetComponent<ItemBehavior>().itemData;
                    backpackScript.AddItem(item);
                    PutInBackpack();
                }
            }
        }
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 5f;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePos);
            worldPosition.y = Mathf.Max(worldPosition.y, startYPosition);
            worldPosition.y = Mathf.Clamp(worldPosition.y, minY, maxY);
            worldPosition.y = Mathf.Min(worldPosition.y, maxHeight);
            worldPosition.z = Mathf.Clamp(worldPosition.z, minZ, maxZ);
            transform.position = worldPosition;
        }
    }

   public void  PutOutBackpack()
    {
        canDrag = true;
        rb.isKinematic = false;
        Image targetImage = FindMatchingImageInPanel();
        if (backpackScript != null)
        {
            InventoryItem item = GetComponent<ItemBehavior>().itemData;
            backpackScript.RemoveItem(item);
        }
        if (targetImage != null)
        {
            targetImage.sprite = null;
        }
    }

    public void PutInBackpack()
    {
        if (backpackTransform == null)
        {
            Debug.LogError("Backpack Transform not Founded!");
            return;
        }

        canDrag = false;
        rb.isKinematic = true;
        Image targetImage = FindMatchingImageInPanel();

        if (targetImage != null)
        {
            targetImage.sprite = itemSprite;
        }
        else
        {
            Debug.LogWarning("Matching Image not found in panel for tag: " + tag);
        }

        Transform targetChild = FindMatchingChildInBackpack();
        if (targetChild != null)
        {
            StartCoroutine(SmoothMoveToBackpack(targetChild));
        }
        else
        {
            Debug.LogWarning("Matching child not found in backpack for tag: " + tag);
        }
    }

    private Image FindMatchingImageInPanel()
    {
        foreach (Transform child in backpackPanel.GetComponentsInChildren<Transform>())
        {
            Image imageComponent = child.GetComponent<Image>();
            if (imageComponent != null && child.CompareTag(tag))
            {
                return imageComponent;
            }
        }
        return null;
    }

    private Transform FindMatchingChildInBackpack()
    {
        foreach (Transform child in backpackTransform.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag(tag))
            {
                return child;
            }
        }
        return null;
    }

    private IEnumerator SmoothMoveToBackpack(Transform targetChild)
    {
        float duration = 0.5f;
        float elapsedTime = 0f;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Vector3 targetPosition = targetChild.position;
        Quaternion targetRotation = targetChild.rotation;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        transform.rotation = targetRotation;
        transform.SetParent(targetChild);
    }
}
