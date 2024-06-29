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
            Building exported550448 = new Building(rooms["EZ_Intercom"].transform.position, rooms["EZ_Intercom"].transform.rotation.eulerAngles - new Vector3(0, 270, 0), Vector3.one, "EXPORTED550448");
            exported550448.Add(new Structure(new Vector3(0.15625f, 0.9599609f, -3.164063f), new Vector3(1f, 1f, 1f), new Vector3(0f, 0f, 0f), PrimitiveType.Cube, new Color(1f, 1f, 1f, 1f), "exported550448"));
            exported550448.SpawnBuilding();
            Log.Info("completed building map");
        }
    }
}
