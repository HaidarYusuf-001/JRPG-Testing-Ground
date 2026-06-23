using UnityEngine;
using JRPG.Core;

namespace JRPG.Overworld
{
    // Mengendalikan pergerakan karakter di map dan memicu random encounter berdasarkan jarak tempuh.
    [RequireComponent(typeof(CharacterController))]
    public class OverworldPlayerController : MonoBehaviour
    {
        public float MoveSpeed = 5f;
        public float EncounterDistanceThreshold = 10f;
        public float EncounterChance = 15f;
        public string CombatSceneName = "Scene_Combat";

        private CharacterController controller;
        private Vector3 moveDirection;
        private float distanceMoved = 0f;
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
            if (isEncounterTriggered) return;

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
            float distanceThisFrame = Vector3.Distance(transform.position, lastPosition);

            if (distanceThisFrame > 0)
            {
                distanceMoved += distanceThisFrame;
                lastPosition = transform.position;

                if (distanceMoved >= EncounterDistanceThreshold)
                {
                    distanceMoved = 0f;
                    RollForEncounter();
                }
            }
        }

        private void RollForEncounter()
        {
            float roll = Random.Range(0f, 100f);

            if (roll <= EncounterChance)
            {
                Debug.Log("Random Encounter Triggered! Memasuki pertarungan...");
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
            else
            {
                Debug.LogError("SceneTransitionManager tidak ditemukan.");
            }
        }
    }
}