using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.GermanTeam
{
    public class PlayerOne : TeamPlayer
    {
        public override void OnUpdate()
        {
            float espera = 118 - GetTimeLeft();
            if (GetTimeLeft() < espera)
            {
                var ballPosition = GetBallPosition();
                var directionToBall = GetDirectionTo(ballPosition);
                MoveBy(directionToBall);
            }
        }

        public override void OnReachBall()
        {
            ShootBall(GetDirectionTo(GetRivalGoalPosition()), ShootForce.High);
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {

        }

        public override FieldPosition GetInitialPosition() => FieldPosition.C1;

        public override string GetPlayerDisplayName() => "Centro";
    }
}