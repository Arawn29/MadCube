using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CameraSys : MonoBehaviour
{
    // Kamera Hareketleri 
    [Header("CameraSys")]
    Camera maincamera;
    GameObject Target;
    [Tooltip("Kameranýn Hedefi Takip Etme Hýzý")]
    public float CameraMoveSpeed;

    public float Sensivity;

    [Tooltip("Mouse kaydýrma hýzýnýn algýlama süresi")]
    public float CameraRotateSensivity;

    [Tooltip("Mouse kaydýrma hýzýnýn algýlama mesafesi ")]
    public float CameraRotateDistance;

    [Tooltip("Kameranýn Dönüþ Hýzý")]
    public float CameraRotationSpeed;

    private delegate void RotateCamView();
    [SerializeField] RotateCamView rotateCamView; // Rotate Inputs RotateDirection
 
    float EndTheDrag;
    float StartTheDrag;
    bool isCameraRotating;

    void TryFindPlayer()
    {
        Target = GameObject.FindGameObjectWithTag("Player");
    }
    private void Start()
    {
        maincamera = transform.GetChild(0).GetComponent<Camera>();
        Target = GameObject.FindGameObjectWithTag("Player");
        // Delegates 
            rotateCamView += Roll;

    }
    private void Update()
    {
        //Player Bulmayý Dene
        if (Target == null) { TryFindPlayer(); }
        // Kamera Pozisyonunu Güncelle
        UpdateCamera();
        // Kamera rotate
       if (Input.GetMouseButton(0)&&!isCameraRotating)
        {
            rotateCamView();
        }
        


    }
    void UpdateCamera()
    {
        Vector3 movecamera = Vector3.Lerp(transform.position, Target.transform.position, Time.deltaTime * CameraMoveSpeed);
        transform.position = movecamera;
    }
    void Roll()
    {
        StartCoroutine(DragTest());
    }
     IEnumerator  DragTest()
    {

        isCameraRotating = true;
            float elapsedTime = 0f;
            StartTheDrag = Input.mousePosition.x;
          // Debug.Log("StartDrag : "+StartTheDrag);
            while (elapsedTime <CameraRotateSensivity)
            {
             elapsedTime += Time.deltaTime;
             yield return null;
             EndTheDrag = Input.mousePosition.x;
          //  Debug.Log("EndDrag :"+EndTheDrag);
            
            }
            elapsedTime = 0f;
        if (Mathf.Abs(EndTheDrag) - Mathf.Abs(StartTheDrag) > CameraRotateDistance || Mathf.Abs(StartTheDrag) - Mathf.Abs(EndTheDrag) > CameraRotateDistance)
        {
            
            
            while (elapsedTime < CameraRotationSpeed)
            {
                elapsedTime += Time.deltaTime;

                int x = Mathf.Abs(EndTheDrag) - Mathf.Abs(StartTheDrag) > 0 ? 1 : -1;
                float Axis = RotateDirection(x);

                Vector3 rotation = new Vector3(0f, Axis + transform.rotation.y, 0f);
                Quaternion FinalRotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotation) * transform.rotation, Time.deltaTime / CameraRotationSpeed);
                transform.rotation = FinalRotation;

              
                yield return null;

            }
            Vector3 yuvarlama = Yuvarlama(transform.rotation);
            transform.rotation = Quaternion.Euler(yuvarlama);
            
            
        }
        elapsedTime = 0f;
        isCameraRotating = false;
    }

    float RotateDirection(int x)
    {                 
        float Degree = x<0 ? 90 : -90;
        return Degree;
    }
  
    Vector3 Yuvarlama(Quaternion FinalRotation)
    {
        Vector3 Aci = FinalRotation.eulerAngles;
        Debug.Log(Aci.ToString());
       // int kat = (int)(Aci.y / 90f); // Bu neymiþ ya 269 da 2 olarak alýyor 180 e götürüyor
        int kat = Mathf.RoundToInt((Aci.y / 90f)); 
        Debug.Log(kat.ToString());
        
        
        return new Vector3(0f, kat*90f, 0f);
    }





    /* void RotateCamera()
     {




         Vector3 rotate = new Vector3(camerax, cameray, 0);
         Quaternion EulerAngles = Quaternion.Euler(rotate);
         float AngleDiff = EulerAngles.y % 90f;
         if (AngleDiff < 10) 
         {
             float elapsedTime = 0f;
             while (elapsedTime < Time.deltaTime * CameraRotateSpeed) 
             {
                 elapsedTime += Time.deltaTime;
             Quaternion RotateAngle = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotate), Time.deltaTime * CameraRotateSpeed);
                 Debug.Log("While içerisinde" +elapsedTime.ToString("0"));


             transform.localRotation = RotateAngle;
             }

         }
     }
        */



}


