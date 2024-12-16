using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

public class ServerManager : MonoBehaviour
{
    private const string serverUrl = "https://wadahub.manerai.com/api/inventory/send";
    private const string authToken = "kPERnYcWAY46xaSy8CEzanosAgsW";

    public IEnumerator SendItemData(InventoryItem item, string action)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", item.id);
        form.AddField("action", action);

        UnityWebRequest request = UnityWebRequest.Post(serverUrl, form);
        request.SetRequestHeader("Authorization", $"Bearer {authToken}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Success: {request.downloadHandler.text}");
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }
}