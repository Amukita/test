using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    InputHandler inputHandler;

    EnemyStats enemyStats;

    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform cameraPivotTransform;
    private Transform myTransform;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraTransformPosition;

    public static CameraHandler singleton;

    public float lookSpeed = 0.01f;
    public float targetPosition;
    public float followSpeed = 0.1f;
    public float pivotSpeed = 0.03f;

    private float defaultPos;
    private float lookAngle;
    private float pivotAngle;
    public float minPivot = -35;
    public float maxPivot = 35;
    public LayerMask ignoreLayers;

    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffset = 0.2f;
    public float minimumCollisionOffset = 0.2f;

    

    public CharacterManager currentLockOnTarget;

    List<CharacterManager> availableTarget = new List<CharacterManager>();
    public float maxLockDistance = 100;
    public CharacterManager nearestLockOn;

    public CharacterManager leftLockTarget;
    public CharacterManager rightLockTarget;


    private void Awake()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        inputHandler = FindObjectOfType<InputHandler>();
        singleton = this;
        myTransform = transform;
        defaultPos = cameraTransform.localPosition.z;
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);

    }

    public void FollowTarget(float delta)
    {

        Vector3 targetPos = Vector3.SmoothDamp(myTransform.position, targetTransform.position, 
            ref cameraFollowVelocity, delta / followSpeed);
        myTransform.position = targetPos;

        HandleCameraCollision(delta);
    }

    public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
    {
        
        if (inputHandler.lockOnFlag == false && currentLockOnTarget == null)
        {
            lookAngle += (mouseXInput * lookSpeed) / delta;
            pivotAngle -= (mouseYInput * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            myTransform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;

            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }
        else
        {
            Vector3 dir = currentLockOnTarget.transform.position - transform.position;
            dir.Normalize();
            dir.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = targetRotation;

            dir = currentLockOnTarget.transform.position - cameraPivotTransform.position;
            dir.Normalize();

            targetRotation = Quaternion.LookRotation(dir);
            Vector3 eulerAngle = targetRotation.eulerAngles;
            eulerAngle.y = 0;
            cameraPivotTransform.localEulerAngles = eulerAngle;
        }

    }

    public void HandleLockOn()
    {

        float shortestDistanceOfRightTarget = Mathf.Infinity;
        float shortestDistanceOfLeftTarget = -Mathf.Infinity;
        float shortestDistance = Mathf.Infinity;
        
        Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26);

        for (int i = 0; i < colliders.Length; i++)
            {

            CharacterManager character = colliders[i].GetComponent<CharacterManager>();

            if (character != null)
            {
                Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                float viewAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);

                if (character.transform.root != targetTransform.transform.root && viewAngle > -100 && viewAngle
                        < 100 && distanceFromTarget <= maxLockDistance)
                {
                        availableTarget.Add(character);
                }
            }

            }

            for (int k = 0; k < availableTarget.Count; k++)
            {
                float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTarget[k].transform.position);

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOn = availableTarget[k];
                }

                if (inputHandler.lockOnFlag)
            {
                //Vector3 relativeEnemyPosition = currentLockOnTarget.transform.InverseTransformPoint(availableTarget[k].transform.position);
                // var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTarget[k].transform.position.x;
                // var distanceFromRightTarget = currentLockOnTarget.transform.position.x - availableTarget[k].transform.position.x;
                Vector3 relativeEnemyPosition = inputHandler.transform.InverseTransformPoint(availableTarget[k].transform.position);
                var distanceFromLeftTarget = relativeEnemyPosition.x;
                var distanceFromRightTarget = relativeEnemyPosition.x;

                if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget && 
                    availableTarget[k] != currentLockOnTarget)
                {
                    shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                    leftLockTarget = availableTarget[k];
                }

                else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget && 
                    availableTarget[k] != currentLockOnTarget)
                {
                    shortestDistanceOfRightTarget = distanceFromRightTarget;
                    rightLockTarget = availableTarget[k];
                }

            }

                

            }
     
    }

    public void ClearLockOnTargets()
    {
        availableTarget.Clear();
        nearestLockOn = null;
        currentLockOnTarget = null;
        
    }
     private void HandleCameraCollision(float delta)
    {
        targetPosition = defaultPos;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
        {
            float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetPosition = -(dis - cameraCollisionOffset);
        }

        if(Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition = -minimumCollisionOffset;
        }

        cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
        cameraTransform.localPosition = cameraTransformPosition;
    }
}
