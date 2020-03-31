using UnityEngine;

public class Steg : Creature
{
  [Space (10)] [Header("STEGOSAURUS SOUNDS")]
	public AudioClip Waterflush;
  public AudioClip Hit_jaw;
  public AudioClip Hit_head;
  public AudioClip Hit_tail;
  public AudioClip Medstep;
  public AudioClip Medsplash;
  public AudioClip Idleherb;
  public AudioClip Sniff1;
  public AudioClip Chew;
  public AudioClip Largestep;
  public AudioClip Largesplash;
  public AudioClip Steg1;
  public AudioClip Steg2;
  public AudioClip Steg3;
	Transform Spine0, Spine1, Spine2, Spine3, Spine4, Neck0, Neck1, Neck2, Neck3, Head, Tail0, Tail1, Tail2, Tail3, Tail4, Tail5, 
	Left_Arm0, Right_Arm0, Left_Arm1, Right_Arm1, Left_Hand, Right_Hand, 
	Left_Hips, Right_Hips, Left_Leg, Right_Leg, Left_Foot, Right_Foot;

	//*************************************************************************************************************************************************
	//Get bone transforms
	void Start()
	{
		Left_Hips = transform.Find ("Steg/root/pelvis/left hips");
		Right_Hips = transform.Find ("Steg/root/pelvis/right hips");
		Left_Leg  = transform.Find ("Steg/root/pelvis/left hips/left leg");
		Right_Leg = transform.Find ("Steg/root/pelvis/right hips/right leg");
		
		Left_Foot = transform.Find ("Steg/root/pelvis/left hips/left leg/left foot");
		Right_Foot = transform.Find ("Steg/root/pelvis/right hips/right leg/right foot");
		
		Left_Arm0 = transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/left arm0");
		Right_Arm0 = transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/right arm0");
		Left_Arm1 = transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/left arm0/left arm1");
		Right_Arm1 = transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/right arm0/right arm1");
		
		Left_Hand = transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/left arm0/left arm1/left hand");
		Right_Hand = transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/right arm0/right arm1/right hand");
		
		Tail0 = transform.Find ("Steg/root/pelvis/tail0");
		Tail1 = transform.Find ("Steg/root/pelvis/tail0/tail1");
		Tail2 = transform.Find ("Steg/root/pelvis/tail0/tail1/tail2");
		Tail3 = transform.Find ("Steg/root/pelvis/tail0/tail1/tail2/tail3");
		Tail4 = transform.Find ("Steg/root/pelvis/tail0/tail1/tail2/tail3/tail4");
		Tail5 = transform.Find ("Steg/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5");

    Spine0 = transform.Find ("Steg/root/spine0");
    Spine1 = transform.Find ("Steg/root/spine0/spine1");
    Spine2 = transform.Find ("Steg/root/spine0/spine1/spine2");
    Spine3 = transform.Find ("Steg/root/spine0/spine1/spine2/spine3");
    Spine4 = transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4");

		Neck0 = transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/neck0");
		Neck1 = transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1");
		Neck2 = transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2");
		Neck3 = transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3");
		Head = transform.Find ("Steg/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/head");

    crouch_max=4f;
		ang_t=0.025f;
		yaw_max=25f;
		pitch_max=10f;
	}

