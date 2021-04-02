using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{

    [SerializeField] private GameObject detailsPanel;
    [SerializeField] private GameObject menuButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMenu()
    {
        detailsPanel.SetActive(false);
        menuButton.SetActive(false);
        gameObject.SetActive(true);
    }

    public void HideMenu()
    {
        gameObject.SetActive(false);
        menuButton.SetActive(true);
    }
}
