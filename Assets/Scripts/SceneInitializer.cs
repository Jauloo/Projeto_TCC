using UnityEngine;
using TMPro;
using System.Collections;

public class SceneInitializer : MonoBehaviour
{
    public GameObject portalUnix;
    public GameObject portalBSD;
    public GameObject portalLinux;
    public GameObject portalDebian;
    public GameObject portalUbuntu;
    public GameObject portalAndroid;
    public GameObject portalRedox;
    public GameObject portalAtualmente;
    
    public GameObject portalFinal;  

    public GameObject endDialoguePanel;          
    public TextMeshProUGUI endDialogueText;

    private bool waitingForInput = false;
    private PlayerMovement playerMovement;

    void Start()
    {
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.canMove = true;
                playerMovement.enabled = true;
            }

            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.simulated = true;
        }

        Time.timeScale = 1f;
        AtualizarPortais();
    }

    void Update()
    {
        if (waitingForInput && Input.GetKeyDown(KeyCode.Space))
        {
            waitingForInput = false;

            if (endDialoguePanel != null)
                endDialoguePanel.SetActive(false);

            if (portalFinal != null)
                portalFinal.SetActive(true);

            if (playerMovement != null)
                playerMovement.canMove = true;
        }
    }

    public void AtualizarPortais()
    {
        if (portalUnix != null)
            portalUnix.SetActive(!GameState.UnixCompleted);

        if (portalBSD != null)
            portalBSD.SetActive(GameState.UnixCompleted && !GameState.BSDCompleted);

        if (portalLinux != null)
            portalLinux.SetActive(GameState.BSDCompleted && !GameState.LinuxCompleted);

        if (portalDebian != null)
            portalDebian.SetActive(GameState.LinuxCompleted && !GameState.DebianCompleted);

        if (portalUbuntu != null)
            portalUbuntu.SetActive(GameState.DebianCompleted && !GameState.UbuntuCompleted);

        if (portalAndroid != null)
            portalAndroid.SetActive(GameState.UbuntuCompleted && !GameState.AndroidCompleted);

        if (portalRedox != null)
            portalRedox.SetActive(GameState.AndroidCompleted && !GameState.RedoxOSCompleted);
            
        if (portalAtualmente != null)
            portalAtualmente.SetActive(GameState.RedoxOSCompleted && !GameState.AtualmenteCompleted);

        if (GameState.AtualmenteCompleted)
        {
            MostrarMensagemFinal();
        }
        else
        {
            if (endDialoguePanel != null) endDialoguePanel.SetActive(false);
            if (portalFinal != null) portalFinal.SetActive(false);
        }        
    }

    void MostrarMensagemFinal()
    {
        if (endDialoguePanel != null)
        {
            endDialoguePanel.SetActive(true);

            if (endDialogueText != null)
                endDialogueText.text = "Parabéns, robô. Você percorreu toda a jornada — do UNIX aos dias atuais — e absorveu cada fragmento de conhecimento guardado pelos portais.\n\nMissão cumprida. Você pode descansar... por enquanto.\n\nMas não relaxe muito. Este laboratório nunca para de evoluir, e novos testes sempre surgem para os mais preparados.\n\nObrigado pelo seu trabalho, robô. O conhecimento que você carrega agora é mais poderoso do que qualquer sistema.";
        }

        if (playerMovement != null)
            playerMovement.canMove = false;

        if (portalFinal != null)
            portalFinal.SetActive(false);

        waitingForInput = true;
    }
}