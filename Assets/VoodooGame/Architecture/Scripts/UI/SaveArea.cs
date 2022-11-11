using UnityEngine;

namespace UI
{
    public class SaveArea : MonoBehaviour
    {
        private void Awake()
        {
            UpdateSaveArea();
        }

        private void UpdateSaveArea()
        {
            var saveArea = Screen.safeArea;
            var myRectTransform = GetComponent<RectTransform>();

            var anchorMin = saveArea.position;
            var anchorMax = saveArea.position + saveArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            myRectTransform.anchorMin = anchorMin;
            myRectTransform.anchorMax = anchorMax;
        }
    }
}
