using UnityEngine;

public class LondonCoordinateChanger : MonoBehaviour
{
    public Coordinate coordinateScript;

    public void ChangeCoordinatesToLondon()
    {
        coordinateScript.latitude = 51.5072;
        coordinateScript.longitude = -0.1275;
    }
}
