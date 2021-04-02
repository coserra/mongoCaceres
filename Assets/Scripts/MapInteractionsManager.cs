using Mapbox.Examples;
using Mapbox.Unity.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInteractionsManager : MonoBehaviour
{
    [SerializeField] private AbstractMap map;

    public void enableMapMovement(bool enable)
    {
        map.GetComponent<QuadTreeCameraMovement>().enabled = enable;
    }
}
