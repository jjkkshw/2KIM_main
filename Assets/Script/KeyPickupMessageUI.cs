using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KeyPickupMessageUI : MonoBehaviour
{
    private const float DisplaySeconds = 2f;

    private static KeyPickupMessageUI instance;

    private Text messageText;
    private Coroutine hideRoutine;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Initialize()
    {
        EnsureInstance();
    }

    public static void Show(KeyType keyType)
    {
        EnsureInstance();
        instance.ShowMessage(keyType + " 열쇠 획득!");
    }

    private static void EnsureInstance()
    {
        if (instance != null)
        {
            return;
        }

        GameObject root = new GameObject("KeyPickupMessageUI");
        instance = root.AddComponent<KeyPickupMessageUI>();
        DontDestroyOnLoad(root);
        instance.CreateUI(root);
    }

    private void CreateUI(GameObject root)
    {
        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        CanvasScaler scaler = root.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        root.AddComponent<GraphicRaycaster>();

        GameObject textObject = new GameObject("MessageText");
        textObject.transform.SetParent(root.transform, false);

        messageText = textObject.AddComponent<Text>();
        messageText.alignment = TextAnchor.MiddleCenter;
        messageText.color = Color.white;
        messageText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        messageText.fontSize = 42;
        messageText.raycastTarget = false;
        messageText.text = "";

        Shadow shadow = textObject.AddComponent<Shadow>();
        shadow.effectColor = new Color(0f, 0f, 0f, 0.75f);
        shadow.effectDistance = new Vector2(2f, -2f);

        RectTransform rect = textObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 1f);
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.anchoredPosition = new Vector2(0f, -90f);
        rect.sizeDelta = new Vector2(900f, 80f);

        textObject.SetActive(false);
    }

    private void ShowMessage(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);

        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
        }

        hideRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(DisplaySeconds);
        messageText.gameObject.SetActive(false);
        hideRoutine = null;
    }
}
