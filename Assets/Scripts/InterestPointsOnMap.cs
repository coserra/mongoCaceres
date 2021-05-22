using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;
using System.Collections;

public class InterestPointsOnMap : MonoBehaviour
{

	[SerializeField]
	AbstractMap _map;
	[SerializeField] DirectionsManager directionsManager;

	List<WaypointData> _waypointList;
	List<Vector2d> _locations;

	[SerializeField]
	float _spawnScale;


	[SerializeField] GameObject user;
	WaypointData userWayPointData;
	Vector2d userLocation;

	[SerializeField] GameObject alojamientoPrefab;
    [SerializeField] GameObject barCopasPrefab;
	[SerializeField] GameObject cafebarPrefab;
    [SerializeField] GameObject centroTuristicoPrefab;
    [SerializeField] GameObject monumentoPrefab;
	[SerializeField] GameObject restaurantePrefab;

	List<GameObject> _spawnedObjects;

	private float distanceFilter;
	private bool[] typeFilter;

	private int imageNumber=0;


	void Start()
	{
		typeFilter = new bool[6];
		_waypointList = new List<WaypointData>();
		_locations = new List<Vector2d>();
		_spawnedObjects = new List<GameObject>();

		CreateUser();
		//TestWaypointCreation();
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
		user.transform.localPosition= _map.GeoToWorldPosition(userLocation, true);
		user.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
	}

