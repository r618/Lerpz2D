using UnityEngine;

public class CameraScrolling : MonoBehaviour 
{
    // The object in our scene that our camera is currently tracking.
    private Transform target;
    // How far back should the camera be from the target?
    public float distance = 15.0f;
    // How strict should the camera follow the target?  Lower values make the camera more lazy.
    public float springiness = 4.0f;

    // Keep handy reference sto our level's attributes.  We set up these references in the Awake () function.
    // This also is very slightly more performant, but it's mostly just convenient.
    private LevelAttributes levelAttributes;
    private Rect levelBounds;

    // private bool targetLock = false;

    //added for C#
    private Rigidbody targetRigidbody; //removed declaration in line 129

    // This is for setting interpolation on our target, but making sure we don't permanently
    // alter the target's interpolation setting.  This is used in the SetTarget () function.
    private RigidbodyInterpolation savedInterpolationSetting = RigidbodyInterpolation.None;

    void Awake () {
	    // Set up our convenience references.
	    levelAttributes = LevelAttributes.GetInstance ();
	    levelBounds = levelAttributes.bounds;
    }

    public void SetTarget (Transform newTarget, bool snap) {
	    // If there was a target, reset its interpolation value if it had a rigidbody.
	    if  (target) {
		    // Reset the old target's interpolation back to the saved value.
		    targetRigidbody = (Rigidbody)target.GetComponent ("Rigidbody");
		    if  (targetRigidbody)
			    targetRigidbody.interpolation = savedInterpolationSetting;
	    }
	
	    // Set our current target to be the value passed to SetTarget ()
	    target = newTarget;
	
	    // Now, save the new target's interpolation setting and set it to interpolate for now.
	    // This will make our camera move more smoothly.  Only do this if we didn't set the
	    // target to null (nothing).
	    if (target) {
		    targetRigidbody = (Rigidbody)target.GetComponent ("Rigidbody");
		    if (targetRigidbody) {
			    savedInterpolationSetting = targetRigidbody.interpolation;
			    targetRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		    }
	    }
	
	    // If we should snap the camera to the target, do so now.
	    // Otherwise, the camera's position will change in the LateUpdate () function.
	    if  (snap) {
		    transform.position = GetGoalPosition ();
	    }
    }

    // Provide another version of SetTarget that doesn't require the snap variable to set.
    // This is for convenience and cleanliness.  By default, we will not snap to the target.
    public void SetTarget (Transform newTarget) {
	    SetTarget (newTarget, false);
    }

    // This is a simple accessor function, sometimes called a "getter".  It is a publically callable
    // function that returns a private variable.  Notice how target defined at the top of the script
    // is marked "private"?  We can not access it from other scripts directly.  Therefore, we just
    // have a function that returns it.  Sneaky!
    public Transform GetTarget () {
	    return target;
    }

    // You almost always want camera motion to go inside of LateUpdate (), so that the camera follows
    // the target _after_ it has moved.  Otherwise, the camera may lag one frame behind.
    void LateUpdate () {
	    // Where should our camera be looking right now?
	    var goalPosition = GetGoalPosition ();
	
	    // Interpolate between the current camera position and the goal position.
	    // See the documentation on Vector3.Lerp () for more information.
	    transform.position = Vector3.Lerp (transform.position, goalPosition, Time.deltaTime * springiness);	
    }

