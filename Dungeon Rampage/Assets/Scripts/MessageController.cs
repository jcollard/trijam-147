using UnityEngine;

public class MessageController : MonoBehaviour
{
    public static MessageController Instance;

    public Transform Container;
    public MessageDisplayController TemplateText;

    public void Start()
    {
        Instance = this;
    }

    public void DisplayMessage(string message)
    {
        Debug.Log(message);
        MessageDisplayController textArea = UnityEngine.Object.Instantiate<MessageDisplayController>(TemplateText);
        // RectTransform t = textArea.GetComponent<RectTransform>();
        // t.anchorMin = new Vector2(0, 0.5f);
        // t.anchorMax = new Vector2(1, 0.5f);
        // t.offsetMin = new Vector2(0, t.offsetMin.y);
        // t.offsetMax = new Vector2(0, t.offsetMax.y);
        textArea.Text.text = message;
        textArea.transform.SetParent(Container, false);
        // textArea.gameObject.transform.localPosition = new Vector3();
        // textArea.transform.localScale = new Vector3(1, 1, 1);
        textArea.gameObject.SetActive(true);
    }
}