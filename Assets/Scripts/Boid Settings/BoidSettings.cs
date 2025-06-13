using UnityEngine;

[CreateAssetMenu(fileName = "Boid Settings", menuName = "Boid Settings", order = 1)]
public class BoidSettings : ScriptableObject
{
   [Range(0, 10)]
   public int CohesionRadius;
   [Range(0, 10)]
   public int AlignmentRadius;
   [Range(0, 10)]
   public int SeparationRadius;
   
   public LayerMask BoidLayer;
   
}
