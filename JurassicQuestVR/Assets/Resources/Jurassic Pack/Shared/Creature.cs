using UnityEngine;
using System.Collections.Generic;

public class Creature : MonoBehaviour 
{
  #region Variables
  [Space (10)] [Header("ARTIFICIAL INTELLIGENCE")]
	public bool UseAI=false;
	const string PathHelp=
	"Use gameobjects as waypoints to define a path for this creature by \n"+
	"taking into account the priority between autonomous AI and its path.";
	const string WaypointHelp=
	"Place your waypoint gameobject in a reacheable position.\n"+
	"Don't put a waypoint in air if the creature are not able to fly";
	const string PriorityPathHelp=
	"Using a priority of 100% will disable all autonomous AI for this waypoint\n"+
	"Obstacle avoid AI and custom targets search still enabled";
	const string TargetHelp=
	"Use gameobjects to assign a custom enemy/friend for this creature\n"+
	"Can be any kind of gameobject e.g : player, other creature.\n"+
	"The creature will include friend/enemy goals in its search. \n"+
	"Enemy: triggered if the target is in range. \n"+
	"Friend: triggered when the target moves away.";
	const string MaxRangeHelp=
	"If MaxRange is zero, range is infinite. \n"+
	"Creature will start his attack/tracking once in range.";
	//Path editor
	[Space (10)]
  [Tooltip(PathHelp)] public List<_PathEditor> PathEditor;
	[HideInInspector] public int nextPath=0;
	[HideInInspector] public enum PathType { Walk, Run };
  [HideInInspector] public enum TargetAction { None, Sleep, Eat, Drink };
	[System.Serializable] public struct _PathEditor
	{
		[Tooltip(WaypointHelp)] public GameObject _Waypoint;
		public PathType _PathType;
    public TargetAction _TargetAction;
		[Tooltip(PriorityPathHelp)] [Range(1, 100)] public int _Priority;
   
    public _PathEditor(GameObject Waypoint, PathType PathType, TargetAction TargetAction, int Priority)
    {_Waypoint=Waypoint; _PathType=PathType; _TargetAction=TargetAction; _Priority=Priority; }
	}

	//Target editor
	[Space (10)]
  [Tooltip(TargetHelp)]  public List< _TargetEditor> TargetEditor;
	[HideInInspector] public enum TargetType { Enemy, Friend };
	[System.Serializable] public struct _TargetEditor
	{
		public GameObject _GameObject;
		public TargetType _TargetType;
		[Tooltip(MaxRangeHelp)]
		public int MaxRange;
	}

  [Space (10)] [Header("CREATURE SETTINGS")]
	public Skin bodyTexture;
	public Eyes eyesTexture;
  [Space (5)]
	[Range(0.0f, 100.0f)] public float Health=100f;
	[Range(0.0f, 100.0f)] public float Water=100f;
	[Range(0.0f, 100.0f)] public float Food=100f;
	[Range(0.0f, 100.0f)] public float Stamina=100f;
  [Space (5)]
	[Range(1.0f, 10.0f)] public float DamageMultiplier=1.0f;
	[Range(1.0f, 10.0f)] public float ArmorMultiplier=1.0f;
	[Range(0.0f, 2.0f)] public float AnimSpeed=1.0f;

  public bool Herbivorous, CanAttack, CanHeadAttack, CanTailAttack, CanWalk, CanJump, CanFly, CanSwim, CanDepthSwim, CanInvertBody;

  [Space (20)] [Header("CREATURE COMPONENTS")]
  public Rigidbody body;
  public LODGroup lod;
	public Animator anm;
  public AudioSource[] source;
  public SkinnedMeshRenderer[] rend;
	public Texture[] skin, eyes;
	public enum Skin {SkinA, SkinB, SkinC};
	public enum Eyes {Type0, Type1, Type2, Type3, Type4, Type5, Type6, Type7, Type8, Type9, Type10, Type11, Type12, Type13, Type14, Type15};


  [HideInInspector] public Manager manager=null;
  [HideInInspector] public AnimatorStateInfo CurrAnm, NextAnm;
	[HideInInspector] public bool IsActive, IsVisible, IsDead, IsOnGround, IsOnWater, IsInWater, IsConstrained, IsOnLevitation;
  [HideInInspector] public bool OnAttack, OnJump, OnCrouch, OnReset, OnInvert, OnHeadMove, OnAutoLook, OnTailAttack;
	[HideInInspector] public float currframe, lastframe, lastHit, behaviorCount, avoidTime, delta, angleAdd;
  [HideInInspector] public float posY, waterY, withersSize, actionDist, Size;
  [HideInInspector] public float rndX, rndY, crouch_max, yaw_max, pitch_max, ang_t;
  [HideInInspector] public float crouch, spineX, spineY, headX, headY, pitch, roll, reverse;
	[HideInInspector] public int rndMove, rndIdle, loop;
	[HideInInspector] public string behavior, specie;
  [HideInInspector] public Transform HeadPos=null;
	[HideInInspector] public GameObject objTGT=null, objCOL=null;
	[HideInInspector] public Vector3 posTGT=Vector3.zero, lookTGT=Vector3.zero, boxscale=Vector3.zero;
  [HideInInspector] public Quaternion angTGT=Quaternion.identity;
  [HideInInspector] float distTGT;
	const int enemyMaxRange=50, waterMaxRange=200, foodMaxRange=200, friendMaxRange=200, preyMaxRange=200;

