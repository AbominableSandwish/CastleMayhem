using System;
using UnityEngine;

[Serializable]
public class Building
{
    public const int COSTMATERIAL1 = 0;
    public const int COSTMATERIAL2 = 0;

    public enum Type
    {
        InContruct = -1,
        Factory = 0,
        MiningCamp = 3
    }
    public Type type;
    public string pathSprite;
    public int idSprite = -1;

    public GameObject button;
    public Vector2Int position;

    public bool CanUpgrade = false;
    public int level = 0;
    public bool CanComplete = false;
    public Vector2Int Position { get => position; set => position = value; }

    public Building(Type type, Vector2Int position, string path, int idSprite)
    {
        this.type = type;
        this.pathSprite = path;
        this.idSprite = idSprite;
        this.position = position;
    }

    public void Upgrade()
    {
        level++;
    }

    public virtual void Start()
    {

    }

    public virtual void Update(float deltaTime)
    {
    }

  
    public virtual RessourceSystem.Package Complete()
    {
        return null;
    }

}


[Serializable]
public class BuildingZone : Building
{
    float timeToBuild;
    public Type NextType;

    public BuildingZone(Vector2Int position, Type NextType, int Level = 0) : base(Type.InContruct, position, "", 0)
    {
        this.level = Level;
        int timeToBuild = 0;
        switch (NextType)
        {
            case Type.Factory:
                timeToBuild = Factory.timeToBuild;
                break;

            case Type.MiningCamp:
                timeToBuild = MiningCamp.timeToBuild;
                break;
        }

        this.timeToBuild = timeToBuild;
    }

    public override void Start()
    {
        
    }

    public override void Update(float deltaTime)
    {
        timeToBuild -= deltaTime;
        if(timeToBuild <= 0.0f)
        {
            CanComplete = true;
        }
    }

    public override RessourceSystem.Package Complete()
    {
        CanUpgrade = true;
        return null;
    }
}


[Serializable]
public class Factory : Building
{
    public const int COSTMATERIAL1 = 100;
    public const int COSTMATERIAL2 = 0;
    public const int XSize = 2, YSize = 2;

    static public int timeToBuild = 10;
    RessourceSystem.Type typeRessource = RessourceSystem.Type.A;

    static int productionQuantityPerMinute = 30;
    static int maximumStorageQuantity = 250;
    static int minimumToCollect = 25;
    float timeToProduct;

    int resource = 0;

    public Factory(Vector2Int position, int level = 1) : base(Type.Factory, position, "Sprites/BuildingSprite" , 0)
    {
        this.level = level;
    }
    public override void Start()
    {
        timeToProduct = productionQuantityPerMinute / 60.0f;


        Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 10));
    }
    public override void Update(float deltaTime)
    {
        if (resource < maximumStorageQuantity)
        {
            timeToProduct -= deltaTime;
            if (timeToProduct <= 0.0f)
            {
                resource++;
                if (resource >= minimumToCollect)
                    CanComplete = true;
                timeToProduct = productionQuantityPerMinute / 60.0f;
            }
        }
    }

    public override RessourceSystem.Package Complete()
    {
        int remain = resource % 5;
        int value = resource;
        resource = remain;
        CanComplete = false;
        return new RessourceSystem.Package(RessourceSystem.Type.A, value - remain);
    }
}


[Serializable]
public class MiningCamp : Building
{
    public const int COSTMATERIAL1 = 500;
    public const int COSTMATERIAL2 = 0;

    public const int XSize = 2, YSize = 2;

    static public int timeToBuild = 60;
    RessourceSystem.Type typeRessource = RessourceSystem.Type.B;

    int productionQuantityPerMinute = 30;
    int maximumStorageQuantity = 250;
    int minimumToCollect = 25;
    float timeToProduct;

    int resource = 0;

    public MiningCamp(Vector2Int position, int level) : base(Type.MiningCamp, position, "Sprites/BuildingSprite", 3)
    {
        this.level = level;
    }

    public override void Start()
    {
        timeToProduct = productionQuantityPerMinute / 60.0f;


        Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 10));
    }

    public override void Update(float deltaTime)
    {
        if (resource < maximumStorageQuantity)
        {
            timeToProduct -= deltaTime;
            if (timeToProduct <= 0.0f)
            {
                resource++;
                if (resource >= minimumToCollect)
                    CanComplete = true;
                timeToProduct = productionQuantityPerMinute / 60.0f;
            }
        }
    }

    public override RessourceSystem.Package Complete()
    {
        int remain = resource % 5;
        int value = resource;
        resource = remain;
        CanComplete = false;
        return new RessourceSystem.Package(RessourceSystem.Type.B, value - remain);
    }
}

