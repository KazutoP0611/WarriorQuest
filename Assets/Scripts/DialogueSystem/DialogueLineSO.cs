using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Dialogue Data/New Line Data", fileName = "Line - ")]
public class DialogueLineSO : ScriptableObject
{
    [Header("Dialogue Info")]
    public DialogueSpeakerSO speaker;
    public string dialogueGroupName;

    [Header("Text Options")]
    [TextArea] public string[] textLines;

    [Header("Answer Setups")]
    public bool playerCanAnswer; // if player has choices, it is true;
    public DialogueLineSO[] answerLines;

    public string GetRandomLine()
    {
        return textLines[Random.Range(0, textLines.Length)];
    }
}
