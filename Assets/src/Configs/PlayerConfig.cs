﻿using Caveman.Setting;
using Newtonsoft.Json;

namespace Caveman.Configs
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PlayerConfig : ISettings
    {
        public PlayerConfig(string name, float speed, int respawnDuration, int invulnerabilityDuration, float strength,
            Types type, string prefabPath)
        {
            this.name = name;
            this.speed = speed;
            this.respawnDuration = respawnDuration;
            this.invulnerabilityDuration = invulnerabilityDuration;
            this.type = type;
            this.strength = strength;
            this.prefabPath = prefabPath;
        }

        [JsonProperty] private readonly string name;
        [JsonProperty] private readonly float speed;
        [JsonProperty] private readonly int respawnDuration;
        [JsonProperty] private readonly int invulnerabilityDuration;
        [JsonProperty] private readonly float strength;
        [JsonProperty] private readonly Types type;
        [JsonProperty] private string prefabPath;

        public string Name
        {
            get { return name; }
        }

        public float Strength
        {
            get { return strength; }
        }

        public float Speed
        {
            get { return speed; }
        }

        public int RespawnDuration
        {
            get { return respawnDuration; }
        }

        public int InvulnerabilityDuration
        {
            get { return invulnerabilityDuration; }
        }

        public string PrefabPath
        {
            get { return prefabPath; }
        }

        public Types Type
        {
            get { return type; }
        }

        public enum Types
        {
            Sample
        }
    }
}