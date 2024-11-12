using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents instance {get; private set;}

    //A simple exemple of variable here. A public EventReference from FMODUnity, with a correlate name, getter and setter 
    /**
    [field: Header("Ambiance SFX")]
    [field: SerializeField] public EventReference ambiance {get; set;}
    **/

    void Awake()
    {
        if(instance == null)
            Debug.LogWarning("No FMOD instance available");
        instance = this;
    } 
}
