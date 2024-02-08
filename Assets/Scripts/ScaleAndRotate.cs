using UnityEngine;

public class ScaleRotateWithSineAndRandomness : MonoBehaviour
{
    public float minScale = 0.8f;
    public float maxScale = 1.2f;
    public float scaleSpeed = 2f;

    public float rotationAngle = 15f; // Maximum rotation angle
    public float rotationSpeed; // Speed of rotation, to be randomized

    void Start()
    {
        // Randomize rotation speed at start between -speed and speed, ensuring some movement
        rotationSpeed = Random.Range(-1f, 1f) * 5;

        minScale = transform.localScale.x * minScale;
        maxScale = transform.localScale.x * maxScale;
    }

    void Update()
    {
        ScaleObjectWithSineWave();
        RotateObjectWithRandomSpeed();
    }

    void ScaleObjectWithSineWave()
    {
        float scaleRange = (maxScale - minScale) / 2f;
        float scaleMidpoint = minScale + scaleRange;
        float scale = scaleMidpoint + Mathf.Sin(Time.time * scaleSpeed) * scaleRange;
        transform.localScale = new Vector3(scale, scale, 1f);
    }

    void RotateObjectWithRandomSpeed()
    {
        float rotation = Mathf.Sin(Time.time * rotationSpeed) * rotationAngle;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
    }
}
