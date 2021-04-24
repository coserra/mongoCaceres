using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;

public class InterestPointsOnMap : MonoBehaviour
{

	[SerializeField]
	AbstractMap _map;
	[SerializeField] DirectionsManager directionsManager;

	List<WaypointData> _waypointList;
	List<Vector2d> _locations;

	[SerializeField]
	float _spawnScale;


	[SerializeField] GameObject usuario;
	[SerializeField] GameObject alojamientoPrefab;
    [SerializeField] GameObject barCopasPrefab;
	[SerializeField] GameObject cafebarPrefab;
    [SerializeField] GameObject centroTuristicoPrefab;
    [SerializeField] GameObject monumentoPrefab;
	[SerializeField] GameObject restaurantePrefab;

	List<GameObject> _spawnedObjects;

	private int imageNumber=0;


	void Start()
	{
		_waypointList = new List<WaypointData>();
		_locations = new List<Vector2d>();
		_spawnedObjects = new List<GameObject>();

		CreateUser();
		TestWaypointCreation();
		_map.OnUpdated += UpdateMap;
	}

    private void OnDestroy()
    {
		_map.OnUpdated -= UpdateMap;
    }

    public void UpdateMap()
    {
		int count = _spawnedObjects.Count;
		for (int i = 0; i < count; i++)
		{
			var spawnedObject = _spawnedObjects[i];
			var location = _locations[i];
			spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
			spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);

		}
	}

    public void addElement(Transform item)
    {

    }

	public void newPlace(string position, WaypointData.TypeOfPlace type, string name, string uri)
    {
		WaypointData waypoint = new WaypointData(position, type, name, uri);
		_waypointList.Add(waypoint);
		_locations.Add(Conversions.StringToLatLon(position));
		GameObject instance=null;
        switch (type)
        {
			case WaypointData.TypeOfPlace.alojamiento:
				instance = Instantiate(alojamientoPrefab);
				break;
			case WaypointData.TypeOfPlace.barCopas:
				instance = Instantiate(barCopasPrefab);
				break;
			case WaypointData.TypeOfPlace.cafebar:
				instance = Instantiate(cafebarPrefab);
				break;
			case WaypointData.TypeOfPlace.centroTuristico:
				instance = Instantiate(centroTuristicoPrefab);
				break;
			case WaypointData.TypeOfPlace.monumento:
				instance = Instantiate(monumentoPrefab);
				break;
			case WaypointData.TypeOfPlace.restaurante:
				instance = Instantiate(restaurantePrefab);
				break;
		}
		instance.transform.GetChild(1).GetComponent<WayPointColliderHandler>().waypointData = waypoint;
		instance.transform.GetChild(0).GetComponent<TextMesh>().text = name;
		instance.transform.localPosition = _map.GeoToWorldPosition(_locations[_locations.Count-1], true);
		instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
		_spawnedObjects.Add(instance);
	}

	private void CreateUser()
    {
		Vector2d vectorPosition= _map.WorldToGeoPosition(new Vector3(0, 1, 0));
		string position = vectorPosition.x + "," + vectorPosition.y;
		Debug.Log("Posición inicial" + position);
		WaypointData waypoint = new WaypointData(position, WaypointData.TypeOfPlace.usuario, "usuario", "");
		_waypointList.Add(waypoint);
		_locations.Add(vectorPosition);
		
		//usuario.transform.GetChild(1).GetComponent<WayPointColliderHandler>().waypointData = waypoint;
		usuario.transform.GetChild(0).GetComponent<TextMesh>().text = "";
		usuario.transform.localPosition = _map.GeoToWorldPosition(_locations[_locations.Count - 1], true);
		usuario.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
		_spawnedObjects.Add(usuario);
		directionsManager.changeWaypoint(0,usuario.transform);
	}

	public void ShowOrHideUser()
    {
		_spawnedObjects[0].SetActive(!_spawnedObjects[0].activeSelf);

		directionsManager.ShowDirection(_spawnedObjects[0].activeSelf);
    }

	public void UpdateUserPosition()
    {
		_locations[0] = _map.WorldToGeoPosition(_spawnedObjects[0].transform.position);
	}


	public void TestWaypointCreation()
    {
		newPlace("39.47306, -6.37132", WaypointData.TypeOfPlace.alojamiento, "Carlos", "www");
		newPlace("39.47306, -6.37248", WaypointData.TypeOfPlace.cafebar, "Rincon", "www");
		newPlace("39.47308, -6.37412", WaypointData.TypeOfPlace.restaurante, "Sitio", "www");
		newPlace("39.47300, -6.37022", WaypointData.TypeOfPlace.barCopas, "Bua", "www");

		//GameObject.Find("Details").GetComponent<PlaceDetailsUIManager>().ShowDetails(_spawnedObjects[0]);
	}

	public void ChangeMapImage()
    {
        switch (imageNumber)
        {
			case 0:
				_map.ImageLayer.SetLayerSource(ImagerySourceType.MapboxStreets);
				imageNumber++;
				break;
			case 1:
				_map.ImageLayer.SetLayerSource(ImagerySourceType.MapboxSatelliteStreet);
				imageNumber++;
				break;
			case 2:
				_map.ImageLayer.SetLayerSource(ImagerySourceType.MapboxSatellite);
				imageNumber = 0;
				break;
		}
		
    }
}