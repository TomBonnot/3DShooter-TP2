using System.Collections;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    //Decal to spawn when the bullet hit the ground
    [SerializeField] private GameObject _decal;

    //main timer, destroy the gameobject at the end of it
    [SerializeField] private float _timer = 3f;

    /**
    *   Whenever there is a collision, spawn a decal and start a coroutine 
    **/
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ContactPoint contact = collision.GetContact(0);
            GameObject decalInstance = Instantiate(_decal, contact.point - contact.normal, Quaternion.identity);
            decalInstance.transform.rotation = Quaternion.LookRotation(contact.normal);
            decalInstance.transform.localScale = Vector3.one * 0.5f;
            StartCoroutine(FadeOutDecal(decalInstance));
        }
    }

    /**
    *   Coroutine to decrease the size of the decal over time and destroy the gameobject at the end
    **/
    private IEnumerator FadeOutDecal(GameObject decal)
    {
        float elapsedTime = 0f;
        Vector3 initialScale = decal.transform.localScale;
        while(elapsedTime < _timer)
        {
            decal.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, elapsedTime / _timer);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(decal);
        Destroy(gameObject);
    }
}
