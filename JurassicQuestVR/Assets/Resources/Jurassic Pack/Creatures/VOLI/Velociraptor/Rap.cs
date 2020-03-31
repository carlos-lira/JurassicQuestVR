using UnityEngine;

public class Rap : Creature
{
  [Space (10)] [Header("VELOCIRAPTOR SOUNDS")]
	public AudioClip Waterflush;
  public AudioClip Hit_jaw;
  public AudioClip Hit_head;
  public AudioClip Hit_tail;
  public AudioClip Smallstep;
  public AudioClip Smallsplash;
  public AudioClip Idlecarn;
  public AudioClip Swallow;
  public AudioClip Bite;
  public AudioClip Rap1;
  public AudioClip Rap2;
  public AudioClip Rap3;
  public AudioClip Rap4;
  public AudioClip Rap5;
  public AudioClip Rap6;
	Transform Spine0, Spine1, Spine2, Spine3, Spine4, Spine5, Neck0, Neck1, Neck2, Neck3, Head, 
	Tail0, Tail1, Tail2, Tail3, Tail4, Tail5, Tail6, Tail7, Tail8, Tail9, Tail10, Tail11, Arm1, Arm2, 
	Left_Hips, Right_Hips, Left_Leg, Right_Leg, Left_Foot0, Right_Foot0;
  Vector3 dir=Vector3.zero;

	//*************************************************************************************************************************************************
	//Get bone transforms
	void Start()
	{
		Right_Hips = transform.Find ("Rap/root/pelvis/right leg0");
		Right_Leg = transform.Find ("Rap/root/pelvis/right leg0/right leg1");
		Right_Foot0 = transform.Find ("Rap/root/pelvis/right leg0/right leg1/right foot0");
		Left_Hips = transform.Find ("Rap/root/pelvis/left leg0");
		Left_Leg = transform.Find ("Rap/root/pelvis/left leg0/left leg1");
		Left_Foot0 = transform.Find ("Rap/root/pelvis/left leg0/left leg1/left foot0");
	
		Tail0 = transform.Find ("Rap/root/pelvis/tail0");
		Tail1 = transform.Find ("Rap/root/pelvis/tail0/tail1");
		Tail2 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2");
		Tail3 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3");
		Tail4 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4");
		Tail5 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5");
		Tail6 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6");
		Tail7 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7");
		Tail8 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7/tail8");
		Tail9 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7/tail8/tail9");
		Tail10 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7/tail8/tail9/tail10");
		Tail11 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7/tail8/tail9/tail10/tail11");
		Spine0 = transform.Find ("Rap/root/spine0");
		Spine1 = transform.Find ("Rap/root/spine0/spine1");
		Spine2 = transform.Find ("Rap/root/spine0/spine1/spine2");
		Spine3 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3");
		Spine4 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4");
		Spine5 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5");
		Arm1  = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5/left arm0");
		Arm2  = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5/right arm0");
		Neck0 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5/neck0");
		Neck1 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5/neck0/neck1");
		Neck2 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5/neck0/neck1/neck2");
		Neck3 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5/neck0/neck1/neck2/neck3");
		Head  = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5/neck0/neck1/neck2/neck3/head");

    crouch_max=2.5f;
		ang_t=0.075f;
		yaw_max=16f;
		pitch_max=9f;
	}

