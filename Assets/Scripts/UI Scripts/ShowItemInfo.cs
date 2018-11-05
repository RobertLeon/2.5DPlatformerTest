//Created by: Robert Bryant
//
//Displays the information about the player's equipped items
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ShowItemInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{ 
    [HideInInspector]
    public string itemInfo;                         //Information the description box shows
    [HideInInspector]
    public object[] infoArguments;                  //

    private GameObject descriptionBox;              //Reference to the description box
    private TMP_Text itemInfoLabel;                 //Reference to the UI text for the description

    //Use this for initialization
    private void Start()
    {
        descriptionBox = GameObject.Find("PlayerInformationCanvas").transform.Find("UIDescription").gameObject;
        itemInfoLabel = descriptionBox.GetComponentInChildren<TMP_Text>();
    }

    private string FormatItemInfo(string info, object[] args)
    {
        string s = info;

        string.Format(s, args);

        return s;
    }


    //On mouse over
    public void OnPointerEnter(PointerEventData pointerEventData)
    {   
        //Show the dscription box and move it over the item's icon
        descriptionBox.SetActive(true);
        descriptionBox.transform.position = transform.position + new Vector3(50, 50, 0);

        //Set the description
        itemInfoLabel.text = FormatItemInfo(itemInfo, infoArguments);        
    }

    //On mouse exit
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //Hide the description box
        descriptionBox.SetActive(false);
    }
}
