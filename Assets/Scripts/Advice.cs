using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Advice : MonoBehaviour
{
    [SerializeField]
    WarningMessage message;
    [SerializeField]
    DialogTextBox ui;

    public void StartReading()
    {
        ui.gameObject.SetActive(true);
        ui.TextDialog = message.Message;
    }

    public void StopReading()
    {
        ui.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            StartReading();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            StopReading();
        }
    }
}

