using System.Collections;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dialogue : MonoBehaviour
{
    [Header("Display Details")]
    [SerializeField] private Image speakerPortrait;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI dialogueChoices;

    [Header("Typing Details")]
    [SerializeField] private float typingSpeedDelay = 0.01f;

    public void PlayDialogueLine(DialogueLineSO line)
    {
        speakerPortrait.sprite = line.speaker.speakerPortrait;
        speakerName.text = line.speaker.speakerName;
        dialogueText.text = line.GetRandomLine();
    }

    private IEnumerator TypeTextCo(string text)
    {
        dialogueText.text = "";

        foreach (char letter in text)
        {
            dialogueText.text = dialogueText.text + letter;
            yield return new WaitForSeconds(typingSpeedDelay);
        }
    }
}
