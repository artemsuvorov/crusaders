using UnityEngine;

public class DialogueContainer : MonoBehaviour
{
    [SerializeField]
    private Dialogue startMissionDialogue;

    [SerializeField]
    private Dialogue victoryDialogue;

    [SerializeField]
    private Dialogue defeatDialogue;

    public Dialogue StartMissionDialogue => startMissionDialogue;
    public Dialogue VictoryDialogue => victoryDialogue;
    public Dialogue DefeatDialogue => defeatDialogue;
}
