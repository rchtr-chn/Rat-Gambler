using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManagerScript : MonoBehaviour
{
    private AudioManagerScript _audioManagerScript;

    public bool muteAudio = false;

    private void Start()
    {
        _audioManagerScript = GameManagerScript.Instance.AudioManagerScript;
    }
}
