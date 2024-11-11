using System.Collections;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    [SerializeField] private GameObject _decal;
    [SerializeField] private float _timer = 3f;

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
