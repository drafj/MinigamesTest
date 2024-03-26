using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingPoints : MonoBehaviour
{
    public Transform targetPosition;
    public TextMeshPro pointsText;

    public int velocity = 0;
    [SerializeField] private float smoothVelocity;
    [SerializeField] private AnimationCurve curve;

    private void Start()
    {
        pointsText.text = PointsManager.instance.points.ToString();
        GameObject target = GameObject.Find("PointsTargetPosition");
        targetPosition = target.transform;
        StartCoroutine(MoveTo(targetPosition.position));
    }

    public IEnumerator MoveTo(Vector3 pos)
    {
        Vector3 firstPosition = transform.position;
        float currentCurveValue = 0;

        while (transform.position != pos)
        {
            currentCurveValue = Mathf.MoveTowards(currentCurveValue, 1, smoothVelocity * Time.deltaTime);

            transform.position = Vector3.Lerp(firstPosition, pos, curve.Evaluate(currentCurveValue));
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
