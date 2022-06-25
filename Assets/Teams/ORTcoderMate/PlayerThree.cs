using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.ORTcoderMate
{
    public class PlayerThree : TeamPlayer
    {

        public override void OnUpdate()
        {
            if (Vector3.Distance(GetBallPosition(), GetMyGoalPosition()) > 12.0f)
            {
                GoTo(GetBallPosition());
            }
            else {
                GoTo(FieldPosition.E3);
            }
        }

        public override void OnReachBall()
        {
            if (Random.Range(0, 10) > 5)
            {
                ShootBall(GetDirectionTo(GetRivalGoalPosition()), ShootForce.High);
            }
            else
            {
                ShootBall(GetDirectionTo(GetRivalGoalPosition()), ShootForce.Medium);
            }
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {

        }

        public override FieldPosition GetInitialPosition() => FieldPosition.C3;

        public override string GetPlayerDisplayName() => "Player 3";
    }
}