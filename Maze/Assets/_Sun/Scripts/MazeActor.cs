using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleMaze
{
    public class MazeActor : MonoBehaviour
    {
        MouseFollower mouseFollower;
        SpriterDotNetUnity.SpriterDotNetBehaviour spriter;

        void Awake()
        {
            spriter = GetComponent<SpriterDotNetUnity.SpriterDotNetBehaviour>();
        }
        void Start()
        {
            mouseFollower = new MouseFollower(GetComponent<Rigidbody2D>());
            mouseFollower.onRigidBodyState += (state) =>
            {
                if (state == MouseFollower.State.Idle) spriter.Animator.Speed = 0;
                else spriter.Animator.Speed = 1;
            };
        }

        void FixedUpdate()
        {
            mouseFollower.UpdateRigidbody();

        }
        void OnTriggerEnter2D(Collider2D collider)
        {
            Destroy(collider.gameObject);
        }


        /// <summary>
        /// - Move rigidbody with mouse.
        /// - Flip rigidbody left / right
        /// - Update rigidbody state (Idle/Move)
        /// </summary>
        public class MouseFollower
        {
            Rigidbody2D rigidbody;
            Transform transform;
            Vector3 previousPosition;
            float speed = 0;

            readonly Vector3 right = new Vector3(1, 1, 1);
            readonly Vector3 left = new Vector3(-1, 1, 1);

            public delegate void OnUpdate(State state);
            public OnUpdate onRigidBodyState;


            public MouseFollower(Rigidbody2D rigidbody)
            {
                this.rigidbody = rigidbody;
                this.transform = rigidbody.transform;
                right = Vector3.Scale(transform.localScale, right);
                left = Vector3.Scale(transform.localScale, left);
            }
            public void UpdateRigidbody()
            {
                Vector3 position = MouseToWorldPosition();
                if (Vector3.Distance(position, transform.position) > 0.3f)
                {
                    if (position.x > transform.position.x) transform.localScale = right;
                    else transform.localScale = left;
                }
                rigidbody.MovePosition(position);
                if (Vector3.Distance(previousPosition, transform.position) == 0)
                {
                    speed += Time.fixedDeltaTime;
                    if (speed > 0.1f)
                    {
                        if (onRigidBodyState != null) onRigidBodyState(State.Idle);
                    }
                }
                else
                {
                    if (onRigidBodyState != null) onRigidBodyState(State.Move);
                }
                previousPosition = rigidbody.transform.position;
            }

            Vector3 MouseToWorldPosition()
            {
                Vector3 pos = Input.mousePosition;
                pos.z = 10;
                pos = Camera.main.ScreenToWorldPoint(pos);
                return pos;
            }


            public enum State
            {
                Idle, Move
            }
        }
    }
}