	public void newPlace(string position, WaypointData.TypeOfPlace type, string name, string direction, string uri, string email, string telephone)
    {
		WaypointData waypoint = new WaypointData(position, type, name, direction, uri,email,telephone);
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

	public void newPlace(WaypointData waypoint)
	{
		
		_waypointList.Add(waypoint);
		_locations.Add(Conversions.StringToLatLon(waypoint.Coordinates));
		GameObject instance = null;
		switch (waypoint.Type)
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
		instance.transform.GetChild(0).GetComponent<TextMesh>().text = waypoint.Name;
		instance.transform.localPosition = _map.GeoToWorldPosition(_locations[_locations.Count - 1], true);
		instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
		_spawnedObjects.Add(instance);
	}

	public void NewWaypoints(List<WaypointData> wayPointDataList)
    {
		foreach(WaypointData wp in wayPointDataList)
        {
			StartCoroutine(CreateWayPointDelayed(wp));
        }
    }

	IEnumerator CreateWayPointDelayed(WaypointData wp)
    {
		yield return new WaitForSeconds(Random.Range(0, 0.5f));
		newPlace(wp);
	}

	public void ClearWaypoints()
    {
		_locations.Clear();
		_waypointList.Clear();
		foreach(GameObject obj in _spawnedObjects)
        {
			obj.Destroy();
        }
		_spawnedObjects.Clear();
		directionsManager.changeWaypoint(1, user.transform);
	}

	private void CreateUser()
    {
		Vector2d vectorPosition= _map.WorldToGeoPosition(new Vector3(0, 1, 0));
		string position = vectorPosition.x + "," + vectorPosition.y;
		Debug.Log("Posición inicial" + position);
		WaypointData waypoint = new WaypointData(position, WaypointData.TypeOfPlace.usuario, "usuario","", "","","");
		userWayPointData = waypoint;
		userLocation = vectorPosition;
		
		//usuario.transform.GetChild(1).GetComponent<WayPointColliderHandler>().waypointData = waypoint;
		user.transform.GetChild(0).GetComponent<TextMesh>().text = "";
		user.transform.localPosition = _map.GeoToWorldPosition(userLocation, true);
		user.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
		directionsManager.changeWaypoint(0,user.transform);
	}

	public void ShowOrHideUser()
    {
		user.SetActive(!user.activeSelf);

		directionsManager.ShowDirection(user.activeSelf);

		if(!user.activeSelf)
			directionsManager.changeWaypoint(1, user.transform);
	}

	public void UpdateUserPosition()
    {
		userLocation = _map.WorldToGeoPosition(user.transform.position);
	}

	public void ChangeDistanceFilter(string distance)
    {
		distanceFilter = float.Parse(distance);
    }

	public void ActiveTypeFilter(int type)
    {
		typeFilter[type] = !typeFilter[type];
		//if (!typeFilter[type])
        //{
		//	typeFilter[type] = true;
        //}
        //else
        //{
		//	typeFilter[type] = false;
        //}
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

    #region mongo
	public void ShowPlacesByType()
    {
		List<WaypointData> waypointDataList=new List<WaypointData>();
		for (int i = 0; i < typeFilter.Length; i++)
        {
			if (typeFilter[i])
			{
				waypointDataList.AddRange(ImaginaryDAO(WaypointData.typeOfPlaces[i], userWayPointData.Coordinates, distanceFilter));
			}
        }
		
		ClearWaypoints();
		NewWaypoints(waypointDataList);
	}

	public void ShowPlacesByName(string name)
    {
		Debug.Log(name);
		List<WaypointData> waypointDataList = ImaginaryDAO(name);
		ClearWaypoints();
		NewWaypoints(waypointDataList);
	}

	public void ShowNearPlaces(string position)
    {
		List<WaypointData> waypointDataList = new List<WaypointData>();
		for (int i = 0; i < typeFilter.Length; i++)
		{
			if (typeFilter[i])
			{
				waypointDataList.AddRange(ImaginaryDAOnear(WaypointData.typeOfPlaces[i], userWayPointData.Coordinates));
			}
		}
		ClearWaypoints();
		NewWaypoints(waypointDataList);
    }

	#endregion


	public List<WaypointData> ImaginaryDAO(WaypointData.TypeOfPlace type, string from, float radius)
    {
		Debug.Log("Usuario " + from + ", radio " + radius + ", "+type.ToString());
		List<WaypointData> waypointDataList = new List<WaypointData>();
		WaypointData wp1 = new WaypointData("39.47343, -6.37132", WaypointData.TypeOfPlace.alojamiento, "Albergue Municipal de Caceres", "Avda. de la Universidad s/n", "http://www.alberguecaceres.es/", "info@alberguecaceres.com", "927102 001");
		WaypointData wp2 = new WaypointData("39.47280, -6.37248", WaypointData.TypeOfPlace.cafebar, "Adarve", "Calle Maestro Sánchez Garrido, 4 ", "", "cafeadarve@gmail.com", "+34 927248064");
		WaypointData wp3 = new WaypointData("39.47332, -6.37412", WaypointData.TypeOfPlace.restaurante, "Botein", "Calle Madre Isabel de Larrañaga, s/n", "http://www.botein.es", "restaurante@botein.es", "+34 927240840");
		WaypointData wp4 = new WaypointData("39.47300, -6.37022", WaypointData.TypeOfPlace.barCopas, "Farmacia Legend", "Plaza Mayor, 20", "", "laminervatapas@gmail.com", "+34 927261052");
		waypointDataList.Add(wp1);
		waypointDataList.Add(wp2);
		waypointDataList.Add(wp3);
		waypointDataList.Add(wp4);

		return waypointDataList;
	}

	public List<WaypointData> ImaginaryDAO(string name)
	{
		Debug.Log(name);
		List<WaypointData> waypointDataList = new List<WaypointData>();
		WaypointData wp1 = new WaypointData("39.47343, -6.37132", WaypointData.TypeOfPlace.alojamiento, "Albergue Municipal de Caceres", "Avda. de la Universidad s/n", "http://www.alberguecaceres.es/", "info@alberguecaceres.com", "927102 001");
		WaypointData wp2 = new WaypointData("39.47280, -6.37248", WaypointData.TypeOfPlace.cafebar, "Adarve", "Calle Maestro Sánchez Garrido, 4 ", "", "cafeadarve@gmail.com", "+34 927248064");
		WaypointData wp3 = new WaypointData("39.47332, -6.37412", WaypointData.TypeOfPlace.restaurante, "Botein", "Calle Madre Isabel de Larrañaga, s/n", "http://www.botein.es", "restaurante@botein.es", "+34 927240840");
		WaypointData wp4 = new WaypointData("39.47300, -6.37022", WaypointData.TypeOfPlace.barCopas, "Farmacia Legend", "Plaza Mayor, 20", "", "laminervatapas@gmail.com", "+34 927261052");
		waypointDataList.Add(wp1);
		waypointDataList.Add(wp2);
		waypointDataList.Add(wp3);
		waypointDataList.Add(wp4);

		return waypointDataList;
	}

	public List<WaypointData> ImaginaryDAOnear(WaypointData.TypeOfPlace type, string position)
	{
		Debug.Log(position + ", " + type.ToString()); ;
		List<WaypointData> waypointDataList = new List<WaypointData>();
		WaypointData wp1 = new WaypointData("39.47306, -6.37132", WaypointData.TypeOfPlace.alojamiento, "Albergue Municipal de Caceres", "Avda. de la Universidad s/n", "http://www.alberguecaceres.es/", "info@alberguecaceres.com", "927102 001");
		waypointDataList.Add(wp1);

		return waypointDataList;
	}

	public void TestWaypointCreation()
	{
		newPlace("39.47306, -6.37132", WaypointData.TypeOfPlace.alojamiento, "Carlos", "", "www", "", "");
		newPlace("39.47306, -6.37248", WaypointData.TypeOfPlace.cafebar, "Rincon", "www", "", "", "");
		newPlace("39.47308, -6.37412", WaypointData.TypeOfPlace.restaurante, "Sitio", "", "www", "", "");
		newPlace("39.47300, -6.37022", WaypointData.TypeOfPlace.barCopas, "Bua", "", "www", "", "");

		//GameObject.Find("Details").GetComponent<PlaceDetailsUIManager>().ShowDetails(_spawnedObjects[0]);
	}

}