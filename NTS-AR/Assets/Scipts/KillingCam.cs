// KillingCam.cs (with Debugging)
using UnityEngine;
using UnityEngine.InputSystem;

public class KillingCam : MonoBehaviour
{
    public GameObject ParticleEffect;
    private Vector2 touchPos;
    private RaycastHit hit;
    private Camera cam;
    public PlayerInput playerInput;
    private InputAction touchPressAction;
    private InputAction touchPosAction;
    public AudioSource death;
	public AudioSource pew;
    public AudioSource win;
    public Material colin;

    void Start()
    {
        Debug.Log("KillingCam Start called."); 

        cam = GetComponent<Camera>();
        if (cam == null) Debug.LogError("Camera component not found on this GameObject!", this);

        if (playerInput == null) {
            Debug.LogError("PlayerInput component is NOT assigned in the Inspector!", this);
            return;
        }
         Debug.Log("PlayerInput component assigned.");

        touchPressAction = playerInput.actions["TouchPress"];
        touchPosAction = playerInput.actions["TouchPos"];

        if (touchPressAction == null) Debug.LogError("InputAction 'TouchPress' NOT FOUND in Action Asset!");
        else Debug.Log("InputAction 'TouchPress' Found.");

        if (touchPosAction == null) Debug.LogError("InputAction 'TouchPos' NOT FOUND in Action Asset!");
        else Debug.Log("InputAction 'TouchPos' Found."); 
        
         if (touchPressAction != null && !touchPressAction.enabled) touchPressAction.Enable();
         if (touchPosAction != null && !touchPosAction.enabled) touchPosAction.Enable();
    }

    void Update()
    {
        if (touchPressAction == null || touchPosAction == null) return;

        if (touchPressAction.WasPerformedThisFrame())
        {
            Debug.Log("--- Touch Press Detected! ---");

            touchPos = touchPosAction.ReadValue<Vector2>();
            Debug.Log($"Touch Position: {touchPos}"); 

            Ray ray = cam.ScreenPointToRay(touchPos);
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.yellow, 1f);

			pew.Play();

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log($"Raycast HIT: {hit.collider.gameObject.name} (Tag: {hit.collider.gameObject.tag})");

                GameObject hitObj = hit.collider.gameObject;
                Enemy enemyComponent = hitObj.GetComponent<Enemy>();
                
                if (enemyComponent != null)
                {
                    if (10 > enemyComponent.TakeHit())
                    {
                        enemyComponent.transform.localScale *= 1.1f;
                    }
                    else
                    {
                        if (ParticleEffect == null) {
                            Debug.LogWarning("ParticleEffect prefab not assigned in Inspector!");
                        } else {
                            var clone = Instantiate(ParticleEffect, hitObj.transform.position, Quaternion.identity);
                            if (clone != null)
                            {
                                ParticleSystem ps = clone.GetComponent<ParticleSystem>();
                                if (ps != null)
                                {
                                    ps.Play();
                                    Destroy(clone, ps.main.duration + ps.main.startLifetime.constantMax);
                                }
                            }
                        }

                        Renderer enemyRenderer = hitObj.GetComponent<Renderer>();
                        if (enemyRenderer.material == colin)
                        {
                            win.Play();
                        }
                        else 
                        {
                            death.Play();
                        }

                        Destroy(hitObj); 
                    }
                } 
            }
        }
    }
}