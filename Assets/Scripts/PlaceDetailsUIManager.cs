using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mapbox.Unity.MeshGeneration.Factories;
using UnityEngine.UI;

public class PlaceDetailsUIManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject detailsPanel;
    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField] private TextMeshProUGUI AddressText;
    [SerializeField] private TextMeshProUGUI UriText;
    [SerializeField] private TextMeshProUGUI EmailText;
    [SerializeField] private TextMeshProUGUI TelephoneText;

    [SerializeField] private Button uriButton;
    [SerializeField] private Button emailButton;
    [SerializeField] private Button telephoneButton;

    [SerializeField] private SnackBar snackBar;

    public float NamePos;
    public float AddressPos;
    public Vector2 UriPos;
    public Vector2 EmailPos;
    public Vector2 TelephonePos;
    public float PosOffset;

    public GameObject waypoint;
    public WaypointData waypointData;

    public void Start()
    {
        uriButton.onClick.AddListener(GoToWeb);
        emailButton.onClick.AddListener(GoToEmail);
        telephoneButton.onClick.AddListener(CopyTelephone);
        NamePos = NameText.rectTransform.anchoredPosition.y;
        AddressPos = AddressText.rectTransform.anchoredPosition.y;
        UriPos = UriText.transform.parent.GetComponent<RectTransform>().anchoredPosition;
        EmailPos = EmailText.transform.parent.GetComponent<RectTransform>().anchoredPosition;
        TelephonePos = TelephoneText.transform.parent.GetComponent<RectTransform>().anchoredPosition;

        //gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ShowDetails(GameObject waypoint)
    {
        //menuPanel.SetActive(false);

        if (menuPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idlePanel"))
            menuPanel.GetComponent<Animator>().SetTrigger("close");
        menuButton.SetActive(false);
        if (detailsPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idlePanel"))
            detailsPanel.GetComponent<Animator>().SetTrigger("close");
        this.waypoint = waypoint;
        WayPointColliderHandler wayPointColliderHandler = waypoint.transform.GetChild(1).GetComponent<WayPointColliderHandler>();
        waypointData = wayPointColliderHandler.waypointData;
        PlaceTextHolders();
        NameText.text = waypointData.Name;
        AddressText.text = waypointData.Address;
        UriText.text = waypointData.Uri;
        EmailText.text = waypointData.Email;
        TelephoneText.text = waypointData.Telephone;
        //detailsPanel.SetActive(true);
        detailsPanel.GetComponent<Animator>().SetTrigger("open");
    }

    private void PlaceTextHolders()
    {
        if (waypointData.Name.Length > 15)
            NameText.fontSize = 20;
        else if (waypointData.Name.Length > 11)
            NameText.fontSize = 25;
        else
            NameText.fontSize = 30;


        if (waypointData.Uri == "" || waypointData.Uri == null)
            UriText.transform.parent.gameObject.SetActive(false);
        else
            UriText.transform.parent.gameObject.SetActive(true);
        if (waypointData.Email == "" || waypointData.Email == null)
            EmailText.transform.parent.gameObject.SetActive(false);
        else
            EmailText.transform.parent.gameObject.SetActive(true);
        if (waypointData.Telephone == "" || waypointData.Telephone == null)
            TelephoneText.transform.parent.gameObject.SetActive(false);
        else
            TelephoneText.transform.parent.gameObject.SetActive(true);

        UriText.transform.parent.GetComponent<RelativePosition>().UpdatePosition();
        EmailText.transform.parent.GetComponent<RelativePosition>().UpdatePosition();
        TelephoneText.transform.parent.GetComponent<RelativePosition>().UpdatePosition();
    }

    public void HideDetails()
    {
        //detailsPanel.SetActive(false);
        detailsPanel.GetComponent<Animator>().SetTrigger("close");
        menuButton.SetActive(true);
    }

    public void SetWaypointInDirecction()
    {
        if(waypoint!=null)
            GameObject.Find("Directions").GetComponent<DirectionsManager>().changeWaypoint(1,waypoint.transform);
    }

    public void GoToWeb()
    {
        if (waypointData != null)
            Application.OpenURL(waypointData.Uri);
    }

    public void GoToEmail()
    {
        if (waypointData != null)
            Application.OpenURL("mailto:" + waypointData.Email);
    }

    public void CopyUri()
    {
        GUIUtility.systemCopyBuffer = waypointData.Uri;
        snackBar.ShowMessage("Uri copiada en el portapales",2);
    }

    public void CopyEmail()
    {
        GUIUtility.systemCopyBuffer = waypointData.Email;
        snackBar.ShowMessage("Email copiado en el portapales", 2);
    }

    public void CopyTelephone()
    {
        GUIUtility.systemCopyBuffer = waypointData.Telephone;
        snackBar.ShowMessage("Teléfono copiado en el portapales", 2);
    }
}
