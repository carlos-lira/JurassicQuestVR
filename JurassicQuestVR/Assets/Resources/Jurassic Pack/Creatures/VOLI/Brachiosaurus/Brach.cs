using UnityEngine;

public class Brach : Creature
{
  [Space (10)] [Header("BRACHIOSAURUS SOUNDS")]
	public AudioClip Waterflush;
  public AudioClip Hit_jaw;
  public AudioClip Hit_head;
  public AudioClip Hit_tail;
  public AudioClip Largestep;
  public AudioClip Largesplash;
  public AudioClip Idleherb;
  public AudioClip Chew;
  public AudioClip Brach1;
  public AudioClip Brach2;
  public AudioClip Brach3;
  public AudioClip Brach4;
	Transform Spine0, Spine1, Spine2, Spine3, Spine4, Head, Tail0, Tail1, Tail2, Tail3, Tail4, Tail5, Tail6, Tail7, Tail8, 
	Neck0, Neck1, Neck2, Neck3, Neck4, Neck5, Neck6, Neck7, Neck8, Neck9, Neck10, Neck11, Neck12, Neck13, Neck14, Neck15, Neck16, 
	Left_Arm0, Right_Arm0, Left_Arm1, Right_Arm1, Left_Hand, Right_Hand, 
	Left_Hips, Right_Hips, Left_Leg, Right_Leg, Left_Foot, Right_Foot;

	//*************************************************************************************************************************************************
	//Get bone transforms
	void Start()
	{
		Left_Hips = transform.Find ("Brach/root/pelvis/left hips");
		Right_Hips = transform.Find ("Brach/root/pelvis/right hips");
		Left_Leg  = transform.Find ("Brach/root/pelvis/left hips/left leg");
		Right_Leg = transform.Find ("Brach/root/pelvis/right hips/right leg");

		Left_Foot = transform.Find ("Brach/root/pelvis/left hips/left leg/left foot");
		Right_Foot = transform.Find ("Brach/root/pelvis/right hips/right leg/right foot");

		Left_Arm0 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/left arm0");
		Right_Arm0 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/right arm0");
		Left_Arm1 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/left arm0/left arm1");
		Right_Arm1 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/right arm0/right arm1");

		Left_Hand = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/left arm0/left arm1/left hand");
		Right_Hand = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/right arm0/right arm1/right hand");

		Tail0 = transform.Find ("Brach/root/pelvis/tail0");
		Tail1 = transform.Find ("Brach/root/pelvis/tail0/tail1");
		Tail2 = transform.Find ("Brach/root/pelvis/tail0/tail1/tail2");
		Tail3 = transform.Find ("Brach/root/pelvis/tail0/tail1/tail2/tail3");
		Tail4 = transform.Find ("Brach/root/pelvis/tail0/tail1/tail2/tail3/tail4");
		Tail5 = transform.Find ("Brach/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5");
		Tail6 = transform.Find ("Brach/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6");
		Tail7 = transform.Find ("Brach/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7");
		Tail8 = transform.Find ("Brach/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7/tail8");

    Spine0 = transform.Find ("Brach/root/spine0");
    Spine1 = transform.Find ("Brach/root/spine0/spine1");
    Spine2 = transform.Find ("Brach/root/spine0/spine1/spine2");
    Spine3 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3");
    Spine4 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4");
		Neck0 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0");
		Neck1 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1");
		Neck2 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2");
		Neck3 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3");
		Neck4 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4");
		Neck5 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5");
		Neck6 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6");
		Neck7 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7");
		Neck8 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8");
		Neck9 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9");
		Neck10 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10");
		Neck11 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11");
		Neck12 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11/neck12");
		Neck13 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11/neck12/neck13");
		Neck14 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11/neck12/neck13/neck14");
		Neck15 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11/neck12/neck13/neck14/neck15");
		Neck16 = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11/neck12/neck13/neck14/neck15/neck16");
		Head = transform.Find ("Brach/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/neck4/neck5/neck6/neck7/neck8/neck9/neck10/neck11/neck12/neck13/neck14/neck15/neck16/head");
		
    crouch_max=8f;
		ang_t=0.01f;
		yaw_max=10f;
		pitch_max=8f;
  }

