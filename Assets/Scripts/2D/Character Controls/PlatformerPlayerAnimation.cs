using UnityEngine;

public class PlatformerPlayerAnimation : MonoBehaviour
{

    // Adjusts the speed at which the walk animation is played back
    public float walkAnimationSpeedModifier = 2.5f;
    // Adjusts the speed at which the run animation is played back
    public float runAnimationSpeedModifier = 1.5f;
    // Adjusts the speed at which the jump animation is played back
    public float jumpAnimationSpeedModifier = 2.0f;
    // Adjusts the speed at which the hang time animation is played back
    public float jumpLandAnimationSpeedModifier = 3.0f;

    // Adjusts after how long the falling animation will be 
    public float hangTimeUntilFallingAnimation = 0.05f;
    
    // private bool jumping = false;

    void Start()
    {
        GetComponent<Animation>().Stop();

        // By default loop all animations
        GetComponent<Animation>().wrapMode = WrapMode.Loop;

        // Jump animation are in a higher layer:
        // Thus when a jump animation is playing it will automatically override all other animations until it is faded out.
        // This simplifies the animation script because we can just keep playing the walk / run / idle cycle without having to spcial case jumping animations.
        int jumpingLayer = 1;
        AnimationState jump = GetComponent<Animation>()["jump"];
        jump.layer = jumpingLayer;
        jump.speed *= jumpAnimationSpeedModifier;
        jump.wrapMode = WrapMode.Once;

        AnimationState jumpFall = GetComponent<Animation>()["jumpFall"];
        jumpFall.layer = jumpingLayer;
        jumpFall.wrapMode = WrapMode.ClampForever;

        AnimationState jumpLand = GetComponent<Animation>()["jumpLand"];
        jumpLand.layer = jumpingLayer;
        jumpLand.speed *= jumpLandAnimationSpeedModifier;
        jumpLand.wrapMode = WrapMode.Once;

        AnimationState run = GetComponent<Animation>()["run"];
        run.speed *= runAnimationSpeedModifier;

        AnimationState walk = GetComponent<Animation>()["walk"];
        walk.speed *= walkAnimationSpeedModifier;
    }

    void Update() {
	PlatformerController controller = GetComponent("PlatformerController") as PlatformerController;

	// We are not falling off the edge right now
	if (controller.GetHangTime() < hangTimeUntilFallingAnimation) {
		// Are we moving the character?
		if (controller.IsMoving())
		{
			if (Input.GetButton ("Fire2"))
				GetComponent<Animation>().CrossFade ("run");
			else
				GetComponent<Animation>().CrossFade ("walk");
		}
		// Go back to idle when not moving
		else
			GetComponent<Animation>().CrossFade ("idle", 0.5f);
	}
	// When falling off an edge, after hangTimeUntilFallingAnimation we will fade towards the ledgeFall animation
	else {
		GetComponent<Animation>().CrossFade ("ledgeFall");
	}
}

    void DidJump()
    {
        GetComponent<Animation>().Play("jump");
        GetComponent<Animation>().PlayQueued("jumpFall");
    }

    void DidLand()
    {
        GetComponent<Animation>().Stop("jumpFall");
        GetComponent<Animation>().Play("jumpLand");
        GetComponent<Animation>().Blend("jumpLand", 0);
    }
}
