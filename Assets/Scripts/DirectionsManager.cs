

using UnityEngine;
using Mapbox.Directions;
using System.Collections.Generic;
using System.Linq;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Modifiers;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Utils;
using Mapbox.Unity.Utilities;
using System.Collections;
using Mapbox.Unity;

public class DirectionsManager : MonoBehaviour
{
    [SerializeField]
    AbstractMap _map;

    [SerializeField]
    MeshModifier[] MeshModifiers;
    [SerializeField]
    Material _material;

    [SerializeField]
    List<Transform> _waypoints;
    [SerializeField] private List<Vector3> _cachedWaypoints;

    [SerializeField]
    [Range(1, 10)]
    private float UpdateFrequency = 2;



    private Directions _directions;
    private int _counter;

    GameObject _directionsGO;
    private bool _recalculateNext;
    private float lastMapZoom;
    private DirectionsResponse lastDirectionsResponse;

    public RoutingProfile routingProfile { set; get; }

    protected virtual void Awake()
    {
        routingProfile = RoutingProfile.Walking;
        if (_map == null)
        {
            _map = FindObjectOfType<AbstractMap>();
        }
        _directions = MapboxAccess.Instance.Directions;
        _map.OnInitialized += Query;
        _map.OnUpdated += UpdateDirections;
    }

    public void Start()
    {
        _cachedWaypoints = new List<Vector3>(_waypoints.Count);
        foreach (var item in _waypoints)
        {
            if (item != null)
                _cachedWaypoints.Add(item.position);
        }
        _recalculateNext = false;

        foreach (var modifier in MeshModifiers)
        {
            modifier.Initialize();
        }
        lastMapZoom = _map.Zoom;
        //StartCoroutine(QueryTimer());
    }

    protected virtual void OnDestroy()
    {
        _map.OnInitialized -= Query;
        _map.OnUpdated -= UpdateDirections;
    }

    public void Query()
    {
        Debug.Log("Llamada a query");
        var count = _waypoints.Count;
        var wp = new Vector2d[count];
        for (int i = 0; i < count; i++)
        {
            wp[i] = _waypoints[i].GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
        }
        var _directionResource = new DirectionResource(wp, routingProfile);
        _directionResource.Steps = true;
        _directions.Query(_directionResource, HandleDirectionsResponse);
    }


    void HandleDirectionsResponse(DirectionsResponse response)
    {
        Debug.Log("Llamada a handle");
        lastDirectionsResponse = response;
        if (response == null || null == response.Routes || response.Routes.Count < 1)
        {
            return;
        }

        var meshData = new MeshData();
        var dat = new List<Vector3>();
        foreach (var point in response.Routes[0].Geometry)
        {
            dat.Add(Conversions.GeoToWorldPosition(point.x, point.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz());
        }

        var feat = new VectorFeatureUnity();
        feat.Points.Add(dat);

        foreach (MeshModifier mod in MeshModifiers.Where(x => x.Active))
        {
            mod.Run(feat, meshData, _map.WorldRelativeScale);
        }

        CreateGameObject(meshData);
    }

    void ProcessLastResponse()
    {
        Debug.Log("Llamada a handle");
        if (lastDirectionsResponse == null || null == lastDirectionsResponse.Routes || lastDirectionsResponse.Routes.Count < 1)
        {
            return;
        }

        var meshData = new MeshData();
        var dat = new List<Vector3>();
        foreach (var point in lastDirectionsResponse.Routes[0].Geometry)
        {
            dat.Add(Conversions.GeoToWorldPosition(point.x, point.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz());
        }

        var feat = new VectorFeatureUnity();
        feat.Points.Add(dat);

        foreach (MeshModifier mod in MeshModifiers.Where(x => x.Active))
        {
            mod.Run(feat, meshData, _map.WorldRelativeScale);
        }

        CreateGameObject(meshData);
    }

    GameObject CreateGameObject(MeshData data)
    {
        Debug.Log("Llamada a create");
        if (_directionsGO != null)
        {
            _directionsGO.Destroy();
        }
        _directionsGO = new GameObject("direction waypoint " + " entity");
        var mesh = _directionsGO.AddComponent<MeshFilter>().mesh;
        mesh.subMeshCount = data.Triangles.Count;

        mesh.SetVertices(data.Vertices);
        _counter = data.Triangles.Count;
        for (int i = 0; i < _counter; i++)
        {
            var triangle = data.Triangles[i];
            mesh.SetTriangles(triangle, i);
        }

        _counter = data.UV.Count;
        for (int i = 0; i < _counter; i++)
        {
            var uv = data.UV[i];
            mesh.SetUVs(i, uv);
        }

        mesh.RecalculateNormals();
        _directionsGO.AddComponent<MeshRenderer>().material = _material;
        return _directionsGO;
    }

    public void changeWaypoint(int index, Transform waypointTransform)
    {
        if (index >= 0 && index < _waypoints.Count)
        {
            _waypoints[index] = waypointTransform;
            Query();
            DecimalZoom();
        }

    }

    public void addWaypoint(Transform waypoint)
    {
        _waypoints.Add(waypoint);

    }

    //public void moveDirection()
    //{
    //    if (_directionsGO != null)
    //    {
    //        _directionsGO.transform.position = _waypoints[0].position+offset;
    //    }
    //}

    public void UpdateDirections()
    {
        ProcessLastResponse();
        DecimalZoom();
     
    }

    public void DecimalZoom()
    {
        if(_directionsGO!=null)
            _directionsGO.transform.localScale = Vector3.one * (_map.Zoom%1+1);
    }

    public void ShowDirection(bool show)
    {
        _directionsGO.SetActive(show);
    }

}

