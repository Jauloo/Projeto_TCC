using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LinuxNPCQuiz : MonoBehaviour
{
    private PlayerMovement playerMovement; 

    [Header("UI Elements")]
    public GameObject interactIcon; 
    public GameObject dialoguePanel;   
    public TextMeshProUGUI dialogueText;
    public GameObject choicePanel; 
    public Button btnAnswerNow;
    public Button btnSearch;

    [Header("Quiz Elements")]
    public GameObject quizPanel;       
    public LinuxQuizManager quizManager;

    [Header("Portal")]
    public GameObject portalBackToMain; 

    bool playerInRange = false;

    void Start()
    {

        if (interactIcon != null) interactIcon.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (quizPanel != null) quizPanel.SetActive(false);
        if (portalBackToMain != null) portalBackToMain.SetActive(false);

        btnAnswerNow.onClick.AddListener(OnAnswerNow);
        btnSearch.onClick.AddListener(OnSearch);

        var player = GameObject.FindWithTag("Player");
        if (player != null)
            playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (!playerInRange) return;

        if (!dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            dialoguePanel.SetActive(true);
            choicePanel.SetActive(true);
            dialogueText.text =
                "Sou o guardião do fragmento Linux." +
                "Este sistema nasceu da liberdade e da colaboração entre programadores."+

                "\nPara me deixar ir, você deve responder 4 perguntas. " +
                
                "As respostas estão nos objetos desta sala. " +
                "\nDeseja responder agora ou procurar as respostas?";

            if (playerMovement != null)
                playerMovement.canMove = false;
        }
        
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            dialoguePanel.SetActive(false);
            if (portalBackToMain != null)
                portalBackToMain.SetActive(true);

            if (playerMovement != null)
                playerMovement.canMove = true;
        }
    }

    void OnAnswerNow()
    {
        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);
        StartQuiz();
    }

    void OnSearch()
    {
        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);

        if (playerMovement != null)
            playerMovement.canMove = true;
    }

    void StartQuiz()
    {
        if (quizManager != null)
        {
            quizPanel.SetActive(true);
            quizManager.StartQuiz();

            if (playerMovement != null)
                playerMovement.canMove = false;
        }
    }

    public void OnQuizSucceeded()
    {
        quizPanel.SetActive(false);

        if (portalBackToMain != null)
            portalBackToMain.SetActive(true);

        GameState.LinuxCompleted = true;
        if (dialoguePanel != null && dialogueText != null)
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = "Parabéns! Você respondeu corretamente todas as perguntas do fragmento Linux. "+
            "\n\nAgora volte por aquele portal e siga adiante para passar seu conhecimento.";
        }
        
        if (playerMovement != null)
            playerMovement.canMove = true;
    }


    public void OnQuizFailed()
    {
        if (quizPanel != null)
            quizPanel.SetActive(false);

        if (dialoguePanel != null && dialogueText != null)
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = "Que pena... parece que você ainda não domina os fundamentos. Vamos tentar novamente?";
        }

        if (choicePanel != null)
            choicePanel.SetActive(true);

        if (playerMovement != null)
            playerMovement.canMove = false;
    }   

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactIcon != null)
                interactIcon.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactIcon != null)
                interactIcon.SetActive(false);

            if (dialoguePanel != null)
                dialoguePanel.SetActive(false);

            if (quizPanel != null)
                quizPanel.SetActive(false);

            if (playerMovement != null)
                playerMovement.canMove = true;
        }
    }
}
