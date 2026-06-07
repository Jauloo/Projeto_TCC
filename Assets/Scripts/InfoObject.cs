using UnityEngine;
using TMPro;

public class InfoObject : MonoBehaviour
{
    [Header("UI References")]
    public GameObject infoPanel;           
    public TextMeshProUGUI infoText;       
    [TextArea(2, 5)]
    public string message;                 

    [Header("Visual Elements")]
    public GameObject interactIcon;        

    private bool playerInRange = false;
    private bool isShowing = false;
    private PlayerMovement playerMovement;

    void Start()
    {
        if (infoPanel != null)
            infoPanel.SetActive(false);

        if (interactIcon != null)
            interactIcon.SetActive(false);

        var player = GameObject.FindWithTag("Player");
        if (player != null)
            playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isShowing)
                ShowInfo();
            else
                HideInfo();
        }
    }

    void ShowInfo()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(true);
            infoText.text = message;
            isShowing = true;

            if (playerMovement != null)
                playerMovement.canMove = false;
        }
    }

    void HideInfo()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
            isShowing = false;

            if (playerMovement != null)
                playerMovement.canMove = true;
        }
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
            if (infoPanel != null)
                infoPanel.SetActive(false);
            isShowing = false;

            if (playerMovement != null)
                playerMovement.canMove = true;
        }
    }
}
