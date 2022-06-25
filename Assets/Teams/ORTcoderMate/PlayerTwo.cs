using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.ORTcoderMate
{
    public class PlayerTwo : TeamPlayer
    {
        public bool IsNearBall()
        {
            return Vector3.Distance(GetBallPosition(), GetPosition()) < 8.0f;
        }

        public bool IsBallNearOwnGoal()
        {
            return Vector3.Distance(GetMyGoalPosition(), GetBallPosition()) < 6.0f;
        }

        public bool IsLossing()
        {
            return GetMyScore() > GetRivalScore();
        }

        public override void OnUpdate()
        {
            if (IsNearBall() || Vector3.Distance(GetBallPosition(), GetMyGoalPosition()) < 12.0f)
            {
                GoTo(GetBallPosition());
            }
            else if (IsLossing())
            {
                GoTo(FieldPosition.E1);
            }
            else
            {
                GoTo(FieldPosition.C2);
            }
        }

        public override void OnReachBall()
        {
            if (IsBallNearOwnGoal())
            {
                ShootBall(GetDirectionTo(GetTeamMatesInformation()[0].Position), ShootForce.Medium);
            }
            else if (Random.Range(0, 10) > 5)
            {
                ShootBall(GetDirectionTo(GetTeamMatesInformation()[1].Position), ShootForce.High);
            }
            else
            {
                ShootBall(GetDirectionTo(GetRivalGoalPosition()), ShootForce.High);
            }
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {

        }

        public override FieldPosition GetInitialPosition() => FieldPosition.C2;

        public override string GetPlayerDisplayName() => "Player 2";
    }
}