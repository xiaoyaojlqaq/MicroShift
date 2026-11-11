using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager intance;
    public GameObject chooseCanvas;
    public int index;
    public bool isInitLocatization;
    public int chooseNumb;
    public StringTable resultTable;

    Action buttonAction;
    private void Awake()
    {
        if (intance == null)
        {
            intance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void Chin()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
    }

    public void En()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
    }

    public void SetChooseNum(int num)
    {
        chooseNumb = num;
        buttonAction();
        buttonAction();
    }

    public void InitLocatization(string tableName, PlayerControl playerControl, NPCControl nPCControl)
    {
        Debug.Log("InitLocatization");
        chooseNumb = 0;
        index = 0;
        resultTable = LocalizationSettings.StringDatabase.GetTable(tableName);
        if (!resultTable)
        {
            Debug.Log("not found table");
            return;
        }
        playerControl.stringEvent.SetTable(tableName);
        nPCControl.stringEvent.SetTable(tableName);
    }
    
    public void DialogueShow(DialogueInfo dialogueInfo, PlayerControl playerControl, NPCControl nPCControl)
    {
        if (!isInitLocatization)
        {
            isInitLocatization = true;
            InitLocatization(dialogueInfo.tableName, playerControl, nPCControl);
            buttonAction = () =>
            {
                DialogueShow(dialogueInfo, playerControl, nPCControl);
            };
        }
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
            index++;
        }
        else if (resultTable.GetEntry("npc" + index.ToString()) != null)
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
            index++;
        }
        else if (resultTable.GetEntry("choose" + index.ToString()) != null)
        {
            Debug.Log("Enter choose");

            chooseCanvas.gameObject.SetActive(true);
            if (chooseNumb == 1)
            {
                InitLocatization(dialogueInfo.choose1Table, playerControl, nPCControl);
                Debug.Log("InitLocatization1");
            }
            else if (chooseNumb == 2)
            {
                InitLocatization(dialogueInfo.choose2Table, playerControl, nPCControl);
                Debug.Log("InitLocatization2");
            }
            else return;
            playerControl.DialogueImage.GetComponentInChildren<Text>().text = null;
            nPCControl.DialogueImage.GetComponentInChildren<Text>().text = null;
        }
        else
        {
            Debug.Log("dialogue end");
            playerControl.DialogueImage.SetActive(false);
            nPCControl.DialogueImage.SetActive(false);
            index = 0;
            isInitLocatization = false;
            return;
        }
    }
}