	//*************************************************************************************************************************************************
	//Play sound
	void OnCollisionStay(Collision col)
	{
		int rndPainsnd=Random.Range(0, 4); AudioClip painSnd=null;
		switch (rndPainsnd) { case 0: painSnd=Rap1; break; case 1: painSnd=Rap2; break; case 2: painSnd=Rap3; break; case 3: painSnd=Rap6; break; }
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
				else if(IsOnWater) source[1].PlayOneShot(Smallsplash, Random.Range(0.25f, 0.5f));
				else if(IsOnGround) source[1].PlayOneShot(Smallstep, Random.Range(0.25f, 0.5f));
				lastframe=currframe; break;
			case "Bite": source[1].pitch=Random.Range(1.0f, 1.25f); source[1].PlayOneShot(Bite, 0.5f);
				lastframe=currframe; break;
			case "Die": source[1].pitch=Random.Range(0.8f, 1.0f); source[1].PlayOneShot(IsOnWater|IsInWater?Smallsplash:Smallstep, 1.0f);
				lastframe=currframe; IsDead=true; break;
			case "Food": source[0].pitch=Random.Range(3.0f, 3.5f); source[0].PlayOneShot(Swallow, 0.025f);
				lastframe=currframe; break;
			case "Repose": source[0].pitch=Random.Range(3.0f, 3.5f); source[0].PlayOneShot(Idlecarn, 0.01f);
				lastframe=currframe; break;
			case "Call": source[0].pitch=Random.Range(0.9F, 1.1F); source[0].PlayOneShot(Rap4, 1.0f);
				lastframe=currframe; break;
			case "AtkA": int rnd1 = Random.Range (0, 2); source[0].pitch=Random.Range(1.0f, 1.25f);
				if(rnd1==0)source[0].PlayOneShot(Rap2, 1.0f);
				else source[0].PlayOneShot(Rap3, 1.0f);
				lastframe=currframe; break;
			case "AtkB": int rnd2 = Random.Range (0, 3); source[0].pitch=Random.Range(1.0f, 1.25f);
				if(rnd2==0)source[0].PlayOneShot(Rap1, 1.0f);
				else if(rnd2==1)source[0].PlayOneShot(Rap6, 1.0f);
				else source[0].PlayOneShot(Rap4, 1.0f);
				lastframe=currframe; break;
			case "Growl": int rnd3 = Random.Range (0, 2); source[0].pitch=Random.Range(1.0f, 1.25f);
				if(rnd3==0)source[0].PlayOneShot(Rap5, 1.0f);
				else source[0].PlayOneShot(Rap6, 1.0f);
				lastframe=currframe; break;
			}
		}
	}
	
	//*************************************************************************************************************************************************
	// Add forces to the Rigidbody
	void FixedUpdate()
	{
		StatusUpdate(); if(!IsActive | AnimSpeed==0.0f) { body.Sleep(); return; }
		OnReset=false; OnAttack=false; IsConstrained=false;

    //Set mass
		if(IsInWater) { if(Health>0) anm.SetInteger ("Move", 2); body.mass=2; body.drag=4; body.angularDrag=4; }
    else { body.mass=1; body.drag=4; body.angularDrag=4; }
   
    //Set Y position
    if(IsOnGround | IsInWater | IsOnWater)
    { 
      if(IsOnGround | IsInWater) anm.SetBool("OnGround", true);
      dir=new Vector3(transform.forward.x, 0, transform.forward.z);
      body.AddForce((Vector3.up*Size)*(posY-transform.position.y), ForceMode.VelocityChange);
    } 
    else
    {
      body.AddForce((Vector3.up*Size)*-256, ForceMode.Acceleration);
      anm.SetBool("OnGround", false);
    }

		//Stopped
		if(NextAnm.IsName("Rap|IdleA") | CurrAnm.IsName("Rap|IdleA") | CurrAnm.IsName("Rap|Die"))
		{
			if(CurrAnm.IsName("Rap|Die")) { OnReset=true; if(!IsDead) { PlaySound("AtkB", 2); PlaySound("Die", 12); } }
		}

		//Jump
		else if(CurrAnm.IsName("Rap|IdleJumpStart") | CurrAnm.IsName("Rap|RunJumpStart") | CurrAnm.IsName("Rap|JumpIdle") |
			CurrAnm.IsName("Rap|IdleJumpEnd") | CurrAnm.IsName("Rap|RunJumpEnd") | CurrAnm.IsName("Rap|JumpAtk"))
		{
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
			if(CurrAnm.IsName("Rap|IdleJumpStart") | CurrAnm.IsName("Rap|RunJumpStart"))
			{
				if(CurrAnm.normalizedTime > 0.4) { body.AddForce((Vector3.up*8)*Size, ForceMode.VelocityChange); } else OnJump=true;
        if(!anm.GetInteger("Move").Equals(0)&&CurrAnm.IsName("Rap|RunJumpStart")) body.AddForce(dir*160*Size*anm.speed);
        PlaySound("Step", 1); PlaySound("Step", 2);
			}
			else if(CurrAnm.IsName("Rap|IdleJumpEnd") | CurrAnm.IsName("Rap|RunJumpEnd"))
			{ 
        if(CurrAnm.IsName("Rap|RunJumpEnd")) body.AddForce(dir*160*Size*anm.speed);
        body.velocity = new Vector3(body.velocity.x, 0.0f, body.velocity.z); OnJump=false;
				PlaySound("Step", 3); PlaySound("Step", 4); 
			}
      else if(CurrAnm.IsName("Rap|JumpAtk"))
      {
        if(anm.GetInteger("Move").Equals(1)|anm.GetInteger("Move").Equals(2)) body.AddForce(dir*160*Size*anm.speed, ForceMode.Acceleration);
        OnAttack=true; PlaySound("AtkB", 1); PlaySound("Bite", 9);
        body.velocity = new Vector3(body.velocity.x, body.velocity.y>0?body.velocity.y:0, body.velocity.z);
      } else if(anm.GetInteger("Move").Equals(1)|anm.GetInteger("Move").Equals(2)) body.AddForce(dir*160*Size*anm.speed, ForceMode.Acceleration);
		}

		//Forward
		else if(NextAnm.IsName("Rap|Walk") | CurrAnm.IsName("Rap|Walk") | CurrAnm.IsName("Rap|WalkGrowl"))
		{
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
			body.AddForce(transform.forward*32*Size*anm.speed);
			if(CurrAnm.IsName("Rap|Walk")) { PlaySound("Step", 6); PlaySound("Step", 14);}
			else if(CurrAnm.IsName("Rap|WalkGrowl")) { PlaySound("Growl", 2); PlaySound("Step", 6); PlaySound("Step", 14); }
		}

		//Running
		else if(NextAnm.IsName("Rap|Run") | CurrAnm.IsName("Rap|Run") |
		   CurrAnm.IsName("Rap|RunGrowl") | CurrAnm.IsName("Rap|RunAtk1") |
		   (CurrAnm.IsName("Rap|RunAtk2") && CurrAnm.normalizedTime < 0.9) |
		   (CurrAnm.IsName("Rap|IdleAtk3") && CurrAnm.normalizedTime > 0.5 && CurrAnm.normalizedTime < 0.9))
		{
      roll=Mathf.Clamp(Mathf.Lerp(roll, spineX*15.0f, 0.1f), -30f, 30f);
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
			body.AddForce(transform.forward*160*Size*anm.speed);
			if(CurrAnm.IsName("Rap|Run")) { PlaySound("Step", 4); PlaySound("Step", 12);  }
			else if(CurrAnm.IsName("Rap|RunGrowl")) { PlaySound("AtkB", 2); PlaySound("Step", 4); PlaySound("Step", 12); }
			else if(CurrAnm.IsName("Rap|RunAtk1")) { OnAttack=true; PlaySound("AtkB", 2); PlaySound("Step", 4); PlaySound("Step", 12); }
			else if(CurrAnm.IsName("Rap|RunAtk2")| CurrAnm.IsName("Rap|IdleAtk3"))
			{ OnAttack=true; PlaySound("AtkA", 2); PlaySound("Step", 4); PlaySound("Bite", 9); PlaySound("Step", 12); }
		}
		
		//Backward
		else if(NextAnm.IsName("Rap|Walk-") | NextAnm.IsName("Rap|WalkGrowl-") |
					CurrAnm.IsName("Rap|Walk-") | CurrAnm.IsName("Rap|WalkGrowl-"))
		{
			if(CurrAnm.normalizedTime > 0.25 && CurrAnm.normalizedTime < 0.45| 
			 CurrAnm.normalizedTime > 0.75 && CurrAnm.normalizedTime < 0.9)
			{
				body.AddForce(transform.forward*-32*Size*anm.speed);
				transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
			}
			if(CurrAnm.IsName("Rap|WalkGrowl-")) { PlaySound("Growl", 1); PlaySound("Step", 6); PlaySound("Step", 13); }
			else { PlaySound("Step", 6); PlaySound("Step", 13); }
		}

		//Strafe/Turn right
		else if(NextAnm.IsName("Rap|Strafe-") | CurrAnm.IsName("Rap|Strafe-"))
		{
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
			body.AddForce(transform.right*16*Size*anm.speed);
			PlaySound("Step", 6); PlaySound("Step", 14);
		}
		
		//Strafe/Turn left
		else if(NextAnm.IsName("Rap|Strafe+") | CurrAnm.IsName("Rap|Strafe+"))
		{
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
			body.AddForce(transform.right*-16*Size*anm.speed);
			PlaySound("Step", 6); PlaySound("Step", 14);
		}

		//Various
		else if(CurrAnm.IsName("Rap|IdleAtk3")) { OnAttack=true; transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t); PlaySound("AtkA", 1); }
		else if(CurrAnm.IsName("Rap|GroundAtk")) { OnAttack=true; PlaySound("AtkB", 2); PlaySound("Bite", 4); }
		else if(CurrAnm.IsName("Rap|IdleAtk1") | CurrAnm.IsName("Rap|IdleAtk2"))
		{ OnAttack=true; transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t); PlaySound("AtkB", 2); PlaySound("Bite", 9); }
		else if(CurrAnm.IsName("Rap|ToSleep")) { OnReset=true; IsConstrained=true; }
		else if(CurrAnm.IsName("Rap|Sleep")) { OnReset=true; PlaySound("Repose", 1); IsConstrained=true; }
		else if(CurrAnm.IsName("Rap|EatA")) { IsConstrained=true; OnReset=true; PlaySound("Food", 1); }
		else if(CurrAnm.IsName("Rap|EatB")) { IsConstrained=true; OnReset=true; PlaySound("Bite", 3); }
		else if(CurrAnm.IsName("Rap|EatC")) { IsConstrained=true; OnReset=true; }
		else if(CurrAnm.IsName("Rap|IdleC")) PlaySound("Growl", 1);
		else if(CurrAnm.IsName("Rap|IdleD")) { PlaySound("Call", 1); PlaySound("Call", 4); PlaySound("Call", 8); }
		else if(CurrAnm.IsName("Rap|IdleE")) { OnReset=true; PlaySound("Bite", 4); PlaySound("Bite", 7); PlaySound("Bite", 9); }
		else if(CurrAnm.IsName("Rap|Die-")) { OnReset=true; PlaySound("AtkA", 1);  IsDead=false; }
	}

	void LateUpdate()
	{
		//*************************************************************************************************************************************************
		// Bone rotation
		if(!IsActive) return;

		TakeDamageAnim(Quaternion.Euler(lastHit, 0, 0));

    //Spine rotation
    RotateBone(60f);
		float headZ =-headY*headX/yaw_max;
		Spine0.rotation*=Quaternion.Euler(-headY, headZ, headX);
		Spine1.rotation*=Quaternion.Euler(-headY, headZ, headX);
		Spine2.rotation*=Quaternion.Euler(-headY, headZ, headX);
		Spine3.rotation*=Quaternion.Euler(-headY, headZ, headX);
		Spine4.rotation*=Quaternion.Euler(-headY, headZ, headX);
		Spine5.rotation*=Quaternion.Euler(-headY, headZ, headX);
		//Arms rotation
		Arm1.rotation*=Quaternion.Euler(headY*8, 0, 0);
		Arm2.rotation*=Quaternion.Euler(0, headY*8, 0);
		Neck0.rotation*=Quaternion.Euler(-headY, headZ, headX);
		Neck1.rotation*=Quaternion.Euler(-headY, headZ, headX);
		Neck2.rotation*=Quaternion.Euler(-headY, headZ, headX);
		Neck3.rotation*=Quaternion.Euler(-headY, headZ, headX);
		Head.rotation*=Quaternion.Euler(-headY, headZ, headX);
		//Tail rotation
		Tail0.rotation*=Quaternion.Euler(0, 0, -spineX);
		Tail1.rotation*=Quaternion.Euler(0, 0, -spineX);
		Tail2.rotation*=Quaternion.Euler(0, 0, -spineX);
		Tail3.rotation*=Quaternion.Euler(0, 0, -spineX);
		Tail4.rotation*=Quaternion.Euler(0, 0, -spineX);
		Tail5.rotation*=Quaternion.Euler(0, 0, -spineX);
		Tail6.rotation*=Quaternion.Euler(0, 0, -spineX);
		Tail7.rotation*=Quaternion.Euler(0, 0, -spineX);
		Tail8.rotation*=Quaternion.Euler(0, 0, -spineX);
    Tail9.rotation*=Quaternion.Euler(0, 0, -spineX);
		Tail10.rotation*=Quaternion.Euler(0, 0, -spineX);
		Tail11.rotation*=Quaternion.Euler(0, 0, -spineX);
    //Legs rotation
    roll=Mathf.Lerp(roll, 0.0f, ang_t);
		Right_Hips.rotation*= Quaternion.Euler(-roll, 0, 0);
		Left_Hips.rotation*= Quaternion.Euler(0, roll, 0);
    HeadPos=Head;

		//Check for ground layer
		GetGroundPos(IkType.SmBiped, Right_Hips, Right_Leg, Right_Foot0, Left_Hips, Left_Leg, Left_Foot0);

		//*************************************************************************************************************************************************
		// CPU
		if(UseAI && Health!=0) { AICore(1, 2, 3, 4, 5, 6, 7); }
		//*************************************************************************************************************************************************
		// Human
		else if(Health!=0) { GetUserInputs(1, 2, 3, 4, 5, 6, 7); }
		//*************************************************************************************************************************************************
		//Dead
		else { anm.SetBool("Attack", false); anm.SetInteger ("Move", 0); anm.SetInteger ("Idle", -1); }
	}

}