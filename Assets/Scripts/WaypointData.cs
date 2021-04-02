using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointData
{
    public enum TypeOfPlace
    {
        usuario,
        alojamiento,
        barCopas,
        cafebar,
        centroTuristico,
        monumento,
        restaurante
    }

    public string position { set; get; }
    public TypeOfPlace type { set; get; }
    public string name { set; get; }
    public string uri { set; get; }

    public WaypointData(string position, TypeOfPlace type, string name, string uri)
    {
        this.position = position;
        this.type = type;
        this.name = name;
        this.uri = uri;
    }
}
