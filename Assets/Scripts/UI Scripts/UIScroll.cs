//Created by Robert Bryant
//
//Handles the scrolling of certain UI menus
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIScroll : MonoBehaviour
{
    public Button backButton;                   //Reference to the back button in the menu

    private float selectedPos;                  //Position of the specified item
    private float prevSelectedPos;              //Position of the previous item
    private ScrollRect scrollRect;              //Reference to the Scroll Rect object
    private GameObject selectedItem;            //The currently selected UI item in the scene
    private GameObject lastSelectedItem;        //The previously selected UI item in the scene 

	// Use this for initialization
	void Start ()
    {
        scrollRect = GetComponent<ScrollRect>();
	}

    // Update is called once per frame
    void Update()
    {
        //Get the current selected item in the menu
        selectedItem = EventSystem.current.currentSelectedGameObject;

        //Return if there is no item currently selected
        if(selectedItem == null)
        {
            return;
        }

        //Return if there is no change in the currently selected item
        if(selectedItem == lastSelectedItem)
        {
            return;
        }

        //Set the position of the current selected item based on the item's parent
        if (selectedItem.transform.parent != scrollRect.content)
        {
            selectedPos = 1f - ((float)selectedItem.transform.parent.GetSiblingIndex()
                / scrollRect.content.childCount);
        }
        else
        {
            selectedPos = 1f - ((float)selectedItem.transform.GetSiblingIndex()
                / scrollRect.content.childCount);
        }

        //Force the position to the bottom if the position is small enough
        if (selectedPos < 0.1f)
        {
            selectedPos = 0f;
        }

        //Keep the scroll view at the bottom
        if (selectedItem.transform == backButton.transform)
        {
            selectedPos = 0f; ;
        }

        //Set the scroll position
        scrollRect.verticalNormalizedPosition = selectedPos;
        
        //Set last item to the selected item
        lastSelectedItem = selectedItem;
        
	}
}
