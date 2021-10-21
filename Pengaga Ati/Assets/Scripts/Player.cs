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
        /*public GameObject crosshair;*/

        public Camera secondCamera;

        public GameObject sumpit;

        public Transform pickUpDest;
        public Rigidbody pickItem;
        public bool pickedItem;


        // List of variable for all chickens
        public Rigidbody pickChic;
        public bool pickedChic;
        public Rigidbody pickChicSec;
        public bool pickedChicSec;
        public Rigidbody pickChicThird;
        public bool pickedChicThird;

        public bool playerShoot = false;

        public GameObject joystick;

        Animator animator;

        PlantInteraction plantInteraction;

        GrowingCrop growingCrop;

        // Awake
        void Awake()
        {
            myTransform = transform;
            cameraTransform = Camera.main.transform;
            controller = GetComponent<CharacterController>();

            /*secondCamera.transform.position = new Vector3(0.5f, 3.4f, transform.position.z);*/
        }

        void Start()
        {
            animator = GetComponent<Animator>();

            GameObject theSeed = GameObject.Find("Seed1");
            plantInteraction = theSeed.GetComponent<PlantInteraction>();

            GameObject theCrop = GameObject.Find("Crops");
            growingCrop = theCrop.GetComponent<GrowingCrop>();
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
                if(growingCrop.harvestReadyToPick == true && growingCrop.playerHit == true)
                {
                    growingCrop.PlayerPickCrop();
                    animator.SetBool("isPickup", true);
                }
               
                if (pickedItem == false)
                {
                    PickUp();
                    animator.SetBool("isPickup", true);
                }

                // List of calling pick chicken up functions
                if (pickedChic == false)
                {
                    PickChicUp();
                    animator.SetBool("isPickup", true);
                    pickedChicSec = true;
                    pickedChicThird = true;
                }
                if (pickedChicSec == false)
                {
                    PickChicUpSec();
                    animator.SetBool("isPickup", true);
                    pickedChic = true;
                    pickedChicThird = true;
                }
                if(pickedChicThird == false)
                {
                    PickChicUpThird();
                    animator.SetBool("isPickup", true);
                    pickedChic = true;
                    pickedChicSec = true;

                }
            }

            // Assigning the item to drop when the button is not pressed
            if (TCKInput.GetAction("pickBtn", EActionEvent.Up))
            {
                if (growingCrop.cropPickedUp == true)
                {
                    growingCrop.PlayerDropCrop();
                    animator.SetBool("isPickup", false);
                }

                if (plantInteraction.isDestroyed != true)
                {
                    PickDown();
                    animator.SetBool("isPickup", false);
                }

                // List of calling pick chicken down functions
                PickChicDown();
                PickChicDownSec();
                PickChicDownThird();
            }

            // Assigning the shooting mechanics to the shooting button
            if ( TCKInput.GetAction( "fireBtn", EActionEvent.Press ) )
            {
                PlayerFiring();
                animator.SetBool("isShooting", true);
                secondCamera.gameObject.SetActive(true);
                sumpit.gameObject.SetActive(true);
                playerShoot = true;
            }
            else
            {
                animator.SetBool("isShooting", false);
                sumpit.gameObject.SetActive(false);
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

            // List of booleans for picking chickens
            pickedChic = true;
            pickedChicSec = true;

            /*float moveX = TCKInput.GetAxis( "Joystick", EAxisType.Horizontal );*/
            /*float moveY = TCKInput.GetAxis( "Joystick", EAxisType.Vertical );*/
            
            // Assign the movement of the character to a joystick
            Vector2 move = TCKInput.GetAxis( "Joystick" ); // NEW func since ver 1.5.5
            PlayerMovement(move.x, move.y);
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



        // Lists of functions handling all chicken functions
        // Function for chicken number 1 [START]
        private void PickChicUp()
        {
            /*Debug.Log("Pick Chicken");*/
            pickChic.useGravity = false;
            pickChic.transform.position = pickUpDest.position;
            pickChic.transform.parent = GameObject.Find("PickUpDestination").transform;
            /*pickChic.constraints = RigidbodyConstraints.FreezeAll;*/
        }
        private void PickChicDown()
        {
            /*Debug.Log("Unparent chicken");*/
            pickChic.transform.parent = null;
            pickChic.useGravity = true;
            animator.SetBool("isPickup", false);
            /*pickedChic = true;*/
        }
        // Function for chicken number 1 [END]
        // Function for chicken number 2 [START]
        private void PickChicUpSec()
        {
            pickChicSec.useGravity = false;
            pickChicSec.transform.position = pickUpDest.position;
            pickChicSec.transform.parent = GameObject.Find("PickUpDestination").transform;
        }
        private void PickChicDownSec()
        {
            pickChicSec.transform.parent = null;
            pickChicSec.useGravity = true;
            animator.SetBool("isPickup", false);
        }
        // Function for chicken number 2 [END]
        // Function for chicken number 3 [START]
        private void PickChicUpThird()
        {
            pickChicThird.useGravity = false;
            pickChicThird.transform.position = pickUpDest.position;
            pickChicThird.transform.parent = GameObject.Find("PickUpDestination").transform;
        }
        private void PickChicDownThird()
        {
            pickChicThird.transform.parent = null;
            pickChicThird.useGravity = true;
            animator.SetBool("isPickup", false);
        }
        // Function for chicken number 3 [END]




        // PlayerClicked
        public void PlayerClicked()
        {
            //Debug.Log( "PlayerClicked" );
        }
    };
}