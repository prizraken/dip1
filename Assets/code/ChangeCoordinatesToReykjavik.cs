using UnityEngine;

public class ReykjavikCoordinateChanger : MonoBehaviour
{
    public Coordinate coordinateScript;

    public void ChangeCoordinatesToReykjavik()
    {
        coordinateScript.latitude = 64.1466;
        coordinateScript.longitude = -21.9426;
    }
}
