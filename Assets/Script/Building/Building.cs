using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Building
{
    public const int COSTMATERIAL1 = 0;
    public const int COSTMATERIAL2 = 0;

    public enum Type
    {
        Factory = 0,
        MiningCamp = 3
    }
    public Type type;
    public string pathSprite;
    public int idSprite = -1;

    public GameObject button;
    private Vector2Int position;


    int level = 0;
    public bool CanComplete = false;
    public Vector2Int Position { get => position; set => position = value; }

    public Building(Type type, Vector2Int position, string path, int idSprite)
    {
        this.type = type;
        this.pathSprite = path;
        this.idSprite = idSprite;
        this.position = position;
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

public class BuildingZone : Building
{
    float timeToBuild;

    public BuildingZone(Type type, Vector2Int position, float timeToBuild) : base(type, position, "", 0)
    {

    }

    public override void Start()
    {
        
    }

    public override void Update(float deltaTime)
    {

    }

    public override RessourceSystem.Package Complete()
    {
        return null;
    }
}

public class Factory : Building
{
    public const int COSTMATERIAL1 = 100;
    public const int COSTMATERIAL2 = 0;
    public const int XSize = 2, YSize = 2;

    float timeToBuild = 30.0f;
    RessourceSystem.Type typeRessource = RessourceSystem.Type.A;

    int productionQuantityPerMinute = 30;
    int maximumStorageQuantity = 250;
    int minimumToCollect = 25;
    float timeToProduct;

    int resource = 0;

    public Factory(Vector2Int position) : base(Type.Factory, position, "Sprites/BuildingSprite" , 0)
    {

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

public class MiningCamp : Building
{
    public const int COSTMATERIAL1 = 500;
    public const int COSTMATERIAL2 = 0;
    public const int XSize = 2, YSize = 2;
    float timeToBuild = 30.0f;
    RessourceSystem.Type typeRessource = RessourceSystem.Type.B;

    int productionQuantityPerMinute = 30;
    int maximumStorageQuantity = 250;
    int minimumToCollect = 25;
    float timeToProduct;

    int resource = 0;

    public MiningCamp(Vector2Int position) : base(Type.MiningCamp, position, "Sprites/BuildingSprite", 3)
    {

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

