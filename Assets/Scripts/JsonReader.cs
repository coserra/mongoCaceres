using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Mapbox.Unity.Utilities;

using static MongoDB.Driver.WriteConcern;
using Mapbox.Utils;
using static WaypointData;

public class JsonReader : MonoBehaviour
{
    private MongoClient dbClient;
    private IMongoDatabase database;
    void Start()
    {
        dbClient = new MongoClient("mongodb+srv://usuario:MDAD@cluster0.99mtz.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");

        database = dbClient.GetDatabase("Database");
        if (database != null)
        {
            Debug.Log("Conectado a la base de datos");
        }


    }

    public List<WaypointData> GetByTypeLong(bool[] types, string from, float radius)
    {
        BsonDocument t = new BsonDocument();
        //Cambio de orden las coordenadas
        string[] coordenadas = new string[2];
        
        coordenadas= from.Split(new string[] { "," }, StringSplitOptions.None);


        t = BsonDocument.Parse("{  geometry:   { $near:  {    $geometry: { type: \"Point\",  coordinates:[" + coordenadas[1]+", "+coordenadas[0] + "] },   $minDistance: 0, $maxDistance: " + radius + "}}   }");

        List<WaypointData> lista = new List<WaypointData>();

        var collection = database.GetCollection<BsonDocument>("Todo"); ;

        var secondDocument = collection.Find(t).ToList();
        WaypointData aux;
        foreach (var document in secondDocument)
        {
            aux = JsonToWaypoint(document.ToString(), TypeOfPlace.barCopas, true);

            switch (aux.Type)
            {
                case TypeOfPlace.alojamiento:
                    if (types[0])
                    {
                        lista.Add(aux);
                    }
                    break;
                case TypeOfPlace.barCopas:
                    if (types[1])
                    {
                        lista.Add(aux);
                    }
                    break;
                case TypeOfPlace.cafebar:
                    if (types[2])
                    {
                        lista.Add(aux);
                    }
                    break;
                case TypeOfPlace.centroTuristico:
                    if (types[3])
                    {
                        lista.Add(aux);
                    }
                    break;
                case TypeOfPlace.monumento:
                    if (types[4])
                    {
                        lista.Add(aux);
                    }
                    break;
                case TypeOfPlace.restaurante:
                    if (types[5])
                    {
                        lista.Add(aux);
                    }
                    break;
                default:

                    break;
            }
            //Debug.Log(document.ToString());
        }

        return lista;
    }

    public List<WaypointData> GetByType(bool[] types)
    {

        List<WaypointData> lista = new List<WaypointData>();

        String tipos = "";
        if (types[0])
        {
            tipos = tipos + "0 ";
        }
        if (types[1])
        {
            tipos = tipos + "1 ";
        }
        if (types[2])
        {
            tipos = tipos + "2 ";
        }
        if (types[3])
        {
            tipos = tipos + "3 ";
        }
        if (types[4])
        {
            tipos = tipos + "4 ";
        }
        if (types[5])
        {
            tipos = tipos + "5 ";
        }

        var collection = database.GetCollection<BsonDocument>("Todo");

        BsonDocument pruebaa = new BsonDocument();
        pruebaa = BsonDocument.Parse("{ $text: { $search: \"" + tipos + "\"}}");


        var firstDocument = collection.Find(pruebaa).ToList(); ;

        foreach (var document in firstDocument.AsEnumerable())
        {
            lista.Add(JsonToWaypoint(document.ToString(), TypeOfPlace.alojamiento, true));
        }

        return lista;
    }

    public List<WaypointData> GetNear(bool[] types, string position)
    {
        BsonDocument t = new BsonDocument();

        string[] coordenadas = new string[2];

        coordenadas = position.Split(new string[] { "," }, StringSplitOptions.None);
        t = BsonDocument.Parse("{  geometry:   { $near:  {    $geometry: { type: \"Point\",  coordinates:[" + coordenadas[1] + ", " + coordenadas[0] + "] } }}   }");

        List<WaypointData> lista = new List<WaypointData>();

        var collection = database.GetCollection<BsonDocument>("Todo"); ;

        var secondDocument = collection.Find(t).ToList();

        WaypointData aux = null;

        int contador = 0;//Contador para devolver max 5 resultados

        foreach (var document in secondDocument.AsEnumerable())
        {
            if (contador < 5)
            {
                aux = JsonToWaypoint(document.ToString(), TypeOfPlace.restaurante, true);

                switch (aux.Type)
                {
                    case TypeOfPlace.alojamiento:
                        if (types[0])
                        {
                            lista.Add(aux);
                            contador = contador + 1;
                        }
                        break;
                    case TypeOfPlace.barCopas:
                        if (types[1])
                        {
                            lista.Add(aux);
                            contador = contador + 1;
                        }
                        break;
                    case TypeOfPlace.cafebar:
                        if (types[2])
                        {
                            lista.Add(aux);
                            contador = contador + 1;
                        }
                        break;
                    case TypeOfPlace.centroTuristico:
                        if (types[3])
                        {
                            lista.Add(aux);
                            contador = contador + 1;
                        }
                        break;
                    case TypeOfPlace.monumento:
                        if (types[4])
                        {
                            lista.Add(aux);
                            contador = contador + 1;
                        }
                        break;
                    case TypeOfPlace.restaurante:
                        if (types[5])
                        {
                            lista.Add(aux);
                            contador = contador + 1;
                        }
                        break;
                    default:

                        break;
                }
            }
            else
            {
                return lista;
            }

        }

        return lista;

    }

