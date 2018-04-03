//Created by Robert Bryant
//
//Gizmos to help visualize in the Scene View
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    public Color gizmoColor;            //Color of the gizmo

    private void OnDrawGizmos()
    {
        //Alpha color
        gizmoColor.a = 0.75f;       
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position,transform.localScale);
    }
}