using UnityEngine;
using TouchControlsKit;

namespace Examples
{
    public class Player : MonoBehaviour
    {
        bool binded;
        Transform myTransform, cameraTransform;
        CharacterController controller;
        float rotation;
        bool jump, prevGrounded;
        float weapReadyTime;
        bool weapReady = true;

        public Transform bulletDest;
        public float range;
        //public GameObject crosshair;

        public Camera secondCamera;

        public Transform pickUpDest;
        public Rigidbody pickItem;
        public bool pickedItem;
        public Rigidbody pickChic;
        public bool pickedChic;

        public bool playerShoot = false;

        public GameObject joystick;

        Animator animator;

        PlantInteraction plantInteraction;

        // Awake
        void Awake()
        {
            myTransform = transform;
            cameraTransform = Camera.main.transform;
            controller = GetComponent<CharacterController>();

            //secondCamera.transform.position = new Vector3(0.5f, 3.4f, transform.position.z);
        }

        void Start()
        {
            animator = GetComponent<Animator>();

            GameObject thePlant = GameObject.Find("Plant1");
            plantInteraction = thePlant.GetComponent<PlantInteraction>();
        }

        // Update
        void Update()
        {
            // Setting the crosshair to false for default
            //crosshair.gameObject.SetActive(false);

            // Setting up the shooting mechanics
            if ( weapReady == false )
            {
                weapReadyTime += Time.deltaTime;
                if( weapReadyTime > 1f )
                {
                    weapReady = true;
                    weapReadyTime = 0f;
                }
            }

            // Assigning the pick up mechanics to the pick up button
            if( TCKInput.GetAction( "pickBtn", EActionEvent.Press))
            {
                if (pickedItem == false)
                {
                    PickUp();
                    animator.SetBool("isPickup", true);
                }

                if (pickedChic == false)
                {
                    PickChicUp();
                }
            }

            // Assigning the item to drop when the button is not pressed
            if (TCKInput.GetAction("pickBtn", EActionEvent.Up))
            {
                if(plantInteraction.isDestroyed != true)
                {
                    PickDown();
                    animator.SetBool("isPickup", false);
                } 
            }

            // Assigning the shooting mechanics to the shooting button
            if ( TCKInput.GetAction( "fireBtn", EActionEvent.Press ) )
            {
                PlayerFiring();
                animator.SetBool("isShooting", true);
                secondCamera.gameObject.SetActive(true);
                playerShoot = true;
            }
            else
            {
                animator.SetBool("isShooting", false);
                playerShoot = false;
            }

            // Navigating the camera angles according to the player's touch on the touchpad area of the screen
            Vector2 look = TCKInput.GetAxis( "Touchpad" );
            PlayerRotation( look.x, look.y );
        }

        // FixedUpdate
        void FixedUpdate()
        {
            pickedItem = true;

            pickedChic = true;

            //float moveX = TCKInput.GetAxis( "Joystick", EAxisType.Horizontal );
            //float moveY = TCKInput.GetAxis( "Joystick", EAxisType.Vertical );
            
            // Assign the movement of the character to a joystick
            Vector2 move = TCKInput.GetAxis( "Joystick" ); // NEW func since ver 1.5.5
            PlayerMovement(move.x, move.y);


            /*if (TCKInput.GetAxis("Joystick", EAxisType.Horizontal))
            {
                Debug.Log("more than 0");
                //
            }*/
        }


        // Jumping
        private void Jumping()
        {
            if( controller.isGrounded )
                jump = true;
        }

                        
        // PlayerMovement
        private void PlayerMovement( float horizontal, float vertical )
        {
            secondCamera.gameObject.SetActive(false);

            bool grounded = controller.isGrounded;

            Vector3 moveDirection = myTransform.forward * vertical * 2f;
            moveDirection += myTransform.right * horizontal * 1f;

            if(playerShoot == true)
            {
                moveDirection = myTransform.forward * vertical * 0.5f;
                moveDirection += myTransform.right * horizontal * 0.5f;
            }

            moveDirection.y = -10f;

            if (moveDirection.z < 0f || moveDirection.z > 0f)
            {
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }

            if ( jump )
            {
                jump = false;
                moveDirection.y = 25f;
                /*isPorjectileCube = !isPorjectileCube;*/
            }

            if( grounded )            
                moveDirection *= 5f;
            
            controller.Move( moveDirection * Time.fixedDeltaTime);
            moveDirection = Vector3.zero;

            if( !prevGrounded && grounded )
                moveDirection.y = 0f;
  
            prevGrounded = grounded;
        }

        // PlayerRotation
        public void PlayerRotation( float horizontal, float vertical )
        {
            myTransform.Rotate( 0f, horizontal * 12f, 0f );
            rotation += vertical * 12f;
            rotation = Mathf.Clamp( rotation, -60f, 60f );
            cameraTransform.localEulerAngles = new Vector3( -rotation, cameraTransform.localEulerAngles.y, 0f );
        }

        // PlayerFiring
        public void PlayerFiring()
        {
            // Camera zooms in on the screen when player shoots
            

            // Crosshair enabled when shooting
            //crosshair.gameObject.SetActive(true);


            if ( !weapReady )
                return;

            weapReady = false;

            // Setting up the bullets 
            GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            primitive.transform.position = bulletDest.position;
            primitive.transform.localScale = Vector3.one * .2f;
            Rigidbody rBody = primitive.AddComponent<Rigidbody>();
            Transform camTransform = secondCamera.transform;
            rBody.AddForce( camTransform.forward * range, ForceMode.Impulse );
            Destroy( primitive, 0.5f );
        }

        // PlayerPickUp
        /*private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            // When players get close to items with tag "PickUp"
            if (hit.gameObject.tag.Equals("PickUp"))
            {
                pickedItem = false;
            }
            else
            {
                pickedItem = true;
            }
        }*/

        private void PickUp()
        {
            // Coding the pickable items to be carried
            pickItem.useGravity = false;
            pickItem.transform.position = pickUpDest.position;
            pickItem.transform.parent = GameObject.Find("PickUpDestination").transform;
            pickItem.constraints = RigidbodyConstraints.FreezeAll;
        }

        private void PickDown()
        {
            //Debug.Log("Player drop item");
            pickItem.constraints = RigidbodyConstraints.None;
            pickItem.transform.parent = null;
            pickItem.useGravity = true;
            pickedItem = true;

            if (plantInteraction.isDestroyed == true)
            {
                pickItem = null;
            }
        }

        private void PickChicUp()
        {
            Debug.Log("Pick Chicken");
            pickChic.useGravity = false;
            pickChic.transform.position = pickUpDest.position;
            pickChic.transform.parent = GameObject.Find("PickUpDestination").transform;
            pickChic.constraints = RigidbodyConstraints.FreezeAll;
        }

        // PlayerClicked
        public void PlayerClicked()
        {
            //Debug.Log( "PlayerClicked" );
        }
    };
}