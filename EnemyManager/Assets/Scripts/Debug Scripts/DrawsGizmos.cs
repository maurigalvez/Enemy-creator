using UnityEngine;
using System.Collections;

public class DrawsGizmos : MonoBehaviour 
{
   public enum eShape
   {
      cube,
      sphere,
   }
   public bool isDrawing;              // True if its drawing, false otherwise.
   public eShape shape;                // Shape to draw
   public float shapeSize;             // Size of the shape to be drawn
   public Transform obj;               // Object with position
	// Use this for initialization
	void Start ()
   {
     
	}   
	
	// Update is called once per frame
	void Update () {}

   void OnDrawGizmos()
   {
      Gizmos.color = Color.red;
       // check if its suppose to draw
      if (isDrawing == true)
         draw();         
   }

   void draw()
   {     
      // draw chosen shape
      switch (shape)
      {
         case eShape.cube:            
            Gizmos.DrawCube(obj.position, new Vector3(shapeSize, shapeSize, shapeSize));
            break;

         case eShape.sphere:
            Gizmos.DrawSphere(obj.position, shapeSize);
            break;
      }// switch      
   }
   
}
