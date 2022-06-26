using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.ORTcoderGerman
{
    public class PlayerThree : TeamPlayer
    {
        bool iniciado = false;
        int estado = 0;
        public override void OnUpdate()
        {
            if (estado == 0)
            {
                var ballPosition = GetBallPosition();
                var directionToBall = GetDirectionTo(ballPosition);
                if (iniciado == false)
                {
                    MoveBy(directionToBall);
                }
                else
                {
                    if (Vector3.Distance(ballPosition, GetRivalGoalPosition()) < 5)
                    {
                        GoTo(FieldPosition.B2);
                    }
                    else
                    {
                        MoveBy(directionToBall);
                    }
                }
            }
        }

        public override void OnReachBall()
        {
            var ballPosition = GetBallPosition();
            var CentroPosition = GetDirectionTo(GetTeamMatesInformation()[0].Position);
            var DefenesaPosition = GetDirectionTo(GetTeamMatesInformation()[1].Position);
            var directionToCentro = GetDirectionTo(CentroPosition);
            var directionToDefenesa = GetDirectionTo(DefenesaPosition);
            if (iniciado == false)
            {
                ShootBall(directionToCentro, ShootForce.Medium);
                iniciado = true;
            }
            else
            {
                if (Vector3.Distance(ballPosition, GetMyGoalPosition()) <10)
                {
                    ShootBall(directionToDefenesa, ShootForce.Low);
                }
                else
                {
                    ShootBall(directionToCentro, ShootForce.High);
                }
            }
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {
            iniciado = false;
        }

        public override FieldPosition GetInitialPosition() => FieldPosition.C2;

        public override string GetPlayerDisplayName() => "Pases";
    }
}