	//*************************************************************************************************************************************************
	//Play sound
	void OnCollisionStay(Collision col)
	{
		int rndPainsnd=Random.Range(0, 3); AudioClip painSnd=null;
		switch (rndPainsnd) { case 0: painSnd=Steg1; break; case 1: painSnd=Steg2; break; case 2: painSnd=Steg3; break; }
		ManageCollision(col, pitch_max, crouch_max, source, painSnd, Hit_jaw, Hit_head, Hit_tail);
	}
	void PlaySound(string name, int time)
	{
		if(time==currframe && lastframe!=currframe)
		{
			switch (name)
			{
			case "Step": source[1].pitch=Random.Range(0.75f, 1.25f); 
				if(IsInWater) source[1].PlayOneShot(Waterflush, Random.Range(0.25f, 0.5f));
				else if(IsOnWater) source[1].PlayOneShot(Medsplash, Random.Range(0.25f, 0.5f));
				else if(IsOnGround) source[1].PlayOneShot(Medstep, Random.Range(0.25f, 0.5f));
				lastframe=currframe; break;
			case "Hit": source[1].pitch=Random.Range(1.0f, 1.25f); source[1].PlayOneShot(IsOnWater|IsInWater?Largesplash:Largestep, 1.0f);
				lastframe=currframe; break;
			case "Die": source[1].pitch=Random.Range(1.0f, 1.25f); source[1].PlayOneShot(IsOnWater|IsInWater?Largesplash:Largestep, 1.0f);
				lastframe=currframe; IsDead=true; break;
			case "Sniff": source[0].pitch=Random.Range(1.0f, 1.25f); source[0].PlayOneShot(Sniff1, 0.5f);
				lastframe=currframe; break;
			case "Chew": source[0].pitch=Random.Range(1.0f, 1.25f); source[0].PlayOneShot(Chew, 0.5f);
				lastframe=currframe; break;
			case "Repose": source[0].pitch=Random.Range(1.0f, 1.25f); source[0].PlayOneShot(Idleherb, 0.25f);
				lastframe=currframe; break;
			case "Growl": int rnd = Random.Range (0, 3); source[0].pitch=Random.Range(1.0f, 1.25f);
				if(rnd==0)source[0].PlayOneShot(Steg1, 1.0f);
				else if(rnd==1)source[0].PlayOneShot(Steg2, 1.0f);
				else source[0].PlayOneShot(Steg3, 1.0f);
				lastframe=currframe; break;
			}
		}
	}

