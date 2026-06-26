using UnityEngine;
using JRPG.Core;

namespace JRPG.Exploration
{
    // Mengendalikan pergerakan karakter dan mengeksekusi random encounter berdasarkan langkah di zona rumput.
    [RequireComponent(typeof(CharacterController))]
    public class OverworldPlayerController : MonoBehaviour
    {
        public float MoveSpeed = 5f;
        public string CombatSceneName = "Scene_Combat";

        [Header("Encounter Settings")]
        public LayerMask GrassLayer;
        public float StepDistance = 1.5f;
        public float EncounterChancePerStep = 10f;
        public float RaycastDistance = 5f;

        private CharacterController controller;
        private Vector3 moveDirection;
        private float stepAccumulator = 0f;
        private Vector3 lastPosition;
        private bool isEncounterTriggered = false;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Start()
        {
            if (PersistentPlayerData.Instance != null && PersistentPlayerData.Instance.HasSavedPosition)
            {
                controller.enabled = false;
                transform.position = PersistentPlayerData.Instance.LastMapPosition;
                controller.enabled = true;
            }

            lastPosition = transform.position;
        }

        private void Update()
        {
            if (isEncounterTriggered || Time.timeScale == 0f) return;

            HandleMovement();
            CheckEncounters();
        }

        private void HandleMovement()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

            if (moveDirection.magnitude >= 0.1f)
            {
                controller.Move(moveDirection * MoveSpeed * Time.deltaTime);
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        private void CheckEncounters()
        {
            Vector3 rayOrigin = transform.position + Vector3.up;
            bool isInGrass = false;

            // Tembakkan laser penembus ke semua objek di bawah
            RaycastHit[] hits = Physics.RaycastAll(rayOrigin, Vector3.down, RaycastDistance);

            float closestDistance = float.MaxValue;
            GameObject closestFloor = null;

            foreach (var hit in hits)
            {
                // Abaikan tabrakan dengan collider karakter kita sendiri
                if (hit.collider.gameObject == gameObject || hit.collider.transform.IsChildOf(transform))
                    continue;

                // Cari pijakan yang jaraknya paling dekat dengan kaki pemain
                if (hit.distance < closestDistance)
                {
                    closestDistance = hit.distance;
                    closestFloor = hit.collider.gameObject;
                }
            }

            // Jika ada lantai yang diinjak
            if (closestFloor != null)
            {
                // Cek apakah layer lantai teratas tersebut adalah rumput
                if (((1 << closestFloor.layer) & GrassLayer.value) != 0)
                {
                    isInGrass = true;
                }
            }

            Debug.DrawRay(rayOrigin, Vector3.down * RaycastDistance, isInGrass ? Color.green : Color.red);

            float distanceThisFrame = Vector3.Distance(transform.position, lastPosition);
            if (distanceThisFrame > 0)
            {
                if (isInGrass)
                {
                    stepAccumulator += distanceThisFrame;
                    if (stepAccumulator >= StepDistance)
                    {
                        stepAccumulator -= StepDistance;
                        RollForEncounter();
                    }
                }
                else
                {
                    stepAccumulator = 0f;
                }
            }

            lastPosition = transform.position;
        }

        private void RollForEncounter()
        {
            float roll = Random.Range(0f, 100f);
            if (roll <= EncounterChancePerStep)
            {
                Debug.Log("Random Encounter di area rumput Triggered!");
                isEncounterTriggered = true;
                TriggerCombat();
            }
        }

        private async void TriggerCombat()
        {
            if (PersistentPlayerData.Instance != null)
            {
                PersistentPlayerData.Instance.LastMapPosition = transform.position;
                PersistentPlayerData.Instance.HasSavedPosition = true;
            }

            if (SceneTransitionManager.Instance != null)
            {
                await SceneTransitionManager.Instance.LoadSceneAsync(CombatSceneName);
            }
        }
    }
}