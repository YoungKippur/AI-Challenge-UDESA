using Core.Games;
using Core.Player;
using Core.Utils;
using UnityEngine;

namespace Teams.ORTcoderGerman
{
    public class PlayerTwo : TeamPlayer
    {
        int estado = 0;
        public override void OnUpdate()
        {
            if (estado == 0)
            {
                var ballPosition = GetBallPosition();
                if (Vector3.Distance(ballPosition, GetMyGoalPosition()) < 8)
                {
                    MoveBy(GetDirectionTo(ballPosition));
                }
                else
                {
                    GoTo(FieldPosition.A2);
                }
            }
        }

        public override void OnReachBall()
        {
            var CentroPosition = GetDirectionTo(GetTeamMatesInformation()[0].Position);
            var directionToCentro = GetDirectionTo(CentroPosition);

            ShootBall(directionToCentro, ShootForce.Medium);
        }

        public override void OnScoreBoardChanged(ScoreBoard scoreBoard)
        {

        }

        public override FieldPosition GetInitialPosition() => FieldPosition.A2;

        public override string GetPlayerDisplayName() => "Defensor";
    }
}