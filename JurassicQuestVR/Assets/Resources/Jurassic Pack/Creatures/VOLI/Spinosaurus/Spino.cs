using UnityEngine;

public class Spino : Creature
{
  [Space (10)] [Header("SPINOSAURUS SOUNDS")]
	public AudioClip Waterflush;
  public AudioClip Hit_jaw;
  public AudioClip Hit_head;
  public AudioClip Hit_tail;
  public AudioClip Bigstep;
  public AudioClip Largesplash;
  public AudioClip Largestep;
  public AudioClip Idlecarn;
  public AudioClip Bite;
  public AudioClip Swallow;
  public AudioClip Sniff1;
  public AudioClip Spino1;
  public AudioClip Spino2;
  public AudioClip Spino3; 
  public AudioClip Spino4;
	Transform Spine0, Spine1, Spine2, Neck0, Neck1, Neck2, Head, Tail2, Tail3, Tail4, Tail5, Tail6, 
	Left_Hips, Right_Hips, Left_Leg, Right_Leg, Left_Foot0, Right_Foot0;

	//*************************************************************************************************************************************************
	//Get bone transforms
	void Start()
	{
		Right_Hips = transform.Find ("Spino/root/tail0/tail1/right hips");
		Right_Leg = transform.Find ("Spino/root/tail0/tail1/right hips/right leg");
		Right_Foot0 = transform.Find ("Spino/root/tail0/tail1/right hips/right leg/right foot0");
		Left_Hips = transform.Find ("Spino/root/tail0/tail1/left hips");
		Left_Leg = transform.Find ("Spino/root/tail0/tail1/left hips/left leg");
		Left_Foot0 = transform.Find ("Spino/root/tail0/tail1/left hips/left leg/left foot0");

		Tail2 = transform.Find ("Spino/root/tail0/tail1/tail2");
		Tail3 = transform.Find ("Spino/root/tail0/tail1/tail2/tail3");
		Tail4 = transform.Find ("Spino/root/tail0/tail1/tail2/tail3/tail4");
		Tail5 = transform.Find ("Spino/root/tail0/tail1/tail2/tail3/tail4/tail5");
		Tail6 = transform.Find ("Spino/root/tail0/tail1/tail2/tail3/tail4/tail5/tail6");
		Spine0 = transform.Find ("Spino/root/spine0");
		Spine1 = transform.Find ("Spino/root/spine0/spine1");
		Spine2 = transform.Find ("Spino/root/spine0/spine1/spine2");
		Neck0 = transform.Find ("Spino/root/spine0/spine1/spine2/spine3/neck0");
		Neck1 = transform.Find ("Spino/root/spine0/spine1/spine2/spine3/neck0/neck1");
		Neck2 = transform.Find ("Spino/root/spine0/spine1/spine2/spine3/neck0/neck1/neck2");
		Head = transform.Find ("Spino/root/spine0/spine1/spine2/spine3/neck0/neck1/neck2/head");

    crouch_max=7f;
		ang_t=0.035f;
		yaw_max=25f;
		pitch_max=12f;
	}

