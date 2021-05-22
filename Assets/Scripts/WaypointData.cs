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
    public static TypeOfPlace[] typeOfPlaces = { TypeOfPlace.alojamiento,TypeOfPlace.barCopas,
        TypeOfPlace.cafebar,TypeOfPlace.centroTuristico,TypeOfPlace.monumento,TypeOfPlace.restaurante};

    public string Coordinates { set; get; }
    public TypeOfPlace Type { set; get; }
   
    public string Name { set; get; }
    public string Address { set; get; }
    public string Uri { set; get; }
    public string Email { set; get; }
    public string Telephone { set; get; }

    public WaypointData(string coordinates, TypeOfPlace type, string name, string direction, string uri,string email, string telephone)
    {
        this.Coordinates = coordinates;
        this.Type = type;
        this.Name = name;
        this.Address = direction;
        this.Uri = uri;
        this.Email = email;
        this.Telephone = telephone;
    }
}
