using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Edge : MonoBehaviour
{
    public static readonly int deltaScore = 1;

    private int _scoresPerJump;
    private ScoreStreak scoreStreak;

    private void Start() {
        scoreStreak = GetComponentInParent<ScoreStreak>();
    }

    public int getScoresPerJump()
   {
      return _scoresPerJump;
   }

    public void setScoresPerJump(int score) {
        _scoresPerJump = score;
        Jelly jelly = GetComponentInParent<Jelly>();
        if (jelly != null) {
            Color[] clrs = jelly.EdgeColors;
            MeshRenderer mr = GetComponent<MeshRenderer>();
            if (mr != null) {
                mr.material.color = clrs[(_scoresPerJump - 1) % clrs.Length];
            }
        }
    }

   public void upgrade()
   {
        _scoresPerJump += deltaScore;
        ////// color change ruquired ///////
        Jelly jelly = GetComponentInParent<Jelly>();
        if (jelly != null) {
            jelly.upgradedEdgeScore = _scoresPerJump;
            setScoresPerJump(_scoresPerJump);
        }
   }
      
   public void OnTriggerEnter(Collider other)
   {
       
      if (other.tag.Equals("Tile") && other.GetComponent<StartTile>() == null)
      {
            gameObject.GetComponentInParent<Jelly>().Score += _scoresPerJump;
            GameObject.Find("ScoreAnimator").transform.position = new Vector3(this.transform.position.x,this.transform.position.y + 12, this.transform.position.z );
            GameObject.Find("ScoreAnimator").GetComponent<TripleTextAnimationAnimator>().play(
                "+" + _scoresPerJump,
                GetComponent<MeshRenderer>().material.color
                );
            StartCoroutine(nameof(StreakCheck));
      }
   }

    private IEnumerator StreakCheck() {
        yield return new WaitForSeconds(0.001f);
        if (scoreStreak.Streak > 0) {
            GetComponentInParent<Jelly>().Score += _scoresPerJump * (scoreStreak.Streak - 1);
            GameObject.Find("StreakMultiplier").transform.position = new Vector3(this.transform.position.x + 4f, this.transform.position.y + 11, this.transform.position.z);
            GameObject.Find("StreakMultiplier").GetComponent<TripleTextAnimationAnimator>().play(
                "x" + scoreStreak.Streak,
                scoreStreak.GetPhraseColor(scoreStreak.Streak)
                );
        }
    }
}
