using System;

namespace Assets.CodeBase.Gameplay.Services.BreedDog.Data
{
    [Serializable]
    public class BreedId
    {
        public string Id;
        public string Name;
    }

    [Serializable]
    public class BreedData
    {
        public string id;
        public string type;
        public BreedAttributes attributes;
    }

    [Serializable]
    public class BreedAttributes
    {
        public string name;
        public string description;
        public LifeSpan life;
        public Weight male_weight;
        public Weight female_weight;
        public bool hypoallergenic;
    }

    [Serializable]
    public class LifeSpan
    {
        public int min;
        public int max;
    }

    [Serializable]
    public class Weight
    {
        public int min;
        public int max;
    }
}
