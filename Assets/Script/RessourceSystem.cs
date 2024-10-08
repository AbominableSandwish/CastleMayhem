using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RessourceSystem : MonoBehaviour
{
    public int RessourceA = 0;
    public int RessourceB = 0;

    public enum Type
    {
        A, B
    }
    public enum Action
    {
        Add,
        Reduce
    }
    public Action lastAction = Action.Add;

    public class Package
    {
        public RessourceSystem.Type Type;
        public int Value;

        public Package(RessourceSystem.Type type, int value)
        {
            Type = type;
            Value = value;
        }
    }
    public int GetRessource(Type type)
    {
        int ressource = -1;
      switch(type)
        {
            case Type.A: ressource = RessourceA; break;
            case Type.B: ressource = RessourceB; break;
        }
        return ressource;
    }

    public void AddRessource(Package packages)
    {
        if (packages != null)
        {
            switch (packages.Type)
            {
                case Type.A: RessourceA += packages.Value; break;
                case Type.B: RessourceB += packages.Value; break;
            }
            lastAction = Action.Add;
        }
    }

    public void ReduceRessource(Package packages)
    {
        if (packages != null)
        {
            switch (packages.Type)
            {
                case Type.A: RessourceA -= packages.Value; break;
                case Type.B: RessourceB -= packages.Value; break;
            }
            lastAction = Action.Reduce;
        }
    }

}
