using UnityEngine;

public class CoordinateChanger : MonoBehaviour
{
    public Coordinate coordinateScript;

    public void ChangeCoordinatesToMoscow()
    {
        coordinateScript.latitude = 55.755826;
        coordinateScript.longitude = 37.617300;
    }
}
