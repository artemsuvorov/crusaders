using UnityEngine;

public class DialogueContainer : MonoBehaviour
{
    [SerializeField]
    private Dialogue startMissionDialogue;

    public Dialogue StartMissionDialogue => startMissionDialogue;
}
