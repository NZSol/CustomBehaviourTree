using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLevels : BTCoreNode
{
    string playerName;
    float audioLimit;
    GameObject currentAudioEmitter;
    GameObject player;
    public AudioLevels(string playerName, float audioLimit, GameObject player)
    {
        this.playerName = playerName;
        this.audioLimit = audioLimit;
        this.player = player;
    }

    void checkForAudio()
    {

    }


    public override NodeState Evaluate()
    {
        checkForAudio();

        if (currentAudioEmitter != null || currentAudioEmitter != player)
        {
            if(currentAudioEmitter.GetComponent<AudioOutput>().volume > player.GetComponent<AudioOutput>().volume)
            {
                currentAudioEmitter = player;
                return NodeState.SUCCESS;
            }
            else
            {
                return NodeState.FAILURE;
            }
        }
        else
        {
            if(player.GetComponent<AudioOutput>().volume > audioLimit)
            {
                currentAudioEmitter = player;
                return NodeState.SUCCESS;
            }
        }
        return NodeState.FAILURE;
    }
}
