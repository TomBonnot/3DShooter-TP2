using System.Collections;
using UnityEngine;

public class WeaponSpawnerBehavior : MonoBehaviour
{
    [SerializeField] private GameObject weaponType;
    [SerializeField] private Transform anchor;
    [SerializeField] private float weaponRespawnTimer;
    void Start()
    {
        StartCoroutine(resetIn(0));
    }

    // Update is called once per frame
    void Update()
    {
        anchor.transform.Rotate(new Vector3(1, 1, 0));
    }

    public void resetWeapon()
    {
        StartCoroutine(resetIn(weaponRespawnTimer));
    }

    private IEnumerator resetIn(float s)
    {
        yield return new WaitForSeconds(s);
        if (anchor.childCount == 0)
            Instantiate(weaponType, anchor);
    }
}