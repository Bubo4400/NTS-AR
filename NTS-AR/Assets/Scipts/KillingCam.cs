using System.Collections;
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
        cam = GetComponent<Camera>();
        touchPressAction = playerInput.actions["TouchPress"];
        touchPosAction = playerInput.actions["TouchPos"];
    }

    void Update()
    {
        if (touchPressAction.WasPerformedThisFrame())
        {
            touchPos = touchPosAction.ReadValue<Vector2>();
            Ray ray = cam.ScreenPointToRay(touchPos);
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObj = hit.collider.gameObject;
                if (hitObj.tag == "Enemy")
                {
                    var clone = Instantiate(ParticleEffect, hitObj.transform.position, Quaternion.identity);
                    clone.transform.localScale = hitObj.transform.localScale;
                    ParticleSystem particleSystem = clone.GetComponent<ParticleSystem>();
                    if (particleSystem != null)
                    {
                        particleSystem.Play();
                    }
                    
                    // Start coroutine after particle effect plays
                    StartCoroutine(ScaleAndDestroyInline(hitObj));
                }
            }
        }
    }

    IEnumerator ScaleAndDestroyInline(GameObject hitObj)
    {       
        for (int i = 0; i < 10; i++)
        {
            hitObj.transform.localScale += new Vector3(1, 1, 1);
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(hitObj);
    }
}