	//*************************************************************************************************************************************************
	//Play sound
	void OnCollisionStay(Collision col)
	{
		int rndPainsnd=Random.Range(0, 4); AudioClip painSnd=null;
		switch (rndPainsnd) { case 0: painSnd=Brach1; break; case 1: painSnd=Brach2; break; case 2: painSnd=Brach3; break; case 3: painSnd=Brach4; break; }
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
				else if(IsOnGround) source[1].PlayOneShot(Largestep, Random.Range(0.25f, 0.5f));
				lastframe=currframe; break;
			case "Hit": source[1].pitch=Random.Range(1.0f, 1.25f); source[1].PlayOneShot(IsOnWater|IsInWater?Largesplash:Largestep, 1.5f);
				lastframe=currframe; break;
			case "Die": source[1].pitch=Random.Range(1.0f, 1.25f); source[1].PlayOneShot(IsOnWater|IsInWater?Largesplash:Largestep, 1.0f);
				lastframe=currframe; IsDead=true; break;
			case "Chew": source[0].pitch=Random.Range(1.0f, 1.25f); source[0].PlayOneShot(Chew, 0.75f);
				lastframe=currframe; break;
			case "Repose": source[0].pitch=Random.Range(1.0f, 1.25f); source[0].PlayOneShot(Idleherb, 0.25f);
				lastframe=currframe; break;
			case "Growl": int rnd = Random.Range (0, 4); source[0].pitch=Random.Range(1.0f, 1.25f);
				if(rnd==0)source[0].PlayOneShot(Brach1, 1.0f);
				else if(rnd==1)source[0].PlayOneShot(Brach2, 1.0f);
				else if(rnd==2)source[0].PlayOneShot(Brach3, 1.0f);
				else if(rnd==3)source[0].PlayOneShot(Brach4, 1.0f);
				lastframe=currframe; break;
			}
		}
	}
	
	//*************************************************************************************************************************************************
	// Add forces to the Rigidbody
	void FixedUpdate ()
	{
		StatusUpdate(); if(!IsActive | AnimSpeed==0.0f) { body.Sleep(); return; }
		OnReset=false; IsConstrained= false;

    //Set mass
		if(IsInWater) { if(Health>0) anm.SetInteger ("Move", 2); body.mass=2; body.drag=4; body.angularDrag=4; }
		else { body.mass=1; body.drag=4; body.angularDrag=4; }
    //Set Y position
    if(IsOnGround | IsInWater | IsOnWater)
    body.AddForce((Vector3.up*Size)*(posY-transform.position.y), ForceMode.VelocityChange);
    else body.AddForce((Vector3.up*Size)*-256, ForceMode.Acceleration);

		//Stopped
		if(NextAnm.IsName("Brach|IdleA") | CurrAnm.IsName("Brach|IdleA") | CurrAnm.IsName("Brach|Die"))
		{
			if(CurrAnm.IsName("Brach|Die")) { OnReset=true; if(!IsDead) { PlaySound("Growl", 3); PlaySound("Die", 12); } }
		}
	
		//Forward
		else if(NextAnm.IsName("Brach|Walk") | CurrAnm.IsName("Brach|Walk") | CurrAnm.IsName("Brach|WalkGrowl"))
		{
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
			body.AddForce(transform.forward*15*Size*anm.speed);
			if(CurrAnm.IsName("Brach|WalkGrowl")) { PlaySound("Growl", 1); PlaySound("Step", 5); PlaySound("Step", 12); }
			else { PlaySound("Step", 5); PlaySound("Step", 12); }
		}

		//Run
		else if(NextAnm.IsName("Brach|Run") | CurrAnm.IsName("Brach|Run") | CurrAnm.IsName("Brach|RunGrowl"))
		{
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
			body.AddForce(transform.forward*30*Size*anm.speed);
			if(CurrAnm.IsName("Brach|RunGrowl")) { PlaySound("Growl", 2); PlaySound("Step", 5); PlaySound("Step", 12); }
			else { PlaySound("Step", 5); PlaySound("Step", 12); }
		}

		//Backward
		else if(NextAnm.IsName("Brach|Walk-") | CurrAnm.IsName("Brach|Walk-") | CurrAnm.IsName("Brach|WalkGrowl-"))
		{
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
			body.AddForce(transform.forward*-15*Size*anm.speed);
			if(CurrAnm.IsName("Brach|WalkGrowl-")) { PlaySound("Growl", 4); PlaySound("Step", 5); PlaySound("Step", 12); }
			else { PlaySound("Step", 5); PlaySound("Step", 12); }
		}

		//Strafe/Turn right
		else if(NextAnm.IsName("Brach|Strafe-") | CurrAnm.IsName("Brach|Strafe-"))
		{
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
			body.AddForce(transform.right*5*Size*anm.speed);
			PlaySound("Step", 5); PlaySound("Step", 12);
		}

		//Strafe/Turn left
		else if(NextAnm.IsName("Brach|Strafe+") | CurrAnm.IsName("Brach|Strafe+"))
		{
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, anm.GetFloat("Turn"), transform.eulerAngles.z), ang_t);
			body.AddForce(transform.right*-5*Size*anm.speed);
			PlaySound("Step", 5); PlaySound("Step", 12);
		}

		//Various
		else if(CurrAnm.IsName("Brach|EatA")) PlaySound("Chew", 10);
		else if(CurrAnm.IsName("Brach|EatB")) OnReset=true;
		else if(CurrAnm.IsName("Brach|EatC")) { OnReset=true; PlaySound("Chew", 1); PlaySound("Chew", 4); PlaySound("Chew", 8); PlaySound("Chew", 12); }
		else if(CurrAnm.IsName("Brach|ToSit")) IsConstrained=true;
		else if(CurrAnm.IsName("Brach|ToSit-")) IsConstrained=true;
		else if(CurrAnm.IsName("Brach|SitIdle")) IsConstrained=true;
		else if(CurrAnm.IsName("Brach|Sleep") | CurrAnm.IsName("Brach|ToSleep") ) { OnReset=true; IsConstrained=true; PlaySound("Repose", 2); }
		else if(CurrAnm.IsName("Brach|SitGrowl")) { PlaySound("Growl", 7); IsConstrained=true; }
		else if(CurrAnm.IsName("Brach|IdleB")) PlaySound("Growl", 2);
		else if(CurrAnm.IsName("Brach|RiseIdle")) OnReset=true;
		else if(CurrAnm.IsName("Brach|RiseGrowl")) { OnReset=true; PlaySound("Growl", 2); }
		else if(CurrAnm.IsName("Brach|ToRise")) { OnReset=true; PlaySound("Growl", 5); }
		else if(CurrAnm.IsName("Brach|ToRise-")) { OnReset=true; PlaySound("Growl", 1); PlaySound("Hit", 4);}
		else if(CurrAnm.IsName("Brach|Die-")) { PlaySound("Growl", 3);  IsDead=false; }
	}

	void LateUpdate()
	{
		//*************************************************************************************************************************************************
		// Bone rotation
		if(!IsActive) return;

		TakeDamageAnim(Quaternion.Euler(-lastHit, 0, 0));

		//Spine rotation
		RotateBone(40f, 0.0f, true, 0.25f);
		float headZ =-headY*headX/yaw_max;
    Spine0.rotation*= Quaternion.Euler(0, 0, spineX);
    Spine1.rotation*= Quaternion.Euler(0, 0, spineX);
    Spine2.rotation*= Quaternion.Euler(0, 0, spineX);
    Spine3.rotation*= Quaternion.Euler(0, 0, spineX);
    Spine4.rotation*= Quaternion.Euler(0, 0, spineX);
		Neck0.rotation*= Quaternion.Euler(0, 0, headX);
		Neck1.rotation*= Quaternion.Euler(0, 0, headX);
		Neck2.rotation*= Quaternion.Euler(0, 0, headX);
		Neck3.rotation*= Quaternion.Euler(0, 0, headX);
		Neck4.rotation*= Quaternion.Euler(0, 0, headX);
		Neck5.rotation*= Quaternion.Euler(0, 0, headX);
		Neck6.rotation*= Quaternion.Euler(0, 0, headX);
		Neck7.rotation*= Quaternion.Euler(0, 0, headX);
		Neck8.rotation*= Quaternion.Euler(headY, 0, 0);
		Neck9.rotation*= Quaternion.Euler(headY, 0, 0);
		Neck10.rotation*= Quaternion.Euler(headY, 0, 0);
		Neck11.rotation*= Quaternion.Euler(headY, 0, 0);
		Neck12.rotation*= Quaternion.Euler(headY, 0, 0);
		Neck13.rotation*= Quaternion.Euler(headY, 0, 0);
		Neck14.rotation*= Quaternion.Euler(headY, 0, 0);
		Neck15.rotation*= Quaternion.Euler(headY, headZ, headX);
		Neck16.rotation*= Quaternion.Euler(headY, headZ, headX);
		Head.rotation*= Quaternion.Euler(headY, headZ, headX);
		//Tail rotation
		Tail0.rotation*= Quaternion.Euler(0, 0, -spineX);
		Tail1.rotation*= Quaternion.Euler(0, 0, -spineX);
		Tail2.rotation*= Quaternion.Euler(0, 0, -spineX);
		Tail3.rotation*= Quaternion.Euler(0, 0, -spineX);
		Tail4.rotation*= Quaternion.Euler(0, 0, -spineX);
		Tail5.rotation*= Quaternion.Euler(0, 0, -spineX);
		Tail6.rotation*= Quaternion.Euler(0, 0, -spineX);
		Tail7.rotation*= Quaternion.Euler(0, 0, -spineX);
		Tail8.rotation*= Quaternion.Euler(0, 0, -spineX);

    HeadPos=Head;

		//Check for ground layer
		GetGroundPos(IkType.Quad, Right_Hips, Right_Leg, Right_Foot, Left_Hips, Left_Leg, Left_Foot, Right_Arm0, Right_Arm1, Right_Hand, Left_Arm0, Left_Arm1, Left_Hand, -0.6f*Size);

		//*************************************************************************************************************************************************
		// CPU
		if(UseAI && Health!=0) { AICore(1, 4, 0, 0, 2, 3, 5); }
		//*************************************************************************************************************************************************
		// Human
		else if(Health!=0) { GetUserInputs(1, 4, 0, 0, 2, 3, 5, 4); }
		//*************************************************************************************************************************************************
		//Dead
		else { anm.SetInteger ("Move", 0); anm.SetInteger ("Idle", -1); }
	}
}