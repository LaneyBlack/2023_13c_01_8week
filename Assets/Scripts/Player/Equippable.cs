using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equippable : MonoBehaviour
{
    [HideInInspector] public bool isEquipped;

    private void Awake()
    {
        isEquipped = false;
    }
}
