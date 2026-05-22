using UnityEngine; 
// https://docs.unity3d.com/ScriptReference/Vector3.MoveTowards.html 
public class SimpleElevator : MonoBehaviour { 
    [Header("References")] 
    public Transform platform;
     public Transform pos0; 
     public Transform pos1; 
     public Transform pos2; 
     public Transform pos3; 
     [Header("Parameters")] 
     public float speed = 2f; 
     private Transform destiny; 
     private bool isMoving= true; 
     // Start is called once before the first execution of Update after the MonoBehaviour is created 
void Start() { } 
// Update is called once per frame 
void Update() { 
    if (!isMoving || destiny == null) return; 
    platform.position = Vector3.MoveTowards(platform.position, destiny.position, speed*Time.deltaTime); //Vector3.MoveTowards(platform.position, destiny.position, speed * Time.deltaTime); 
    if (Vector3.Distance(platform.position, destiny.position) < 0.001f) {
         platform.position = destiny.position; 
         // isMoving = false; 
         } } 
public void MoveToLevel(Transform level) { 
    destiny = level; //isMoving = true; 
    } 
public void ReturnToBase() { 
    MoveToLevel(pos0); 
    } 
[ContextMenu("Go To Level A")] 
public void Test_LevelA() { 
    MoveToLevel(pos1); 
       } 
   [ContextMenu("Go To Level B")]
public void Test_LevelB() { 
    MoveToLevel(pos2); }
    [ContextMenu("Go To Level C")]
     public void Test_LevelC() { 
        MoveToLevel(pos3); } 
        [ContextMenu("Return To Base")] 
        public void Test_Return() { ReturnToBase(); }
         }