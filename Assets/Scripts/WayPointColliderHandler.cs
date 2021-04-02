using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointColliderHandler : MonoBehaviour
{
    public WaypointData waypointData { set; get; }
    [SerializeField] private PlaceDetailsUIManager detailsPanel;
    // Start is called before the first frame update
    private void Start()
    {
        detailsPanel = GameObject.Find("Details").GetComponent<PlaceDetailsUIManager>();
    }
    private void OnMouseDown()
    {
        detailsPanel.ShowDetails(transform.parent.gameObject);
    }
}
