//ORG: Ghostyii & MoonLight Game
using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour
{

    public int gunDamage = 1;
    public float fireRate = 0.25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Texture2D bullueTexture;

    public OnXXXEvent onShoot = new OnXXXEvent();

    private RaycastHit hit;
    private Camera fpsCam;
    private float nextFire;

    void Start()
    {
        fpsCam = GetComponentInParent<Camera>();
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
                if (hit.rigidbody != null)
                    hit.rigidbody.AddForce(-hit.normal * hitForce);

            onShoot.Invoke();
        }

    }


    public RaycastHit? GetHitObject()
    {
        if (hit.rigidbody != null) return hit;
        else return null;
    }
}

[System.Serializable]
public class OnXXXEvent : UnityEvent { }