using MapGeneration;
using PluginAPI.Core;
using pluginprank.CustomClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace pluginprank
{
    public static class mapspawner
    {
        public static void spawnMap()
        {
            Log.Info("attempting the uhhh map spawning shit");
            Dictionary<string, RoomIdentifier> rooms = new Dictionary<string, RoomIdentifier>();
            Log.Info("made rooms variable");
            foreach (RoomIdentifier room in RoomIdentifier.AllRoomIdentifiers.ToArray())
            {
                try
                {
                    rooms.Add(room.name, room);
                    //Log.Info($"added room {room.name} to rooms");
                }
                catch (Exception ex)
                {
                    Log.Error($"Error adding room {room.name} to rooms: {ex.Message}");
                }
            }
            Log.Info("building map now");
            Building exported552122 = new Building(rooms["EZ_Intercom"].transform.position, rooms["EZ_Intercom"].transform.rotation.eulerAngles, Vector3.one, "EXPORTED552122");
            exported552122.Add(new Structure(new Vector3(-2.150772f, -5.50946f, -5.579681f), new Vector3(0.5999999f, 1f, 1.05f), new Vector3(0f, 0f, 0f), PrimitiveType.Cube, new Color(0f, 0f, 0f, 1f), "exported552122"));
            exported552122.SpawnBuilding();
            Building exported553403 = new Building(rooms["EZ_Intercom"].transform.position, Vector3.zero, Vector3.one, "EXPORTED553403");
            exported553403.Add(new Structure(new Vector3(-3.361725f, -5.159546f, 1.258606f), new Vector3(0.4999999f, 1.35f, 1.1f), new Vector3(0f, 0f, 0f), PrimitiveType.Cube, new Color(0.5f, 0.5f, 0.5f, 1f), "exported553403"));
            exported553403.SpawnBuilding();
            Building exported554269 = new Building(rooms["HCZ_079"].transform.position, Vector3.zero, Vector3.one, "EXPORTED554269");
            exported554269.Add(new Structure(new Vector3(6.773308f, -1.921814f, 6.370316f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0f, 0f, 0f), PrimitiveType.Cube, new Color(1f, 0f, 0f, 1f), "exported554269"));
            exported554269.SpawnBuilding();
            Building exported555531 = new Building(rooms["HCZ_106"].transform.position, Vector3.zero, Vector3.one, "EXPORTED555531");
            exported555531.Add(new Structure(new Vector3(-8.17971f, 6.565247f, 10.49374f), new Vector3(3.5f, 1f, 3.5f), new Vector3(0f, 0f, 0f), PrimitiveType.Cylinder, new Color(1f, 1f, 1f, 1f), "exported555531"));
            exported555531.SpawnBuilding();
            Building exported556826 = new Building(rooms["HCZ_106"].transform.position, Vector3.zero, Vector3.one, "EXPORTED556826");
            exported556826.Add(new Structure(new Vector3(-2.472683f, 6.248779f, 10.65233f), new Vector3(4f, 1f, 4f), new Vector3(0f, 0f, 0f), PrimitiveType.Cylinder, new Color(1f, 1f, 1f, 1f), "exported556826"));
            exported556826.SpawnBuilding();
            Building exported595331 = new Building(rooms["HCZ_106"].transform.position, Vector3.zero, Vector3.one, "EXPORTED595331");
            exported595331.Add(new Structure(new Vector3(-19.10159f, 0.8932495f, 3.815613f), new Vector3(1f, 0.3f, 2.5f), new Vector3(0.4617488f, 0f, 0f), PrimitiveType.Cube, new Color(0.5f, 0.5f, 0.5f, 1f), "exported595331"));
            exported595331.SpawnBuilding();
            Building exported596274 = new Building(rooms["HCZ_Servers"].transform.position, Vector3.zero, Vector3.one, "EXPORTED596274");
            exported596274.Add(new Structure(new Vector3(2.917984f, 0.5599365f, 1.710876f), new Vector3(0.5f, 1.15f, 3f), new Vector3(0f, 0f, 0f), PrimitiveType.Cube, new Color(0.5f, 0.5f, 0.5f, 1f), "exported596274"));
            exported596274.SpawnBuilding();
            Building exported597445 = new Building(rooms["Outside"].transform.position, Vector3.zero, Vector3.one, "EXPORTED597445");
            exported597445.Add(new Structure(new Vector3(0.04296875f, 3.191711f, 0.7539062f), new Vector3(2f, 2f, 2f), new Vector3(0f, 0f, 0f), PrimitiveType.Sphere, new Color(0f, 0f, 0f, 0f), "exported597445"));
            exported597445.SpawnBuilding();
            Log.Info("completed building map");
        }
    }
}