    // Based on the camera attributes and the target's special camera attributes, find out where the
    // camera should move to.
    public Vector3 GetGoalPosition () 
    {
	    // If there is no target, don't move the camera.  So return the camera's current position as the goal position.
	    if  (!target)
		    return transform.position;
	
	    // Our camera script can take attributes from the target.  If there are no attributes attached, we have
	    // the following defaults.
	
	    // How high in world space should the camera look above the target?
	    float heightOffset = 0.0f;
	    // How much should we zoom the camera based on this target?
	    float distanceModifier = 1.0f;
	    // By default, we won't account for any target velocity in our calculations;
	    float velocityLookAhead = 0.0f;
	    Vector2 maxLookAhead = new Vector2 (0.0f, 0.0f);
	
	    // Look for CameraTargetAttributes in our target.
	    CameraTargetAttributes cameraTargetAttributes = (CameraTargetAttributes)target.GetComponent ("CameraTargetAttributes");
	
	    // If our target has special attributes, use these instead of our above defaults.
	    if  (cameraTargetAttributes) {
		    heightOffset = cameraTargetAttributes.heightOffset;
		    distanceModifier = cameraTargetAttributes.distanceModifier;
		    velocityLookAhead = cameraTargetAttributes.velocityLookAhead;
		    maxLookAhead = cameraTargetAttributes.maxLookAhead;
	    }
	
	    // First do a rough goalPosition that simply follows the target at a certain relative height and distance.
	    Vector3 goalPosition = target.position + new Vector3(0, heightOffset, -distance * distanceModifier);
	
	    // Next, we refine our goalPosition by taking into account our target's current velocity.
	    // This will make the camera slightly look ahead to wherever the character is going.
	
	    // First assume there is no velocity.
	    // This is so if the camera's target is not a Rigidbody, it won't do any look-ahead calculations because everything will be zero.
	    Vector3 targetVelocity = Vector3.zero;
	
	    // If we find a Rigidbody on the target, that means we can access a velocity!
	    targetRigidbody = (Rigidbody)target.GetComponent ("Rigidbody");
	    if (targetRigidbody)
		    targetVelocity = targetRigidbody.velocity;
	
	    // If we find a PlatformerController on the target, we can access a velocity from that!
	    PlatformerController targetPlatformerController = (PlatformerController)(target.GetComponent ("PlatformerController"));
	    if (targetPlatformerController)
		    targetVelocity = targetPlatformerController.GetVelocity ();
	
	    // If you've had a physics class, you may recall an equation similar to: position = velocity * time;
	    // Here we estimate what the target's position will be in velocityLookAhead seconds.
	    Vector3 lookAhead = targetVelocity * velocityLookAhead;
	
	    // We clamp the lookAhead vector to some sane values so that the target doesn't go offscreen.
	    // This calculation could be more advanced (lengthy), taking into account the target's viewport position,
	    // but this works pretty well in practice.
	    lookAhead.x = Mathf.Clamp (lookAhead.x, -maxLookAhead.x, maxLookAhead.x);
	    lookAhead.y = Mathf.Clamp (lookAhead.y, -maxLookAhead.y, maxLookAhead.y);
	    // We never want to take z velocity into account as this is 2D.  Just make sure it's zero.
	    lookAhead.z = 0.0f;
	
	    // Now add in our lookAhead calculation.  Our camera following is now a bit better!
	    goalPosition += lookAhead;
	
	    // To put the icing on the cake, we will make so the positions beyond the level boundaries
	    // are never seen.  This gives your level a great contained feeling, with a definite beginning
	    // and ending.
	
	    Vector3 clampOffset = Vector3.zero;
	
	    // Temporarily set the camera to the goal position so we can test positions for clamping.
	    // But first, save the previous position.
	    Vector3 cameraPositionSave = transform.position;
	    transform.position = goalPosition;
	
	    // Get the target position in viewport space.  Viewport space is relative to the camera.
	    // The bottom left is (0,0) and the upper right is (1,1)
	    // @TODO Viewport space changing in Unity 2.0?
	    Vector3 targetViewportPosition = GetComponent<Camera>().WorldToViewportPoint (target.position);
	
	    // First clamp to the right and top.  After this we will clamp to the bottom and left, so it will override this
	    // clamping if it needs to.  This only occurs if your level is really small so that the camera sees more than
	    // the entire level at once.
	
	    // What is the world position of the very upper right corner of the camera?
	    Vector3 upperRightCameraInWorld = GetComponent<Camera>().ViewportToWorldPoint (new Vector3(1.0f, 1.0f, targetViewportPosition.z));
	
	    // Find out how far outside the world the camera is right now.
	    clampOffset.x = Mathf.Min (levelBounds.xMax - upperRightCameraInWorld.x, 0.0f);
	    clampOffset.y = Mathf.Min ((levelBounds.yMax - upperRightCameraInWorld.y), 0.0f);
	
	    // Now we apply our clamping to our goalPosition.  Now our camera won't go past the right and top boundaries of the level!
	    goalPosition += clampOffset;
	
	    // Now we do basically the same thing, except clamp to the lower left of the level.  This will override any previous clamping
	    // if the level is really small.  That way you'll for sure never see past the lower-left of the level, but if the camera is
	    // zoomed out too far for the level size, you will see past the right or top of the level.
	
	    transform.position = goalPosition;
	    Vector3 lowerLeftCameraInWorld = GetComponent<Camera>().ViewportToWorldPoint (new Vector3 (0.0f, 0.0f, targetViewportPosition.z));
	
	    // Find out how far outside the world the camera is right now.
	    clampOffset.x = Mathf.Max ((levelBounds.xMin - lowerLeftCameraInWorld.x), 0.0f);
	    clampOffset.y = Mathf.Max ((levelBounds.yMin - lowerLeftCameraInWorld.y), 0.0f);
	
	    // Now we apply our clamping to our goalPosition once again.  Now our camera won't go past the left and bottom boundaries of the level!
	    goalPosition += clampOffset;
	
	    // Now that we're done calling functions on the camera, we can set the position back to the saved position;
	    transform.position = cameraPositionSave;
	
	    // Send back our spiffily calculated goalPosition back to the caller!
	    return goalPosition;
    }
}
