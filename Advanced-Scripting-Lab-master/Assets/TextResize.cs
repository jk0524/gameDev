using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextResize : MonoBehaviour
{
   #region Editor Variables
   [SerializeField]
   [Range(0, 0.8f)]
   [Tooltip("How large the text should be.")]
   private float m_Scale;
   #endregion

   #region First Time Initialization and Set Up
   private void Awake()
   {
      StartCoroutine("Resize");
   }

   private IEnumerator Resize()
   {
      yield return new WaitForEndOfFrame();  // At the end of the frame the surrounding UI has stabilized and we can correctly scale
      Text myText = gameObject.GetComponent<Text>();
      myText.fontSize = (int)(gameObject.GetComponent<RectTransform>().rect.height * m_Scale);
   }
   #endregion
}