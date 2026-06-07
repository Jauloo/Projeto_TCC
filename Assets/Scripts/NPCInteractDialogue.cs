using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCInteractDialogue : MonoBehaviour
{
    [Header("Visual")]
    public GameObject interactIcon;

    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerNameText;
    public GameObject choiceButtonsPanel;
    public Button btnThank;
    public Button btnAsk;

    [Header("Text")]
    [TextArea] public string[] introLines;
    [TextArea] public string explainAnswer;
    public float typingSpeed = 0.03f;

    bool playerInRange = false;
    bool isTyping = false;
    bool showingExplanation = false;

    void Start()
    {
        if (interactIcon != null) interactIcon.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (choiceButtonsPanel != null) choiceButtonsPanel.SetActive(false);

        if (speakerNameText != null) speakerNameText.text = "HEX";

        if (btnThank != null) btnThank.onClick.AddListener(OnThank);
        if (btnAsk != null) btnAsk.onClick.AddListener(OnAsk);
    }

    void Update()
    {
        if (!playerInRange) return;
        if (isTyping) return;

        if (!dialoguePanel.activeSelf && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.E)))
        {
            StartCoroutine(TypeSequence(introLines));
            return;
        }

        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            if (showingExplanation)
            {
                showingExplanation = false;
                dialoguePanel.SetActive(false);
                choiceButtonsPanel.SetActive(true);
            }
        }
    }

    IEnumerator TypeSequence(string[] lines)
    {
        dialogCloseAll();
        dialoguePanel.SetActive(true);
        choiceButtonsPanel.SetActive(false);

        isTyping = true;
        for (int i = 0; i < lines.Length; i++)
        {
            dialogueText.text = "";
            string processedLine = lines[i].Replace("\\n", "\n");
            foreach (char c in processedLine.ToCharArray())
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }
        isTyping = false;
        ShowChoices();
    }

    void ShowChoices()
    {
        dialoguePanel.SetActive(false);
        choiceButtonsPanel.SetActive(true);
    }

    void OnThank()
    {
        dialogCloseAll();
    }

    void OnAsk()
    {
        StartCoroutine(ShowExplainAndReturn());
    }

    IEnumerator ShowExplainAndReturn()
    {
        choiceButtonsPanel.SetActive(false);
        dialoguePanel.SetActive(true);
        dialogueText.text = "";
        showingExplanation = true;

        isTyping = true;
        string processed = explainAnswer.Replace("\\n", "\n");
        foreach (char c in processed.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void dialogCloseAll()
    {
        dialoguePanel.SetActive(false);
        choiceButtonsPanel.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactIcon != null) StartCoroutine(BlinkIcon());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactIcon != null) { interactIcon.SetActive(false); StopAllCoroutines(); }
            dialogCloseAll();
        }
    }

    IEnumerator BlinkIcon()
    {
        while (playerInRange)
        {
            interactIcon.SetActive(true);
            yield return new WaitForSeconds(0.6f);
            interactIcon.SetActive(false);
            yield return new WaitForSeconds(0.4f);
        }
    }
}