  //IK TYPES
  public enum IkType { None, Convex, Quad, Flying, SmBiped, LgBiped }
	// IK goal position
	Vector3 FR_HIT, FL_HIT, BR_HIT, BL_HIT;
  // IK goal normal
  Vector3 FR_Norm=Vector3.up, FL_Norm=Vector3.up, BR_Norm=Vector3.up, BL_Norm=Vector3.up;
	//Back Legs
	float BR1, BR2, BR3, BR_Add; //Right
	float BL1, BL2, BL3, BL_Add; //Left
	//Front Legs
	float FR1, FR2, FR3, FR_Add; //Right
	float FL1, FL2, FL3, FL_Add; //Left
  #endregion
  #region Creature Start
	void OnEnable()
	{
    //Get manager compononent
		manager=Camera.main.GetComponent<Manager>();
    //Start scale and materials
		SetScale(transform.localScale.x);
		SetMaterials(bodyTexture.GetHashCode(), eyesTexture.GetHashCode());
    //Randomise start action
		loop=Random.Range(0, 50);
    //Creature specie name
		specie=transform.GetChild(0).name;
	}
#endregion
  #region Creature Setup
  //AI on/off
  public void SetAI(bool UseAI) { this.UseAI=UseAI; if(!this.UseAI) { posTGT=Vector3.zero; objTGT=null; objCOL=null; behaviorCount=0; } }
  //Change materials
  #if UNITY_EDITOR
  void OnDrawGizmosSelected()
	{
		foreach (SkinnedMeshRenderer o in rend)
		{
			if(o.sharedMaterials[0].mainTexture!=skin[bodyTexture.GetHashCode()]) o.sharedMaterials[0].mainTexture=skin[bodyTexture.GetHashCode()];
      if(o.sharedMaterials[1].mainTexture!=eyes[eyesTexture.GetHashCode()]) o.sharedMaterials[1].mainTexture=eyes[eyesTexture.GetHashCode()];
		}
	}
	#endif
  
	public void SetMaterials(int bodyindex, int eyesindex)
	{
		bodyTexture= (Skin) bodyindex; eyesTexture= (Eyes) eyesindex;
		foreach (SkinnedMeshRenderer o in rend)
		{
			o.materials[0].mainTexture = skin[bodyindex];
			o.materials[1].mainTexture = eyes[eyesindex];
		}
	}