	//*************************************************************************************************************************************************
	//Play sound
	void OnCollisionStay(Collision col)
	{
		int rndPainsnd=Random.Range(0, 4); AudioClip painSnd=null;
		switch (rndPainsnd) { case 0: painSnd=Spino1; break; case 1: painSnd=Spino2; break; case 2: painSnd=Spino3; break; case 3: painSnd=Spino4; break; }
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
				else if(IsOnWater) source[1].PlayOneShot(Largesplash, Random.Range(0.25f, 0.5f));
				else if(IsOnGround) source[1].PlayOneShot(Bigstep, Random.Range(0.25f, 0.5f));
				lastframe=currframe; break;
			case "Bite": source[1].pitch=Random.Range(0.5f, 0.75f); source[1].PlayOneShot(Bite, 2.0f);
				lastframe=currframe; break;
			case "Die": source[1].pitch=Random.Range(1.0f, 1.25f); source[1].PlayOneShot(IsOnWater|IsInWater?Largesplash:Largestep, 1.0f);
				lastframe=currframe; IsDead=true; break; 
			case "Food": source[0].pitch=Random.Range(1.0f, 1.25f); source[0].PlayOneShot(Swallow, 0.1f);
				lastframe=currframe; break;
			case "Sniff": source[0].pitch=Random.Range(1.0f, 1.25f); source[0].PlayOneShot(Sniff1, 0.5f);
				lastframe=currframe; break;
			case "Repose": source[0].pitch=Random.Range(0.75f, 1.25f); source[0].PlayOneShot(Idlecarn, 0.25f);
				lastframe=currframe; break;
			case "Atk": int rnd1 = Random.Range (0, 2); source[0].pitch=Random.Range(0.75f, 1.75f);
				if(rnd1==0)source[0].PlayOneShot(Spino3, 0.5f);
				else source[0].PlayOneShot(Spino4, 0.5f);
				lastframe=currframe; break;
			case "Growl": int rnd2 = Random.Range (0, 2); source[0].pitch=Random.Range(1.0f, 1.25f);
				if(rnd2==0)source[0].PlayOneShot(Spino1, 1.0f);
				else source[0].PlayOneShot(Spino2, 1.0f);
				lastframe=currframe; break;
			}
		}
	}

	//*************************************************************************************************************************************************
	// Add forces to the Rigidbody
	void FixedUpdate ()
	{
		StatusUpdate(); if(!IsActive | AnimSpeed==0.0f) { body.Sleep(); return; }
		OnReset=false; OnAttack=false; IsConstrained= false;

    //Set mass
		if(IsInWater) { if(Health>0) anm.SetInteger ("Move", 2); body.mass=2; body.drag=4; body.angularDrag=4; }
		else { body.mass=1; body.drag=4; body.angularDrag=4; }
    //Set Y position
    if(IsOnGround | IsInWater | IsOnWater)
    body.AddForce((Vector3.up*Size)*(posY-transform.position.y), ForceMode.VelocityChange);
    else body.AddForce((Vector3.up*Size)*-256, ForceMode.Acceleration);

		//Stopped
		if(NextAnm.IsName("Spino|Idle1A") | NextAnm.IsName("Spino|Idle2A") | CurrAnm.IsName("Spino|Idle1A") | CurrAnm.IsName("Spino|Idle2A") |
			CurrAnm.IsName("Spino|Die1") | CurrAnm.IsName("Spino|Die2"))
		{
			if(CurrAnm.IsName("Spino|Die1")) { OnReset=true; if(!IsDead) { PlaySound("Atk", 2); PlaySound("Die", 12); } }
			else if(CurrAnm.IsName("Spino|Die2")) { OnReset=true; if(!IsDead) { PlaySound("Atk", 2); PlaySound("Die", 10); } }
		}

		//End Forward
		else if((CurrAnm.IsName("Spino|Step1+") && CurrAnm.normalizedTime > 0.5) |
		 (CurrAnm.IsName("Spino|Step2+") && CurrAnm.normalizedTime > 0.5) |
		 (CurrAnm.IsName("Spino|ToIdle1C") && CurrAnm.normalizedTime > 0.5) | 
		 (CurrAnm.IsName("Spino|ToIdle2B") && CurrAnm.normalizedTime > 0.5) |
		 (CurrAnm.IsName("Spino|ToIdle2D") && CurrAnm.normalizedTime > 0.5) |
		 (CurrAnm.IsName("Spino|ToEatA") && CurrAnm.normalizedTime > 0.5) |
		 (CurrAnm.IsName("Spino|ToEatC") && CurrAnm.normalizedTime > 0.5) |
		 (CurrAnm.IsName("Spino|StepAtk1") && CurrAnm.normalizedTime > 0.5) |
		 (CurrAnm.IsName("Spino|StepAtk2") && CurrAnm.normalizedTime > 0.5))
			PlaySound("Step", 9);

		//Forward
		else if(CurrAnm.IsName("Spino|Walk") | CurrAnm.IsName("Spino|WalkGrowl") |
		   (CurrAnm.IsName("Spino|Step1+") && CurrAnm.normalizedTime < 0.5) |
		   (CurrAnm.IsName("Spino|Step2+") && CurrAnm.normalizedTime < 0.5) |
		   (CurrAnm.IsName("Spino|ToIdle2B") && CurrAnm.normalizedTime < 0.5) |
		   (CurrAnm.IsName("Spino|ToIdle1C") && CurrAnm.normalizedTime < 0.5) | 
		   (CurrAnm.IsName("Spino|ToIdle2D") && CurrAnm.normalizedTime < 05) |
		   (CurrAnm.IsName("Spino|ToEatA") && CurrAnm.normalizedTime < 0.5) |
		   (CurrAnm.IsName("Spino|ToEatC") && CurrAnm.normalizedTime < 0.5))
		{
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
			body.AddForce(transform.forward*48*Size*anm.speed);
			if(CurrAnm.IsName("Spino|WalkGrowl")) { PlaySound("Growl", 1); PlaySound("Step", 6); PlaySound("Step", 13); }
			else if(CurrAnm.IsName("Spino|Walk")) { PlaySound("Step", 6); PlaySound("Step", 13); }
			else { PlaySound("Step", 8); PlaySound("Step", 12); }
		}

		//Run
		else if(CurrAnm.IsName("Spino|Run") | CurrAnm.IsName("Spino|RunGrowl") |
		   CurrAnm.IsName("Spino|WalkAtk1") | CurrAnm.IsName("Spino|WalkAtk2") |
		   (CurrAnm.IsName("Spino|StepAtk1") && CurrAnm.normalizedTime < 0.6) |
		   (CurrAnm.IsName("Spino|StepAtk2") && CurrAnm.normalizedTime < 0.6))
		{
      roll=Mathf.Clamp(Mathf.Lerp(roll, spineX*5.0f, 0.05f), -20f, 20f);
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
			body.AddForce(transform.forward*128*Size*anm.speed);
			if(CurrAnm.IsName("Spino|RunGrowl")) { PlaySound("Growl", 1); PlaySound("Step", 6); PlaySound("Step", 13); }
			else if(CurrAnm.IsName("Spino|Run")) { PlaySound("Step", 6); PlaySound("Step", 13); }
			else if(CurrAnm.IsName("Spino|StepAtk1") | CurrAnm.IsName("Spino|StepAtk2")) { OnAttack=true; PlaySound("Atk", 2); PlaySound("Bite", 5); }
			else { OnAttack=true; PlaySound("Atk", 2); PlaySound("Step", 6); PlaySound("Bite", 10); PlaySound("Step", 13); }
		}

		//Backward
		else if((CurrAnm.IsName("Spino|Step1-") && CurrAnm.normalizedTime < 0.8) |
		   (CurrAnm.IsName("Spino|Step2-") && CurrAnm.normalizedTime < 0.8) |
		   (CurrAnm.IsName("Spino|ToSleep2")&& CurrAnm.normalizedTime < 0.8))
		{
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
			body.AddForce(transform.forward*-48*Size*anm.speed);
			PlaySound("Step", 12);
		}

		//Strafe/Turn right
		else if(CurrAnm.IsName("Spino|Strafe1-") | CurrAnm.IsName("Spino|Strafe2+"))
		{
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
			body.AddForce(transform.right*25*Size*anm.speed);
			PlaySound("Step", 6); PlaySound("Step", 13);
		}

		//Strafe/Turn left
		else if(CurrAnm.IsName("Spino|Strafe1+") | CurrAnm.IsName("Spino|Strafe2-"))
		{
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
			body.AddForce(transform.right*-25*Size*anm.speed);
			PlaySound("Step", 6); PlaySound("Step", 13);
		}

		else if(CurrAnm.IsName("Spino|IdleAtk1") | CurrAnm.IsName("Spino|IdleAtk2"))
		{ 
      OnAttack=true;
      transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
      PlaySound("Atk", 1); PlaySound("Step", 3); PlaySound("Bite", 6);
    } 

		//Various
		else if(CurrAnm.IsName("Spino|EatA")) { OnReset=true; IsConstrained=true; PlaySound("Food", 4); PlaySound("Bite", 5); }
		else if(CurrAnm.IsName("Spino|EatB") | CurrAnm.IsName("Spino|EatC")) { OnReset=true; IsConstrained=true; }
		else if(CurrAnm.IsName("Spino|Sleep")) { OnReset=true; IsConstrained=true; PlaySound("Repose", 2); }
		else if(CurrAnm.IsName("Spino|ToSleep1") | CurrAnm.IsName("Spino|ToSleep2")) { OnReset=true; IsConstrained=true; }
		else if(CurrAnm.IsName("Spino|ToIdle2A")) PlaySound("Sniff", 1);
		else if(CurrAnm.IsName("Spino|Idle1B")) PlaySound("Growl", 2);
		else if(CurrAnm.IsName("Spino|Idle1C")) { PlaySound("Sniff", 4); PlaySound("Sniff", 7); PlaySound("Sniff", 10);}
		else if(CurrAnm.IsName("Spino|Idle2B")) { OnReset=true; PlaySound("Bite", 4); PlaySound("Bite", 6); PlaySound("Bite", 8);}
		else if(CurrAnm.IsName("Spino|Idle2C")) PlaySound("Growl", 2);
		else if(CurrAnm.IsName("Spino|Idle2D")) { OnReset=true; PlaySound("Atk", 2); }
		else if(CurrAnm.IsName("Spino|Die1-")) { IsConstrained=true; PlaySound("Growl", 3);  IsDead=false; }
		else if(CurrAnm.IsName("Spino|Die2-")) { IsConstrained=true; PlaySound("Growl", 3);  IsDead=false; }
	}
	
	void LateUpdate()
	{
		//*************************************************************************************************************************************************
  	// Bone rotation
		if(!IsActive) return;

		TakeDamageAnim(Quaternion.Euler(lastHit, 0, 0));

		//Spine rotation
    RotateBone(65f);
		float headZ =-headY*headX/yaw_max;
		Spine0.rotation*= Quaternion.Euler(-headY, 0, headX);
		Spine1.rotation*= Quaternion.Euler(-headY, 0, headX);
		Spine2.rotation*= Quaternion.Euler(-headY, 0, headX);
		Neck0.rotation*= Quaternion.Euler(-headY, headZ, headX);
		Neck1.rotation*= Quaternion.Euler(-headY, headZ, headX);
		Neck2.rotation*= Quaternion.Euler(-headY, headZ, headX);
		Head.rotation*= Quaternion.Euler(-headY, headZ, headX);
		//Tail rotation
		Tail2.rotation*= Quaternion.Euler(0, 0, -spineX);
		Tail3.rotation*= Quaternion.Euler(0, 0, -spineX);
		Tail4.rotation*= Quaternion.Euler(0, 0, -spineX);
		Tail5.rotation*= Quaternion.Euler(0, 0, -spineX);
		Tail6.rotation*= Quaternion.Euler(0, 0, -spineX);
    //Legs rotation
    roll=Mathf.Lerp(roll, 0.0f, ang_t);
		Right_Hips.rotation*= Quaternion.Euler(-roll, 0, 0);
		Left_Hips.rotation*= Quaternion.Euler(-roll, 0, 0);

    HeadPos=Head;

		//Check for ground layer
		GetGroundPos(IkType.LgBiped, Right_Hips, Right_Leg, Right_Foot0, Left_Hips, Left_Leg, Left_Foot0);

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
