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

            Log.Info("completed building map");
        }
    }
}
