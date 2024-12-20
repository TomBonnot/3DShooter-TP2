using UnityEngine;

public class WeaponSpawnerBehavior : MonoBehaviour
{
    [SerializeField] private GameObject weaponType;
    [SerializeField] private Transform anchor;
    void Start()
    {
        resetWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        anchor.transform.Rotate(new Vector3(1, 1, 0));
    }

    public void resetWeapon()
    {
        if (anchor.childCount == 0)
            Instantiate(weaponType, anchor);
    }
}
