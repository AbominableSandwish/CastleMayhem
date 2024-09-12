using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Building
{
    public enum Type
    {
        factory,
        miningCamp
        
    }

    Type type;
    const int COSTMATERIAL1 = 0;
    const int COSTMATERIAL2 = 0;
    int level = 0;
    public bool CanComplete = false;

    public GameObject obj;
    public GameObject button;

    public Building(Type type, GameObject obj)
    {
        this.type = type;
        this.obj = obj;
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

    public BuildingZone(Type type, GameObject zone, float timeToBuild) : base(type, zone)
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
    const int COSTMATERIAL1 = 100;
    const int COSTMATERIAL2 = 0;
    float timeToBuild = 30.0f;
    RessourceSystem.Type typeRessource = RessourceSystem.Type.A;

    int productionQuantityPerMinute = 30;
    int maximumStorageQuantity = 250;
    int minimumToCollect = 25;
    float timeToProduct;

    int resource = 0;

    public Factory(GameObject zone) : base(Type.factory, zone)
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
    const int COSTMATERIAL1 = 100;
    const int COSTMATERIAL2 = 0;
    float timeToBuild = 30.0f;
    RessourceSystem.Type typeRessource = RessourceSystem.Type.B;

    int productionQuantityPerMinute = 30;
    int maximumStorageQuantity = 250;
    int minimumToCollect = 25;
    float timeToProduct;

    int resource = 0;

    public MiningCamp(GameObject zone) : base(Type.miningCamp, zone)
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

