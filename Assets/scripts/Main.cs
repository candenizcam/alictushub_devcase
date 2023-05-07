using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

/** Main is where the main game logic lives
 * interactions between game elements are handled in a centralized way
 */
public class Main : MonoBehaviour
{
    public CanvasScript mainCanvas;
    public MainCharacterScript mainCharacterScript;
    public Camera mainCamera;
    public CoinScript coin;
    public BruteScript brute;
    public BruteProjectileScript bruteProjectile;
    public BoomerangScript boomerangScript;
    public GameObject plane;
    private List<BruteScript> activeBruteScripts = new();
    private List<BruteProjectileScript> projectileList = new ();
    private List<CoinScript> coinList = new();
    
    private bool _death = false; // the state of game, death true is menu and death false is game
    
    // these are variables for design adjustments, in final version they may be private or non serializable
    public float characterSpeed = 1f;
    public float bruteSpeed = 1f;
    public int activeBrutes = 3;
    public int startCoins = 50;
    public float attackDistance = 2f;
    public float closeContactDistance = .5f;
    public float boomerangFirePeriod = 2f;
    public float bruteProjectileSpeed = 1f;
    public float boomerangSpeed = 2f;
    public int characterDamageLimit = 10;
    private int coins;
    private int kills;
    private int characterHp;
    private float _coinDistance = 100f;

    
    void Start()
    {
        FirstGame();
        mainCanvas.RestartAction = () =>
        {
            ResetGame();
        };
        Application.targetFrameRate = 60;
    }

    /** called on instantiation, instantiates brutes and coins
     */
    private void FirstGame() 
    {
        for (int i = 0; i < activeBrutes; i++) // spawn first brutes
        { 
            var b = GameObject.Instantiate(brute);
            activeBruteScripts.Add(b);
            b.ThrowAction = (activeBrute) =>
            {
                FireBruteBullet(activeBrute);
            };
            
        }
        
        for (int i = 0; i < startCoins; i++)
        {
            var c = GameObject.Instantiate(coin);
            c.SpawnRadius = _coinDistance;
            coinList.Add(c);
        }
        mainCharacterScript.SetAttackRadius(attackDistance); 
        
        ResetGame();
    }

    /** Resets game after death for restart
     */
    private void ResetGame()
    {
        var mainPos = mainCharacterScript.transform.position;
        foreach (var coinScript in coinList) // resets coins
        {
            coinScript.SetNewPosition(mainPos);
        }
        
        foreach (var activeBruteScript in activeBruteScripts)
        {
            activeBruteScript.SpawnAround(mainPos,30f);
        }
        
        
        characterHp = characterDamageLimit;
        mainCharacterScript.SetHealth(1f);
        _death = false;
    }
    

    void MoveCharacter(Vector2 direction)
    {
        var movementVector = direction.normalized * (Time.deltaTime * characterSpeed);
        mainCharacterScript.MoveCharacter(movementVector);
        mainCamera.transform.position = mainCharacterScript.transform.position + new Vector3(0f, 30f, -30f);
    }
    
    
    void Update()
    {
        FloorMovement();
        
        if (_death)
        {
            var t = mainCharacterScript.NormalDyingTime();
            mainCanvas.bg.color = new Color(.6f, 0f, 0f, t*.8f);
        }
        else
        {
            var characterIsMoving = mainCanvas.floatingJoystick.Direction.magnitude > 1e-3;
            mainCharacterScript.IsMoving(characterIsMoving);
            if (characterIsMoving) 
            {
                MoveCharacter(mainCanvas.floatingJoystick.Direction);
            }
            Brutes();
            Projectiles();
            Coins();

            mainCanvas.UpdateText($"{kills}",$"{coins}");

            if (characterHp < 1)
            {
                _death = true;
                mainCharacterScript.StartDeath();
            }
        }
        
        
        
    }

    private void FloorMovement()
    {
        // keeps floor around the character
        mainCanvas.GameOverEnabled(_death);
        var mainPos = mainCharacterScript.transform.position;
        var planeX = (int) (mainPos.x / 20)*20;
        var planeZ = (int) (mainPos.z / (23.094f)) * (23.094f);
        plane.transform.position = new Vector3(planeX, 0f, planeZ);
    }

