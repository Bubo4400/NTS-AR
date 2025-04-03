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

    void Start()
    {
        Debug.Log("KillingCam Start called."); // 1. Check Start runs

        cam = GetComponent<Camera>();
        if (cam == null) Debug.LogError("Camera component not found on this GameObject!", this);

        if (playerInput == null) {
            Debug.LogError("PlayerInput component is NOT assigned in the Inspector!", this);
            return; // Stop if PlayerInput isn't assigned
        }
         Debug.Log("PlayerInput component assigned."); // 2. Check PlayerInput is assigned

        touchPressAction = playerInput.actions["TouchPress"];
        touchPosAction = playerInput.actions["TouchPos"];

        if (touchPressAction == null) Debug.LogError("InputAction 'TouchPress' NOT FOUND in Action Asset!");
        else Debug.Log("InputAction 'TouchPress' Found."); // 3. Check TouchPress action found

        if (touchPosAction == null) Debug.LogError("InputAction 'TouchPos' NOT FOUND in Action Asset!");
        else Debug.Log("InputAction 'TouchPos' Found."); // 4. Check TouchPos action found

         // Make sure the actions are enabled (they usually are by default with PlayerInput)
         if (touchPressAction != null && !touchPressAction.enabled) touchPressAction.Enable();
         if (touchPosAction != null && !touchPosAction.enabled) touchPosAction.Enable();
    }

    void Update()
    {
        // Optional: Log every frame to ensure Update is running
        // Debug.Log("KillingCam Update running...");

        if (touchPressAction == null || touchPosAction == null) return; // Don't proceed if actions are missing

        if (touchPressAction.WasPerformedThisFrame())
        {
            Debug.Log("--- Touch Press Detected! ---"); // 5. Check if input is detected

            touchPos = touchPosAction.ReadValue<Vector2>();
            Debug.Log($"Touch Position: {touchPos}"); // 6. Check touch position value

            Ray ray = cam.ScreenPointToRay(touchPos);
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.yellow, 1f); // Visualize ray in Scene view

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log($"Raycast HIT: {hit.collider.gameObject.name} (Tag: {hit.collider.gameObject.tag})"); // 7. Check what the ray hit

                GameObject hitObj = hit.collider.gameObject;
                Enemy enemyComponent = hitObj.GetComponent<Enemy>();
                
                if (enemyComponent != null)
                {
                    if (ParticleEffect == null) {
                         Debug.LogWarning("ParticleEffect prefab not assigned in Inspector!");
                    } else {
                        var clone = Instantiate(ParticleEffect, hitObj.transform.position, Quaternion.identity);
                        // ... (rest of particle logic)
                        ParticleSystem ps = clone.GetComponent<ParticleSystem>();
                        Destroy(clone, ps != null ? ps.main.duration + ps.main.startLifetime.constantMax : 5f);
                    }

                    // Call TakeHit on the actual Enemy component instance
                    if (10 > enemyComponent.TakeHit())
                    {
                        // Optional: Scale up smoothly
                        enemyComponent.transform.localScale *= 1.1f;
                        // Or your original scaling:
                        // transform.localScale += new Vector3(1, 1, 1);
                    }
                    else
                    {
                        Destroy(hitObj); 
                    }
                    
                   
                }
                
            }
        }
    }
}