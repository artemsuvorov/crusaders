using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class Dialogue : IEnumerable<string>
{
    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private string name;

    [SerializeField, TextArea(3, 10)]
    private string[] sentences;

    public Sprite Sprite => sprite;
    public string Name => name;
    public string[] Sentences => sentences;

    public IEnumerator<string> GetEnumerator()
    {
        return Sentences.AsEnumerable().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Sentences.GetEnumerator();
    }
}

public class DialogueController : MonoBehaviour
{
    private readonly Queue<string> sentences = new();

    [SerializeField]
    private Text dialogueNpcName, dialogueTextField;

    [SerializeField]
    private GameObject dialogueNpcImage;

    public event UnityAction DialogueStarted;
    public event UnityAction DialogueEnded;

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();

        foreach (var sentence in dialogue)
            sentences.Enqueue(sentence);

        DialogueStarted?.Invoke();

        dialogueNpcImage.SetActive(true);
        var image = dialogueNpcImage.GetComponent<Image>();
        image.sprite = dialogue.Sprite;
        image.type = Image.Type.Simple;
        dialogueNpcName.text = dialogue.Name;

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        var sentence = sentences.Dequeue();
        dialogueTextField.text = sentence;
    }

    private void EndDialogue()
    {
        DialogueEnded?.Invoke();
    }
}