	//*************************************************************************************************************************************************
	// Add forces to the Rigidbody
	void FixedUpdate ()
	{
		StatusUpdate(); if(!IsActive | AnimSpeed==0.0f) { body.Sleep(); return; }
    anm.SetInteger("Delta", (int)delta);
		OnReset=false; OnAttack=false; OnTailAttack=false; IsConstrained= false;

    //Set mass
		if(IsInWater) { if(Health>0) anm.SetInteger ("Move", 2); body.mass=2; body.drag=4; body.angularDrag=4; }
		else { body.mass=1; body.drag=4; body.angularDrag=4; }
    //Set Y position
    if(IsOnGround | IsInWater | IsOnWater)
    body.AddForce((Vector3.up*Size)*(posY-transform.position.y), ForceMode.VelocityChange);
    else body.AddForce((Vector3.up*Size)*-256, ForceMode.Acceleration);

		//Stopped
		if(NextAnm.IsName("Steg|Idle1A") | NextAnm.IsName("Steg|Idle2A") | CurrAnm.IsName("Steg|Idle1A") | CurrAnm.IsName("Steg|Idle2A") |
			CurrAnm.IsName("Steg|Die1") | CurrAnm.IsName("Steg|Die2"))
		{
			if(CurrAnm.IsName("Steg|Die1")) { OnReset=true; if(!IsDead) { PlaySound("Growl", 2); PlaySound("Die", 12); } }
			else if(CurrAnm.IsName("Steg|Die2")) { OnReset=true; if(!IsDead) { PlaySound("Growl", 2); PlaySound("Die", 10); } }
		}

		//Forward
		else if(CurrAnm.IsName("Steg|Walk") | CurrAnm.IsName("Steg|WalkGrowl") | NextAnm.IsName("Steg|Step1") | CurrAnm.IsName("Steg|Step1") |
					NextAnm.IsName("Steg|Step2") | CurrAnm.IsName("Steg|Step2") | CurrAnm.IsName("Steg|ToIdle2C") | CurrAnm.IsName("Steg|ToEatA") |
					(CurrAnm.IsName("Steg|ToEatC") && CurrAnm.normalizedTime < 0.9))
		{
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
			body.AddForce(transform.forward*15*Size*anm.speed);
			if(CurrAnm.IsName("Steg|WalkGrowl")) { PlaySound("Growl", 1); PlaySound("Step", 6); PlaySound("Step", 13); }
			else if(CurrAnm.IsName("Steg|Walk")) { PlaySound("Step", 6); PlaySound("Step", 13); }
			else PlaySound("Step", 9);
		}

		//Running
		else if(NextAnm.IsName("Steg|Run") | CurrAnm.IsName("Steg|Run") | CurrAnm.IsName("Steg|RunGrowl"))
		{
      roll=Mathf.Clamp(Mathf.Lerp(roll, spineX*10.0f, 0.1f), -20f, 20f);
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
			body.AddForce(transform.forward*60*Size*anm.speed);
			if(CurrAnm.IsName("Steg|Run") | NextAnm.IsName("Steg|Run")) { PlaySound("Step", 3); PlaySound("Step", 9); }
			else { PlaySound("Growl", 2); PlaySound("Step", 3); PlaySound("Step", 9); }
		}
		
		//Backward
		else if(CurrAnm.IsName("Steg|Step1-") | CurrAnm.IsName("Steg|Step2-") | CurrAnm.IsName("Steg|ToIdle1C") | CurrAnm.IsName("Steg|ToSit1"))
		{
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
			body.AddForce(transform.forward*-15*Size*anm.speed);
			PlaySound("Step", 9);
		}

		//Strafe/Turn right
		else if(NextAnm.IsName("Steg|Strafe1-") | CurrAnm.IsName("Steg|Strafe1-") | NextAnm.IsName("Steg|Strafe2+") | CurrAnm.IsName("Steg|Strafe2+"))
		{
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
			body.AddForce(transform.right*8*Size*anm.speed);
			PlaySound("Step", 5); PlaySound("Step", 12);
		}

		//Strafe/Turn left
		else if(NextAnm.IsName("Steg|Strafe1+") | CurrAnm.IsName("Steg|Strafe1+") | NextAnm.IsName("Steg|Strafe2-") | CurrAnm.IsName("Steg|Strafe2-"))
		{
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
			body.AddForce(transform.right*-8*Size*anm.speed);
			PlaySound("Step", 5); PlaySound("Step", 12);
		}

    //Attack Idle
    else if(CurrAnm.IsName("Steg|AtkIdle") | CurrAnm.IsName("Steg|AtkA") | CurrAnm.IsName("Steg|AtkGrowl"))
    {
      OnTailAttack=true;
      if(CurrAnm.IsName("Steg|AtkGrowl")) PlaySound("Growl", 2);
      else if(CurrAnm.IsName("Steg|AtkA")) { OnAttack=true; PlaySound("Growl", 2); PlaySound("Sniff", 3); }
    }
     
		//Attack 
		else if(CurrAnm.IsName("Steg|AtkB-") | CurrAnm.IsName("Steg|AtkB+"))
		{
      OnTailAttack=true;
			if(CurrAnm.normalizedTime < 0.9)
			{
				if(CurrAnm.IsName("Steg|AtkB-")) transform.rotation*= Quaternion.Euler(0, Mathf.Lerp(0, -10.0f, 0.5f), 0);
				else if(CurrAnm.IsName("Steg|AtkB+")) transform.rotation*= Quaternion.Euler(0, Mathf.Lerp(0, 10.0f, 0.5f), 0);
        OnAttack=true; anm.SetFloat("Turn", transform.eulerAngles.y);
			}
			PlaySound("Hit", 8); PlaySound("Hit", 10); { PlaySound("Sniff", 3); PlaySound("Growl", 2); }
		}
		
		//Various
		else if(CurrAnm.IsName("Steg|EatA")) PlaySound("Chew", 10);
		else if(CurrAnm.IsName("Steg|EatB")) { PlaySound("Chew", 1); PlaySound("Chew", 4); PlaySound("Chew", 8); PlaySound("Chew", 12); }
		else if(CurrAnm.IsName("Steg|EatC")) OnReset=true;
		else if(CurrAnm.IsName("Steg|ToSit")) IsConstrained=true; 
		else if(CurrAnm.IsName("Steg|SitIdle")) IsConstrained=true; 
		else if(CurrAnm.IsName("Steg|Sleep")) { OnReset=true; IsConstrained=true; PlaySound("Repose", 2); }
		else if(CurrAnm.IsName("Steg|SitGrowl")) { IsConstrained=true; PlaySound("Growl", 2); PlaySound("Step", 8); }
		else if(CurrAnm.IsName("Steg|Idle1B")) PlaySound("Growl", 2); 
		else if(CurrAnm.IsName("Steg|Idle1C")) PlaySound("Growl", 2);
		else if(CurrAnm.IsName("Steg|Idle2B")) PlaySound("Growl", 2);
		else if(CurrAnm.IsName("Steg|Idle2C")) { OnReset=true; PlaySound("Sniff", 1); }
		else if(CurrAnm.IsName("Steg|Die1-")) { PlaySound("Growl", 3);  IsDead=false; }
		else if(CurrAnm.IsName("Steg|Die2-")) { PlaySound("Growl", 3);  IsDead=false; }
	}

