using AdminToys;
using Mirror;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace pluginprank.CustomClasses
{
    public class Structure
    {
        private Vector3 position;
        private Vector3 scale;
        private Vector3 rotation;
        private PrimitiveType primitiveType;
        private Color color;
        public string name;
        private bool collision;
        private PrimitiveObjectToy prim;

        public Structure(Vector3 position, Vector3 scale, Vector3 rotation, PrimitiveType type, Color color, string name, bool collision = true)
        {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
            this.primitiveType = type;
            this.color = color;
            this.name = name;
            this.collision = collision;
        }

        public void Spawn(Vector3 basepos, Vector3 scalemult, Vector3 rotationmod)
        {
            Vector3 newpos = basepos + position;
            Vector3 newscale = new Vector3(scale.x * scalemult.x, scale.y * scalemult.y, scale.z * scalemult.z);
            
            prim = Handlers.SpawnPrim(newpos, newscale, rotation, color, primitiveType, collision);
            Vector3 dir = prim.transform.position - basepos;
            Quaternion rot = Quaternion.Euler(rotationmod);
            dir = rot * dir;
            prim.transform.position = basepos + dir;
            prim.transform.rotation = rot * prim.transform.rotation;
        }

        public void Despawn()
        {
            if (prim != null) { NetworkServer.Destroy(prim.gameObject); }
        }

    }

    public class Light
    {
        private Vector3 position;
        private Color color;
        public string name;
        private float range;
        private float intensity;
        private LightSourceToy lightsource;

        public Light(Vector3 position, Color color, string name, float range, float intensity)
        {
            this.position = position;
            this.color = color;
            this.name = name;
            this.range = range;
            this.intensity = intensity;
        }

        public void Spawn(Vector3 basepos)
        {
            lightsource = Handlers.AddLight(basepos + position, color, range, intensity);
        }

        public void Despawn()
        {
            if (lightsource != null) { NetworkServer.Destroy(lightsource.gameObject); }
        }

    }

    public class Building
    {
        public List<Light> lights;
        public List<Structure> structures;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        public string name;

        public Building(Vector3 position, Vector3 rotation, Vector3 scale, string name)
        {
            this.structures = new List<Structure>();
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.name = name;
            this.lights = new List<Light>();
        }

        public void AddLight(Light lightsource)
        {
            lights.Add(lightsource);
        }

        public void RemoveLight(Light lightsource)
        {
            lights.Remove(lightsource);
        }

        public void Add(Structure struc)
        {
            structures.Add(struc);
        }

        public void Remove(Structure struc)
        {
            structures.Remove(struc);
        }

        public void SpawnBuilding()
        {
            foreach (Structure structure in structures)
            {
                structure.Spawn(position, scale, rotation);
            }
            foreach (Light light in lights)
            {
                light.Spawn(position);
            }
        }
    }
}
