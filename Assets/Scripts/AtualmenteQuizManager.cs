using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AtualmenteQuizManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string question;
        public string correctAnswer;
        public List<string> wrongAnswers;
    }

    public List<Question> questions;

    [HideInInspector] public int currentIndex = 0;
    [HideInInspector] public int totalErrors = 0;
    public int maxErrors = 3;

    public TextMeshProUGUI questionText;
    public Button[] optionButtons;

    public GameObject attemptsPanel;
    public Image[] attemptIcons;

    public GameObject successPanel;
    public GameObject failurePanel;

    void Start()
    {
        HideAll();
    }

    void HideAll()
    {
        if (successPanel != null) successPanel.SetActive(false);
        if (failurePanel != null) failurePanel.SetActive(false);
    }

    public void StartQuiz()
    {
        currentIndex = 0;
        totalErrors = 0;

        foreach (var btn in optionButtons)
            btn.gameObject.SetActive(true);

        if (questionText != null)
            questionText.text = "";

        if (successPanel != null)
            successPanel.SetActive(false);

        if (failurePanel != null)
            failurePanel.SetActive(false);

        UpdateAttemptIcons();
        ShowQuestion();
    }

    void ShowQuestion()
    {
        if (currentIndex >= questions.Count)
        {
            OnQuizSuccess();
            return;
        }

        var q = questions[currentIndex];
        questionText.text = q.question;

        var options = new List<string>(q.wrongAnswers);
        options.Add(q.correctAnswer);

        for (int i = 0; i < optionButtons.Length; i++)
        {
            int pick = Random.Range(0, options.Count);
            string text = options[pick];
            options.RemoveAt(pick);

            var btn = optionButtons[i];
            btn.GetComponentInChildren<TextMeshProUGUI>().text = text;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => OnAnswerSelected(text));
        }
    }

    void OnAnswerSelected(string selected)
    {
        if (currentIndex >= questions.Count)
            return;

        var q = questions[currentIndex];

        if (selected == q.correctAnswer)
        {
            currentIndex++;

            if (currentIndex >= questions.Count)
            {
                OnQuizSuccess();
                return;
            }

            ShowQuestion();
        }
        else
        {
            totalErrors++;
            UpdateAttemptIcons();

            if (totalErrors >= maxErrors)
                OnQuizFail();
        }
    }

    void UpdateAttemptIcons()
    {
        for (int i = 0; i < attemptIcons.Length; i++)
            attemptIcons[i].enabled = (i < totalErrors && totalErrors > 0);
    }

    void OnQuizSuccess()
    {

        foreach (var btn in optionButtons)
            btn.gameObject.SetActive(false);

        if (questionText != null)
            questionText.text = "";

        if (successPanel != null)
            successPanel.SetActive(true);

        currentIndex = questions.Count;

        NotifyNPCQuizComplete();
    }

    void NotifyNPCQuizComplete()
    {
        var npc = FindObjectOfType<AtualmenteNPCQuiz>();
        if (npc != null)
        {
            npc.OnQuizSucceeded();
        }
    }

    void NotifyNPCQuizFailed()
    {
        var npc = FindObjectOfType<AtualmenteNPCQuiz>();
        if (npc != null)
            npc.OnQuizFailed();
    }

    void OnQuizFail()
    {
        totalErrors = maxErrors;
        UpdateAttemptIcons();

        if (questionText != null)
            questionText.text = "";

        foreach (var btn in optionButtons)
            btn.gameObject.SetActive(false);

        if (failurePanel != null)
            failurePanel.SetActive(true);

        NotifyNPCQuizFailed();
    }
}
