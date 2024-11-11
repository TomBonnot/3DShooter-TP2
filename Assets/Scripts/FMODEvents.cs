using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents instance {get; private set;}

    void Awake()
    {
        if(instance == null)
            Debug.LogWarning("No FMOD instance available");
        instance = this;
    } 
}
