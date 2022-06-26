using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.ORTcoderGerman
{
    public class PlayerOne : TeamPlayer
    {
        float esperaDif = 118;
        int estado = 0;
        public override void OnUpdate()
        {
            if (estado == 0)
            {
                float espera = esperaDif - GetTimeLeft();
                if (GetTimeLeft() < espera)
                {
                    var ballPosition = GetBallPosition();
                    var directionToBall = GetDirectionTo(ballPosition);
                    MoveBy(directionToBall);
                }
                else
                {
                    Stop();
                }
            }
        }

        public override void OnReachBall()
        {
            ShootBall(GetDirectionTo(GetRivalGoalPosition()), ShootForce.High);
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {
            esperaDif = GetTimeLeft() * 2;
            esperaDif = esperaDif - 1.5f;
        }

        public override FieldPosition GetInitialPosition() => FieldPosition.C1;

        public override string GetPlayerDisplayName() => "Atacante";
    }
}