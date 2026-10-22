using UnityEngine;
using System;

public class PlayerUsernameGet : MonoBehaviour
{
    private string playerUsername;
    public DialogueSO[] dialogues;
    public ObjectInteract objectInteract;
    public ActorSO player;

    void Start()
    {
        playerUsername = Environment.UserName;
        player.actorName = playerUsername;
        dialogues[0].lines[2].text = $"At least I kill for a reason, {playerUsername}.";
        dialogues[1].lines[3].text = $"Ah, but for you see, my head would not be so different to yours, {playerUsername}, for our minds are more alike than you may believe.";
        dialogues[2].lines[1].text = $"Who is asking this question, the mind you pilot, or you, {playerUsername}? For you see, the screen in front of you is only so thick.";
        dialogues[3].lines[1].text = $"Oh dear player, you are not evil, you are only clicking buttons, are you not? Unless, of course, you enjoyed killing them, {playerUsername}.";
        objectInteract.textsDay4[0] = $"Come and find me, {playerUsername}";

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
