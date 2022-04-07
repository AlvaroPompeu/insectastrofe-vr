using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHolster : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;

    private float baseRotateSpeed = 50f;
    private float yOffset = 0.55f;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - yOffset, mainCamera.transform.position.z);

        // The holster rotate speed must adapt as the head rotation is closer or further
        float finalRotateSpeed = CalculateFinalRotateSpeed();
        float step = finalRotateSpeed * Time.deltaTime;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0), step);
    }

    private float CalculateFinalRotateSpeed()
    {
        float rotationDifference = Mathf.Abs(mainCamera.transform.eulerAngles.y - transform.eulerAngles.y);

        if (rotationDifference <= 20)
        {
            return (baseRotateSpeed / 4);
        }
        else if (rotationDifference <= 40)
        {
            return (baseRotateSpeed / 2);
        }
        else if (rotationDifference <= 60)
        {
            return baseRotateSpeed;
        }
        else
        {
            return (baseRotateSpeed * 2);
        }
    }
}
