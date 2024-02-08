using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSoundPlayer : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);

        // Add hover event

        if (GetComponent<EventTrigger>() == null)
        {
            gameObject.AddComponent<EventTrigger>();
        }

        if (GetComponent<EventTrigger>().triggers.Count == 0)
        {
            EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => { OnHover(); });
            trigger.triggers.Add(entry);
        }
    }

    public void OnClick()
    {
        EventManager.ButtonClicked(new CustomEventArgs(gameObject));
    }

    public void OnHover()
    {
        EventManager.ButtonHover(new CustomEventArgs(gameObject));
    }
}
