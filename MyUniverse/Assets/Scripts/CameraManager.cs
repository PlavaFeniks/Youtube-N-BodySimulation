using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Absolutly disgusting code that was thrown together for testing and used to swap player/ship perspective
public class CameraManager : MonoBehaviour
{
    public enum CameraDictatorShip { orbit, player, ship, goblin, cinematic};
    public CameraDictatorShip putin;
    public Camera orbitCamera;
    public Camera GoblinView;
    public Camera cinematic;
    SpaceSuitControls spc;
    SpaceShip ss;
    public void Start()
    {
        spc = FindObjectOfType<SpaceSuitControls>();
        ss = FindObjectOfType<SpaceShip>();
        if (putin==CameraDictatorShip.orbit)
        {
            MoveCamToOrbit();
        }
        else if (putin==CameraDictatorShip.ship)
        {
            MoveCamToShip();
        }
        else if (putin==CameraDictatorShip.player)
        {
            MoveCamToPlayer();
        }
        else if (putin==CameraDictatorShip.goblin)
        {
            MoveCamToGoblin();
        }
        else
        {
            MoveCamToCinematic();
        }
    }

    private void Update()
    {
        if (Input.GetKey("e")) {
            if (putin == CameraDictatorShip.ship)
            {
                MoveCamToPlayer();
                putin = CameraDictatorShip.player;
                spc.transform.position = ss.transform.position;
            }
            else if (putin == CameraDictatorShip.player)
            {
                MoveCamToShip();
                putin = CameraDictatorShip.ship;
            }
            else
            {
                MoveCamToPlayer();
                putin = CameraDictatorShip.player;
                spc.transform.position = ss.transform.position;
            }
        }
    }

    public void MoveCamToPlayer()
    {
        if (spc)
        {
            spc.cam.gameObject.SetActive(true);
            spc.movementActive = true;
        }
        if (ss)
        {
            ss.cam.gameObject.SetActive(false);
            ss.movementActive = false;
        }
        if (orbitCamera)
        {
            orbitCamera.gameObject.SetActive(false);
        }
        if(GoblinView)
        {
            GoblinView.gameObject.SetActive(false);
        }
    }

    public void MoveCamToShip()
    {
        if (ss)
        {
            ss.cam.gameObject.SetActive(true);
            ss.movementActive = true;
        }
        if (orbitCamera)
        {
            orbitCamera.gameObject.SetActive(false);
        }
        if (spc)
        {
            spc.cam.gameObject.SetActive(false);
            spc.movementActive = false;
        }
        if(GoblinView)
        {
            GoblinView.gameObject.SetActive(false);
        }
        if(cinematic)
        {
            cinematic.gameObject.SetActive(false);
        }
    }

    public void MoveCamToOrbit()
    {
        if (orbitCamera)
        {
            orbitCamera.gameObject.SetActive(true);
        }
        if (ss)
        {
            ss.cam.gameObject.SetActive(false);
            ss.movementActive = false;
        }  
        if (spc)
        {
            spc.cam.gameObject.SetActive(false);
            spc.movementActive = false;
        }
        if(GoblinView)
        {
            GoblinView.gameObject.SetActive(false);
        }
        if(cinematic)
        {
            cinematic.gameObject.SetActive(false);
        }
    }

    public void MoveCamToGoblin() {
        if (orbitCamera)
        {
            orbitCamera.gameObject.SetActive(false);
        }
        if (ss)
        {
            ss.cam.gameObject.SetActive(false);
            ss.movementActive = false;
        }  
        if (spc)
        {
            spc.cam.gameObject.SetActive(false);
            spc.movementActive = false;
        }
        if(GoblinView)
        {
            GoblinView.gameObject.SetActive(true);
        }
        if(cinematic)
        {
            cinematic.gameObject.SetActive(false);
        }
        
    }
    public void MoveCamToCinematic()
    {
        if (orbitCamera)
        {
            orbitCamera.gameObject.SetActive(false);
        }
        if (ss)
        {
            ss.cam.gameObject.SetActive(false);
            ss.movementActive = false;
        }  
        if (spc)
        {
            spc.cam.gameObject.SetActive(false);
            spc.movementActive = false;
        }
        if(GoblinView)
        {
            GoblinView.gameObject.SetActive(false);
        }
        if(cinematic)
        {
            cinematic.gameObject.SetActive(true);
            spc.movementActive = true;
            ss.movementActive = true;
        }
    }
}