  //Creature size
	public void SetScale(float resize)
	{
    Size=resize;
		transform.localScale=new Vector3(resize, resize, resize); //creature size
		withersSize = (transform.GetChild(0).GetChild(0).position-transform.position).magnitude; //At the withers altitude
		boxscale = rend[0].bounds.extents; //bounding box scale
	}
  #endregion
  #region Counters and culling
  public void StatusUpdate()
	{
		// Check if this creature are visible or near the camera, if not, and game are not in realtime mode, turn off all activity
    IsVisible=false;
    foreach (SkinnedMeshRenderer o in rend) { if(o.isVisible) IsVisible=true; }
    
		/*if(!manager.RealtimeGame)
    {
      float dist=(Camera.main.transform.position-transform.position).magnitude;
      if(!IsVisible && dist>100f) { IsActive=false; anm.cullingMode=AnimatorCullingMode.CullCompletely; return; }
      else { IsActive=true; anm.cullingMode=AnimatorCullingMode.AlwaysAnimate; }
    } else { IsActive=true; anm.cullingMode=AnimatorCullingMode.AlwaysAnimate; }
	*/
    //Set animation speed
    anm.speed = AnimSpeed;

		//Get current animation/frame
    CurrAnm=anm.GetCurrentAnimatorStateInfo(0);
		NextAnm=anm.GetNextAnimatorStateInfo(0);
		if(currframe==15f | anm.GetAnimatorTransitionInfo(0).normalizedTime>0.5) { currframe=0.0f; lastframe=-1; }
		else currframe = Mathf.Round((anm.GetCurrentAnimatorStateInfo (0).normalizedTime % 1.0f) * 15f);

		//Manage health bar
		if(Health>0.0f)
		{
			if(loop==50)	
			{
        if(CanSwim)
        { 
          if(anm.GetInteger("Move")!=0) Food=Mathf.Clamp(Food-0.01f, 0.0f, 100f);
          if(IsInWater | IsOnWater) { Stamina=Mathf.Clamp(Stamina+1.0f, 0.0f, 100f); Water=Mathf.Clamp(Water+1.0f, 0.0f, 100f); }  
          else if(CanWalk) { Stamina=Mathf.Clamp(Stamina-0.01f, 0.0f, 100f);  Water=Mathf.Clamp(Water-0.01f, 0.0f, 100f); }
          else { Stamina=Mathf.Clamp(Stamina-1.0f, 0.0f, 100f); Water=Mathf.Clamp(Water-1.0f, 0.0f, 100f); Health=Mathf.Clamp(Health-1.0f, 0.0f, 100f); }
        }
        else
        { 
          if(anm.GetInteger("Move")!=0) { Stamina=Mathf.Clamp(Stamina-0.01f, 0.0f, 100f); Water=Mathf.Clamp(Water-0.01f, 0.0f, 100f); Food=Mathf.Clamp(Food-0.01f, 0.0f, 100f); }
          if(IsInWater) { Stamina=Mathf.Clamp(Stamina-1.0f, 0.0f, 100f); Health=Mathf.Clamp(Health-1.0f, 0.0f, 100f); } 
        }

        if(Food==0.0f | Stamina==0.0f | Water==0.0f) Health=Mathf.Clamp(Health-0.1f, 0.0f, 100f); else Health=Mathf.Clamp(Health+0.1f, 0.0f, 100f);
				loop=0;
			}
      else loop++;
		}
		else
		{
			/*
			Water=0.0f; Food=0.0f; Stamina=0.0f; behavior="Dead";
			if(manager.TimeAfterDead==0) return;
			if(behaviorCount>0) behaviorCount=0;
			else if(behaviorCount==-manager.TimeAfterDead)
			{
				//Delete from list and destroy gameobject
				if(manager.selected>=manager.creaturesList.IndexOf(transform.gameObject)) { if(manager.selected>0) manager.selected--; }
				manager.creaturesList.Remove(transform.gameObject); Destroy(transform.gameObject);
			}
			else behaviorCount--;
			*/
		}

    if(anm.GetFloat("Turn")>=360f) anm.SetFloat("Turn", 0.0000f);
    else if(anm.GetFloat("Turn")<0.0f) anm.SetFloat("Turn", 359.9999f);
	}
#endregion
  #region Player Inputs
  public void GetUserInputs(int idle1=0, int idle2=0, int idle3=0, int idle4=0, int eat=0, int drink=0, int sleep=0, int rise=0)
	{
    if(AnimSpeed==0.0f) return;
		if(behavior=="Repose" && anm.GetInteger("Move")!=0) behavior="Player";
		else if(behaviorCount<=0) { objTGT=null; behavior="Player"; behaviorCount=0; } else behaviorCount--;

		// Current camera manager target ?
		if(manager.UseManager && transform.gameObject==manager.creaturesList[manager.selected].gameObject && manager.CameraMode!=0)
		{
			//Run key
			bool run=Input.GetKey(KeyCode.LeftShift)?true:false;

			//Attack key
			if(CanAttack)
      { 
        if(Input.GetKey(KeyCode.Mouse0)) { behaviorCount=500; behavior="Hunt"; anm.SetBool ("Attack", true); }
        else anm.SetBool ("Attack", false);
      }

			//Crouch key
			if(IsOnGround && Input.GetKey(KeyCode.LeftControl)) { crouch = Mathf.Lerp(crouch, crouch_max*Size, ang_t); OnCrouch=true; }
			else OnCrouch=false;

			//Fly/swim up/down key
			if(CanFly | CanSwim)
			{
				if(Input.GetKey(KeyCode.Mouse1))
				{
					anm.SetFloat("Turn", Mathf.Lerp(anm.GetFloat("Turn"), anm.GetFloat("Turn")+Input.GetAxis("Mouse X")*32f, ang_t));//Mouse turn
					if(Input.GetAxis("Mouse Y")!=0 && anm.GetInteger("Move")==3) //Pitch with mouse if is moving
					anm.SetFloat("Pitch", Mathf.Lerp(anm.GetFloat("Pitch"), Input.GetAxis("Mouse Y"), ang_t));
					else if(Input.GetKey(KeyCode.LeftControl)) anm.SetFloat("Pitch", Mathf.Lerp(anm.GetFloat("Pitch"), 1.0f, ang_t));
					else if(Input.GetKey(KeyCode.Space)) anm.SetFloat("Pitch", Mathf.Lerp(anm.GetFloat("Pitch"), -1.0f, ang_t));
				}
				else
				{
					if(Input.GetKey(KeyCode.LeftControl)) anm.SetFloat("Pitch", Mathf.Lerp(anm.GetFloat("Pitch"), 1.0f, ang_t));
					else if(Input.GetKey(KeyCode.Space)) anm.SetFloat("Pitch", Mathf.Lerp(anm.GetFloat("Pitch"), -1.0f, ang_t));
					else anm.SetFloat("Pitch", Mathf.MoveTowards(anm.GetFloat("Pitch"), 0.0f, 0.01f));
				}
			}

			//Space key
			if(Input.GetKeyDown(KeyCode.Space))
      {
        if(CanJump && IsOnGround) anm.SetInteger ("Move", 3);  //Jump
        else if(CanInvertBody) { if(OnInvert) OnInvert=false; else OnInvert=true; } //Invert body
      }
     
			//Moving keys
			else if(Input.GetAxis("Horizontal")!=0 | Input.GetAxis("Vertical")!=0)
			{
        //Flying/swim
				if(CanSwim | (CanFly&&!IsOnGround))
				{
					if(Input.GetKey(KeyCode.Mouse1))
					{
						if(Input.GetAxis("Vertical")<0) anm.SetInteger ("Move", -1); //Backward
						else if(Input.GetAxis("Vertical")>0) anm.SetInteger ("Move", 3); //Forward
						else if(Input.GetAxis("Horizontal")>0) anm.SetInteger ("Move", -10); //Strafe-
						else if(Input.GetAxis("Horizontal")<0) anm.SetInteger ("Move", 10); //Strafe+
						else anm.SetInteger ("Move", 0);
					}
					else
					{
						if(run) anm.SetInteger ("Move", CanSwim?2:1); else  anm.SetInteger ("Move", CanSwim?1:2); 
            float ang=manager.transform.eulerAngles.y+Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))*Mathf.Rad2Deg;
            anm.SetFloat("Turn", Mathf.LerpAngle(anm.GetFloat("Turn"), ang, ang_t)); //Turn
					}
				}
        //Terrestrial
				else
				{
					if(Input.GetKey(KeyCode.Mouse1))
					{
						if(Input.GetAxis("Vertical")>0 && !run) anm.SetInteger ("Move", 1); //Forward
						else if(Input.GetAxis("Vertical")>0) anm.SetInteger ("Move", 2); //Run
						else if(Input.GetAxis("Vertical")<0) anm.SetInteger ("Move", -1);	//Backward
						else if(Input.GetAxis("Horizontal")>0) anm.SetInteger ("Move", -10); //Strafe-
						else if(Input.GetAxis("Horizontal")<0) anm.SetInteger ("Move", 10); //Strafe+
						anm.SetFloat("Turn", Mathf.LerpAngle(anm.GetFloat("Turn"), anm.GetFloat("Turn")+Input.GetAxis("Mouse X")*16f, ang_t));//Mouse turn
					}
					else
					{
            float ang=manager.transform.eulerAngles.y+Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))*Mathf.Rad2Deg;
            anm.SetInteger ("Move", run?2:1);
						anm.SetFloat("Turn", Mathf.LerpAngle(anm.GetFloat("Turn"), ang, ang_t)); //Turn
            delta=Input.GetAxis("Horizontal")>=0?45:-45;
					}
				}
			}
			//Stoped
			else
			{
        //Flying/Swim
				if((CanSwim | CanFly) && !IsOnGround)
				{
					if(CanSwim && anm.GetFloat("Pitch")!=0 && !Input.GetKey(KeyCode.Mouse1)) anm.SetInteger ("Move", run?2:1);
					else anm.SetInteger ("Move", 0);
				}
         //Terrestrial
				else
				{
					delta = Mathf.DeltaAngle(manager.transform.eulerAngles.y, transform.eulerAngles.y);
					if(Input.GetKey(KeyCode.Mouse1))
					{
						if(Input.GetAxis("Mouse X")>0) anm.SetInteger ("Move", 10); //Strafe- 
						else if(Input.GetAxis("Mouse X")<0) anm.SetInteger ("Move", -10); //Strafe+
						else anm.SetInteger ("Move", 0);
						anm.SetFloat("Turn", Mathf.LerpAngle(anm.GetFloat("Turn"), anm.GetFloat("Turn")+Input.GetAxis("Mouse X")*16f, ang_t));//Mouse turn
					}
					else
          {
            anm.SetFloat("Turn", Mathf.LerpAngle(anm.GetFloat("Turn"), transform.eulerAngles.y, ang_t));
            anm.SetInteger ("Move", 0); //Stop
          }
				}
			}


			//Idles key
			if(Input.GetKey(KeyCode.E))
			{
				if(Input.GetKeyDown(KeyCode.E))
				{
					int idles=0; if(idle1>0) idles++; if(idle2>0) idles++; if(idle3>0) idles++; if(idle4>0) idles++; //idles to play
					rndIdle = Random.Range(1, idles+1);
				}
        switch(rndIdle)
        {
				  case 1: anm.SetInteger ("Idle", idle1); break;
          case 2: anm.SetInteger ("Idle", idle2); break;
          case 3: anm.SetInteger ("Idle", idle3); break;
          case 4: anm.SetInteger ("Idle", idle4); break;
        }
			}
			else if(Input.GetKey(KeyCode.F)) //Eat / Drink
			{
				if(posTGT==Vector3.zero) FindPlayerFood(); //looking for food
				//Drink
				if(IsOnWater)
				{
					anm.SetInteger ("Idle", drink);
					if(Water<100) { behavior="Water"; Water=Mathf.Clamp(Water+0.05f, 0.0f, 100f); }
					if(Input.GetKeyUp(KeyCode.F)) posTGT=Vector3.zero;
					else posTGT=transform.position;
				}
				//Eat
				else if(posTGT!=Vector3.zero)
				{
					anm.SetInteger ("Idle", eat); behavior="Food";
					if(Food<100) Food=Mathf.Clamp(Food+0.05f, 0.0f, 100f);
					if(Water<25) Water+=0.05f;
					if(Input.GetKeyUp(KeyCode.F)) posTGT=Vector3.zero;
				}
				//nothing found
				else manager.message=1;
			}
			//Sleep/Sit
			else if(Input.GetKey(KeyCode.Q))
			{ 
				anm.SetInteger("Idle", sleep);
				if(anm.GetInteger("Move")!=0) anm.SetInteger ("Idle", 0);
			}
			//Rise
			else if(rise!=0 && Input.GetKey(KeyCode.Space)) anm.SetInteger ("Idle", rise);
			else { anm.SetInteger ("Idle", 0); posTGT=Vector3.zero; }

      //Head move
      if(Input.GetKey(KeyCode.Mouse2))
      {
        OnHeadMove=true;
        headX=Mathf.Lerp(headX, Mathf.Clamp(headX-Input.GetAxis("Mouse X"), -yaw_max, yaw_max), 0.5f);
        headY=Mathf.Lerp(headY, Mathf.Clamp(headY+Input.GetAxis("Mouse Y"), -pitch_max, pitch_max),  0.5f);
      } else OnHeadMove=false;

			if(CurrAnm.IsName(specie+"|Sleep"))
			{ behavior="Repose"; Stamina=Mathf.Clamp(Stamina+0.05f, 0.0f, 100f); }

		}
		// Not current camera target, reset parameters
		else
		{
			anm.SetFloat("Turn", Mathf.LerpAngle(anm.GetFloat("Turn"), transform.eulerAngles.y, ang_t));
      anm.SetInteger ("Move", 0); anm.SetInteger ("Idle", 0); //Stop
			if(CanAttack) anm.SetBool ("Attack", false);
			if(CanFly | CanSwim) anm.SetFloat ("Pitch", 0.0f);
		}
	}
  
