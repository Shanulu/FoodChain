using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(player))] //always require player so we dont have to check for it
public class playerSetup : NetworkBehaviour {



    //list of components we want to disable for network behavoir
    [SerializeField]
    Behaviour[] componentsToDisable;

    Camera sceneCamera;

    [SerializeField]
    string remoteLayerName = "remotePlayer"; //our layer mask

    [SerializeField]
    string dontDrawLayerName = "dontDraw";
    [SerializeField]
    GameObject playerGraphics;
    

    void Start()
    {

        if (!isLocalPlayer) //are we the server if not...
        {
            DisableComponents(); //turn off components for networking
            AssignRemoteLayer(); //change our layer so we cant shoot ourselves
        }
        else
        {
            Debug.Log("We are the host");
            sceneCamera = Camera.main;
            //turn camera on and off(on disconnect)
            if (sceneCamera)
            {
                sceneCamera.gameObject.SetActive(false);
            }

            //disable Network Graphics for host
           SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName)); //calling a self calling method? could cause infinite loop

        }

        GetComponent<player>().Setup(); //Setup our player

    }

  void SetLayerRecursively(GameObject _obj, int _newLayer)
  {
        _obj.layer = _newLayer;
      foreach (Transform child in _obj.transform)
       {
          SetLayerRecursively(child.gameObject, _newLayer);
     }
 }


    //called everytime a client is setup locally
    public override void OnStartClient() //override the original
    {
        base.OnStartClient(); //run the original ??
        string _netID = GetComponent<NetworkIdentity>().netId.ToString(); //save our netid as a string
        player _player = GetComponent<player>(); //get our player entity
        gameManager.RegisterPlayer(_netID, _player); //pass them along
            
    }


    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++) //look through our list of components
        {
            componentsToDisable[i].enabled = false; //set it to false
        }
    }


    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName); //converts our string to our layer number and sets our layer
    }

    void OnDisable()
    {
        if (sceneCamera)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        gameManager.UnregisterPlayer(transform.name);
    }


}
