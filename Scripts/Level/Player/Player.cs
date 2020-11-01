using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField] private byte IdOfBlockInHand = 1; 
    [SerializeField] private BlockSelectingType DeformMode;
    [SerializeField] private CameraMovement cameraMovement;
    private byte controlMod = 0;
    private Vector3 globalPointer;
    private GameObject DATA;
    private VoxelMapGenerator VMG;
    private Vector2 firstScreenTouh;
    private Vector2 firstScreenTouhWheel;
    private Vector2 screenSize = new Vector2(Screen.width, Screen.height);


    void Start()
    {
        firstScreenTouh = -Vector3.zero;
        firstScreenTouhWheel = -Vector3.zero;
        DATA = GameObject.FindGameObjectWithTag("DATA");
        VMG = DATA.GetComponent<VoxelMapGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        PcControls();
    }
    private void PcControls()
    {
       
        
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (IsPointerOverUIObject())
                        return;

                    globalPointer = hit.point;

                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        Deformator.InstantiateBlockToChunk(BlockSelectingType.ThisBlock, 0, ref globalPointer, ref VMG);
                    }
                    else
                    {
                        Deformator.InstantiateBlockToChunk(BlockSelectingType.OutOfThisBlock, IdOfBlockInHand, ref globalPointer, ref VMG);
                    }
                }
            } 
        
    
            /*if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                firstScreenTouh = NormalizedMousePos();          
            }
            if (Input.GetKeyUp(KeyCode.Mouse1)) 
            {
                firstScreenTouh = -Vector2.zero;
            }      
            if (firstScreenTouh != -Vector2.zero) 
            {
                Vector2 nmp = NormalizedMousePos();
                cameraMovement.AddToNormalizedAngle((nmp.x - firstScreenTouh.x) * cameraMovement.GetSensitivity() * Time.deltaTime);
                firstScreenTouh = nmp;
            }     
             */
                cameraMovement.AddToNormalizedAngle(0.01f * cameraMovement.GetSensitivity() * Time.deltaTime);
            if(Input.GetKeyDown(KeyCode.Mouse2))
            {
                firstScreenTouhWheel = NormalizedMousePos();                      
            }
             if(Input.GetKeyUp(KeyCode.Mouse2))
            {
                firstScreenTouhWheel = -Vector2.zero;                      
            } 
            if (firstScreenTouhWheel != -Vector2.zero) 
            {
                Vector2 nmp = NormalizedMousePos();
                Vector3 camPos = cameraMovement.GameCamera.transform.position;
                camPos.y = 0;
                Vector3 centerPos = cameraMovement.CenterOfView.transform.position;
                centerPos.y = 0;
                Vector3 cameraDirectionY = (centerPos - camPos);
                Vector3 cameraDirectionX = Quaternion.Euler(new Vector3(0 , 90 , 0)) * cameraDirectionY / 2;
                Vector3 OffsetVector = (firstScreenTouhWheel.y - nmp.y) * (cameraDirectionY);
                OffsetVector += (firstScreenTouhWheel.x - nmp.x) * (cameraDirectionX);
                OffsetVector *= cameraMovement.GetMoveSensitivity() * Time.deltaTime;
                cameraMovement.CenterOfView.transform.position += OffsetVector;
                firstScreenTouhWheel = nmp;
            }

            
 
            cameraMovement.AddRange(-Input.mouseScrollDelta.y * cameraMovement.GetZoomSensitivity());
            cameraMovement.UpdateCameraTransform();
            
    }
    private  Vector2 NormalizedMousePos() 
    {
        return (Input.mousePosition / screenSize) + (-Vector2.zero / 2);
    }
    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
     public void ChangeDeformMode()
    {
        DeformMode = (DeformMode == BlockSelectingType.ThisBlock) ? BlockSelectingType.OutOfThisBlock : BlockSelectingType.ThisBlock;
        
    }
     public void SetBlockID(int ID) 
    {
        if (ID > VMG.worldResources.Resources.Length || ID <= 0)
        {
            return; 
        }
        IdOfBlockInHand = (byte)ID;
    }
}
