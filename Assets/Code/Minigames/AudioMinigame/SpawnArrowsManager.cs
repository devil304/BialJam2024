using System;
using UnityEngine;

public class SpawnArrowsManager : MonoBehaviour
{
		[SerializeField] Transform ArrowLeftSpawnPoint;
		[SerializeField] Transform ArrowUpSpawnPoint;
		[SerializeField] Transform ArrowRightSpawnPoint;
		[SerializeField] Transform ArrowDownSpawnPoint;
		[SerializeField] Transform ArrowPrefab;

		// [SerializeField]
		private float spawnTime = 1f;
		private float lastSpawnTime;

    // Update is called once per frame
    void Update()
    {
        lastSpawnTime += Time.deltaTime;
				if(spawnTime <= lastSpawnTime) {
					lastSpawnTime = 0;
					SpawnNewArrow();
				}
    }

		void SpawnNewArrow() {
			int arrowIndex = UnityEngine.Random.Range(0, 4);
			Vector3 newArrowPosition = new Vector3();
			Quaternion newArrowRotation = Quaternion.identity;
			switch(arrowIndex) {
				case 0: //Spawn Left Arrow in Left Direction
					newArrowPosition = ArrowLeftSpawnPoint.position;
					newArrowRotation = Quaternion.Euler(0, 0, -180);
				break;
				case 1: //Spawn Up Arrow in Up Direction
					newArrowPosition = ArrowUpSpawnPoint.position;
					newArrowRotation = Quaternion.Euler(0, 0, 90);
				break;
				case 2: //Spawn Right Arrow in Right Direction
					newArrowPosition = ArrowRightSpawnPoint.position;
				break;
				case 3: //Spawn Down Arrow in Down Direction
					newArrowPosition = ArrowDownSpawnPoint.position;
					newArrowRotation = Quaternion.Euler(0, 0, -90);
				break;
			}
			Instantiate(ArrowPrefab, newArrowPosition, newArrowRotation);
		}
}
