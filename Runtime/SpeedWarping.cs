using System.Linq;
using UnityEngine;

namespace Janskia.SpeedWarping
{
    public class SpeedWarping : MonoBehaviour
    {
        [Header("Model parts")]
        [SerializeField] private Transform leftFoot;
        [SerializeField] private Transform rightFoot;
        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;
        [SerializeField] private Transform leftShoulder;
        [SerializeField] private Transform rightShoulder;
        [SerializeField] private Transform legsOrigin;

        [Header("Tweakable parameters")]
        [Range(0f, 1f)]
        [SerializeField] private float footWeight = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float handWeight = 0.3f;
        [Range(0f, 1f)]
        [SerializeField] private float hipJump = 0.15f;
        [SerializeField] private Vector3 legsOriginOffset = new Vector3(-0.4f, 0.2f, 0f);

        [Header("Value")]
        [Range(0f, 5f)]
        [SerializeField] public float Spread;

        private Animator animator;

        private Vector3 LegsOriginWithOffset
        {
            get
            {
                return legsOrigin.position + legsOriginOffset;
            }
        }

        private float ScaledSpread
        {
            get
            {
                return Spread / transform.localScale.z;
            }
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Reset()
        {
            Setup();
        }

        [ContextMenu("Setup")]
        private void Setup()
        {
            animator = GetComponent<Animator>();

            SetupBone("LeftFoot", ref leftFoot);
            SetupBone("RightFoot", ref rightFoot);
            SetupBone("LeftHand", ref leftHand);
            SetupBone("RightHand", ref rightHand);
            SetupBone("Hips", ref legsOrigin);
            SetupBone("LeftShoulder", ref leftShoulder);
            SetupBone("RightShoulder", ref rightShoulder);

            void SetupBone(string humanName, ref Transform boneTransform)
            {
                var humanBone = animator.avatar.humanDescription.human.Single(x => x.humanName == humanName);

                Transform[] allChildren = transform.GetComponentsInChildren<Transform>();
                foreach (Transform child in allChildren)
                {
                    if (child.name == humanBone.boneName)
                    {
                        boneTransform = child;
                        return;
                    }
                }
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {
            Vector3 hipJumpOffset = Vector3.up * Mathf.Max(0, ScaledSpread - 1) * -hipJump * transform.InverseTransformPoint(leftFoot.position).z * transform.InverseTransformPoint(rightFoot.position).z * transform.localScale.z;

            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, footWeight);
            ProcessLimb(AvatarIKGoal.LeftFoot, leftFoot.position + hipJumpOffset, LegsOriginWithOffset + hipJumpOffset);

            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, footWeight);
            ProcessLimb(AvatarIKGoal.RightFoot, rightFoot.position + hipJumpOffset, LegsOriginWithOffset + hipJumpOffset);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, handWeight);
            ProcessLimb(AvatarIKGoal.LeftHand, leftHand.position + hipJumpOffset, leftShoulder.position + hipJumpOffset);

            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, handWeight);
            ProcessLimb(AvatarIKGoal.RightHand, rightHand.position + hipJumpOffset, rightShoulder.position + hipJumpOffset);

            animator.bodyPosition = animator.bodyPosition + hipJumpOffset;
        }

        private void ProcessLimb(AvatarIKGoal ikGoal, Vector3 limbPosition, Vector3 origin)
        {
            Vector3 localLimbPosition = transform.InverseTransformPoint(limbPosition);
            Vector3 localOrigin = transform.InverseTransformPoint(origin);

            Vector3 scaledPosition = new Vector3(localLimbPosition.x, localLimbPosition.y, localLimbPosition.z * ScaledSpread);

            Vector3 legVector = localLimbPosition - localOrigin;
            float legLength = legVector.magnitude;
            Vector3 newVector = scaledPosition - localOrigin;
            float newLength = newVector.magnitude;
            Vector3 finalPosition = Vector3.Lerp(localOrigin, scaledPosition, legLength / newLength);
            finalPosition = transform.TransformPoint(finalPosition);

            animator.SetIKPosition(ikGoal, finalPosition);
        }
    }
}
