using UnityEngine;

public class GlasgowCoordinateChanger : MonoBehaviour
{
    public Coordinate coordinateScript;

    public void ChangeCoordinatesToGlasgow()
    {     
        coordinateScript.latitude = 55.8642;
        coordinateScript.longitude = -4.2518;
    }
}
