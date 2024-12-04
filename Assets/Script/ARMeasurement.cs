using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARMeasurement : MonoBehaviour
{
    public ARRaycastManager raycastManager; // Yüzeye raycast yapmak için
    public GameObject pointPrefab; // İşaret noktasının prefab’i

    private List<GameObject> points = new List<GameObject>(); // Oluşturulan noktaları tutar

    void Update()
    {
        // Kullanıcı ekrana dokunduğunda
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                
                // Dokunulan konumu raycast yaparak kontrol et
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;

                    // İşaret noktasını oluştur ve listeye ekle
                    GameObject point = Instantiate(pointPrefab, hitPose.position, hitPose.rotation);
                    points.Add(point);

                    // En az iki nokta varsa mesafeyi hesapla
                    if (points.Count >= 2)
                    {
                        CalculateDistance();
                    }
                }
            }
        }
    }

    private void CalculateDistance()
    {
        // İki nokta arasındaki mesafeyi hesapla
        if (points.Count >= 2)
        {
            Vector3 start = points[points.Count - 2].transform.position;
            Vector3 end = points[points.Count - 1].transform.position;
            float distance = Vector3.Distance(start, end);

            // Mesafeyi göster
            Debug.Log("Mesafe: " + distance + " metre");
        }
    }
}
