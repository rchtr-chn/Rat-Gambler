using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManagerScript : MonoBehaviour
{
    private AudioManagerScript audioManagerScript;

    public bool muteAudio = false;

    private void Start()
    {
        audioManagerScript = GameManagerScript.instance.AudioManagerScript;
    }
}