	void LateUpdate()
	{
		//*************************************************************************************************************************************************
		// Bone rotation
		if(!IsActive) return;

		TakeDamageAnim(Quaternion.Euler(-lastHit, 0, 0));

		//Spine rotation
    RotateBone(40f);
		float headZ =headY*headX/yaw_max;
    Spine0.rotation*= Quaternion.Euler(0, 0, spineX);
    Spine1.rotation*= Quaternion.Euler(0, 0, spineX);
    Spine2.rotation*= Quaternion.Euler(0, 0, spineX);
    Spine3.rotation*= Quaternion.Euler(0, 0, spineX);
    Spine4.rotation*= Quaternion.Euler(0, 0, spineX);
		Neck0.rotation*= Quaternion.Euler(headY, headZ, headX);
		Neck1.rotation*= Quaternion.Euler(headY, headZ, headX);
		Neck2.rotation*= Quaternion.Euler(headY, headZ, headX);
		Neck3.rotation*= Quaternion.Euler(headY, headZ, headX);
		Head.rotation*= Quaternion.Euler(headY, headZ, headX);
		//Tail rotation
    //float spineZ =spineY*spineX/yaw_max;
		Tail0.rotation*= Quaternion.Euler(0, 0, -spineX);
		Tail1.rotation*= Quaternion.Euler(0, 0, -spineX);
		Tail2.rotation*= Quaternion.Euler(0, 0, -spineX);
		Tail3.rotation*= Quaternion.Euler(0, 0, -spineX);
		Tail4.rotation*= Quaternion.Euler(0, 0, -spineX);
		Tail5.rotation*= Quaternion.Euler(0, 0, -spineX);
    //Legs rotation
		roll=Mathf.Lerp(roll, 0.0f, ang_t);
		Right_Hips.rotation*= Quaternion.Euler(-roll, 0, 0);
		Left_Hips.rotation*= Quaternion.Euler(-roll, 0, 0);
		Right_Arm0.rotation*= Quaternion.Euler(-roll, 0, 0);
		Left_Arm1.rotation*= Quaternion.Euler(0, roll, 0);

    spineY=Tail5.position.y;
    HeadPos=Head;

		//Check for ground layer
		GetGroundPos(IkType.Quad, Right_Hips, Right_Leg, Right_Foot, Left_Hips, Left_Leg, Left_Foot, Right_Arm0, Right_Arm1, Right_Hand, Left_Arm0, Left_Arm1, Left_Hand, -0.5f*Size);

		//*************************************************************************************************************************************************
		// CPU
		if(UseAI && Health!=0) { AICore(1, 2, 3, 0, 4, 5, 6); }
		//*************************************************************************************************************************************************
		// Human
		else if(Health!=0) { GetUserInputs(1, 2, 3, 0, 4, 5, 6); }
		//*************************************************************************************************************************************************
		//Dead
		else { anm.SetBool("Attack", false); anm.SetInteger ("Move", 0); anm.SetInteger ("Idle", -1); }
	}
}




