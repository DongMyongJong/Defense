using UnityEngine;
using System.Collections;

public class BulletTrace : MonoBehaviour {
	public float life = 0.5f;
	public float bulletSpeed = 1.0f;
	public GameObject dustPrefab;
	public GameObject bulletHolePrefab;
	
	private float destroyTime;
	public Vector3 velocity;
	private float gravity = 9.8f;
	// Use this for initialization
	void Start () {
		destroyTime = Time.time + life;
		velocity += transform.forward * bulletSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > destroyTime){
			Destroy(gameObject);
		}
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(ray, out hit, bulletSpeed * Time.deltaTime)){
			TriggerChildrenCollider triggerChildrenColliderScript = hit.transform.root.GetComponent<TriggerChildrenCollider>();
			bool reCheck = false; //Re-check if there's a hit for children collider.
			Collider mainColliderHit = hit.collider; //Parent collider. (must be re enabled)
			if(triggerChildrenColliderScript != null){ //Trigger children property. Enable children collider and disable root collider.
				hit.collider.enabled = false;
				Collider[] childrenColliderList = triggerChildrenColliderScript.childrenColliderList;
				for (int i = 0; i < childrenColliderList.Length; i++){
					childrenColliderList[i].enabled = true;
				}
				reCheck = Physics.Raycast(ray, out hit, bulletSpeed * Time.deltaTime); //Recheck collision for children collider.
			}
			if(reCheck || triggerChildrenColliderScript == null){ //If children collider or root collider are hit, process the bullet hit.
				Destroy(gameObject);
				bool makeDust = true;
				bool makeHole = true;
				BulletHoleProperty bulletHolePropertyScript =  hit.transform.root.GetComponent<BulletHoleProperty>();
				GameObject extraPrefab = null;
				float holeSize = 1.0f;
				Material[] differentBulletMaterialArray, bulletMaterials = null;
				if (bulletHolePropertyScript != null){ //Bullet hole property.
					makeDust = bulletHolePropertyScript.dust;
					makeHole = bulletHolePropertyScript.hole;
					extraPrefab = bulletHolePropertyScript.extraPrefab;
					holeSize = bulletHolePropertyScript.sizeModifier;
					bulletMaterials = bulletHolePropertyScript.bulletMaterials;
				}
				if (extraPrefab != null){ //Extra prefab.
					Instantiate(extraPrefab,transform.position,transform.rotation);
				}
				if (makeDust){ //Dust.
					GameObject newdustCloudGenerator = Instantiate(dustPrefab,hit.point - transform.forward * 0.1f, Quaternion.identity) as GameObject;
					newdustCloudGenerator.GetComponent<DustCloudGenerator>().velocity = hit.normal * (2 + Random.value * 10);
				}
				if (makeHole){ //Bullet hole.
					GameObject newBulletHole = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.01f, Quaternion.identity) as GameObject;
					newBulletHole.transform.LookAt(newBulletHole.transform.position + hit.normal);
					newBulletHole.transform.parent = hit.collider.transform;
					BulletHole bulletHoleScript = newBulletHole.GetComponent<BulletHole>();
					bulletHoleScript.size *= holeSize;
					if (bulletHolePropertyScript != null && bulletMaterials.Length > 0){
						bulletHoleScript.materials = bulletMaterials;
					}	
				}
				if(hit.rigidbody != null){ //Apply force.
					hit.rigidbody.AddForceAtPosition(velocity* 1.5f, hit.point);
				}
				Health healthScript = hit.transform.root.GetComponent<Health>(); //Health property.
				if(healthScript != null){
					healthScript.health -= 20;
					healthScript.SetLastHitTime();
					Vector3 hitPointNoHeight = hit.point;//hit.point;
					hitPointNoHeight.y = hit.transform.position.y;
					Vector3 hitDirection = hitPointNoHeight - hit.transform.position;
					hitDirection.Normalize();
					hitDirection = hit.transform.root.InverseTransformDirection(hitDirection);
					healthScript.SetHitDirection(hitDirection);
					Vector3 recoilDirection = hit.transform.position - hitPointNoHeight;
					recoilDirection.Normalize();
					healthScript.SetrecoilDirecion(recoilDirection);
				}
			}
			if(triggerChildrenColliderScript != null){//Trigger children property. Disable children collider and enable root collider.
				mainColliderHit.enabled = true;
				Collider[] childrenColliderList = triggerChildrenColliderScript.childrenColliderList;
				for (int n = 0; n < childrenColliderList.Length; n++){
					childrenColliderList[n].enabled = false;
				}
			}
		}
		velocity.y -= gravity * Time.deltaTime;
		transform.position += velocity * Time.deltaTime;
		transform.LookAt(transform.position + velocity);
	}
}
