using UnityEngine;

public class AnimatedObject : MonoBehaviour
{
    public float newAngle;
    public float newPosition;
    public float rotationSpeed = 1.0f;
    public float movementSpeed = 1.0f;

    public bool isX = false;
    public bool isY = false;
    public bool isZ = false;
    public bool isPositiveOffset = false;

    public bool isAnimating = false;
    public bool isMovement = false;
    public bool isRotation = true;


    private void Update()
    {
        if (isAnimating)
        {

            if (isRotation)
            {
                if (isX)
                {
                    if (isPositiveOffset)
                    {
                        if (transform.eulerAngles.x < newAngle)
                        {
                            transform.Rotate(Vector3.right * (Time.deltaTime * rotationSpeed), Space.Self);
                        }
                        else
                        {
                            isAnimating = false;
                        }
                    }
                    else
                    {
                        if (transform.eulerAngles.x > newAngle)
                        {
                            transform.Rotate(Vector3.left * (Time.deltaTime * rotationSpeed), Space.Self);
                        }
                        else
                        {
                            isAnimating = false;
                        }
                    }


                }

                if (isZ)
                {

                    if (isPositiveOffset)
                    {
                        if (transform.eulerAngles.z < newAngle)
                        {
                            transform.Rotate(Vector3.forward * (Time.deltaTime * rotationSpeed), Space.Self);
                        }
                        else
                        {
                            isAnimating = false;
                        }
                    }
                    else
                    {
                        Quaternion target = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z);

                        print(target.eulerAngles.z);
                        if (target.eulerAngles.z > newAngle)
                        {
                            transform.Rotate(Vector3.back * (Time.deltaTime * rotationSpeed), Space.Self);
                        }
                        else
                        {
                            isAnimating = false;
                        }
                    }

                }

            }

            if (isMovement)
            {

                if (transform.position.y > newPosition)
                {
                    transform.Translate(Vector3.down * (Time.deltaTime * movementSpeed), Space.World);
                }
                else
                {
                    isAnimating = false;
                }
            }


        }
    }
}