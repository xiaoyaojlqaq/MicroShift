using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static int index;

    public void Chin()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
    }

    public void En()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
    }
    public static void InitLocatization(string tableName, PlayerControl playerControl, NPCControl nPCControl)
    {
        var resultTable = LocalizationSettings.StringDatabase.GetTable(tableName);
        if (!resultTable)
        {
            Debug.Log("not found table");
            return;
        }
        playerControl.stringEvent.SetTable(tableName);
        nPCControl.stringEvent.SetTable(tableName);
    }
    public static void DialogueShow(string tableName,PlayerControl playerControl,NPCControl nPCControl)
    {
        var resultTable = LocalizationSettings.StringDatabase.GetTable(tableName);
        if (resultTable.GetEntry("player" + index.ToString()) != null)
        {
            Debug.Log("player dialogue");
            playerControl.DialogueImage.SetActive(true);
            nPCControl.DialogueImage.SetActive(false);
            playerControl.stringEvent.SetEntry("player" + index.ToString());
            //string content= LocalizationSettings.StringDatabase.GetTableEntry("tableName", "player" + index.ToString()).Entry.GetLocalizedString();
            Text text = playerControl.DialogueImage.GetComponentInChildren<Text>();
            string content = text.text;
            text.text = null;
            text.DOText(content, 1);
        }
        else if(resultTable.GetEntry("npc" + index.ToString()) != null)
        {
            Debug.Log("npc dialogue");
            playerControl.DialogueImage.SetActive(false);
            nPCControl.DialogueImage.SetActive(true);
            nPCControl.stringEvent.SetEntry("npc" + index.ToString());
            //string content = LocalizationSettings.StringDatabase.GetTableEntry("tableName", "npc" + index.ToString()).Entry.GetLocalizedString();
            Text text = nPCControl.DialogueImage.GetComponentInChildren<Text>();
            string content = text.text;
            text.text = null;
            text.DOText(content, 1);
        }
        else
        {
            Debug.Log("dialogue end");
            playerControl.DialogueImage.SetActive(false);
            nPCControl.DialogueImage.SetActive(false);
            index = 0;
            return;
        }
        index++;
    }
}
