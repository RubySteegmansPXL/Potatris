using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PreviewShape : MonoBehaviour
{
    public List<ShapeSegment> segments;

    public void CreateSegment(int x, int y, SpriteData data, Sprite[] sprites, Sprite[] faces)
    {
        ShapeSegment newSegment = new GameObject("PreviewSegment").AddComponent<ShapeSegment>();
        newSegment.transform.parent = transform;
        newSegment.transform.position = new Vector3(x, y, 0); // Set position in world space
        newSegment.Instantiate(data, sprites, faces);
        newSegment.Create(x, y, true);
        segments.Add(newSegment);
    }

    public void MoveUp()
    {
        foreach (ShapeSegment segment in segments)
        {
            segment.position = new Vector2(segment.position.x, segment.position.y + 6);
        }
    }

    public void Shrink()
    {
        StartCoroutine(ShrinkCoroutine());
    }

    private IEnumerator ShrinkCoroutine()
    {
        float t = 0;
        while (t < 0.2f)
        {
            t += Time.deltaTime;
            foreach (ShapeSegment segment in segments)
            {
                segment.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t * 5);
            }
            yield return null;
        }
        Destroy(gameObject);
    }

    public void Grow()
    {
        StartCoroutine(GrowCoroutine());
    }

    private IEnumerator GrowCoroutine()
    {
        float t = 0;
        while (t < 0.2f)
        {
            t += Time.deltaTime;
            foreach (ShapeSegment segment in segments)
            {
                segment.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t * 5);
            }
            yield return null;
        }
    }
}