    public WaypointData JsonToWaypoint(string t, TypeOfPlace a, bool todo)
    {
        string[] splitArray = new string[10000];
        string[] splitArray2 = new string[10000];

        string coordenadas, url, telefono, nombre, email, direccion;

        TypeOfPlace tipo;

        //Coordenadas
        splitArray = t.Split(new string[] { "[" }, StringSplitOptions.None);
        if (!splitArray[0].Equals(t))
        {
            splitArray2 = splitArray[1].Split(new string[] { "]" }, StringSplitOptions.None);
            splitArray = splitArray2[0].Split(new string[] { "," }, StringSplitOptions.None);
            coordenadas = splitArray[1] +", "+ splitArray[0];
        }
        else
            coordenadas = "";

        //URL del lugar
        splitArray = t.Split(new string[] { "\"schema_url\" : \"" }, StringSplitOptions.None);
        if (!splitArray[0].Equals(t))
        {
            splitArray2 = splitArray[1].Split(new string[] { "\"" }, StringSplitOptions.None);
            url = splitArray2[0];
        }
        else
            url = "";

        //email del lugar
        splitArray = t.Split(new string[] { "\"schema_email\" : \"" }, StringSplitOptions.None);
        if (!splitArray[0].Equals(t))
        {
            splitArray2 = splitArray[1].Split(new string[] { "\"" }, StringSplitOptions.None);
            email = splitArray2[0];
        }
        else
            email = "";

        //teléfono del lugar
        splitArray = t.Split(new string[] { "\"schema_telephone\" : \"" }, StringSplitOptions.None);
        if (!splitArray[0].Equals(t))
        {
            splitArray2 = splitArray[1].Split(new string[] { "\"" }, StringSplitOptions.None);
            telefono = splitArray2[0];
        }
        else
            telefono = "";

        //email del lugar
        splitArray = t.Split(new string[] { "\"rdfs_label\" : \"" }, StringSplitOptions.None);
        if (!splitArray[0].Equals(t))
        {
            splitArray2 = splitArray[1].Split(new string[] { "\"" }, StringSplitOptions.None);
            nombre = splitArray2[0];
        }
        else
            nombre = "";

        //email del lugar
        splitArray = t.Split(new string[] { "\"schema_address_streetAddress\" : \"" }, StringSplitOptions.None);
        if (!splitArray[0].Equals(t))
        {
            splitArray2 = splitArray[1].Split(new string[] { "\"" }, StringSplitOptions.None);
            direccion = splitArray2[0];
        }
        else
            direccion = "";

        if (todo)
        {
            //tipo
            splitArray = t.Split(new string[] { "\"type\" : \"" }, StringSplitOptions.None);

            splitArray2 = splitArray[1].Split(new string[] { "\"" }, StringSplitOptions.None);
            switch (splitArray2[0])
            {
                case "0":
                    tipo = TypeOfPlace.alojamiento;
                    break;
                case "1":
                    tipo = TypeOfPlace.barCopas;
                    break;
                case "2":
                    tipo = TypeOfPlace.cafebar;
                    break;
                case "3":
                    tipo = TypeOfPlace.centroTuristico;
                    break;
                case "4":
                    tipo = TypeOfPlace.monumento;
                    break;
                case "5":
                    tipo = TypeOfPlace.restaurante;
                    break;
                default:
                    tipo = TypeOfPlace.cafebar;
                    //Debug.Log("FALLO: " + t);
                    break;
            }

        }
        else
        {
            tipo = a;
        }
        WaypointData final = new WaypointData(coordenadas, tipo, nombre, direccion, url, email, telefono);
        return final;
    }

    public List<WaypointData> GetByName(string name)
    {

        var collection = database.GetCollection<BsonDocument>("Todo"); ;

        List<WaypointData> lista = new List<WaypointData>();

        BsonDocument filtro = new BsonDocument();

        filtro = BsonDocument.Parse("{ $text: { $search: \" " + name + "\"}}");

        var secondDocument = collection.Find(filtro).ToList();

        int contador = 0; //contador; no queremos más de 5 resultados
        foreach (var document in secondDocument)
        {
            if (contador < 5)
            {
                lista.Add(JsonToWaypoint(document.ToString(), TypeOfPlace.alojamiento, true));
                contador++;
            }
            else
            {
                return lista;
            }
        }

        return lista;
    }

    public List<WaypointData> GetAllPlaces()
    {
        List<WaypointData> lista = new List<WaypointData>();

        var collection = database.GetCollection<BsonDocument>("Todo");

        var firstDocument = collection.Find(new BsonDocument()).ToList();

        WaypointData d;

        foreach (var document in firstDocument.AsEnumerable())
        {
            lista.Add(JsonToWaypoint(document.ToString(), TypeOfPlace.barCopas, true));
        }

        return lista;
    }

}