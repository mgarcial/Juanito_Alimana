using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementLimiter : MonoBehaviour
{
    [SerializeField] bool _initialCharacterCanMove = true;
    public bool CharacterCanMove;

    private void Start()
    {
        CharacterCanMove = _initialCharacterCanMove;
    }
}
