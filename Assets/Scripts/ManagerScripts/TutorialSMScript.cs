using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSMScript : MonoBehaviour
{
    public GameObject _fireBallIntstructions;
    public GameObject _lightningBlastInstructions;
    // Start is called before the first frame update
    void Start()
    {
        if(PPrefsList.GetBool(PPrefsList.DefeatedFire, true))
            _fireBallIntstructions.GetComponent<SpriteRenderer>().color = Color.white;
        else
            _fireBallIntstructions.GetComponent<SpriteRenderer>().color = Color.clear;

        if(PPrefsList.GetBool(PPrefsList.DefeatedLightning, true))
            _lightningBlastInstructions.GetComponent<SpriteRenderer>().color = Color.white;
        else
            _lightningBlastInstructions.GetComponent<SpriteRenderer>().color = Color.clear;
    }
}
