using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
   #region Editor Variables
   [SerializeField]
   [Tooltip("The effects that should be played whenever the player clicks somewhere.")]
   private ParticleSystem m_Particles;
   #endregion

   #region Main Updates
   private void Update()
   {
      if (Input.GetMouseButtonDown(0))
      {
         Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         wp.z = 0;

         ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
         emitParams.position = wp;
         emitParams.applyShapeToPosition = true;
         m_Particles.Emit(emitParams, 100);

         Collider2D[] allOverlaps = Physics2D.OverlapCircleAll(wp, 1.1f);
         Debug.Log(string.Format("Hit {0} objects", allOverlaps.Length));
         for (int i = 0; i < allOverlaps.Length; i++)
         {
            GameObject target = allOverlaps[i].gameObject;
            //Do your thing here.
            if (target.tag == "Good Enemy") {
               Score.Singleton.AddScore(10);
            } else if (target.tag == "Bad Enemy") {
               Score.Singleton.AddScore(-10000);
            }
            Destroy(target);
            
         }
      }
   }
   #endregion
}
