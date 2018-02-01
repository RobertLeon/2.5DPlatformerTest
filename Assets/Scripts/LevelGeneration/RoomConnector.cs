using UnityEngine;

public class RoomConnector: MonoBehaviour
{
    public string[] exits;          //Type of rooms the exit can lead to.
    public bool isDeadEnd;          //Is this room a dead end?

    private void OnDrawGizmos()
    {
        float gizmoScale = .5f;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.125f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - transform.right * gizmoScale);
        Gizmos.DrawLine(transform.position, transform.position + transform.right * gizmoScale);
        Gizmos.DrawLine(transform.position, transform.position - transform.up * gizmoScale);
        Gizmos.DrawLine(transform.position, transform.position + transform.up * gizmoScale);
    }
}