using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mapbox.Unity.MeshGeneration.Factories;

public class PlaceDetailsUIManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject detailsPanel;
    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField] private TextMeshProUGUI UriText;

    public GameObject waypoint;
    
    public void ShowDetails(GameObject waypoint)
    {
        menuPanel.SetActive(false);
        menuButton.SetActive(false);
        this.waypoint = waypoint;
        WayPointColliderHandler wayPointColliderHandler = waypoint.transform.GetChild(1).GetComponent<WayPointColliderHandler>();
        NameText.text = wayPointColliderHandler.waypointData.name;
        UriText.text = wayPointColliderHandler.waypointData.uri;
        detailsPanel.SetActive(true);
    }

    public void HideDetails()
    {
        detailsPanel.SetActive(false);
        menuButton.SetActive(true);
    }

    public void SetWaypointInDirecction()
    {
        GameObject.Find("Directions").GetComponent<DirectionsManager>().changeWaypoint(1,waypoint.transform);
    }
}
