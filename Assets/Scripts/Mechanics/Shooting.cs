using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;
using GreenByteSoftware.UNetController;

[System.Serializable]
public class Weapon {
	public float ammo;
	public float cooldown;
	public float ammo_clip_cur;
	public bool projectile;
	public float damage;
	public float damage_radius;
	public GameObject gameObject;

	public Weapon (float a, float cool, float acc, bool projec, float dmg, float dmg_rad, GameObject go) {
		ammo = a;
		cooldown = cool;
		ammo_clip_cur = acc;
		projectile = projec;
		damage = dmg;
		damage_radius = dmg_rad;
		gameObject = go;
	}
}

public class Shooting : NetworkBehaviour {

	public List<Weapon> weapons;
	public Controller controller;

	public Transform shootPlace;

	public Vector3 shootDir;

	[SyncVar]
	public bool vr = false;

	private int lastscroll;

	[SyncVar]
	public int weaponNum = 0;

	public override void OnStartLocalPlayer () {
		//vr = true;
		UIStatic.singleton.shootingStars = this;
	}

	void Update () {
		if (isServer && !vr)
			shootDir = Quaternion.Euler (controller.GetCamY(), transform.rotation.eulerAngles.y, 0) * Vector3.forward;
		
		if (!isLocalPlayer)
			return;

		//vr = true;

		if (CrossPlatformInputManager.GetAxisRaw ("Mouse ScrollWheel") > 0 && lastscroll != 1) {
			lastscroll = 1;
			CmdSetWeapon (weaponNum + 1);
		} else if (CrossPlatformInputManager.GetAxisRaw ("Mouse ScrollWheel") < 0 && lastscroll != -1) {
			lastscroll = -1;
			CmdSetWeapon (weaponNum - 1);
		} else {
			lastscroll = 0;
		}

		if (CrossPlatformInputManager.GetButtonDown ("Fire1")) {
			CmdShoot ();
			PlayShootEffects ();
		}
	}

	public void PlayShootEffects () {
		Debug.Log ("Effekt");
	}

	public void PlayShootEffects (int weaponNum, Vector3 pos) {
		if (!isLocalPlayer)
			PlayShootEffects ();
		Debug.Log ("boom " + pos);
		if (weapons [weaponNum].projectile)
			Destroy(GameObject.Instantiate (weapons [weaponNum].gameObject, pos, new Quaternion(0,0,0,1)), 10);
	}

	[Command]
	public void CmdSetWeapon (int wN) {
		weaponNum = wN;
		if (weaponNum > weapons.Count)
			weaponNum = 0;
		else if (weaponNum < 0)
			weaponNum = weapons.Count - 1;
	}

	[ClientRpc]
	public void RpcShootEffects (int weaponNum, Vector3 pos) {
		PlayShootEffects (weaponNum, pos);
	}

	[Command]
	public void CmdShoot () {
		if (!vr)
			Shoot (weaponNum, controller.camTargetFPS.transform.position, shootDir, controller.transform.rotation);
		else
			Shoot (weaponNum, shootPlace.position, shootPlace.forward, VRPlayer.VRCamera.camRoot.cam.transform.rotation);
	}

	public void Shoot (int wpNum, Vector3 pos, Vector3 dir, Quaternion rotation) {
		Debug.Log ("shoot " + pos + " " + dir);
		//if (!wpn.projectile) {
		RaycastHit hit;
		if (Physics.Raycast (pos, dir, out hit)) {
			Debug.Log (hit.transform);
			Debug.DrawLine (pos, hit.point, Color.red, 1f);
			if ((vr && hit.collider.tag == "Player") || hit.collider.tag == "Tower")
				hit.transform.root.GetComponent<HealthSystem> ().DoDamage (weapons[wpNum].damage);
				
			if (weapons[wpNum].projectile) {
				Collider[] cols = Physics.OverlapSphere (hit.point, weapons[wpNum].damage_radius);

				foreach (Collider col in cols) {
					if (col.transform.parent == null && ((vr && hit.collider.tag == "Player") || hit.collider.tag == "Tower"))
						hit.transform.root.GetComponent<HealthSystem> ().DoDamage (weapons[wpNum].damage);
				}
			}

			RpcShootEffects (wpNum, hit.point);
		}

		//} else if (isServer) {
			//GameObject go = GameObject.Instantiate (wpn.gameObject, pos, rotation);
			//go.GetComponent<NetworkProjectile> ().weapon = wpn;
			//NetworkServer.Spawn (go);
		//}
	}
}
