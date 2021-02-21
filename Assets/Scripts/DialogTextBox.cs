using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogTextBox : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI dialog;

    public string TextDialog
    {
        set =>dialog.text = value;
    }
}
