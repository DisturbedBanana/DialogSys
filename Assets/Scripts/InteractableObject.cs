using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public void CallDialog()
    {
        GameManager.instance.PlayDialog(gameObject.name, false);
    }
}
