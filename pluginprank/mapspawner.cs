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
            Building exported550429 = new Building(rooms["HCZ_Nuke"].transform.position, rooms["HCZ_Nuke"].transform.rotation.eulerAngles - new Vector3(0, 0, 0), Vector3.one, "EXPORTED550429");
            exported550429.Add(new Structure(new Vector3(-0.3431015f, 286.8965f, 2.256332f), new Vector3(11f, 0.15f, 12.5f), new Vector3(0f, 0f, 0f), PrimitiveType.Cylinder, new Color(1f, 1f, 1f, 1f), "exported550429"));
            exported550429.SpawnBuilding();
            Building exported574503 = new Building(rooms["Outside"].transform.position, rooms["Outside"].transform.rotation.eulerAngles - new Vector3(0, 0, 0), Vector3.one, "EXPORTED574503");
            exported574503.Add(new Structure(new Vector3(-3.962499f, 1.459961f, -30.06564f), new Vector3(0.5f, 3f, 2.4f), new Vector3(0f, 0f, 0f), PrimitiveType.Cube, new Color(1f, 1f, 1f, 1f), "exported574503"));
            exported574503.SpawnBuilding();
            Building exported575133 = new Building(rooms["Outside"].transform.position, rooms["Outside"].transform.rotation.eulerAngles - new Vector3(0, 0, 0), Vector3.one, "EXPORTED575133");
            exported575133.Add(new Structure(new Vector3(-3.962499f, 1.459961f, -33.46564f), new Vector3(0.5f, 3f, 2.1f), new Vector3(0f, 0f, 0f), PrimitiveType.Cube, new Color(1f, 1f, 1f, 1f), "exported575133"));
            exported575133.SpawnBuilding();
            Building exported576122 = new Building(rooms["Outside"].transform.position, rooms["Outside"].transform.rotation.eulerAngles - new Vector3(0, 0, 0), Vector3.one, "EXPORTED576122");
            exported576122.Add(new Structure(new Vector3(-3.962499f, 2.459961f, -32.56565f), new Vector3(0.5f, 1f, 3.1f), new Vector3(0f, 0f, 0f), PrimitiveType.Cube, new Color(1f, 1f, 1f, 1f), "exported576122"));
            exported576122.SpawnBuilding();
            Building exported616634 = new Building(rooms["Outside"].transform.position, rooms["Outside"].transform.rotation.eulerAngles - new Vector3(0, 0, 0), Vector3.one, "EXPORTED616634");
            exported616634.Add(new Structure(new Vector3(-8.632811f, 0.460083f, -34.10156f), new Vector3(9f, 1.05f, 0.5f), new Vector3(0f, 0f, 0f), PrimitiveType.Cube, new Color(1f, 1f, 1f, 1f), "exported616634"));
            exported616634.SpawnBuilding();
            Building exported617625 = new Building(rooms["Outside"].transform.position, rooms["Outside"].transform.rotation.eulerAngles - new Vector3(0, 0, 0), Vector3.one, "EXPORTED617625");
            exported617625.Add(new Structure(new Vector3(-8.632811f, 1.510193f, -34.10156f), new Vector3(9f, 1.05f, 0.5f), new Vector3(0f, 0f, 0f), PrimitiveType.Cube, new Color(0f, 0f, 0f, 0f), "exported617625"));
            exported617625.SpawnBuilding();
            Building exported618643 = new Building(rooms["Outside"].transform.position, rooms["Outside"].transform.rotation.eulerAngles - new Vector3(0, 0, 0), Vector3.one, "EXPORTED618643");
            exported618643.Add(new Structure(new Vector3(-8.632811f, 2.560181f, -34.10156f), new Vector3(9f, 1.05f, 0.5f), new Vector3(0f, 0f, 0f), PrimitiveType.Cube, new Color(1f, 1f, 1f, 1f), "exported618643"));
            exported618643.SpawnBuilding();
            Building exported619724 = new Building(rooms["Outside"].transform.position, rooms["Outside"].transform.rotation.eulerAngles - new Vector3(0, 0, 0), Vector3.one, "EXPORTED619724");
            exported619724.Add(new Structure(new Vector3(-8.386719f, 3.197998f, -31.59375f), new Vector3(9.5f, 0.5f, 6f), new Vector3(0f, 0f, 0f), PrimitiveType.Cube, new Color(1f, 1f, 1f, 1f), "exported619724"));
            exported619724.SpawnBuilding();
            Building exported620761 = new Building(rooms["Outside"].transform.position, rooms["Outside"].transform.rotation.eulerAngles - new Vector3(0, 0, 0), Vector3.one, "EXPORTED620761");
            exported620761.Add(new Structure(new Vector3(-8.609375f, 0.9099731f, -33.40234f), new Vector3(9f, 0.09999996f, 1f), new Vector3(0f, 0f, 0f), PrimitiveType.Cube, new Color(1f, 1f, 1f, 1f), "exported620761"));
            exported620761.SpawnBuilding();
            Log.Info("completed building map");
        }
    }
}