    /** Functinons relating to brutes
     */
    private void Brutes()
    {
        var mainCharacterPosition = mainCharacterScript.transform.position;
        foreach (var activeBruteScript in activeBruteScripts)
        {
            if (!activeBruteScript.active) // if this brute is dead, it is respawned
            {
                activeBruteScript.SpawnAround(mainCharacterPosition,30f);
                continue;
            }

            if (activeBruteScript.dying) // if it is dying, we move on
            {
                continue;
            }
            var activePosition = activeBruteScript.transform.position;
            var boomerangDistance =
                (boomerangScript.transform.position - activePosition).magnitude;

            if (boomerangDistance < closeContactDistance) // if it is hit, it dies
            {
                kills += 1;
                activeBruteScript.Kill();
                continue;
            }
            
            
            var distance = (activePosition - mainCharacterPosition).magnitude;
        
            if (distance > attackDistance*1.2f) // if it is not in shooting distance, it runs
            {
                activeBruteScript.HandleMovement(mainCharacterPosition,bruteSpeed*Time.deltaTime);
                activeBruteScript.SetState(0);
            }else if (distance > closeContactDistance) { // if close, it shoots
                if (distance < attackDistance)
                {
                    FireBoomerang(activeBruteScript);
                }
                activeBruteScript.SetState(1);
            }
            else // if very close, it kills
            {
                characterHp = 0;
                mainCharacterScript.SetHealth(0f);
                // die
            }
        }
    }

    private void FireBoomerang(BruteScript activeBruteScript)
    {
        if (mainCharacterScript.boomerangTimer < 0f && !boomerangScript.active) // if available and not on cooldown it fires
        {
            boomerangScript.SetFire(activeBruteScript,boomerangSpeed,mainCharacterScript);
            mainCharacterScript.boomerangTimer = boomerangFirePeriod;
        }
        else
        {
            mainCharacterScript.boomerangTimer -= Time.deltaTime; // timer 
        }
    }
    
    private void FireBruteBullet(BruteScript activeBruteScript)
    {
        var activePosition = activeBruteScript.transform.position;
        var fireDirection = (mainCharacterScript.transform.position - activePosition).normalized*bruteProjectileSpeed;
        // can recycle old bullets for efficiency
        var anyOldBullet = false;
        for (var i = 0; i < projectileList.Count; i++)
        {
            if (projectileList[i].inactive)
            {
                projectileList[i].SetFire(activePosition,fireDirection); 
                anyOldBullet = true;
                break;
            }
        }
        if (!anyOldBullet)
        {
            var bp = GameObject.Instantiate(bruteProjectile);
            bp.SetFire(activePosition,fireDirection);
            projectileList.Add(bp);
        }
    }
    

    private void Projectiles()
    {
        // bullet logic
        foreach (var bruteProjectileScript in projectileList)
        {
            var distance = (bruteProjectileScript.transform.position - mainCharacterScript.transform.position).magnitude;

            if (distance > 30f)
            { // remove
                bruteProjectileScript.inactive = true;
                bruteProjectileScript.transform.Translate(0f,-10f,0f);
            }
            else if (distance<closeContactDistance)
            {
                // damage
                characterHp -= 1;
                mainCharacterScript.SetHealth((float)characterHp/characterDamageLimit);
                bruteProjectileScript.inactive = true;
                bruteProjectileScript.transform.Translate(0f,-10f,0f);
                
            }
        }
    }

    private void Coins()
    {
        // new coins do not come based on time, instead their total number stays the same
        var mainPos = mainCharacterScript.transform.position;
        foreach (var coinScript in coinList)
        {
            var dist = (coinScript.transform.position - mainPos).magnitude;
            if (dist < closeContactDistance)
            { // collected coin is collected
                coinScript.ResetCoin(mainPos);
                coins += 1;
            }else if (dist > _coinDistance)
            { // too far coin is reset to the middle
                coinScript.ResetCoin(mainPos);
            }
        }
    }

}
