using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WasaaMP {
public class CursorTool : MonoBehaviourPun {
	float x, previousX = 0 ;
	float y, previousY = 0 ;
	float z, lastZ ;
	public bool active ;

	private bool caught ;

	public Interactive interactiveObjectToInstanciate ;
	private GameObject target ;
	Rigidbody rb;
	MonoBehaviourPun targetParent ;
	protected Vector3 positionInit, positionEnd, torusPosition;
	Transform gameObjectL, gameObjectM, gameObjectR;
	Transform tbTransform;
	bool canBeCatched, canBeReleased = false;
	Transform cylinder;
	public Transform[] torusList;
	private List<float> torusLength;
	float maxScale, minScale;
	static public int notice = 0;
	static public int moves = 0;
	static public int torusId = 3;
	bool updateTorusList = true;

	MonoBehaviourPun player ; 

	void Start () {
		active = false ;
		caught = false ;
		player = (MonoBehaviourPun)this.GetComponentInParent(typeof (Navigation)) ;
		name = player.name + "_" + name ;
		gameObjectL = GameObject.Find("GameObjectL").transform;
		gameObjectM = GameObject.Find("GameObjectM").transform;
		gameObjectR = GameObject.Find("GameObjectR").transform;
		var torus = gameObjectL.GetComponentsInChildren<Transform>(true);
		torusList = new Transform[6];
		for (int i = 0; i <= torusId + 1; i++) {
			if (torus[i].name != "GameObjectL") {
				torusList[i-1] = torus[i];
			}
        }
	}
	
	void Update () {
		// control of the 3D cursor
		if (player.photonView.IsMine  || ! PhotonNetwork.IsConnected) {
			if (Input.GetButtonDown ("Fire1")) {
				Fire1Pressed (Input.mousePosition.x, Input.mousePosition.y) ;
			}
			if (Input.GetButtonUp ("Fire1")) {
				Fire1Released (Input.mousePosition.x, Input.mousePosition.y) ;
			}
			if (active) {
				Fire1Moved (Input.mousePosition.x, Input.mousePosition.y, Input.mouseScrollDelta.y) ;
			}
			if (Input.GetKeyDown (KeyCode.C)) {
				CreateInteractiveCube () ;
			}
			if (Input.GetKeyDown (KeyCode.B)) {
				Catch () ;
			}
			if (Input.GetKeyDown (KeyCode.N)) {
				Release () ;
				target = null ;
			}
            if (updateTorusList == true) {
				UpdateTorusList();
			}
		}
	}

	public void Fire1Pressed (float mouseX, float mouseY) {
		active = true ;
		x = Input.mousePosition.x;
		previousX = x ;
		y = Input.mousePosition.y;
		previousY = y ;
	}

	public void Fire1Released (float mouseX, float mouseY) {
		active = false ;
	}

	public void Fire1Moved (float mouseX, float mouseY, float mouseZ) {
		x = mouseX ;
		float deltaX = (x - previousX) / 130.0f ;
		previousX = x ;
		y = mouseY ;
		float deltaY = (y - previousY) / 130.0f ;
		previousY = y ;
		float deltaZ = mouseZ / 10.0f ;
		transform.Translate (deltaX, deltaY, deltaZ) ;
	}

	public void Catch () {
		print ("B ?") ;
		if (target != null && canBeCatched == true) {
			print("B :");
			var tb = target.GetComponent<Interactive>();
			rb = tb.GetComponent<Rigidbody>();
			rb.isKinematic = true;
			positionInit = target.transform.position;
			if (tb != null) {
				if ((!caught) && (this != tb.GetSupport())) { // pour ne pas prendre 2 fois l'objet et lui faire perdre son parent
					targetParent = tb.GetSupport();
				}
				print("ChangeSupport of object " + tb.photonView.ViewID + " to " + photonView.ViewID);
				photonView.RPC("ChangeSupport", RpcTarget.All, tb.photonView.ViewID, photonView.ViewID);
				tb.photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
				PhotonNetwork.SendAllOutgoingCommands();
				caught = true;
				print("B !");
			}
		} else if (target != null && canBeCatched == false) {
			notice = 1;
		} else {
			print ("pas B") ;
        }
	}

	public void Release () {
		if (target != null) {
			print ("N :") ;
			var tb = target.GetComponent<Interactive> () ;
			rb = tb.GetComponent<Rigidbody>();
			rb.isKinematic = false;
			if (tb != null) {
				if (targetParent != null) {
					photonView.RPC ("ChangeSupport", RpcTarget.All, tb.photonView.ViewID, targetParent.photonView.ViewID) ;
					targetParent = null ;
				} else {
					photonView.RPC ("RemoveSupport", RpcTarget.All, tb.photonView.ViewID) ;
				}
				PhotonNetwork.SendAllOutgoingCommands () ;
				print ("N !") ;
				caught = false ;
				

				positionEnd = target.transform.position;
				if (positionEnd.x > -5 && positionEnd.x < -3 && positionEnd.z > 4 && positionEnd.z < 6) {
					target.transform.position = new Vector3(-4, positionEnd.y, 5);
					target.transform.SetParent(gameObjectL, true);
					CanBeRelease(target);
				} else if (positionEnd.x > -1 && positionEnd.x < 1 && positionEnd.z > 4 && positionEnd.z < 6) {
					target.transform.position = new Vector3(0, positionEnd.y, 5);
					target.transform.SetParent(gameObjectM, true);
					CanBeRelease(target);
				} else if (positionEnd.x > 3 && positionEnd.x < 5 && positionEnd.z > 4 && positionEnd.z < 6) {
					target.transform.position = new Vector3(4, positionEnd.y, 5);
					target.transform.SetParent(gameObjectR, true);
					CanBeRelease(target);
				} else {
					target.transform.position = positionInit;
					if (positionInit.x > -5 && positionInit.x < -3) {
						target.transform.SetParent(gameObjectL, true);
					} else if (positionInit.x > -1 && positionInit.x < 1) {
						target.transform.SetParent(gameObjectM, true);
					} else if (positionInit.x > 3 && positionInit.x < 5) {
						target.transform.SetParent(gameObjectR, true);
					}
                }
				moves++;
			}
		} else {
			print ("pas N") ;
		}
	}

	public void CreateInteractiveCube () {
		if (torusId < 5) {
			var torusRotation = Quaternion.Euler(new Vector3(90, 0, 0));
			var torusPosition = new Vector3(-4, 2, 5);
			var objectToInstanciate = PhotonNetwork.Instantiate(interactiveObjectToInstanciate.name, torusPosition, torusRotation, 0);
			objectToInstanciate.transform.localScale = new Vector3(torusList[torusId].localScale.x - 0.1f, torusList[torusId].localScale.y - 0.1f, 0.5f);
			objectToInstanciate.transform.SetParent(gameObjectL, true);
			torusId++;
			torusList[torusId] = objectToInstanciate.transform;
			notice = 3;
		} else {
			notice = 4;
        }
	}

	void OnTriggerEnter (Collider other) {
		updateTorusList = false;
		print (name + " : OnTriggerEnter") ;
		target = other.gameObject ;
		cylinder = target.transform.parent.gameObject.transform;
		torusLength = new List<float>();
		foreach(Transform torus in cylinder.GetComponentsInChildren<Transform>(true)) {
			if (torus.localScale.x != 1) {
				torusLength.Add(torus.localScale.x);
			}
		}
		if (target.transform.localScale.x == GetMinScale(torusLength)) {
			canBeCatched = true;
		} 
	}

	void OnTriggerExit (Collider other) {
		updateTorusList = true;
		print (name + " : OnTriggerExit") ;
		target = null ;
		canBeCatched = false;
		canBeReleased = false;
	}

	float GetMinScale (List<float> torusLength) {
		minScale = 1;
		foreach (float num in torusLength) {
			if (num < minScale) {
				minScale = num;
			}
        }
		return minScale;
    }

/*	float GetMaxScale (List<float> torusLength) {
		maxScale = 0;
		foreach (float num in torusLength) {
			if (num > maxScale) {
				maxScale = num;
			}
		}
		return maxScale;
	}*/

	void CanBeRelease (GameObject trajet) {
		cylinder = target.transform.parent.gameObject.transform;
		torusLength = new List<float>();
		foreach (Transform torus in cylinder.GetComponentsInChildren<Transform>(true)) {
			if (torus.localScale.x != 1) {
				torusLength.Add(torus.localScale.x);
			}
		}
		notice = 0;
		if (target.transform.localScale.x != GetMinScale(torusLength)) {
			target.transform.position = positionInit;
			notice = 2;
			if (positionInit.x > -5 && positionInit.x < -3) {
				target.transform.SetParent(gameObjectL, true);
			} else if (positionInit.x > -1 && positionInit.x < 1) {
				target.transform.SetParent(gameObjectM, true);
			} else if (positionInit.x > 3 && positionInit.x < 5) {
				target.transform.SetParent(gameObjectR, true);
			}
		}
	}

	void UpdateTorusList () {
		for (int i = 0; i <= torusId; i++) {
			torusPosition = torusList[i].position;
			if (torusPosition.x > -5 && torusPosition.x < -3) {
				torusList[i].SetParent(gameObjectL, true);
			} else if (torusPosition.x > -1 && torusPosition.x < 1) {
				torusList[i].SetParent(gameObjectM, true);
			} else if (torusPosition.x > 3 && torusPosition.x < 5) {
				torusList[i].SetParent(gameObjectR, true);
			}
		}
    }

	[PunRPC] public void ChangeSupport (int interactiveID, int newSupportID) {
        Interactive go = PhotonView.Find (interactiveID).gameObject.GetComponent<Interactive> () ;
        MonoBehaviourPun s = PhotonView.Find (newSupportID).gameObject.GetComponent<MonoBehaviourPun> () ;
        print ("ChangeSupport of object " +  go.name + " to " + s.name) ;
        go.SetSupport (s) ;
    }

    [PunRPC] public void RemoveSupport (int interactiveID) {
        Interactive go = PhotonView.Find (interactiveID).gameObject.GetComponent<Interactive> () ;
        print ("RemoveSupport of object " +  go.name) ;
        go.RemoveSupport () ;
    }

}
}