using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.ORTcoderMate
{
    public class PlayerOne : TeamPlayer
    {
        public bool IsBallNearOwnGoal()
        {
            return Vector3.Distance(GetMyGoalPosition(), GetBallPosition()) < 4.0f;
        }

        public Vector3 GetPos()
        {
            var ballPosition = GetBallPosition();
            Vector3 origin = new Vector3(-10.0f, 0.0f, 0.0f);
            var radius = 3;

            Vector3 originToBall = ballPosition - origin;
            Vector3 originToBallNorm = originToBall.normalized;
            Vector3 originToPos = radius * originToBallNorm;
            Vector3 posFinal = origin + originToPos;

            return posFinal;
        }

        public override void OnUpdate()
        {
            if (IsBallNearOwnGoal())
            {
                GoTo(GetBallPosition());
            }
            else
            {
                MoveBy(GetDirectionTo(GetPos()));
            }
        }

        public override void OnReachBall()
        {
            
            ShootBall(GetDirectionTo(GetTeamMatesInformation()[1].Position), ShootForce.High);
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {

        }

        public override FieldPosition GetInitialPosition() => FieldPosition.A2;

        public override string GetPlayerDisplayName() => "Player 1";
    }
}