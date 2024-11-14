using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Jelly : MonoBehaviour
{
    public GUI gui;
    [SerializeField] public ParticleSystem inGodModParticles;
    [SerializeField]
    private ParticleSystem deathParticles;
    private ParticleSystem upgradeParticles;
    [SerializeField] private AudioClip[] sounds;
    [SerializeField]
    public Edge[] edges;

    public System.DateTime freeSpinLastTime;
    
    [SerializeField]
    private Color[] edgeColors;

    private GameObject foodScoreAnimator;
    private GameObject foodObject;
    private AudioSource audio;
    private ScoreStreak scoreStreak;
    public Color[] EdgeColors {
        get {
            return edgeColors;
        }
    }

    [NonSerialized]
    public int lastUpgradedEdge = 5;
    /////////// for debug
    public bool resetSave;
    //////////
    public int upgradedEdgeScore;
    public Mesh[] skins;
    private int activeSkin;
    public int ActiveSkin { 
        get { 
            return activeSkin; 
        }
        set
        {
            activeSkin = value;
        }
    }
    private Queue<int> coinsGot = new Queue<int>();
    private bool isAnimatingCoins = false;
    
    #region PlayerData

    public int Score {
        get {
            return score;
        }

        set {
            score = value;
            gui.setScoreCounterText(score.ToString());
            if (Score > highScore)
            {
                //show text new high score!
                gui.showNewHighScore();
            }
        }
    }
    

    public int Level {
        get {
            return level;
        }
    }

    public int HighScore {
        get {
            return highScore;
        }
    }

    public int Food {
        get {
            return food;
        }
    }

    public bool[] UnlockedSkins {
        get {
            return unlockedSkins;
        }
    }

    public int Coins {
        get {
            return coins;
        }
    }

    public int upgradePrice { get; set; }

    private int score;
    private int level;
    private int highScore;
    private int food;
    private int totalFood;
    private bool[] unlockedSkins;
    private int coins;
    public bool firstVisit;

    #endregion

    private Vector3 startPosition;

    private int cashedScore;
    private int cashedFood;

    private Tile currentTile;

    private void Awake() {
        upgradeParticles = GetComponent<ParticleSystem>();
        if (upgradeParticles == null) {
            throw new Exception("Particle System for upgrade animation must be attached to jelly GameObject");
        }
        if (resetSave) {
            firstVisit = true;
            ///////////////debug part
            coins = 0;
            level = 1;
            //////////////
            unlockedSkins = new bool[skins.Length];
            unlockedSkins[0] = true;
            activeSkin = 0;
            for (int i = 1; i < unlockedSkins.Length; ++i) {
                unlockedSkins[i] = false;
            }
            for (int i = 0; i < edges.Length; ++i) {
                edges[i].setScoresPerJump(level);
            }
            upgradePrice = 5;
            SaveSystem.Save(this);
        }
        startPosition = transform.localPosition;
        LoadData();
        gui.highScoreText.text = "HIGH SCORE: " + HighScore.ToString();
    }

    private void Update()
    {
        inGodModParticles.transform.position =new Vector3(transform.position.x, transform.position.y, transform.position.z); ;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreStreak = GetComponent<ScoreStreak>();
        audio = GetComponent<AudioSource>();
        foodObject = GameObject.FindWithTag("Food");
        gui.setCoinCounterText(coins+"");
        foodScoreAnimator = GameObject.Find("FoodScoreAnimator");
        //GameObject.Find("LevelCounterBackground").GetComponent<Image>().enabled = false;
    }

    public void addSkin(int number) {
        GetComponent<MeshFilter>().mesh = skins[number];
        activeSkin = number;
        unlockedSkins[number] = true;
    }

    public void SetSkin(int number) {
        if (number < unlockedSkins.Length && unlockedSkins[number]) {
            GetComponent<MeshFilter>().mesh = skins[number];
            activeSkin = number;
        }
    }

    #region CoinsAnim
    public void achieveCoins(int value) {
        coinsGot.Enqueue(value);
        StartCoroutine("animateCoinsGet");
    }

    public void spendCoins(int value) {
        coins -= value;
        gui.setCoinCounterText(coins.ToString());
    }

    private bool coinAnimationStreamIsEmpty() {
        return !isAnimatingCoins;
    }
    private IEnumerator animateCoinsGet() {
        yield return new WaitUntil(coinAnimationStreamIsEmpty);
        isAnimatingCoins = true;
        int c = coinsGot.Dequeue();
        gui.setCoinsGetAnimator("+" + c);
        yield return new WaitForSeconds(0.5f * GameField.getTimeScale() / GameField.MIN_TIMESCALE);
        int delta = Math.Max(c / 100, 1);
        while (c > delta) {
            c -= delta;
            coins += delta;
            gui.setCoinsGetAnimator("+" + c);
            gui.setCoinCounterText(coins.ToString());
            yield return new WaitForSeconds(0.000001f * GameField.getTimeScale() / GameField.MIN_TIMESCALE);
        }
        coins += c;
        gui.setCoinCounterText(coins.ToString());
        yield return new WaitForSeconds(0.000001f * GameField.getTimeScale() / GameField.MIN_TIMESCALE);
        //while (c > 0) {
        //    c--;
        //    coins++;
        //    gui.setCoinsGetAnimator("+" + c);
        //    gui.setCoinCounterText(coins.ToString());
        //    yield return new WaitForSeconds(0.000001f * GameField.getTimeScale() / GameField.MIN_TIMESCALE);
        //}
        gui.setCoinsGetAnimator("");
        isAnimatingCoins = false;
        SaveSystem.Save(this);
    }
    #endregion

    public void startGame() {
        // activate swipe detector
        gui.swipeDetector.SetActive(true);
        RendererEnabled(true);
        Vibration.Init();
        score = 0;
        food = 0;
        totalFood = Math.Min(level / 3 + 1, 8);
        foodObject.GetComponent<ColorChanger>().resetColor();
        gui.SetFoodProgressBar((float)food / totalFood);
        gui.SetLevelCounterText(level);

        currentTile = null;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) {
            rb.useGravity = true;
        } 
        GameField.setTimeScale(Math.Min(GameField.MIN_TIMESCALE + level*0.02f, 3));
        Time.timeScale = GameField.getTimeScale();
    }

    public void ContinueGame() {
        gui.swipeDetector.SetActive(true);
        RendererEnabled(true);

        currentTile = null;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) {
            rb.useGravity = true;
        }
        Time.timeScale = GameField.getTimeScale();

        score = cashedScore;
        food = cashedFood;
    }

    public void EndGame(bool win) {
        gui.foodProgressPanel.SetActive(false);
        gui.coinCounterLabel.SetActive(true);
        if (scoreStreak.inGodMode)
        {
            scoreStreak.endGodMode();
        }
        // deactivate swipe detection
        gui.swipeDetector.SetActive(false);
        gui.showDarkBackGround();
        Time.timeScale = GameField.MIN_TIMESCALE;
        gui.stopAnimatingNewHighScore();
        if (win) {
            // if level is completed
            audio.clip = sounds[0];
            audio.Play();
            gui.levelPassed();
            level++;
            achieveCoins((int)Math.Round(score / 5.0f));
            for (int i = 0; i < edges.Length; i++)
            {
                upgrade();
            }
        } else {
            if (VibroButton.vibroEnabled)
            {
                long[] pattern = { 0, 60, 70, 80 };
                Vibration.Vibrate(pattern, -1);
            }
            gui.die();
            audio.clip = sounds[2];
            audio.Play();
            deathParticles.transform.position = transform.position + new Vector3(0, 1f, 0);
            deathParticles.Play();
            //if dead  
        }
        GetComponent<Rigidbody>().isKinematic = true;
        if (score > highScore) {
            highScore = score;
            gui.highScoreText.text = "HIGH SCORE: " + highScore;
            //GameObject.Find("HighScore").GetComponent<MoveAnimation>().PlayIn();
        }
        GetComponent<ScoreStreak>().ResetStreak();
        SaveSystem.Save(this);
        cashedScore = score;
        cashedFood = food;
        Reset();
        RendererEnabled(false);
    }

    public void OnTriggerEnter(Collider other) {
        switch(other.gameObject.tag) {
            case "Death":
                EndGame(false);
                break;
            case "Food":
                gui.GetComponent<GUIShaker>().play();
                gui.SetFoodProgressBar((float)++food / totalFood);
                Score += food * upgradedEdgeScore * 2;
                foodScoreAnimator.transform.position = new Vector3(this.transform.position.x + 3, this.transform.position.y + 12, this.transform.position.z);
                foodScoreAnimator.GetComponent<TripleTextAnimationAnimator>().play(
                    "+" + food * upgradedEdgeScore * 2,
                    foodObject.GetComponent<MeshRenderer>().material.color
                    );
                if (food == totalFood) {
                    EndGame(true);
                }
                break;
            case "Coin":
                coins++;
                gui.setCoinCounterText(coins.ToString());
                break;
            default:

                break;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        
        Mover mover = GetComponent<Mover>();
        if (collision.gameObject.tag.Equals("Tile") && mover != null && !mover.IsBouncing) {
            currentTile = collision.gameObject.GetComponent<Tile>();
            Vector3 position = new Vector3(
                collision.gameObject.transform.position.x,
                gameObject.transform.position.y,
                collision.gameObject.transform.position.z
                );
            gameObject.transform.position = position;
            //adjust rotation for perfect values
            Vector3 angles = transform.rotation.eulerAngles;
            angles.z = Mathf.Round(angles.z / 90) * 90f;
            angles.x = Mathf.Round(angles.x / 90) * 90f;
            angles.y = Mathf.Round(angles.y / 90) * 90f;
            Quaternion rotation = transform.rotation;
            rotation.eulerAngles = angles;
            transform.rotation = rotation;

            //translate to radians
            angles *= (float)(Math.PI / 180);

            Vector3 bounceVector = new Vector3(
                    (float)Math.Abs(Math.Cos(angles.x) * Math.Sin(angles.z)),
                    (float)Math.Abs(Math.Cos(angles.z) * Math.Cos(angles.x)),
                    (float)Math.Abs(Math.Sin(angles.x))
                );

            if (mover != null) {
                mover.Move();
            }
            if (currentTile != null) {
                currentTile.Notify(gameObject);
                if (currentTile != null && !(currentTile is StartTile) && VibroButton.vibroEnabled)
                {
                    if (scoreStreak.scoreStreak == 0)
                    {
                        audio.clip = sounds[1];
                        audio.Play();
                    }
                    long[] pattern = { 0, 35 };
                    Vibration.Vibrate(pattern, -1);
                }
            }
        }
        if (GetComponent<ScoreStreak>().inGodMode || true)
        {
            Vector3 rot;
            Vector3 dir = mover.Direction;
            if (dir.x > 0) {
                rot = new Vector3(0,270,0);
            } else if (dir.x < 0) {
                rot = new Vector3(0,90,0);
            } else if (dir.z < 0) {
                rot = new Vector3(0, 0, 0);
            } else if (dir.z > 0) {
                rot = new Vector3(0,180,0);
            } else {
                rot = inGodModParticles.transform.eulerAngles;
            }
            StartCoroutine(RotateToRotation(inGodModParticles.gameObject, rot));
        }
    }

    //          for bounce
    //public void OnBouncingFinished() {
    //    Score++;

    //    if (currentTile != null)
    //    {
    //        currentTile.Notify(gameObject);
    //    }
    //}

    public void Reset() {
        score = 0;
        food = 0;

        currentTile = null;
        
        transform.localPosition = startPosition;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) {
            rb.isKinematic = true;
            rb.isKinematic = false;
            rb.rotation = Quaternion.identity;
            rb.useGravity = false;
        }
        RendererEnabled(true);
        Mover mover = GetComponent<Mover>();
        if (mover != null) {
            mover.Reset();
        }
    }

    public void interactThornTile() {
        if (VibroButton.vibroEnabled)
        {
            long[] pattern = { 0, 60, 70, 80 };
            Vibration.Vibrate(pattern, -1);
        }

        if (!GetComponent<ScoreStreak>().inGodMode)
        {
            EndGame(false);
        }
        else
        {
            GetComponent<ScoreStreak>().endGodMode();
        }
        
    }

    public void interactSpringTile() {
        Rigidbody rb = GetComponent<Rigidbody>();
        Mover mover = GetComponent<Mover>();
        if (rb != null && mover != null) {
            // max heoght of spring jump = 1.5 * height of standart jump
            float maxJumpHeightKoef = 1.5f;
            rb.AddForce(
                new Vector3(mover.Direction.x * (2f / maxJumpHeightKoef - 1), 
                            mover.Direction.y * (maxJumpHeightKoef - 1), 
                            mover.Direction.z * (2f / maxJumpHeightKoef - 1)), 
                ForceMode.VelocityChange
                );
            Vector3 torque = new Vector3(mover.Direction.z, 0, -mover.Direction.x);
            torque = torque.normalized;
            rb.AddTorque(torque * (float)Math.PI / (mover.durationOfJump * 2) * (2f / maxJumpHeightKoef - 1),
                ForceMode.VelocityChange);
        }
    }

    public void interactStartTile() {
        
    }

    public void upgrade()
    {
        // UpgradeAnimation upAnim = GetComponent<UpgradeAnimation>();
        // if (upAnim !=  null) {
        //     upAnim.Animate();
        // }
        lastUpgradedEdge += 1;
        if (lastUpgradedEdge == 5)
        {
            upgradePrice *= Edge.deltaScore * 2;
        }
        lastUpgradedEdge %= edges.Length;
        edges[lastUpgradedEdge].upgrade();
        //upgradeParticles.GetComponent<ParticleSystemRenderer>().material = edges[lastUpgradedEdge].GetComponent<MeshRenderer>().material;
        //upgradeParticles.Play();
        SaveSystem.Save(this);
    }

    /// <summary>
    /// hides/shows Jelly and all edges
    /// </summary>
    public void RendererEnabled(bool enabled) {
        List<Renderer> renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
        renderers.Add(GetComponent<Renderer>());
        foreach (Renderer renderer in renderers) {
            renderer.enabled = enabled;
        }
    }

    private void LoadData() {
        try {
            PlayerData data = SaveSystem.Load();
            freeSpinLastTime = data.freeSpinLastTime;
            level = 540;
            highScore = data.highScore;
            coins = data.coins;
            activeSkin = data.activeSkin;
            GetComponent<MeshFilter>().mesh = skins[activeSkin];
            upgradePrice = data.upgradePrice;
            unlockedSkins = data.unlockedSkins;
            lastUpgradedEdge = data.lastUpgradedEdge;
            firstVisit = data.firstVisit;
            if (data.upgradedEdgeScore == 0)
            {
                upgradedEdgeScore = 1;
            } else {
                upgradedEdgeScore = data.upgradedEdgeScore;
            }
            for (int i = 0; i <= lastUpgradedEdge; ++i) {
                edges[i].setScoresPerJump(540);
            }
            Debug.Log(lastUpgradedEdge);
            for (int i = lastUpgradedEdge + 1; i < edges.Length; ++i) {
                edges[i].setScoresPerJump(upgradedEdgeScore / Edge.deltaScore);
            }
            firstVisit = data.firstVisit;
        } catch (Exception) {
            firstVisit = true;
            level = 1;
            highScore = 0;
            coins = 0;
            activeSkin = 0;
            GetComponent<MeshFilter>().mesh = skins[activeSkin];
            unlockedSkins = new bool[skins.Length];
            unlockedSkins[0] = true;
            firstVisit = true;
            for (int i = 1; i < unlockedSkins.Length; ++i) {
                unlockedSkins[i] = false;
            }
            for (int i = 0; i < edges.Length; ++i) {
                edges[i].setScoresPerJump(1); 
            }
            lastUpgradedEdge = 5;
            upgradePrice = 5;
            freeSpinLastTime = DateTime.MinValue;
        }
    }

    private IEnumerator RotateToRotation(GameObject obj, Vector3 angles) {
        float time = 0.7f;
        float timeStep = 0.1f;
        Vector3 startAngles = obj.transform.eulerAngles;
        if (NearlyEquals(angles.y, 0) && NearlyEquals(startAngles.y, 270)) {
            angles.y = 360;
        } else if (NearlyEquals(angles.y, 270) && NearlyEquals(startAngles.y, 0)) {
            startAngles.y = 360;
            obj.transform.eulerAngles = startAngles;
        }
        Vector3 step = (angles - startAngles) / (time / timeStep);
        while (time > 0) {
            time -= timeStep;
            startAngles += step;
            obj.transform.eulerAngles = startAngles;
            yield return new WaitForSeconds(timeStep);
        }
        obj.transform.eulerAngles = angles;
    }

    private static bool NearlyEquals(float a, float b) {
        return Math.Abs(a - b) < 0.0001;
    }
}
