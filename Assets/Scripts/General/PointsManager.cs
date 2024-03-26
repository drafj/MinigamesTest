using iText.StyledXmlParser.Node;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public static PointsManager instance;

    [SerializeField] private TextMeshProUGUI pointsText;
    public int points;
    public int goalPoints;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            DestroyImmediate(gameObject);
    }

    public void AddPoints(int count)
    {
        points += count;
        pointsText.text = points.ToString();
    }

    public void ResetPoints()
    {
        points = 0;
        pointsText.text = points.ToString();
    }
}
