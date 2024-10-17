using System;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Building
{
    public const int COSTMATERIAL1 = 0;
    public const int COSTMATERIAL2 = 0;

    private int health = 100;
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
    public GameObject Object;

    public bool IsUpgrading = false;
    public bool CanUpgrade = false;

    public int level = 0;
    public bool CanComplete = false;
    public Vector2Int Position { get => position; set => position = value; }
    public int Health { get => health; set => health = value; }

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

    public virtual bool AABB(Point point)
    {
        return false;
    }

    public virtual void Start()
    {

    }

    public virtual void Update(float deltaTime)
    {
    }

    public virtual int GetTimeToConstruct() {  return 0; }
    public virtual int GetCapacity() { return 0; }
    public virtual int GetproductionQuantityPerHour()
    {
        return 0;
    }

    public virtual int GetMaximumStorageQuantity()
    {
        return 0;
    }

    public virtual RessourceSystem.Package Complete()
    {
        return null;
    }

    //Bad
    public Sprite sprite;
}

[Serializable]
public class BuildingZone : Building
{
    public float TimeToBuild;
    float timer = 0.0f;
    public Type NextType;
    Vector2Int size;
    

    public BuildingZone(Vector2Int position, Type NextType, float timeToBuild, int Level = 0) : base(Type.InContruct, position, "", 0)
    {
        this.level = Level;
        switch (NextType)
        {
            case Type.Factory:
                size = Factory.Size;
                break;

            case Type.MiningCamp:
                size = MiningCamp.Size;
                break;
        }

        this.TimeToBuild = timeToBuild;
        this.NextType = NextType;
    }

    public Vector2Int GetSize()
    {
        return size;
    }

    public override bool AABB(Point point)
    {
        bool isEnter = false;
        if ((point.X >= position.x && point.X <= position.x + size.x)
            && (point.Y >= position.y && point.Y <= position.y + size.y))
        {
            isEnter = true;
        }

        return isEnter;
    }

    public override void Start()
    {
        
    }

    public override void Update(float deltaTime)
    {
        if (!CanComplete)
        {
            timer += deltaTime;
            if (timer >= TimeToBuild)
            {
                CanComplete = true;
            }
        }
    }

    public override RessourceSystem.Package Complete()
    {
        IsUpgrading = true;
        return null;
    }
}


[Serializable]
public class Factory : Building
{
    public new const int COSTMATERIAL1 = 100;
    public new const int COSTMATERIAL2 = 0;
    static public int timeToBuild = 30;

    static public  Vector2Int Size = new Vector2Int(2,2);
    RessourceSystem.Type typeRessource = RessourceSystem.Type.A;
    public static int productionQuantityPerHour = 300;
    public static int maximumStorageQuantity = 250;
    static int minimumToCollect = 25;
    float timeToProduct;


    int resource = 0;


    public override int GetCapacity()
    {
        return resource;
    }

    public override int GetTimeToConstruct()
    {
        return timeToBuild * (level + 1);
    }
    public override int GetproductionQuantityPerHour() 
    {
        return productionQuantityPerHour * level;
    }

    public override int GetMaximumStorageQuantity()
    {
        return maximumStorageQuantity * level;
    }
    public Factory(Vector2Int position, int level = 1) : base(Type.Factory, position, "Sprites/BuildingSprite" , 0)
    {
        CanUpgrade = true;
        this.level = level;
    }
    public override void Start()
    {
        timeToProduct = 3600.0f / productionQuantityPerHour;


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
                timeToProduct = 3600.0f / GetproductionQuantityPerHour();
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

    public override bool AABB(Point point)
    {
        bool isEnter = false;
        if ((point.X >= position.x && point.X <= position.x + Size.x)
            && (point.Y >= position.y && point.Y <= position.y + Size.y))
        {
            isEnter = true;
        }

        return isEnter;
    }
}


[Serializable]
public class MiningCamp : Building
{
    public const int COSTMATERIAL1 = 500;
    public const int COSTMATERIAL2 = 0;

    static public Vector2Int Size = new Vector2Int(2, 2);
    static public int timeToBuild = 60;
    RessourceSystem.Type typeRessource = RessourceSystem.Type.B;

    int productionQuantityPerHour = 5;
    int maximumStorageQuantity = 250;
    int minimumToCollect = 25;
    float timeToProduct;

    int resource = 0;

    public override int GetCapacity()
    {
        return resource;
    }

    public override int GetTimeToConstruct()
    {
        return timeToBuild * (level + 1);
    }
    public override int GetproductionQuantityPerHour()
    {
        return productionQuantityPerHour * level;
    }

    public override int GetMaximumStorageQuantity()
    {
        return maximumStorageQuantity * level;
    }

    public MiningCamp(Vector2Int position, int level) : base(Type.MiningCamp, position, "Sprites/BuildingSprite", 3)
    {
        CanUpgrade = true;
        this.level = level;
    }

    public  override void Start()
    {
        timeToProduct = productionQuantityPerHour / 60.0f;


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
                timeToProduct = 3600.0f / GetproductionQuantityPerHour();
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

    public override bool AABB(Point point)
    {
        bool isEnter = false;
        if ((point.X >= position.x && point.X <= position.x + Size.x)
            && (point.Y >= position.y && point.Y <= position.y + Size.y))
        {
            isEnter = true;
        }

        return isEnter;
    }
}