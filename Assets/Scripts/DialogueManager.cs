using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerNameText;

    public GameObject optionButtonsPanel;

    public string[] dialogueLines;
    private int index;
    private bool showingExplanation = false;

    public float typingSpeed = 0.005f;
    private bool isTyping = false;

    public PlayerMovement playerMovement;

    public GameObject portalToMain;

    void Start()
    {
        dialoguePanel.SetActive(true);
        optionButtonsPanel.SetActive(false);
        playerMovement.enabled = false;

        if (speakerNameText != null)
            speakerNameText.text = "KERN";

        StartCoroutine(TypeLine());
    }

    void Update()
    {
        if (isTyping) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (showingExplanation)
            {
                showingExplanation = false;
                dialoguePanel.SetActive(false);
                optionButtonsPanel.SetActive(true);
                return;
            }

            if (index < dialogueLines.Length)
            {
                index++;
                if (index < dialogueLines.Length)
                {
                    dialogueText.text = "";
                    StartCoroutine(TypeLine());
                }
                else
                {
                    ShowOptions();
                }
            }
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        string processedLine = dialogueLines[index].Replace("\\n", "\n");
        foreach (char letter in processedLine.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void ShowOptions()
    {
        dialoguePanel.SetActive(false);
        optionButtonsPanel.SetActive(true);
    }

    public void ContinueMission()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
            playerMovement.canMove = true;
        }

        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.simulated = true;
        }

        if (portalToMain != null)
            portalToMain.SetActive(true);

        Time.timeScale = 1f;
        dialoguePanel.SetActive(false);
        optionButtonsPanel.SetActive(false);
    }

    public void ExplainOpenSource()
    {
        optionButtonsPanel.SetActive(false);
        dialoguePanel.SetActive(true);
        dialogueText.text = "Sistemas Operacionais Open Source são softwares cujo código fonte é aberto para qualquer pessoa estudar, modificar e distribuir.\n\n\nEles promovem colaboração, liberdade e evolução tecnológica.\n\nPressione ESPAÇO para continuar...";dialogueText.text = "Sistemas Operacionais Open Source são softwares cujo código fonte é aberto para qualquer pessoa estudar, modificar e distribuir.\n\n\nEles promovem colaboração, liberdade e evolução tecnológica.";
        showingExplanation = true;
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("EndScene");
    }
}