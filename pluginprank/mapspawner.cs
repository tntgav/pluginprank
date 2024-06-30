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
            Building exported557279 = new Building(rooms["HCZ_106"].transform.position, rooms["HCZ_106"].transform.rotation.eulerAngles - new Vector3(0, 180, 0), Vector3.one, "EXPORTED557279");
            exported557279.Add(new Structure(new Vector3(-5.273464f, 6.625122f, 10.08592f), new Vector3(9.5f, 1.5f, 4.5f), new Vector3(0f, 0f, 0f), PrimitiveType.Cube, new Color(0f, 0f, 0f, 0f), "exported557279"));
            exported557279.SpawnBuilding();
            Building exported558745 = new Building(rooms["HCZ Part"].transform.position, rooms["HCZ Part"].transform.rotation.eulerAngles - new Vector3(0, 0, 0), Vector3.one, "EXPORTED558745");
            exported558745.Add(new Structure(new Vector3(0.2047119f, 0.9589844f, -6.464844f), new Vector3(4f, 0.15f, 1f), new Vector3(0f, 0f, 0f), PrimitiveType.Cube, new Color(1f, 1f, 1f, 1f), "exported558745"));
            exported558745.SpawnBuilding();
            Building exported559568 = new Building(rooms["HCZ Part"].transform.position, rooms["HCZ Part"].transform.rotation.eulerAngles - new Vector3(0, 0, 0), Vector3.one, "EXPORTED559568");
            exported559568.Add(new Structure(new Vector3(2.054703f, 0.4591064f, -6.464844f), new Vector3(0.3f, 1f, 1f), new Vector3(0f, 0f, 0f), PrimitiveType.Cube, new Color(1f, 1f, 1f, 1f), "exported559568"));
            exported559568.SpawnBuilding();
            Building exported560333 = new Building(rooms["HCZ Part"].transform.position, rooms["HCZ Part"].transform.rotation.eulerAngles - new Vector3(0, 0, 0), Vector3.one, "EXPORTED560333");
            exported560333.Add(new Structure(new Vector3(-1.645279f, 0.4591064f, -6.464844f), new Vector3(0.3f, 1f, 1f), new Vector3(0f, 0f, 0f), PrimitiveType.Cube, new Color(1f, 1f, 1f, 1f), "exported560333"));
            exported560333.SpawnBuilding();
            Building exported562941 = new Building(rooms["HCZ_Servers"].transform.position, rooms["HCZ_Servers"].transform.rotation.eulerAngles - new Vector3(0, 90, 0), Vector3.one, "EXPORTED562941");
            exported562941.Add(new Structure(new Vector3(1.49894f, 0.6098022f, -0.9928284f), new Vector3(0.7f, 1.25f, 7.8f), new Vector3(0f, 0f, 0f), PrimitiveType.Cube, new Color(0f, 0f, 0f, 1f), "exported562941"));
            exported562941.SpawnBuilding();
            Building exported603287 = new Building(rooms["HCZ_Hid"].transform.position, rooms["HCZ_Hid"].transform.rotation.eulerAngles - new Vector3(0, 90, 0), Vector3.one, "EXPORTED603287");
            exported603287.Add(new Structure(new Vector3(-4.670235f, 1.210144f, 6.005478f), new Vector3(0.3f, 2.5f, 2.5f), new Vector3(0f, 0f, 0f), PrimitiveType.Cube, new Color(1f, 1f, 1f, 1f), "exported603287"));
            exported603287.SpawnBuilding();
            Building exported604335 = new Building(rooms["HCZ_Hid"].transform.position, rooms["HCZ_Hid"].transform.rotation.eulerAngles - new Vector3(0, 90, 0), Vector3.one, "EXPORTED604335");
            exported604335.Add(new Structure(new Vector3(-4.670235f, 1.210144f, -4.894485f), new Vector3(0.3f, 2.5f, 2.5f), new Vector3(0f, 0f, 0f), PrimitiveType.Cube, new Color(1f, 1f, 1f, 1f), "exported604335"));
            exported604335.SpawnBuilding();
            Building exported606154 = new Building(rooms["HCZ_079"].transform.position, rooms["HCZ_079"].transform.rotation.eulerAngles - new Vector3(0, 90, 0), Vector3.one, "EXPORTED606154");
            exported606154.Add(new Structure(new Vector3(-6.789063f, -1.921814f, -6.270309f), new Vector3(0.5f, 0.5f, 0.15f), new Vector3(0f, 0f, 0f), PrimitiveType.Cube, new Color(1f, 1f, 1f, 1f), "exported606154"));
            exported606154.SpawnBuilding();
            Building exported607964 = new Building(rooms["HCZ_Testroom"].transform.position, rooms["HCZ_Testroom"].transform.rotation.eulerAngles - new Vector3(0, 90, 0), Vector3.one, "EXPORTED607964");
            exported607964.Add(new Structure(new Vector3(0.2187462f, 5.795532f, -0.2031326f), new Vector3(2.5f, 1f, 3f), new Vector3(0f, 0f, 0f), PrimitiveType.Cylinder, new Color(0f, 0f, 0f, 0f), "exported607964"));
            exported607964.SpawnBuilding();
            Log.Info("completed building map");
        }
    }
}