bool FindPlayerFood()
{
		//Find carnivorous food (looking for a dead creature in range)
		if(!Herbivorous)
		{
			foreach (GameObject o in manager.creaturesList.ToArray())
			{
				if((o.transform.position-HeadPos.position).magnitude>boxscale.z) continue; //not in range
				Creature other= o.GetComponent<Creature>(); //Get other creature script
				if(other.IsDead) { objTGT=other.gameObject; posTGT = other.body.worldCenterOfMass; return true; } // meat found
			}
		}
		else
		{
			//Find herbivorous food (looking for trees/details on terrain in range )
			if(manager.t)
			{
				//Large creature, look for trees
				if(withersSize>8) 
				{
          if(Physics.CheckSphere(HeadPos.position, withersSize, manager.treeLayer)) { posTGT = HeadPos.position; return true; }
          else return false;
				}
				//Look for grass detail
				else
				{
					int layer=0;
					float x = ((transform.position.x - manager.t.transform.position.x) / manager.tdata.size.z * manager.tres);
          float y = ((transform.position.z - manager.t.transform.position.z) / manager.tdata.size.x * manager.tres);

					for(layer=0; layer<manager.tdata.detailPrototypes.Length; layer++)
					{
						if(manager.tdata.GetDetailLayer( (int) x,  (int) y, 1, 1, layer) [ 0, 0]>0)
						{
							posTGT.x=(manager.tdata.size.x/manager.tres)*x+manager.t.transform.position.x;
							posTGT.z=(manager.tdata.size.z/manager.tres)*y+manager.t.transform.position.z;
							posTGT.y = manager.t.SampleHeight( new Vector3(posTGT.x, 0, posTGT.z)); 
							objTGT=null; return true; 
						}
					}
				}
			}
		}

		objTGT=null; posTGT=Vector3.zero; return false; //nothing found...
}
#endregion
  #region Rotate Creature Skeleton
  public void RotateBone(float maxX, float maxY=0, bool CanMoveHead=true, float t=0.5f)
  {
    if(AnimSpeed==0.0f) return;

    if(OnReset)
    {
      pitch = Mathf.Lerp(pitch, 0.0f, t/10f);
      roll = Mathf.Lerp(roll, 0.0f, t/10f);
      headX= Mathf.LerpAngle(headX, 0.0f, t/10f);
      headY= Mathf.LerpAngle(headY, 0.0f, t/10f);
      crouch=Mathf.Lerp(crouch, 0.0f, t/10f);
      spineX= Mathf.LerpAngle(spineX, 0.0f, t/10f);
      spineY= Mathf.LerpAngle(spineY, 0.0f, t/10f);
      return;
    }
 
    if(objTGT)
		{
			if(behavior.EndsWith("Hunt") | behavior.Equals("Battle") |  behavior.EndsWith("Contest") ) lookTGT=objTGT.transform.position;
      else if(Herbivorous && behavior.Equals("Food")) lookTGT=posTGT;
			else if(loop==0) lookTGT=Vector3.zero;
		} else if(loop==0) lookTGT=Vector3.zero;

    if(CanMoveHead)
    {
      if(!OnTailAttack && !anm.GetInteger("Move").Equals(0))
      {
        spineX= Mathf.MoveTowardsAngle(spineX, (Mathf.DeltaAngle(anm.GetFloat("Turn"), transform.eulerAngles.y)/360f)*maxX, t);
        spineY= Mathf.LerpAngle(spineY, 0.0f, t/10f);
      }
      else
      {
        spineX= Mathf.MoveTowardsAngle(spineX, 0.0f, t/10f);
        spineY= Mathf.LerpAngle(spineY, 0.0f, t/10f);
      }

		  crouch=Mathf.Lerp(crouch, 0.0f, t/10f);

      if(OnHeadMove) return;

      if(lookTGT!=Vector3.zero && HeadPos)
      {
        Quaternion dir;
			  if(objTGT && objTGT.tag.Equals("Creature")) dir= Quaternion.LookRotation(objTGT.GetComponent<Rigidbody>().worldCenterOfMass-HeadPos.position);
			  else dir= Quaternion.LookRotation(lookTGT-HeadPos.position);
        headX = Mathf.MoveTowardsAngle(headX, (Mathf.DeltaAngle(dir.eulerAngles.y, transform.eulerAngles.y)/(180f-yaw_max))*yaw_max, t);
        headY = Mathf.MoveTowardsAngle(headY, (Mathf.DeltaAngle(dir.eulerAngles.x, transform.eulerAngles.x)/(90f-pitch_max))*pitch_max, t);
      }
      else
      {
        if(Mathf.RoundToInt(anm.GetFloat("Turn"))==Mathf.RoundToInt(transform.eulerAngles.y))
        {
          if(loop==0 && Mathf.RoundToInt(headX*100)==Mathf.RoundToInt(rndX*100) && Mathf.RoundToInt(headY*100)==Mathf.RoundToInt(rndY*100))
          {
            rndX=Random.Range(-yaw_max/2, yaw_max/2);
            rndY=Random.Range(-pitch_max/2, pitch_max/2);
          }
          headX= Mathf.LerpAngle(headX, rndX, t/10f);
          headY= Mathf.LerpAngle(headY, rndY, t/10f);
        } 
        else
        {
          headX= Mathf.LerpAngle(headX, spineX, t/10f);
          headY= Mathf.LerpAngle(headY, 0.0f, t/10f);
        }
      }
    }
    else
    {
      spineX = Mathf.MoveTowardsAngle(spineX, (Mathf.DeltaAngle(anm.GetFloat("Turn"), transform.eulerAngles.y)/360f)*maxX, t);
      if(IsOnGround && !IsInWater) { spineY= Mathf.LerpAngle(spineY, 0.0f, t/10f); roll=Mathf.LerpAngle(roll, 0.0f, t/10f); pitch=Mathf.Lerp(pitch, 0.0f, t/10f); }
      else { spineY = Mathf.LerpAngle(spineY, (Mathf.DeltaAngle(anm.GetFloat("Pitch")*90f, pitch)/180f)*maxY, t); roll=Mathf.LerpAngle(roll, -spineX, 0.1f); }
      headX= Mathf.LerpAngle(headX, spineX, t);
      headY= Mathf.LerpAngle(headY, spineY, t);
    }
  }
  #endregion
  #region Creature collisions and damages
  //Taking damage animation
  public void TakeDamageAnim(Quaternion JawAngle)
  {
    if(lastHit!=0)
		{
      if(!IsDead)
      {
			  if(CanWalk) crouch=Mathf.Lerp(crouch, (crouch_max*Size)/2, 1.0f);
        HeadPos.GetChild(0).transform.rotation*= JawAngle;
      }
      lastHit--;
		}
  }

  //Spawn blood particle
  void SpawnBlood(Vector3 position)
	{
		ParticleSystem particle=Instantiate(manager.blood, position, Quaternion.Euler(-90, 0, 0))as ParticleSystem; //spawn particle
		particle.transform.localScale=new Vector3(boxscale.z/10, boxscale.z/10, boxscale.z/10); //particle size
		Destroy(particle.gameObject, 1.0f); //destroy particle
	}

  //Collisions
  void OnCollisionExit() { objCOL=null; }
  public void ManageCollision(Collision col, float pitch_max, float crouch_max, AudioSource[] source, AudioClip pain, AudioClip Hit_jaw, AudioClip Hit_head, AudioClip Hit_tail)
	{
		//Collide with a Creature
	  if(col.transform.root.tag.Equals("Creature"))
		{
			Creature other=col.gameObject.GetComponent<Creature>(); objCOL=other.gameObject;

      //Is Player?
		  if(!UseAI && OnAttack)
		  {
			  objTGT=other.gameObject; other.objTGT=transform.gameObject;
			  behaviorCount=500; other.behaviorCount=500;
			  if(other.specie==specie) { behavior="Contest"; other.behavior="Contest"; }
			  else if(other.CanAttack) { behavior="Battle"; other.behavior="Battle"; }
			  else { behavior="Battle"; other.behavior="ToFlee"; }
		  }

			//Eat ?
      if(IsDead && lastHit==0 && other.IsConstrained)
      { 
        SpawnBlood(col.GetContact(0).point);
        body.AddForce(-col.GetContact(0).normal, ForceMode.Impulse);
        lastHit=25; return;
      }
      //Attack ?
			else if(lastHit==0 && other.OnAttack)
			{
				float baseDamages=(10f*other.DamageMultiplier) / ArmorMultiplier;

				if(col.collider.gameObject.name.Equals("jaw0")) //bite damage
				{
          SpawnBlood(col.GetContact(0).point);
          if(!IsInWater) body.AddForce(-col.GetContact(0).normal*other.boxscale.z*2, ForceMode.Impulse);
          lastHit=50; if(IsDead) return;
          source[0].pitch=Random.Range(1.0f, 1.5f); source[0].PlayOneShot(pain, 1.0f);
					source[1].PlayOneShot(Hit_jaw, Random.Range(0.1f, 0.4f));
					Health=Mathf.Clamp(Health-baseDamages, 0.0f, 100f);
				}
				else if(col.collider.gameObject.name.Equals("head")) //head damage
				{
          SpawnBlood(col.GetContact(0).point); 
					if(!IsInWater) body.AddForce(col.GetContact(0).normal*other.boxscale.z*4, ForceMode.Impulse);
          lastHit=50; if(IsDead) return;
          source[0].pitch=Random.Range(1.0f, 1.5f); source[0].PlayOneShot(pain, 1.0f);
					source[1].PlayOneShot(Hit_head, Random.Range(0.1f, 0.4f));
					if(!Herbivorous) Health=Mathf.Clamp(Health-baseDamages, 0.0f, 100f);
					else Health=Mathf.Clamp(Health-baseDamages/10, 0.0f, 100f);
				}
				else  if(!col.collider.gameObject.name.Equals("root")) //tail damage
				{
					SpawnBlood(col.GetContact(0).point);
          if(!IsInWater) body.AddForce(col.GetContact(0).normal*other.boxscale.z*4, ForceMode.Impulse);
          lastHit=50; if(IsDead) return;
          source[0].pitch=Random.Range(1.0f, 1.5f); source[0].PlayOneShot(pain, 1.0f);
          source[1].PlayOneShot(Hit_tail, Random.Range(0.1f, 0.4f));
					if(!Herbivorous) Health=Mathf.Clamp(Health-baseDamages, 0.0f, 100f);
					else Health-= Health=Mathf.Clamp(Health-baseDamages/10, 0.0f, 100f);
				 }
			}
      //Not the current target, avoid and look at
      if(objTGT!=other.gameObject && lookTGT==Vector3.zero) { lookTGT=objCOL.transform.position; avoidTime=45; angleAdd++; }
		}
		//Collide with world, avoid
		else if(col.gameObject!=objTGT && col.GetContact(0).point.y>transform.position.y+withersSize/2)
    { objCOL=col.gameObject; avoidTime=45; angleAdd++; }
	}

  #endregion
  #region Check for ground and water position
  public void GetGroundPos(IkType ikType, Transform RLeg1=null, Transform RLeg2=null, Transform RLeg3=null, Transform LLeg1=null, Transform LLeg2=null, Transform LLeg3=null,
                     Transform RArm1=null, Transform RArm2=null, Transform RArm3=null, Transform LArm1=null, Transform LArm2=null, Transform LArm3=null, float FeetOffset=0.0f)
	{
		Vector3 normal=Vector3.up, slope=Vector3.up; float terrainY=-transform.position.y, crouching=0, rx, rz;
    //USE RAYCAST
		if(manager.UseRaycast)
		{
			if(Physics.Raycast(transform.position+Vector3.up*withersSize, -Vector3.up, out RaycastHit hit, withersSize*2.0f, manager.walkableLayer))
			{ terrainY=hit.point.y; normal=hit.normal; slope=normal; }

      //IK ON
      if(ikType!=IkType.None && !IsDead && manager.UseIK)  {  manager.message=2; manager.UseIK=false; ikType=IkType.None; return; }
		}
    //TERRAIN ONLY
		else
		{
			float x = ((transform.position.x -manager.t.transform.position.x) / manager.t.terrainData.size.x ) * manager.tres;
			float y = ((transform.position.z - manager.t.transform.position.z) / manager.t.terrainData.size.z ) * manager.tres;
			normal=manager.t.terrainData.GetInterpolatedNormal(x/manager.tres, y/manager.tres);
      terrainY=manager.t.SampleHeight(transform.position)+manager.t.GetPosition().y;

      //IK ON
      if(ikType!=IkType.None && !IsDead && manager.UseIK) { manager.message=2; manager.UseIK=false; ikType=IkType.None; return; }
		}

    //Update status
		if((transform.position.y-Size)<=terrainY) IsOnGround=true; else IsOnGround=false; //On ground?
    if(transform.position.y<waterY && body.worldCenterOfMass.y>waterY) IsOnWater=true; else  IsOnWater=false; //On water ?
    if(body.worldCenterOfMass.y<waterY) IsInWater=true; else IsInWater=false; // In water ?

    if(IsDead | IsConstrained) body.maxDepenetrationVelocity=0.0f; else body.maxDepenetrationVelocity=5.0f;
    if(IsConstrained)
    {
      if(!IsOnGround) body.constraints=RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
      else body.constraints=RigidbodyConstraints.FreezeAll;
    }
    else body.constraints=RigidbodyConstraints.None;

    //Slip on slopes
    slope.y=Mathf.Clamp(slope.y, manager.MaxSlope, 1.0f);
    if(slope.y<=manager.MaxSlope && !IsInWater && !IsDead)
    body.AddForce(new Vector3(slope.x, -slope.y, slope.z)*(1.0f-slope.y)*100, ForceMode.Acceleration);

    if(IsOnGround && !IsInWater)
    {
      if(IsDead)
      {
        transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.Cross(transform.right, slope), slope), 0.1f);
        posY=terrainY;
      }
      else
      {
        if(ikType<=IkType.Convex)
        { 
          Quaternion n=Quaternion.LookRotation(Vector3.Cross(transform.right, slope), slope);
          transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.Cross(transform.right, normal), normal), 0.1f);
        }
        else
        {
          Quaternion n=Quaternion.LookRotation(Vector3.Cross(transform.right, normal), normal);
          rx = Mathf.Clamp(Mathf.DeltaAngle(n.eulerAngles.x, 0.0f), -30f, 30f);  rz = Mathf.Clamp(Mathf.DeltaAngle(n.eulerAngles.z, 0.0f), -10f, 10f);
          transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(-rx, transform.eulerAngles.y, -rz), objCOL?0.1f:ang_t);
        }
         posY=terrainY-crouching;
      }
    
    }
    else if(IsInWater | IsOnWater)
    {
      transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,transform.eulerAngles.y,0), ang_t);
      posY=waterY-body.centerOfMass.y;
    }
    else
    {
      if(slope.y<=manager.MaxSlope)
      { 
        body.maxDepenetrationVelocity=1000.0f;
        body.constraints=RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        avoidTime=45; angleAdd++;
      }
      transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.eulerAngles.y, 0), objCOL?0.1f:ang_t);
      posY=transform.position.y;
    }

    if(!IsVisible | !manager.UseIK) return;
	}

  //AI CORE
  public void AICore(int idle1=0, int idle2=0, int idle3=0, int idle4=0, int eat=0, int drink=0, int sleep=0)
	{ manager.message=2; UseAI=false; return; }

  #endregion

}


