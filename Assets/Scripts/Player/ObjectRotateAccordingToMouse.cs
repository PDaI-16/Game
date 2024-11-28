using UnityEngine;

public class ObjectRotateAccordingToMouse
{
    public static void RotateObjectForRangedWeapon(Transform objectTransform, Camera camera)
    {
        // Validate inputs
        if (objectTransform == null)
        {
            Debug.LogError("RotateObjectForRangedWeapon failed: objectTransform is null.");
            return;
        }

        if (camera == null)
        {
            Debug.LogError("RotateObjectForRangedWeapon failed: camera is null.");
            return;
        }

        // Validate if camera is active
        if (!camera.isActiveAndEnabled)
        {
            Debug.LogError("RotateObjectForRangedWeapon failed: camera is not active or enabled.");
            return;
        }

        // Get the mouse position in screen space
        Vector3 mousePos = Input.mousePosition;

        // Convert mouse position to world space
        mousePos.z = Vector3.Distance(camera.transform.position, objectTransform.position); // Distance to object
        Vector3 worldMousePos = camera.ScreenToWorldPoint(mousePos);

        // Validate world position (sanity check to ensure the calculation is reasonable)
        if (float.IsNaN(worldMousePos.x) || float.IsNaN(worldMousePos.y) || float.IsNaN(worldMousePos.z))
        {
            Debug.LogError("RotateObjectForRangedWeapon failed: Calculated world mouse position contains invalid values.");
            return;
        }

        // Calculate direction to the mouse
        Vector3 direction = (worldMousePos - objectTransform.position).normalized;

        // Validate direction (sanity check)
        if (direction == Vector3.zero)
        {
            Debug.LogWarning("Direction vector is zero. Object and mouse position might be the same.");
            return;
        }

        // Calculate rotation angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply rotation to the object
        objectTransform.localRotation = Quaternion.Euler(0, 0, angle-45);

        // Debug log the applied rotation for verification
/*        Debug.Log($"Applied rotation: {angle} degrees to object {objectTransform.name}");*/
    }
}